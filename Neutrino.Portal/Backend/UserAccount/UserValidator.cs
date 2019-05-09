using Microsoft.AspNet.Identity;
using Neutrino.Entities;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Neutrino.Portal
{
    public class UserValidator : UserValidator<User, int>
    {
        private readonly UserManager<User, int> manager;
        public UserValidator(UserManager<User, int> manager)
            : base(manager)
        {
            this.manager = manager;
        }
        public override async Task<IdentityResult> ValidateAsync(User item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            var errors = new List<string>();
            await ValidateUserName(item, errors);
            if (RequireUniqueEmail)
            {
                await ValidateEmail(item, errors);
            }
            if (errors.Count > 0)
            {
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success;
        }
        private async Task ValidateUserName(User user, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                //PropertyTooShort
                //{0} cannot be null or empty.
                errors.Add("نام کاربری نمیتواند خالی باشد");
            }
            else if (AllowOnlyAlphanumericUserNames && !Regex.IsMatch(user.UserName, @"^[A-Za-z0-9@_\.]+$"))
            {
                // If any characters are not letters or digits, its an illegal user name
                //InvalidUserName
                //User name {0} is invalid, can only contain letters or digits.
                errors.Add("نام کاربری استفاده شده صحیح نمیباشد.لطفا از حروف انگلیسی ، اعداد و علامت @ و _ برای ایجاد نام کاربری استفاده کنید");
            }
            else
            {
                var owner = await manager.FindByNameAsync(user.UserName);
                if (owner != null && !EqualityComparer<int>.Default.Equals(owner.Id, user.Id))
                {
                    //DuplicateName
                    //Name {0} is already taken.
                    errors.Add("نام کاربری وارد شده تکراری میباشد");
                }
            }
        }
        private async Task ValidateEmail(User user, List<string> errors)
        {
            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                try
                {
                    var m = new MailAddress(user.Email);
                }
                catch (FormatException)
                {
                    //InvalidEmail
                    //Email '{0}' is invalid.
                    errors.Add("ایمیل وارد شده معتبر نمیباشد");
                    return;
                }
            }
            else
            {
                //PropertyTooShort
                //{0} cannot be null or empty.
                errors.Add("ایمیل نمیتواند خالی  باشد");
                return;
            }
            var owner = await manager.FindByEmailAsync(user.Email);
            if (owner != null && !EqualityComparer<int>.Default.Equals(owner.Id, user.Id))
            {
                //DuplicateEmail
                //Email '{0}' is already taken
                errors.Add("ایمیل وارد شده تکراری میباشد");
            }
        }


    }
}
