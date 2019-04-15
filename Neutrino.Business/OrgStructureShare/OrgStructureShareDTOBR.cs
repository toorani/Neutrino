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
                .LessThanOrEqualTo(100)
                .WithMessage(".لطفا مقداری بین صفر تا صد برای سهم وصول خصوصی مشخص کنید");

                RuleFor(entity => entity.PrivateReceiptPercent)
                   .GreaterThanOrEqualTo(0)
                   .WithMessage(".لطفا مقداری بین صفر تا صد برای سهم وصول خصوصی مشخص کنید");
            });

            When(x => x.TotalReceiptPercent.HasValue, () =>
            {
                RuleFor(x => x.TotalReceiptPercent)
                .LessThanOrEqualTo(100)
                .WithMessage(".لطفا مقداری بین صفر تا صد برای سهم وصول کل مشخص کنید");

                RuleFor(entity => entity.TotalReceiptPercent)
                   .GreaterThanOrEqualTo(0)
                   .WithMessage(".لطفا مقداری بین صفر تا صد برای سهم وصول کل مشخص کنید");
            });

            When(x => x.SalesPercent.HasValue, () =>
            {
                RuleFor(x => x.SalesPercent)
                .LessThanOrEqualTo(100)
                .WithMessage(".لطفا مقداری بین صفر تا صد برای سهم فروش مشخص کنید");

                RuleFor(entity => entity.SalesPercent)
                   .GreaterThanOrEqualTo(0)
                   .WithMessage(".لطفا مقداری بین صفر تا صد برای سهم فروش مشخص کنید");
            });
        }

    }
}
