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
        public string PhoneNumber { get; set; }
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

    public class ChangePasswordViewModel : ViewModelBase
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
    

}
