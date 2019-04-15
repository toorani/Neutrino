
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

namespace Neutrino.Business
{
    public class UploadedFileBusinessRules : NeutrinoValidator<UploadedFile>
    {
        public UploadedFileBusinessRules(IEntityRepository<UploadedFile> dataService)
            : base(dataService)
        {
            RuleFor(x => x.OriginalFileName)
                .NotNull()
                .WithMessage("نام فایل باید مشخص شده باشد");

            RuleFor(x => x.CompanyId)
                .NotNull()
                .WithMessage("صاحب فایل را مشخص نمیباشد");
            RuleFor(x => x)
                .Must(x => !Duplicate(x))
                .WithMessage("پست سازمانی وارد شده تکراری میباشد");
        }

        private bool Duplicate(UploadedFile entity)
        {
            return dbContext.UploadedFiles
                .AsNoTracking()
                .Any(x => x.HashValue == entity.HashValue && x.Deleted == false);

        }
    }
}
