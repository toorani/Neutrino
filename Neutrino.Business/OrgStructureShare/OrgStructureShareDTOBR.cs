using System.Data.Entity;
using System.Linq;
using FluentValidation;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;

namespace Neutrino.Business
{
    public class OrgStructureShareDTOBR : NeutrinoValidator<OrgStructureShareDTO>
    {
        public OrgStructureShareDTOBR(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            RuleFor(x => x.Branch)
                .NotNull()
                .WithMessage("مرکز را مشخص کنید");
            RuleForEach(x => x.Items)
                .SetValidator(new OrgStructureShareBR());
        
        }

        

        private bool Duplicate(OrgStructureShare entity)
        {
            var dbEntity = unitOfWork.OrgStructureShareDataService
                .GetQuery()
                .AsNoTracking()
                .SingleOrDefault(
               x => x.Deleted == false
               && x.BranchId == entity.BranchId);

            if (dbEntity == null)
                return false;

            return !(dbEntity.Id == entity.Id);
        }
    }
    class OrgStructureShareBR : NeutrinoValidator<OrgStructureShare>
    {
        public OrgStructureShareBR()
        {
            RuleFor(x => x.BranchId)
                .NotNull()
                .WithMessage("مرکز را مشخص کنید");
            RuleFor(x => x.OrgStructureId)
                .NotNull()
                .WithMessage("پست سازمانی را مشخص کنید");

            When(x => x.PrivateReceiptPercent.HasValue, () =>
            {
                RuleFor(x => x.PrivateReceiptPercent)
                 .GreaterThanOrEqualTo(0)
                 .WithMessage(".لطفا عددی بزرگتر از صفر برای درصد سهم وصول خصوصی مشخص کنید");
            });

            When(x => x.TotalReceiptPercent.HasValue, () =>
            {
                RuleFor(entity => entity.TotalReceiptPercent)
                   .GreaterThanOrEqualTo(0)
                   .WithMessage(".لطفا عددی بزرگتر از صفر برای سهم وصول کل مشخص کنید");
            });

            When(x => x.SalesPercent.HasValue, () =>
            {
                RuleFor(entity => entity.SalesPercent)
                   .GreaterThanOrEqualTo(0)
                   .WithMessage(".لطفا عددی بزرگتر از صفر برای سهم فروش مشخص کنید");
            });
        }

    }
}
