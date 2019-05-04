﻿using System;
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
        Task<IBusinessResultValue<List<ReportBranchSalesGoal>>> LoadReportBranchSalesGoal(DateTime startDate, DateTime endDate, int goalGoodsCategoryId);
        Task<IBusinessResultValue<List<BranchPromotion>>> LoadReportOverView(int year, int month);
        Task<IBusinessResultValue<List<ReportBranchReceiptGoal>>> LoadReportBranchReceipt(int year, int month, GoalGoodsCategoryTypeEnum goalGoodsCategoryTypeId);
    }
}
