using Espresso.Entites;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neutrino.Entities
{
    public class Permission : EntityBase
    {
        #region [ Public Property(ies) ]
        public int RoleId { get; set; }
        public int ApplicationActionId { get; set; }
        [NotMapped]
        public List<string> Urls { get; set; }
        public virtual Role Role { get; set; }
        public virtual ApplicationAction ApplicationAction { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public Permission()
        {
            Urls = new List<string>();
        }
        #endregion
    }


}
