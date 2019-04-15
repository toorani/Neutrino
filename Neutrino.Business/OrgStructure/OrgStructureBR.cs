using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Espresso.DataAccess.Interfaces;
using FluentValidation;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;

namespace Neutrino.Business
{
    public class OrgStructureCollectionBR : AbstractValidator<List<OrgStructure>>
    {
        public OrgStructureCollectionBR()
        {
            RuleFor(x => x.Count)
                .NotEmpty()
                .WithMessage("اطلاعاتی جهت ثبت وجود ندارد")
                .Configure(x => x.CascadeMode = CascadeMode.StopOnFirstFailure);

            RuleFor(x => x)
                .SetCollectionValidator(x=> new OrgStructureBR());
        }
    }
    public class OrgStructureBR : NeutrinoValidator<OrgStructure>
    {
        #region [ Constructor(s) ]
        public OrgStructureBR()
        {
            RuleFor(x => x.PositionTypeId)
                .NotNull()
                .WithMessage("پست سازمانی را مشخص کنید");

            RuleFor(x => x.BranchId)
                .NotNull()
                .WithMessage("مرکز را مشخص کنید");
            //RuleFor(x => x)
            //    .Must(x => !Duplicate(x))
            //    .WithMessage("پست سازمانی وارد شده تکراری میباشد");
        }
        #endregion

        private bool Duplicate(OrgStructure entity)
        {
            var dbEntity = unitOfWork.OrgStructureDataService
                .GetQuery()
                .AsNoTracking()
                .SingleOrDefault(
               x => x.PositionTypeId == entity.PositionTypeId
               && x.Deleted == false
               && x.BranchId == entity.BranchId);

            if (dbEntity == null)
                return false;

            return !(dbEntity.Id == entity.Id);
        }
    }

}
