using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class BranchQuntityGoal : EntityBase
    {
        public int Month { get; set; }
        public int Year { get; set; }

        public int GoalId { get; set; }
        public int GoodsId { get; set; }
        public int BranchId { get; set; }
        public Double TotalNumber { get; set; }
        public int Quntity { get; set; }
        public bool IsTouchTarget { get; set; }
        public int TouchingPercent { get; set; }

        #region [ Constructor(s) ]
        public BranchQuntityGoal()
        {
                
        }
        #endregion
    }
}
