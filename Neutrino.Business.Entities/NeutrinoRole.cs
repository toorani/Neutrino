using Espresso.Entites;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Neutrino.Entities
{
    public class Role : EntityBase 
    {
        [StringLength(256)]
        public string FaName { get; set; }
        [DefaultValue(false)]
        public bool IsUsingBySystem { get; set; }
        [Required, StringLength(256)]
        public string Name { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

        public Role()
        {
            IsUsingBySystem = false;
            UserRoles = new HashSet<UserRole>();
        }
    }
}
