using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// قیمت محصول
    /// </summary>
    public class GoodsPrice : EntityBase
    {
        public int GoodsId { get; set; }
        public virtual Goods Goods { get; set; }
        /// <summary>
        /// قیمت تامین کننده
        /// </summary>
        public decimal SupplierPrice { get; set; }
        /// <summary>
        /// قیمت فروشنده 
        /// فروشنده مثل داروخانه - فروشگاه 
        /// </summary>
        public decimal SalerPrice { get; set; }
        /// <summary>
        /// قیمت توزیع کننده
        /// </summary>
        public decimal DistributorPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// قیمت موجود و فعال محصول 
        /// </summary>
        public bool IsActive { get; set; }

        #region [ Constructor(s) ]
        public GoodsPrice()
        {
            
        }
        #endregion
    }
}
