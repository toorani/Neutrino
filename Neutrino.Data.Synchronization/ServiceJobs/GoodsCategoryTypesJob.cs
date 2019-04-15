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
using Espresso.BusinessService.Interfaces;
using Espresso.Core.Ninject;

namespace Neutrino.Data.Synchronization.ServiceJobs
{
    class GoodsCategoryTypesJob : ServiceJob
    {
        #region [ Varibale(s) ]
        
        #endregion

        #region [ Override Property(ies) ]
        public override ExternalServices serviceName
        {
            get { return ExternalServices.GoodsCatType; }
        }
        #endregion

        #region [ Constructor(s) ]
        public GoodsCategoryTypesJob():base()
        {
        }
        #endregion

        #region [ Override Method(s) ]
        protected override async Task Execute()
        {
            //call web service
            List<GoodsCategoryType> lstEliteData = await ServiceWrapper.Instance.LoadGoodsCategoryTypesAsync();

            //compare & mapping the exist and just loaded data 
            var existData = await unitOfWork.GoodsCategoryTypeDataService.GetAllAsync();


            //seperation the new data 
            var newData = lstEliteData.Except(existData, x => x.RefId).ToList();

            //insert batch data
            var newDataInserted = await unitOfWork.GoodsCategoryTypeDataService.InsertBulkAsync(newData);

            LogInsertedData(lstEliteData.Count, newDataInserted, lstEliteData.Count - newDataInserted);

            //insert/update data sync status
            await RecordDataSyncStatusAsync(newDataInserted, lstEliteData.Count);
        }
        #endregion
    }
}
