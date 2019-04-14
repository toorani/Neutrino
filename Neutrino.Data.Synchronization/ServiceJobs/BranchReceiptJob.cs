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
    /// <summary>
    /// اطلاعات وصول مراکز
    /// </summary>
    class BranchReceiptJob : ServiceJob
    {
        #region [ Varibale(s) ]
        #endregion

        #region [ Protected Property(ies) ]
        public override ExternalServices serviceName
        {
            get
            {
                return ExternalServices.BranchReceipts;
            }
        }
        #endregion

        #region [ Constructor(s) ]
        public BranchReceiptJob() : base()
        {
        }
        #endregion

        #region [ Protected Method(s) ]
        protected override async Task<YearMonth> GetLatestYearMonthAsync()
        {
            var result = new YearMonth();

            //get the latest year and month 
            var lastestYearMonth = await unitOfWork.BranchReceiptDataService.GetLatestYearMonthAsync();

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
            //DoLogServiceStarted();

            // load branches
            var lstBranches = await unitOfWork.BranchDataService.GetAsync(where: x => x.RefId != 0);
            //call web service
            var lstEliteData = await ServiceWrapper.Instance.LoadBranchReceiptsAsync(yearMonth.Year
                , yearMonth.Month, lstBranches);

            //insert batch data
            var newDataInserted = await unitOfWork.BranchReceiptDataService.InsertBulkAsync(lstEliteData);

            LogInsertedData(lstEliteData.Count, newDataInserted);


            //insert/update data sync status
            await RecordDataSyncStatusAsync(newDataInserted, lstEliteData.Count);

        }
        private async Task ReloadNotCompleteDataAsync()
        {
            //get the list of month and year which those saved in BranchReceipt table.
            var lstMonthYear = await unitOfWork.BranchReceiptDataService.GetMonthYearListAsync();

            //specify the range data for reloading  
            NotCompleteRecord notCompleteRecord = await SetNotCompleteRangeTime(
                lstMonthYear.Select(x => new YearMonth() { Month = x.Month, Year = x.Year }).ToList());


            if (notCompleteRecord.ExistAnyRecord)
            {
                //DoLogServiceStarted();

                // load branches
                var lstBranches = await unitOfWork.BranchDataService.GetAsync(where: x => x.RefId != 0);

                //call web service
                var lstEliteData = await ServiceWrapper.Instance.LoadBranchReceiptsAsync(yearMonth.Year
                    , yearMonth.Month, lstBranches);

                //compare & mapping the exist and just loaded data 
                var lstExistData = await unitOfWork.BranchReceiptDataService.GetAsync(x => x.Year == yearMonth.Year && x.Month == yearMonth.Month);
                
                var lstNewData = new List<BranchReceipt>();

                //seperation the new data 
                lstEliteData
                    .Where(x => lstBranches.Any(y => y.RefId == x.BranchRefId))
                    .ToList()
                    .ForEach(x =>
                    {
                        if (!lstExistData.Any(y => y.BranchRefId == x.BranchRefId))
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
                            , x, yearMonth.Year, yearMonth.Month);
                    });

                //insert batch data
                var newDataInserted = await unitOfWork.BranchReceiptDataService.InsertBulkAsync(lstNewData);

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
