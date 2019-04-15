using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class GoalGoodsCategory : EntityBase
    {
        /// <summary>
        /// نام دسته دارویی
        /// </summary>
        [StringLength(100), Required]
        public string Name { get; set; }
        /// <summary>
        /// فعال/غیرفعال 
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// دارویی ها موجود در دسته دارویی
        /// </summary>
        public virtual ICollection<GoalGoodsCategoryGoods> GoodsCollection { get; set; }
        /// <summary>
        /// نوع دسته دارویی
        /// </summary>
        [Required]
        public virtual GoalGoodsCategoryTypeEnum GoalGoodsCategoryTypeId { get; set; }
        /// <summary>
        /// نوع دسته دارویی
        /// </summary>
        [ForeignKey("GoalGoodsCategoryTypeId")]
        public virtual GoalGoodsCategoryType GoalGoodsCategoryType { get; set; }
        /// <summary>
        /// نوع هدف تعریف شده
        /// </summary>
        public virtual GoalTypeEnum GoalTypeId { get; set; }
        /// <summary>
        /// نوع هدف تعریف شده
        /// </summary>
        [ForeignKey("GoalTypeId")]
        public virtual GoalType GoalType { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }
        

        public bool IsVisible { get; set; }

        public GoalGoodsCategory()
        {
            GoodsCollection = new HashSet<GoalGoodsCategoryGoods>();
            Goals = new HashSet<Goal>();
            IsActive = true;
            IsVisible = true;
        }


    }
}