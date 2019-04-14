using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Espresso.Core;
using Neutrino.Entities;
using Neutrino.External.Sevices;
using Espresso.BusinessService.Interfaces;
using Espresso.Core.Ninject;

namespace Neutrino.Data.Synchronization.ServiceJobs
{
    /// <summary>
    /// اطلاعات شعبات و مراکز پخش الیت را مشخص مینماید
    /// </summary>
    class BranchJob : ServiceJob
    {
        #region [ Varibale(s) ]
        
        #endregion

        #region [ Protected Property(ies) ]
        public override ExternalServices serviceName { get { return ExternalServices.Branch; } }
        #endregion

        #region [ Constructor(s) ]
        public BranchJob() : base()
        {
            
        }
        #endregion

        #region [ Protected Method(s) ]
        protected override async Task Execute()
        {
            //call web service
            List<Branch> lstEliteData = await ServiceWrapper.Instance.LoadBranchesAsync();

            //load exist data
            var lst_existData = await unitOfWork.BranchDataService.GetAsync(where: x => x.RefId != 0);

            //seperation the new data
            var newData = lstEliteData.Except(lst_existData, x => x.RefId).ToList();

            //insert batch data
            var newDataInserted = await unitOfWork.BranchDataService.InsertBulkAsync(newData);

            LogInsertedData(lstEliteData.Count, newDataInserted, lstEliteData.Count - newDataInserted);
            
            //insert/update data sync status
            await RecordDataSyncStatusAsync(newDataInserted, lstEliteData.Count, false);

        }
        #endregion


    }
}
