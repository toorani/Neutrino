using Espresso.Entites;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Neutrino.Entities
{
    public class UserRole : IdentityUserRole<int>, IKeyEnabled
    {
        [Key]
        public int Id { get; set; }
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatorID { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? EditorId { get; set; }

    }
}
