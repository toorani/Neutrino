using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class ApplicationAction : EntityBase
    {
        #region [ Public Property(ies) ]
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(100)]
        public string FaTitle { get; set; }
        public int? ParentId { get; set; }
        [StringLength(200)]
        public string HtmlUrl { get; set; }
        [StringLength(200)]
        public string ActionUrl { get; set; }
        public AppActionTypes ActionTypeId { get; set; }
        public virtual ApplicationAction Parent { get; set; }
        public virtual ICollection<ApplicationAction> ChildActions { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public ApplicationAction()
        {
            ChildActions = new HashSet<ApplicationAction>();
        }
        #endregion
    }

    public enum AppActionTypes : int
    {
        Create = 1,
        Read = 2,
        Update = 3,
        Delete = 4
    }
}
