using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Espresso.Entites;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Neutrino.Entities
{
    public class UserClaim : IdentityUserClaim<int>
    {
        public virtual User User { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatorID { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? EditorId { get; set; }
    }
}
