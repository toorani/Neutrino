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
    class GoodsJob : ServiceJob
    {
        #region [ Varibale(s) ]
        #endregion

        #region [ Override Property(ies) ]
        public override ExternalServices serviceName
        {
            get { return ExternalServices.Goods; }
        }
        #endregion

        #region [ Constructor(s) ]
        public GoodsJob()
            :base()
        {
            
            
        }
        #endregion

        #region [ Override Method(s) ]
        protected override async Task Execute()
        {
            //DoLogServiceStarted();

            // load companies
            var lstCompanies = await unitOfWork.CompanyDataService.GetAsync(where: x => x.RefId != 0);

            //call web service 
            List<Goods> lstEliteData = await ServiceWrapper.Instance.LoadGoodsAsync(lstCompanies);

            //compare & mapping the exist and just loaded data 
            var lstExistGoods = await unitOfWork.GoodsDataService.GetAllAsync();

            List<Goods> lstNewGoods = new List<Goods>();

            //seperation the new data 
            if (lstExistGoods.Count != 0)
            {
                lstNewGoods.AddRange(lstEliteData.Except(lstExistGoods, x => x.RefId));
            }
            else
            {
                lstNewGoods.AddRange(lstEliteData);
            }

            //insert batch data
            var newDataInserted = await unitOfWork.GoodsDataService.InsertBulkAsync(lstNewGoods);

            LogInsertedData(lstEliteData.Count, newDataInserted, lstEliteData.Count - newDataInserted);

            //insert/update data sync status
            await RecordDataSyncStatusAsync(newDataInserted, lstEliteData.Count);


        } 
        #endregion
    }
}
