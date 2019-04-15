using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Core;
using Espresso.Portal;
using Neutrino.Entities;

namespace Neutrino.Portal.Models
{
    public class PermissionViewModel : ViewModelBase
    {
        public int RoleId { get; set; }
        public List<AppActionPermissionViewModel> Actions { get; set; }
        public RoleViewModel Role { get; set; }

        public PermissionViewModel() 
        {
            Actions = new List<AppActionPermissionViewModel>();
        }
    }

    public class UserAccessToken
    {
        public int RoleId { get; set; }
        public int ActionId { get; set; }
        public string HtmlUrl { get; set; }
        public string ActionUrl { get; set; }
        public int ActionTypeId { get; set; }
        public UserAccessToken()
        {}
    } 
}