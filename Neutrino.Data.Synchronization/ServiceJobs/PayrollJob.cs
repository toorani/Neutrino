using Neutrino.Entities;
using Neutrino.External.Sevices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Data.Synchronization.ServiceJobs
{
    class PayrollJob : ServiceJob
    {
        #region [ Varibale(s) ]
        #endregion

        #region [ Override Property(ies) ]
        public override ExternalServices serviceName
        {
            get { return ExternalServices.Payroll; }
        }
        #endregion

        #region [ Constructor(s) ]
        public PayrollJob() : base()
        {
        }
        #endregion

        #region [ Override Method(s) ]
        protected override async Task<YearMonth> GetLatestYearMonthAsync()
        {
            var result = new YearMonth();

            //get the latest year and month 
            var lastestYearMonth = await unitOfWork.MemberPayrollDataService.GetLatestYearMonthAsync();
            
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

            //load the members records to mapping data
            var lstMembers = await unitOfWork.MemberDataService.GetAllAsync();

            //call web service
            var lstEliteData = await ServiceWrapper.Instance.LoadPayrollsAsync(yearMonth.Year
                , yearMonth.Month
                , lstMembers);

            //insert batch data
            var newDataInserted = await unitOfWork.MemberPayrollDataService.InsertBulkAsync(lstEliteData);

            LogInsertedData(lstEliteData.Count, newDataInserted);
            
            //insert/update data sync status
            await RecordDataSyncStatusAsync(newDataInserted, lstEliteData.Count);
            
        }
        private async Task ReloadNotCompleteDataAsync()
        {
            //get the list of month and year which those saved in MemberPayroll table.
            var lstMonthYear = await unitOfWork.MemberPayrollDataService.GetMonthYearListAsync();

            //specify the range data for reloading  
            NotCompleteRecord notCompleteRecord = await SetNotCompleteRangeTime(
                lstMonthYear.Select(x => new YearMonth() { Month = x.Month, Year = x.Year }).ToList());


            if (notCompleteRecord.ExistAnyRecord)
            {
                //load the members records to mapping data
                var lstMembers = await unitOfWork.MemberDataService.GetAllAsync();
                
                //call web service
                var lstEliteData = await ServiceWrapper.Instance.LoadPayrollsAsync(yearMonth.Year
                    , yearMonth.Month
                    , lstMembers);

                //compare & mapping the exist and just loaded data 
                var lstExistData = await unitOfWork.MemberPayrollDataService.GetAsync(x=>x.Year ==  yearMonth.Year && x.Month == yearMonth.Month);

                var lstNewData = new List<MemberPayroll>();

                //seperation the new data 
                lstEliteData
                    .Where(x => lstMembers.Any(y => y.RefId == x.MemberRefId))
                    .ToList()
                    .ForEach(x =>
                    {
                        if (!lstExistData.Any(y => y.MemberRefId == x.MemberRefId))
                            lstNewData.Add(x);
                    });

                //checking the new data with the branch data  
                lstEliteData
                    .Where(x => !lstMembers.Any(y => y.RefId == x.MemberRefId))
                    .Select(x => x.MemberRefId)
                    .Distinct()
                    .ToList()
                    .ForEach(x =>
                    {
                        logger.Warn(serviceName, "there isn't any member with following data : MemberRefId={0}--Year={1}--Month={2}"
                            , x, yearMonth.Year, yearMonth.Month);
                    });


                //insert batch data
                var newDataInserted = await unitOfWork.MemberPayrollDataService.InsertBulkAsync(lstNewData);

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
