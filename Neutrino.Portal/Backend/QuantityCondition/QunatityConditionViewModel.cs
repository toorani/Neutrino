using System;
using System.Collections.Generic;
using Espresso.Portal;

namespace Neutrino.Portal.Models
{
    public class QuantityConditionViewModel : ViewModelBase
    {
        public int GoalId { get; set; }
        public string GoalCategoryName { get; set; }
        public int Quantity { get; set; }
        public decimal ExtraEncouragePercent { get; set; }
        public decimal NotReachedPercent { get; set; }
        public decimal ForthCasePercent { get; set; }
        public List<GoodsQuantityConditionVModel> GoodsQuantityConditions { get; set; }
        public int? QuantityConditionTypeId { get; set; }
        public QuantityConditionViewModel()             
        {
            GoodsQuantityConditions = new List<GoodsQuantityConditionVModel>();
        }
    }

    public class GoodsQuantityConditionVModel
    {
        public int Id { get; set; }
        public int QuantityConditionId { get; set; }
        public string EnName { get; set; }
        public string FaName { get; set; }
        public string Code { get; set; }
        public int GoodsId { get; set; }
        public int Quantity { get; set; }
        public List<BranchQuantityConditionVModel> BranchQuantityConditions { get; set; }

        public GoodsQuantityConditionVModel()
        {
            BranchQuantityConditions = new List<BranchQuantityConditionVModel>();
        }

    }

    public class BranchQuantityConditionVModel
    {
        public int Id { get; set; }
        public int GoodsQuantityConditionId { get; set; }
        public int GoodsId { get; set; }
        public string BranchName { get; set; }
        public int BranchId { get; set; }
        public int Quantity { get; set; }
    }
}