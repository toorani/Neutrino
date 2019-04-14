using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class GoodsPromotion : EntityBase
    {
        public int GoodsId { get; set; }
        public virtual Goods Goods { get; set; }
        public int GoodsPriceId { get; set; }
        public virtual GoodsPrice Price { get; set; }
        /// <summary>
        /// پروموشن
        /// </summary>
        public decimal PromotionValue
        {
            get
            {
                decimal value = 0.1m;
                return value;
            }
            private set
            {
                //Need for EF
            }
        }
        /// <summary>
        /// سود خرید
        /// </summary>
        public decimal Eran
        {
            get
            {
                decimal eran = 0;
                if (Price != null)
                {
                    eran = (Price.SalerPrice - Price.SupplierPrice) + (Price.SupplierPrice * PromotionValue); 
                }
                return eran;
            }
            private set
            {
                //Need for EF 
            }
        }

        #region [ Constructor(s) ]
        public GoodsPromotion()
        {
        }
        #endregion
    }
}
