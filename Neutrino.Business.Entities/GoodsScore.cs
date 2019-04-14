using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// امتیاز دارو
    /// </summary>
    public class GoodsScore : EntityBase
    {
        #region [ Public Property(ies) ]

        /// <summary>
        /// موجودی اول محدوده تاریخی
        /// </summary>
        public int StartedStock { get; set; }
        /// <summary>
        /// موجودی اضافه شده در طول محدوده تاریخی
        /// </summary>
        public int AddedStock { get; set; }
        /// <summary>
        /// فروش محدوده تاریخی
        /// </summary>
        public int Sales { get; set; }
        /// <summary>
        ///  شرکت صاحب کالا
        /// </summary>
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        /// <summary>
        /// کد دارو
        /// </summary>
        public int GoodsId { get; set; }
        public virtual Goods Goods { get; set; }
        /// <summary>
        /// تاریخ شروع محدوده تاریخی
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// تاریخ پایان محدوده تاریخی
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// درصد فروش رفته موجودی اضافه شده در طول محدوده تاریخی
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal AddedStockSalesPercent { get; private set; }
        /// <summary>
        /// درصد فروش رفته موجودی اول محدوده تاریخی
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal StartedStockSalesPercent { get; private set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal Score { get; private set; }
        #endregion

        #region [ Constructor(s) ]
        public GoodsScore()
        {
            AddedStockSalesPercent = (Sales / AddedStock) * 100;
            StartedStockSalesPercent = (Sales / (AddedStock + StartedStock)) * 100;
            Score = (((AddedStockSalesPercent + StartedStockSalesPercent) / 2) * 3.2m * 1.5m) / 100;
        }
        #endregion
    }
}
