using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System.Data.Entity;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Business
{
    class FulfillmentPercentBR : NeutrinoValidator<FulfillmentPercent>
    {
        #region [ Constructor(s) ]
        public FulfillmentPercentBR(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            RuleFor(entity => entity.BranchId)
               .NotNull()
               .WithMessage(".مرکز مشخص نشده است");

            RuleFor(entity => entity.ManagerFulfillmentPercent)
                .GreaterThanOrEqualTo(0)
                .LessThan(100)
                .WithMessage(".لطفا مقداری بین صفر تا صد برای درصد مشمول مشخص کنید");

            RuleFor(entity => entity.ManagerReachedPercent)
                .GreaterThanOrEqualTo(0)
                .LessThan(100)
                .WithMessage(".لطفا مقداری بین صفر تا صد برای درصد مشخص کنید");

            RuleFor(entity => entity.Month)
                .NotNull()
                .WithMessage(".لطفا ماه را مشخص نماید");

            RuleFor(entity => entity.Year)
                .NotNull()
                .WithMessage(".لطفا سال را مشخص نماید");
        }

        #endregion

        #region [ Private Method(s) ]
        private bool IsDuplicate(BranchGoal branchGoal)
        {
            var dbBranchBenefit = unitOfWork.BranchGoalDataService.GetQuery()
                .AsNoTracking()
                .Where(
                    x => x.Deleted == false
                    && x.BranchId == branchGoal.BranchId
                    && x.Percent == branchGoal.Percent)
                .Include(x => x.Goal)
                .AsEnumerable()
                .FirstOrDefault();

            if (dbBranchBenefit == null)
                return false;
            return !(dbBranchBenefit.Id == branchGoal.Id);
        }
        #endregion
    }

    public class FulfillmentPercentRules : AbstractValidator<List<FulfillmentPercent>>
    {
        #region [ Constructor(s) ]
        public FulfillmentPercentRules(NeutrinoUnitOfWork unitOfWork)
        {
            RuleFor(entity => entity)
               .SetCollectionValidator(x => new FulfillmentPercentBR(unitOfWork));

        }
        #endregion
    }

}
