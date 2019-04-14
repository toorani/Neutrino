using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class BranchGoalItem 
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal? Percent { get; set; }
        public decimal? Amount { get; set; }
        public int? BranchGoalId { get; set; }
        public string GoalGoodsCategoryName { get; set; }
        public int GoalId { get; set; }

        #region [ Constructor(s) ]
        public BranchGoalItem()
        {
        }
        #endregion

    }
    public class BranchGoalDTO
    {
        public List<BranchGoalItem> Items { get; set; }
        public Goal Goal { get; set; }
        #region [ Constructor(s) ]
        public BranchGoalDTO()
        {
            Items = new List<BranchGoalItem>();
            
        }
        #endregion
    }
    public class BranchGoalIndex :EntityBase
    {
        public string GoalGoodsCategoryName { get; set; }
        public int GoalGoodsCategoryId { get; set; }
        public decimal TotalPercent { get; set; }

        #region [ Constructor(s) ]
        public BranchGoalIndex()
        {

        }
        #endregion
    }
}
