using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class AppMenu : EntityBase
    {
        [StringLength(256)]
        public string Title { get; set; }
        public int OrderId { get; set; }
        [StringLength(256)]
        public string Url { get; set; }
        public int? ParentId { get; set; }
        public string Icon { get; set; }
        public AppMenu Parent { get; set; }
        public virtual ICollection<AppMenu> ChildItems { get; set; }

        #region [ Constructor(s) ]
        public AppMenu()
        {
            ChildItems = new HashSet<AppMenu>();
            
        }
        #endregion

    }
}
