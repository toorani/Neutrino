using Espresso.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Neutrino.Entities
{
    public class User : IdentityUser<int, IdentityUserLogin<int>, UserRole, UserClaim>, IUser<int>
    {
        [Required, StringLength(256)]
        public string Name { get; set; }
        [Required, StringLength(256)]
        public string LastName { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatorID { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? EditorId { get; set; }
        public User()
            :base()
        {
           
        }
    }
}
