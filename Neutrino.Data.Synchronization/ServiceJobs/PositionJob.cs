using Espresso.Core;
using Neutrino.Entities;
using Neutrino.External.Sevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Data.Synchronization.ServiceJobs
{
    class PositionJob : ServiceJob
    {
        #region [ Varibale(s) ]

        #endregion

        #region [ Override Property(ies) ]
        public override ExternalServices serviceName
        {
            get { return ExternalServices.Position; }
        }
        #endregion

        #region [ Constructor(s) ]
        public PositionJob() : base()
        {

        }
        #endregion

        #region [ Override Method(s) ]
        protected override async Task Execute()
        {
            //call web service
            List<ElitePosition> lstEliteData = await ServiceWrapper.Instance.LoadPositionInfo();

            //compare & mapping the exist and just loaded data 
            var lst_existData = await unitOfWork.ElitePositionDataService.GetAsync(where: x => x.RefId != 0);


            //seperation the new data 
            var lst_newData = lstEliteData.Except(lst_existData, x => x.RefId).ToList();

            //insert batch data
            var newDataInserted = await unitOfWork.ElitePositionDataService.InsertBulkAsync(lst_newData);

            LogInsertedData(lstEliteData.Count, newDataInserted, lstEliteData.Count - newDataInserted);

            //insert/update data sync status
            await RecordDataSyncStatusAsync(newDataInserted, lstEliteData.Count);

        }
        #endregion
    }
}
