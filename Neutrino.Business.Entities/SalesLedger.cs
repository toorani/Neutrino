using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// دفتر فروش
    /// </summary>
    public class SalesLedger : EntityBase
    {
        public int GoodsId { get; set; }
        public virtual Goods Goods { get; set; }
        /// <summary>
        /// تعداد فروش
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// تاریخ شروع
        /// </summary>
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        /// <summary>
        ///  شناسه فروشنده 
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// فروشنده
        /// </summary>
        public virtual Company Company { get; set; }
    }
}
