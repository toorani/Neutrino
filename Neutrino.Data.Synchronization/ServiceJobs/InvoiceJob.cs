using System;
using System.Linq;
using System.Threading.Tasks;
using Espresso.Core;
using Neutrino.Entities;
using Neutrino.External.Sevices;
using Neutrino.Core;

using Neutrino.Interfaces;
using Espresso.BusinessService.Interfaces;
using Espresso.Core.Ninject;
using System.Collections.Generic;

namespace Neutrino.Data.Synchronization.ServiceJobs
{
    class InvoiceJob : ServiceJob
    {

        #region [ Varibale(s) ]
        #endregion

        #region [ Override Property(ies) ]
        public override ExternalServices serviceName
        {
            get { return ExternalServices.Invoice; }
        }
        #endregion

        #region [ Constructor(s) ]
        public InvoiceJob() : base()
        {
        }
        #endregion

        #region [ Override Method(s) ]
        protected override async Task<YearMonth> GetLatestYearMonthAsync()
        {
            var result = new YearMonth();

            //get the latest year and month 
            var lastestYearMonth = await unitOfWork.InvoiceDataService.GetLatestYearMonthAsync();

            result.Month = lastestYearMonth.Month;
            result.Year = lastestYearMonth.Year;
            return result;
        }
        protected override async Task Execute()
        {
            try
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
            catch (Exception ex)
            {
                LogException(ex);
            }


        }
        #endregion

        #region [ Private Method(s) ]
        private async Task AcquireDataAsync()
        {
            //load the members records to mapping data
            var lstMembers = await unitOfWork.MemberDataService.GetAllAsync();

            //load the goods records to mapping data
            var lstGoods = await unitOfWork.GoodsDataService.GetAllAsync();


            //call web service
            var lstEliteData = await ServiceWrapper.Instance.LoadInvoicesAsync(yearMonth.Year, yearMonth.Month
                , lstGoods
                , lstMembers);
            
            var validData = lstEliteData.Where(x => x.SellerId != 0 && x.GoodsId != 0).ToList();
            //insert batch data
            var newDataInserted = await unitOfWork.InvoiceDataService.InsertBulkAsync(validData);

            LogInsertedData(lstEliteData.Count, newDataInserted);
            
            //insert/update data sync status
            await RecordDataSyncStatusAsync(newDataInserted, lstEliteData.Count);

        }
        private async Task ReloadNotCompleteDataAsync()
        {
            //get the list of month and year which those saved in Invoice table.
            var lstMonthYear = await unitOfWork.InvoiceDataService.GetMonthYearListAsync();

            //specify the range data for reloading  
            NotCompleteRecord notCompleteRecord = await SetNotCompleteRangeTime(
                lstMonthYear.Select(x => new YearMonth() { Month = x.Month, Year = x.Year }).ToList());


            if (notCompleteRecord.ExistAnyRecord)
            {
                //DoLogServiceStarted();

                //load the members records to mapping data
                var lstMembers = await unitOfWork.MemberDataService.GetAllAsync();

                //load the goods records to mapping data
                var lstGoods = await unitOfWork.GoodsDataService.GetAllAsync();

                //call web service
                var lstEliteData = await ServiceWrapper.Instance.LoadInvoicesAsync(yearMonth.Year
                , yearMonth.Month
                , lstGoods
                , lstMembers);

                //compare & mapping the exist and just loaded data 
                var lstExistData = await unitOfWork.InvoiceDataService.GetAsync(where: x => x.Year == yearMonth.Year && x.Month == yearMonth.Month);
                
                var lstNewData = new List<Invoice>();
                //seperation the new data 
                lstEliteData
                    .Where(x =>
                    lstMembers.Any(y => y.RefId == x.SellerRefId)
                    && lstGoods.Any(y => y.RefId == x.GoodsRefId))
                    .ToList()
                    .ForEach(x =>
                    {
                        if (!lstExistData.Any(y => y.GoodsRefId == x.GoodsRefId && x.SellerRefId == y.SellerRefId))
                            lstNewData.Add(x);
                    });

                //checking the new data 
                lstEliteData
                    .Where(x => !lstMembers.Any(y => y.RefId == x.SellerRefId))
                    .Select(x => x.SellerRefId)
                    .Distinct()
                    .ToList()
                    .ForEach(x =>
                    {
                        logger.Warn(serviceName, "there isn't any seller with following data : SellerRefId={0}--Year={1}--Month={2} not found."
                        , x
                        , yearMonth.Year
                        , yearMonth.Month);
                    });



                //insert batch data
                var newDataInserted = await unitOfWork.InvoiceDataService.InsertBulkAsync(lstNewData);

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
