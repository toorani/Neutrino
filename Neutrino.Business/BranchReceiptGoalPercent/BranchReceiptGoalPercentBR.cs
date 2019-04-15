using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Neutrino.Entities;

namespace Neutrino.Business
{
    public class BranchReceiptGoalPercentBR : AbstractValidator<BranchReceiptGoalPercentDTO>
    {
        public BranchReceiptGoalPercentBR()
        {
            RuleFor(x => x)
                .Must(x => CheckNotEmpty(x))
                .WithMessage("اطلاعات مراکز وجود ندارد");
            RuleFor(x=>x.GoalId)
                .NotEmpty()
                .WithMessage("اطلاعات هدف وجود ندارد");

        }

        private bool CheckNotEmpty(BranchReceiptGoalPercentDTO entity)
        {
            return entity.Branches.Count != 0 || entity.DeselectedBranches.Count != 0;
        }
    }
}
