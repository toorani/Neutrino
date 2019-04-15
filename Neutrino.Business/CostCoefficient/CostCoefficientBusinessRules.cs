
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
    public class CostCoefficientBusinessRules : AbstractValidator<CostCoefficient>
    {
        public CostCoefficientBusinessRules()
        {
            RuleForEach(x => x.Records)
                .SetValidator(new CostCoefficientItemBusinessRules());
        }
       
    }
    class CostCoefficientItemBusinessRules : NeutrinoValidator<CostCoefficient>
    {
        public CostCoefficientItemBusinessRules()
        {
            RuleFor(x => x.GoodsCategoryTypeId)
                .NotNull()
                .WithMessage("نوع محصول را مشخص کنید");

            RuleFor(x => x.Coefficient)
                .NotNull()
                .WithMessage("ضریب هزینه را مشخص کنید");
            
        }

    }
}
