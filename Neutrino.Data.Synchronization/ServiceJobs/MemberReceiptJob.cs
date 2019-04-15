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
using Espresso.Core.Ninject;
using Espresso.BusinessService.Interfaces;

namespace Neutrino.Data.Synchronization.ServiceJobs
{
    class MemberReceiptJob : ServiceJob
    {
        #region [ Varibale(s) ]
        private readonly IMemberReceiptBS businessService;
        private readonly IEntityListLoader<Member> memberListLoader;
        #endregion

        #region [ Protected Property(ies) ]
        protected override ExternalServices serviceName
        {
            get
            {
                return ExternalServices.MemberReceipts;
            }
        }
        #endregion

        #region [ Constructor(s) ]
        public MemberReceiptJob() : base()
        {
            businessService = NinjectContainer.Resolve<IMemberReceiptBS>();
            memberListLoader = NinjectContainer.Resolve<IEntityListLoader<Member>>();
        }
        #endregion

        #region [ Protected Method(s) ]
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
                DoLogException(ex);
            }
        }
        #endregion

        #region [ Private Method(s) ]
        private async Task AcquireDataAsync()
        {
            //get the latest year and month in MemberReceipt table
            var lastestYearMonth = await businessService.LoadLatestYearMonthAsync();

            if (!lastestYearMonth.ReturnStatus)
            {
                logger.exError(serviceName, lastestYearMonth.ReturnMessage.ConcatAll());
                return;
            }

            //manipulate the year and month 
            if (lastestYearMonth.ResultValue != null)
            {
                externalData.Month = lastestYearMonth.ResultValue.Month;
                externalData.Year = lastestYearMonth.ResultValue.Year;

                if (SetNextRangeTime() == false)
                    return;
            }

            //DoLogServiceStarted();

            //load the members records to mapping data
            var lstMembers = await memberListLoader.LoadListAsync();

            if (!lstMembers.ReturnStatus)
            {
                logger.exError(serviceName, lstMembers.ReturnMessage.ConcatAll());
                return;
            }

            //call web service
            var lstEliteData = await ServiceWrapper.Instance.LoadMemberReceiptsAsync(externalData.Year
                , externalData.Month
                , lstMembers.ResultValue);

            //insert batch data
            var newDataInserted = await businessService.AddBatchAsync(lstEliteData);

            //check result of the inserted data
            if (newDataInserted.ReturnStatus)
            {
                DoLogInsertedData(lstEliteData.Count, newDataInserted.ResultValue);
            }
            else
            {
                logger.exError(serviceName, newDataInserted.ReturnMessage.ConcatAll());
            }

            //insert/update data sync status
            await RecordDataSyncStatusAsync(newDataInserted.ResultValue, lstEliteData.Count);

            //
            //DoLogServiceCompleted();
        }
        private async Task ReloadNotCompleteDataAsync(ExternalData externalData)
        {
            //get the list of month and year which those saved in BranchReceipt table.
            var lstMonthYear = await businessService.LoadMonthYearListAsync();

            if (!lstMonthYear.ReturnStatus)
            {
                logger.exError(serviceName, lstMonthYear.ReturnMessage.ConcatAll());
                return;
            }

            //specify the range data for reloading  
            NotCompleteRecord notCompleteRecord = await SetNotCompleteRangeTime(
                lstMonthYear.ResultValue.Select(x => new YearMonth() { Month = x.Month, Year = x.Year }).ToList());


            if (notCompleteRecord.ExistAnyRecord)
            {
                //DoLogServiceStarted();

                //load the members records to mapping data
                var lstMembers = await memberListLoader.LoadAllAsync();

                if (!lstMembers.ReturnStatus)
                {
                    logger.exError(serviceName, lstMembers.ReturnMessage.ConcatAll());
                    return;
                }

                //call web service
                var lstEliteData = await ServiceWrapper.Instance.LoadMemberReceiptsAsync(externalData.Year
                    , externalData.Month
                    , lstMembers.ResultValue);

                //compare & mapping the exist and just loaded data 
                var lstExistData = await businessService.LoadListAsync(externalData.Year, externalData.Month);

                if (!lstExistData.ReturnStatus)
                {
                    logger.exError(serviceName, lstExistData.ReturnMessage.ConcatAll());
                    return;
                }

                var lstNewData = new List<MemberReceipt>();

                //seperation the new data 
                lstEliteData
                    .Where(x => lstMembers.ResultValue.Any(y => y.RefId == x.MemberRefId))
                    .ToList()
                    .ForEach(x =>
                    {
                        if (!lstExistData.ResultValue.Any(y => y.MemberRefId == x.MemberRefId))
                            lstNewData.Add(x);
                    });

                //checking the new data with the branch data  
                lstEliteData
                    .Where(x => !lstMembers.ResultValue.Any(y => y.RefId == x.MemberRefId))
                    .Select(x => x.MemberRefId)
                    .Distinct()
                    .ToList()
                    .ForEach(x =>
                    {
                        logger.exError(serviceName, "there isn't any member with following data : MemberRefId={0}--Year={1}--Month={2}"
                            , x, externalData.Year, externalData.Month);
                    });


                //insert batch data
                var newDataInserted = await businessService.AddBatchAsync(lstNewData);

                //check result of the inserted data
                if (newDataInserted.ReturnStatus)
                {
                    DoLogInsertedData(lstEliteData.Count, newDataInserted.ResultValue);
                }
                else
                {
                    logger.exError(serviceName, newDataInserted.ReturnMessage.ConcatAll());
                }

                //insert/update data sync status
                await RecordDataSyncStatusAsync(newDataInserted.ResultValue, lstEliteData.Count, notCompleteRecord.DataSyncStatus);

                //DoLogServiceCompleted();

            }
            else
            {
                DoLogNotDataForReloading();
            }

        }
        #endregion
    }
}
