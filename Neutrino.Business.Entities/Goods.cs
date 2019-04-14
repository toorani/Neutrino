using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;


namespace Neutrino.Entities
{
    public class Goods : EntityBase
    {
        #region [ Public Property(ies) ]
        
        public int RefId { get; set; }
        [Required]
        [StringLength(256)]
        public string EnName { get; set; }
        [Required]
        [StringLength(256)]
        public string FaName { get; set; }
        [StringLength(20)]
        public string GoodsCode { get; set; }
        /// <summary>
        /// کد رسمی کالا
        /// </summary>
        public int OfficalCode { get; set; }
        /// <summary>
        /// کد ژنریک
        /// </summary>
        public int GenericCode { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public int CompanyRefId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        [ForeignKey("ProducerId")]
        public virtual Company Producer { get; set; }
        public int? ProducerId { get; set; }
        public int ProducerRefId { get; set; }
        [StringLength(250)]
        public string Brand { get; set; }
        [DefaultValue(1)]
        public short? StatusId { get; set; }
        /// <summary>
        /// قیمت خرید
        /// </summary>
        public decimal PurchasePrice { get; set; }
        /// <summary>
        /// قیمت فروش
        /// </summary>
        public decimal SalesPrice { get; set; }
        /// <summary>
        /// قیمت مصرف کننده
        /// </summary>
        public decimal ConsumerPrice { get; set; }
        /// <summary>
        /// تایید فنی دارد؟
        /// </summary>
        public bool IsTechnicalApproved { get; set; }
        public virtual SupplierTypeEnum? SupplierTypeId { get; set; }
        /// <summary>
        /// نحوه تامین
        /// </summary>
        [ForeignKey("SupplierTypeId")]
        public virtual SupplierType SupplierType { get; set; }
        /// <summary>
        /// مالیات دارد؟
        /// </summary>
        public bool HasTaxable { get; set; }
        /// <summary>
        /// عوارض دارد؟
        /// </summary>
        public bool HasExtraCharge { get; set; }
        /// <summary>
        /// تعداد در بسته
        /// </summary>
        public int PackageCount { get; set; }
        public virtual ICollection<GoalGoodsCategoryGoods> GoalGoodsCategoryCollection { get; set; }
        public virtual ICollection<GoodsCategory> GoodsCategoryCollection { get; set; }
        public virtual ICollection<GoodsPrice> Prices { get; set; }
        public string ATC { get; set; }
        public virtual TherapeuticTypeEnum? TherapeuticTypeId { get; set; }
        /// <summary>
        /// نوع دسته بندی درمان
        /// </summary>
        [ForeignKey("TherapeuticTypeId")]
        public virtual TherapeuticType TherapeuticType { get; set; }
        public int? GeneralId { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public Goods()
        {
            GoalGoodsCategoryCollection = new HashSet<GoalGoodsCategoryGoods>();
            GoodsCategoryCollection = new HashSet<GoodsCategory>();
            Prices = new HashSet<GoodsPrice>();
        }
        #endregion

    }
}
