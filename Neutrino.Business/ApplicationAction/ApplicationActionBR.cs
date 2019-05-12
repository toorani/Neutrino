using FluentValidation;
using Neutrino.Entities;
using System.Collections.Generic;

namespace Neutrino.Business
{
    public class ApplicationActionBR : NeutrinoValidator<List<ApplicationAction>>
    {
        #region [ Constructor(s) ]

        public ApplicationActionBR()
            : base()
        {
            RuleForEach(entity => entity)
               .SetValidator(x => new ApplicationActionRules());
        }

        #endregion
    }

    class ApplicationActionRules : AbstractValidator<ApplicationAction>
    {
        #region [ Constructor(s) ]
        public ApplicationActionRules()
        {
            RuleFor(x => x.ActionUrl)
                .NotEmpty()
                .WithMessage("مسیر اکشن مشخص نشده است");
            RuleFor(x => x.HtmlUrl)
                .NotEmpty()
                .WithMessage("مسیر صفحه مشخص نشده است");
        }
     
        #endregion
    }
  

}
