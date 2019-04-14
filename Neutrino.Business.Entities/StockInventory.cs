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
    /// لیست موجودی
    /// </summary>
    public class StockInventory : EntityBase
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
        ///  شناسه شرکت - داروخانه 
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// شناسه شرکت - داروخانه
        /// </summary>
        public virtual Company Company { get; set; }
    }
}
