
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Core;
using Espresso.DataAccess.Interfaces;
using Espresso.BusinessService;
using FluentValidation;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Data.EntityFramework;
using System.Data.Entity;
using System.Threading;

namespace Neutrino.Business
{
    public class GoalStepBusinessRules : NeutrinoValidator<GoalStep>
    {
        #region [ Varibale(s) ]
        
        #endregion

        #region [ Constructor(s) ]
        public GoalStepBusinessRules(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            RuleFor(x => x)
                .Must(checkNotUsedGoal)
                .WithMessage("با توجه به اینکه با هدف انتخاب شده محاسبات انجام شده است ،امکان اصلاح وجود ندارد")
                .Configure(x => x.CascadeMode = CascadeMode.StopOnFirstFailure);


            RuleFor(entity => entity.ComputingTypeId)
               .NotNull()
               .WithMessage(".اطلاعات نحوه محاسبه مشخص نشده است");

            RuleFor(entity => entity.ComputingValue)
                .NotEmpty()
                .WithMessage(".مقدار پایه مشخص نشده است");

            RuleFor(x => x)
                .Must(x => checkSequentialValue(x))
                .WithMessage("مقدار پایه باید از مقدار پایه قبلی بزرگتر باشد");

            RuleFor(entity => entity.GoalId)
                .NotNull()
                .WithMessage(".هدف مشخص نشده است");

            RuleFor(x => x.Items)
                .Must(x => x.Count != 0)
                .WithMessage(".پاداش و جریمه هر دو نامشخص میباشد شما باید یکی از این دو ایتم را انتخاب کنید");

            RuleForEach(x => x.Items)
                .SetValidator(new GoalStepItemInfoRules());


            When(x => x.GoalTypeId == GoalTypeEnum.Supplier, () =>
            {
                RuleFor(x => x.IncrementPercent)
                    .NotNull()
                    .WithMessage("لطفا درصد افزایش را مشخص نماید");

            });

            RuleFor(entity => entity)
                .Must(entity => !IsDuplicate(entity))
                .WithMessage(".امکان ثبت اطلاعات تکراری وجود ندارد");

        }
        #endregion

        #region [ Private Method(s) ]
        private bool checkNotUsedGoal(GoalStep arg)
        {
            var goal = arg.Goal;
            if (goal == null)
            {
                goal = unitOfWork.GoalDataService.GetById(arg.GoalId);
            }
            return !goal.IsUsed;
        }
        private bool IsDuplicate(GoalStep goalStep)
        {
            var dbGoalStep = unitOfWork.GoalStepDataService
                .GetQuery()
                .AsNoTracking()
                .Where(
                    gal => gal.GoalId == goalStep.GoalId
                    && gal.ComputingTypeId == goalStep.ComputingTypeId
                    && gal.ComputingValue == goalStep.ComputingValue
                    && gal.Deleted == false)
                .Join(unitOfWork.GoalStepItemInfoDataService.GetQuery(), f => f.Id, s => s.GoalStepId, (f, s) => new { ItemInfo = s, GoalStepId = f.Id })
                .AsEnumerable()
                .Where(
                item => goalStep.Items.Any(x => x.ActionTypeId == item.ItemInfo.ActionTypeId
                        && x.Amount == item.ItemInfo.Amount
                        && x.ChoiceValueId == item.ItemInfo.ChoiceValueId
                        && x.ComputingTypeId == item.ItemInfo.ComputingTypeId
                        && x.EachValue == item.ItemInfo.EachValue
                        && x.ItemTypeId == item.ItemInfo.ItemTypeId
                        && x.Deleted == false))
                .FirstOrDefault();

            if (dbGoalStep == null)
                return false;
            return !(dbGoalStep.GoalStepId == goalStep.Id);

        }
        private bool checkSequentialValue(GoalStep goalStep)
        {
            var perviouseGoalSteps = unitOfWork.GoalStepDataService
                .Get(x => x.GoalId == goalStep.GoalId && x.Id < goalStep.Id
                , or => or.OrderByDescending(x => x.Id))
                .FirstOrDefault();

            if (perviouseGoalSteps == null)
                return true;

            return perviouseGoalSteps.ComputingValue < goalStep.ComputingValue;
        }

        #endregion

    }
}
