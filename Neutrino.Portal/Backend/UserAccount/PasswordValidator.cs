using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Portal
{
    public class PassValidator : Microsoft.AspNet.Identity.PasswordValidator
    {
        #region [ Constructor(s) ]

        #endregion

        public override Task<IdentityResult> ValidateAsync(string item)
        {
            if (item == null) throw new ArgumentNullException("item");
            var list = new List<string>();

            if (string.IsNullOrWhiteSpace(item) || item.Length < RequiredLength)
            {
                //PasswordTooShort
                //Passwords must be at least {0} characters.
                list.Add(string.Format("طول رمز عبور باید بیشتر از {0} باشد",RequiredLength));
            }

            if (RequireNonLetterOrDigit && item.All(IsLetterOrDigit))
            {
                //PasswordRequireNonLetterOrDigit
                //Passwords must have at least one non letter or digit character.
                list.Add("رمز عبور باید حداقل یک حرف و یا یک عدد داشته باشد");
            }

            if (RequireDigit && item.All(c => !IsDigit(c)))
            {
                //PasswordRequireDigit
                //Passwords must have at least one digit ('0'-'9').
                list.Add("رمز عبور باید حداقل یه عدد داشته باشد");
            }

            if (RequireLowercase && item.All(c => !IsLower(c)))
            {
                //PasswordRequireLower
                //Passwords must have at least one lowercase ('a'-'z').
                list.Add("رمز عبور باید حداقل یک حرف الفبا انگلیسی با حال کوچک داشته باشد");
            }
            if (RequireUppercase && item.All(c => !IsUpper(c)))
            {
                //PasswordRequireUpper
                //Passwords must have at least one uppercase ('A'-'Z').
                list.Add("رمز عبور باید حداقل یک حرف الفبا انگلیسی با حال بزرگ داشته باشد");
            }

            return Task.FromResult(list.Count == 0
                ? IdentityResult.Success
                : new IdentityResult(list));
        }
    }
}
