using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class GoalGoodsCategoryGoods : EntityBase
    {
        public int GoodsId { get; set; }
        public virtual Goods Goods { get; set; }
        public int GoalGoodsCategoryId { get; set; }
        public virtual GoalGoodsCategory GoalGoodsCategory { get; set; }
        
    }
}
