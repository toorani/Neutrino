using System.Runtime.Serialization;

namespace Elite.Neutrino.Services
{
    /// <summary>
    /// اطلاعات کالا
    /// </summary>
    [DataContract]
    public class GoodsInfo
    {
        /// <summary>
        /// شناسه کالا
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// English goods name
        /// </summary>
        [DataMember]
        public string EnName { get; set; }
        /// <summary>
        /// Farsi goods name
        /// </summary>
        [DataMember]
        public string FaName { get; set; }
        /// <summary>
        /// کد رسمی کالا
        /// </summary>
        [DataMember]
        public int OfficalCode { get; set; }
        /// <summary>
        /// کد محصول
        /// </summary>
        [DataMember]
        public int GoodsCode { get; set; }
        /// <summary>
        /// کد ژنریک
        /// </summary>
        [DataMember]
        public int GenericCode { get; set; }
        /// <summary>
        /// شناسه شرکت تامین کننده موجود در اطلاعات شرکت ها
        /// </summary>
        [DataMember]
        public int SupplierId { get; set; }
        /// <summary>
        /// شناسه شرکت تولید کننده موجود در اطلاعات شرکت ها
        /// </summary>
        [DataMember]
        public int ProducerId { get; set; }
        /// <summary>
        /// برند
        /// </summary>
        [DataMember]
        public string Brand { get; set; }
        /// <summary>
        /// نام ژنریک
        /// </summary>
        [DataMember]
        public string Generic { get; set; }
        /// <summary>
        /// قیمت خرید
        /// </summary>
        [DataMember]
        public decimal PurchasePrice { get; set; }
        /// <summary>
        /// قیمت فروش
        /// </summary>
        [DataMember]
        public decimal SalesPrice { get; set; }
        /// <summary>
        /// قیمت مصرف کننده
        /// </summary>
        [DataMember]
        public decimal ConsumerPrice { get; set; }
        /// <summary>
        /// تایید فنی دارد؟
        /// </summary>
        [DataMember]
        public bool IsTechnicalApproved { get; set; }
        /// <summary>
        /// نحوه تامین
        /// </summary>
        [DataMember]
        public SupplierTypes SupplierType { get; set; }
        /// <summary>
        /// مالیات دارد؟
        /// </summary>
        [DataMember]
        public bool HasTaxable { get; set; }
        /// <summary>
        /// عوارض دارد؟
        /// </summary>
        [DataMember]
        public bool HasExtraCharge { get; set; }
        /// <summary>
        /// دسته بندی محصول
        /// </summary>
        [DataMember]
        public GoodsCategories Category { get; set; }
        /// <summary>
        /// تعداد در بسته
        /// </summary>
        [DataMember]
        public int PackageCount { get; set; }
        /// <summary>
        /// شناسه کالا
        /// </summary>
        [DataMember]
        public int CompanyId { get; set; }
    }

    /// <summary>
    /// نوع تامین کننده
    /// </summary>
    [DataContract]
    public enum SupplierTypes : int
    {
        /// <summary>
        /// داخلی
        /// </summary>
        [EnumMember]
        Domestic,
        /// <summary>
        /// وارداتی
        /// </summary>
        [EnumMember]
        Foreign
    }
    /// <summary>
    /// دسته بندی کالا
    /// </summary>
    [DataContract]
    public enum GoodsCategories : int
    {
        /// <summary>
        /// قطره
        /// </summary>
        [EnumMember]
        Drops ,
        /// <summary>
        /// الگزیر
        /// </summary>
        [EnumMember]
        Elixir,
        /// <summary>
        /// آمپول
        /// </summary>
        [EnumMember]
        Injection,
        /// <summary>
        /// سرم
        /// </summary>
        [EnumMember]
        Infusion,
        /// <summary>
        /// بدون ای ار سی
        /// </summary>
        [EnumMember]
        WithoutIRC,
        /// <summary>
        /// پماد
        /// </summary>
        [EnumMember]
        Ointment,
        /// <summary>
        /// کرم
        /// </summary>
        [EnumMember]
        Cream,
        /// <summary>
        /// ژل
        /// </summary>
        [EnumMember]
        Gel,
        /// <summary>
        /// سوسپانسیون
        /// </summary>
        [EnumMember]
        Suspension,
        /// <summary>
        /// شربت
        /// </summary>
        [EnumMember]
        Syrup,
        /// <summary>
        /// قرص
        /// </summary>
        [EnumMember]
        Tablet,
        /// <summary>
        /// کپسول
        /// </summary>
        [EnumMember]
        Capsoul,
        /// <summary>
        /// محلول
        /// </summary>
        [EnumMember]
        Solution,
        /// <summary>
        /// ویال
        /// </summary>
        [EnumMember]
        Vial,
        /// <summary>
        /// قوطی -- به عنوان مثال شیرخشک
        /// </summary>
        [EnumMember]
        Can,
        /// <summary>
        /// کیسه - مثل پوشک
        /// </summary>
        [EnumMember]
        Bag


    }
}

