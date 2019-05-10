using Espresso.Entites;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Neutrino.Entities
{
    public class ApplicationAction : EntityBase
    {
        #region [ Public Property(ies) ]
        [StringLength(200),Required]
        public string HtmlUrl { get; set; }
        [StringLength(200), Required]
        public string ActionUrl { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public ApplicationAction()
        {
            Permissions = new HashSet<Permission>();
        }
        #endregion
    }
}
