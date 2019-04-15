using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class GoodsCategory : EntityBase
    {
        public int GoodsRefId { get; set; }
        public int GoodsCategoryTypeRefId { get; set; }
        public int GoodsId { get; set; }
        public virtual Goods Goods { get; set; }
        [ForeignKey("GoodsCatgeoryTypeId")]
        public virtual GoodsCategoryType GoodsCategoryType { get; set; }
        public int GoodsCatgeoryTypeId { get; set; }

    }
}
