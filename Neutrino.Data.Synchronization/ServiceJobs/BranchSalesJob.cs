using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Core;
using Neutrino.Business;
using Neutrino.Entities;
using Neutrino.External.Sevices;
using Neutrino.Core;

using Quartz;
using Neutrino.Interfaces;
using Espresso.BusinessService.Interfaces;
using Espresso.Core.Ninject;

namespace Neutrino.Data.Synchronization.ServiceJobs
{
    class BranchSalesJob : ServiceJob
    {
        #region [ Varibale(s) ]

        #endregion

        #region [ Protected Property(ies) ]
        public override ExternalServices serviceName
        {
            get
            {
                return ExternalServices.BranchSales;
            }
        }
        #endregion


        #region [ Constructor(s) ]
        public BranchSalesJob() : base()
        {
        }
        #endregion

        #region [ Override Method(s) ]
        protected override async Task<YearMonth> GetLatestYearMonthAsync()
        {
            var result = new YearMonth();

            //get the latest year and month 
            var lastestYearMonth = await unitOfWork.BranchSalesDataService.GetLatestYearMonthAsync();
            result.Month = lastestYearMonth.Month;
            result.Year = lastestYearMonth.Year;
            return result;
        }
        protected override async Task Execute()
        {
            if (jobCallTypes == JobCallTypes.AcquireData)
            {
                await AcquireDataAsync();
            }
            else if (jobCallTypes == JobCallTypes.CheckNotCompleted)
            {
                await ReloadNotCompleteDataAsync();
            }
        }
        #endregion

        #region [ Private Method(s) ]
        private async Task AcquireDataAsync()
        {
            //load branches
            var lstBranches = await unitOfWork.BranchDataService.GetAllAsync();

            //load goods
            var lstGoods = await unitOfWork.GoodsDataService.GetAllAsync();

            //call web service
            var lstEliteData = await ServiceWrapper.Instance.LoadBranchSalesAsync(yearMonth.Year
                , yearMonth.Month
                , lstGoods
                , lstBranches);

            //insert batch data
            var newDataInserted = await unitOfWork.BranchSalesDataService.InsertBulkAsync(lstEliteData.Where(x => x.BranchId != 0 && x.GoodsId != 0).ToList());

            LogInsertedData(lstEliteData.Count, newDataInserted);
            
            //insert/update data sync status
            await RecordDataSyncStatusAsync(newDataInserted, lstEliteData.Count);

        }
        private async Task ReloadNotCompleteDataAsync()
        {

            var result_lstMonthYear = await unitOfWork.BranchSalesDataService.GetMonthYearListAsync();

            //specify the range data for reloading  
            NotCompleteRecord notCompleteRecord = await SetNotCompleteRangeTime(
                result_lstMonthYear.Select(x => new YearMonth() { Month = x.Month, Year = x.Year }).ToList());


            if (notCompleteRecord.ExistAnyRecord)
            {

                //load branches
                var lstBranches = await unitOfWork.BranchDataService.GetAsync(where : null);
                
                //load goods
                var lstGoods = await unitOfWork.GoodsDataService.GetAsync(where: null);

                //call web service
                var lstEliteData = await ServiceWrapper.Instance.LoadBranchSalesAsync(yearMonth.Year
                    , yearMonth.Month
                    , lstGoods
                    , lstBranches);

                //compare & mapping the exist and just loaded data 
                var lstExistData = await unitOfWork.BranchSalesDataService.GetAsync(x=>x.Year == yearMonth.Year && x.Month == yearMonth.Month);

                var lstNewData = new List<BranchSales>();
                //seperation the new data 
                lstEliteData
                  .Where(x =>
                  lstBranches.Any(y => y.RefId == x.BranchRefId)
                  && lstGoods.Any(y => y.RefId == x.GoodsRefId))
                  .ToList()
                  .ForEach(x =>
                  {
                      if (!lstExistData.Any(y => y.BranchRefId == x.BranchRefId && x.GoodsRefId == y.GoodsRefId))
                          lstNewData.Add(x);
                  });

                //checking the new data with the branch data  
                lstEliteData
                    .Where(x => !lstBranches.Any(y => y.RefId == x.BranchRefId))
                    .Select(x => x.BranchRefId)
                    .Distinct()
                    .ToList()
                    .ForEach(x =>
                    {
                        logger.Warn(serviceName, "there isn't any branch with following data : BranchRefId={0}--Year={1}--Month={2}"
                            , x
                            , yearMonth.Year
                            , yearMonth.Month);
                    });

                lstEliteData
                    .Where(x => !lstGoods.Any(y => y.RefId == x.GoodsRefId))
                    .Select(x => x.GoodsRefId)
                    .Distinct()
                    .ToList()
                    .ForEach(x =>
                    {
                        logger.Warn(serviceName, "there isn't any goods with following data :GoodsRefId={0}--Year={1}--Month={2}."
                        , x
                        , yearMonth.Year
                        , yearMonth.Month);
                    });


                //insert batch data
                var newDataInserted = await unitOfWork.BranchSalesDataService.InsertBulkAsync(lstNewData);

                LogInsertedData(lstEliteData.Count, newDataInserted);

                //insert/update data sync status
                await RecordDataSyncStatusAsync(newDataInserted, lstEliteData.Count, notCompleteRecord.DataSyncStatus);
            }
            else
            {
                LogNotDataForReloading();
            }

        }
        #endregion

    }
}
