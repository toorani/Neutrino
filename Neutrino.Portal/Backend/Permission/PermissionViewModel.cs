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
        public List<string> Urls { get; set; }

        public PermissionViewModel() 
        {
            Urls = new List<string>();
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