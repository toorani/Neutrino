using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using FluentValidation;
using Neutrino.Business;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public class GoalNonFulfillmentPercentBR : NeutrinoValidator<GoalNonFulfillmentPercent>
    {
        public GoalNonFulfillmentPercentBR(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            RuleFor(x => x.Percent)
                .NotNull()
                .WithMessage("درصد را مشخص نماید")
                .LessThanOrEqualTo(100)
                .WithMessage("درصد بیشتر از مقدار صد نمیتواند باشد");

            RuleFor(x => x)
                .Must(x => x.GoalNonFulfillmentBranches.Count != 0)
                .WithMessage("مراکز را مشخص نماید")
                .Must(x => !checkedForUniqueBranchAndPercent(x))
                .WithMessage(x => getMessage(x));

        }

        private string getMessage(GoalNonFulfillmentPercent entity)
        {
            var result = new StringBuilder();
            
            var branchIds = entity.GoalNonFulfillmentBranches.Select(x => x.BranchId).Distinct();

            unitOfWork.GoalNonFulfillmentBranchDataService
                .GetQuery()
                .Include(x => x.Branch)
                .Include(x => x.GoalNonFulfillmentPercent)
                .Where(x => branchIds.Contains(x.BranchId) && x.Deleted == false && x.GoalNonFulfillmentPercent.GoalId == entity.GoalId
                && x.GoalNonFulfillmentPercentId != entity.Id)
                .ToList()
                .ForEach(x =>
                {
                    result.AppendLine($"مرکز {x.Branch.Name} با درصد {x.GoalNonFulfillmentPercent.Percent} موجود میباشد");
                    result.Append("<br/>");
                });
            return result.ToString();
        }

        private bool checkedForUniqueBranchAndPercent(GoalNonFulfillmentPercent entity)
        {
            var branchIds = entity.GoalNonFulfillmentBranches.Select(x => x.BranchId).Distinct();
            return unitOfWork.GoalNonFulfillmentBranchDataService
                .GetQuery()
                .Where(x => branchIds.Contains(x.BranchId) && x.Deleted == false && x.GoalNonFulfillmentPercent.GoalId == entity.GoalId
                && x.GoalNonFulfillmentPercentId != entity.Id)
                .Any();
        }

    }


}
