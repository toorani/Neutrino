using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Espresso.Core;
using Espresso.Portal;
using Neutrino.Portal.Attributes;

namespace Neutrino.Portal.Models
{

   
    public class LoginViewModel : ViewModelBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public LoginViewModel() 
        {
        }

        
    }
    public class RegisterViewModel : ViewModelBase
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public int RoleId { get; set; }
        public List<UserAccessToken> UserAccessTokens { get; set; }
        public List<int> BranchesUnderControl { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }

        #region [ Constructor(s) ]
        public RegisterViewModel() 
        {
            UserAccessTokens = new List<UserAccessToken>();
            BranchesUnderControl = new List<int>();
        }
        #endregion
    }
    public class UserIndexViewModel : ViewModelBase
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }

        #region [ Constructor(s) ]
        public UserIndexViewModel()
        {
            
        }
        #endregion
    }
    public class RoleViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string FaName { get; set; }

        public RoleViewModel() 
        {
        }
    }
    

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
   
}
