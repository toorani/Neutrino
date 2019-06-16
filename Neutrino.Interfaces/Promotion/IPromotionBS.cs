using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IPromotionBS : IBusinessService
        ,IEnabledEntityListByPagingLoader<Promotion>
        ,IEnabledEntityLoader<Promotion>
        
    {
        Task<IBusinessResultValue<Promotion>> AddPromotionAsync(Promotion entity);
        Task<IBusinessResult> PutInProcessQueueAsync(int year,int month);
        Task<IBusinessResult> CalculateGoalsAsync(Promotion entity);
        Task<IBusinessResult> CalculateSalesGoalsAsync(Promotion entity);
        Task<IBusinessResult> CalculateReceiptGoalsAsync(Promotion entity);
        Task<IBusinessResultValue<List<ReportBranchSalesGoal>>> LoadReport_Amount_Quantities_Goal(DateTime startDate, DateTime endDate, int goalGoodsCategoryId);
        Task<IBusinessResultValue<List<ReportBranchSalesGoal>>> LoadReport_Percent_Goal(DateTime startDate, DateTime endDate, int goalGoodsCategoryId);
        Task<IBusinessResultValue<List<ReportBranchPromotionOverview>>> LoadReportOverView(int year, int month);
        Task<IBusinessResultValue<List<ReportBranchReceiptGoal>>> LoadReportBranchReceipt(int yar, int month, GoalGoodsCategoryTypeEnum goalGoodsCategoryTypeId);
        Task<IBusinessResultValue<List<ReportBranchPromotionDetail>>> LoadReportBranchPromotionDetail(DateTime startDate, DateTime endDate);
        Task<IBusinessResultValue<List<ReportSellerGoal>>> LoadReport_Seller_Goal(DateTime startDate, DateTime endDate, int goalGoodsCategoryId);
        Task<IBusinessResultValue<BranchPromotion>> LoadActiveBranchPromotionDetail(int branchId);
        Task<IBusinessResultValue<List<BranchPromotion>>> LoadBranchPromotions(PromotionReviewStatusEnum promotionReviewStatusId);
        Task<IBusinessResultValue<BranchPromotion>> LoadBranchPromotion(int branchId, PromotionReviewStatusEnum statusId);
    }
}
