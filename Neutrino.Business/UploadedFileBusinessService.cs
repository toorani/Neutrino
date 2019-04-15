
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
    public class UploadedFileBusinessService : NeutrinoBSBase<UploadedFile, IUploadedFile>
    {
        private readonly AbstractValidator<UploadedFile> _businessRulesService;

        #region [ Override Property(ies) ]
        protected override AbstractValidator<UploadedFile> businessRulesService
        {
            get { return _businessRulesService; }
        }
        #endregion

        #region [ Constructor(s) ]
        public UploadedFileBusinessService(ITransactionalData transactionalData)
            : base(transactionalData)
        {
            _businessRulesService = new UploadedFileBusinessRules(dataService);
        }
        public UploadedFileBusinessService()
            : base()
        {
            _businessRulesService = new UploadedFileBusinessRules(dataService);
        }

        #endregion

        #region [ Public Method(s) ]
        
        #endregion
    }
}
