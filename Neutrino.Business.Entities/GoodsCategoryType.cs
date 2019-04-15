using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class GoodsCategoryType : EntityBase
    {
        #region [ Public Property(ies) ]
        [StringLength(100)]
        public string Name { get; set; }
        public int RefId { get; set; }
        public virtual ICollection<GoodsCategory> GoodsCategoryCollection { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public GoodsCategoryType()
        {
            GoodsCategoryCollection = new HashSet<GoodsCategory>();
        }
        #endregion

    }
}
