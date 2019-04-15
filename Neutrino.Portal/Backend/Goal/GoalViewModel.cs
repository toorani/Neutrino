using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Portal;
using Neutrino.Entities;
using Espresso.Core;
using AutoMapper.Configuration.Conventions;
using Espresso.Portal.Attributes;

namespace Neutrino.Portal.Models
{
    public class GoalViewModel : ViewModelBase
    {
        #region [ Public Property(ies) ]
        public int GoalTypeId { get; set; }
        public CompanyViewModel Company { get; set; }
        public int GoalGoodsCategoryId { get; set; }
        public GoalGoodsCategoryViewModel GoalGoodsCategory { get; set; }
        public int GoalGoodsCategoryTypeId { get; set; }
        public TypeEntityViewModel GoalGoodsCategoryType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool IsUsed { get; set; }
        public List<GoodsViewModel> GoodsSelectionList { get; set; }
        public List<GoalStepViewModel> GoalSteps { get; set; }
        public List<FulfillmentPromotionConditionViewModel> TotalSalesGoalRanges { get; set; }
        public int ComputingTypeId { get; set; }
        public int? ApprovePromotionTypeId { get; set; }
        public decimal? Amount { get; set; }
        /// <summary>
        /// سال هدف 
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// ماه هدف
        /// </summary>
        public int Month { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public GoalViewModel()
        {
            GoalSteps = new List<GoalStepViewModel>();
            GoodsSelectionList = new List<GoodsViewModel>();
            TotalSalesGoalRanges = new List<FulfillmentPromotionConditionViewModel>();
            IsUsed = false;
            ApprovePromotionTypeId = 1;
        }
        #endregion


    }
}