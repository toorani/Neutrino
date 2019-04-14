using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class Permission : EntityBase
    {
        #region [ Public Property(ies) ]
        public int RoleId { get; set; }
        public int ApplicationActionId { get; set; }
        public virtual NeutrinoRole Role { get; set; }
        public virtual ApplicationAction ApplicationAction { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public Permission()
        {
            
        }
        #endregion
    }
}
