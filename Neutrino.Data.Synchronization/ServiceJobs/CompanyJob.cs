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
    class CompanyJob : ServiceJob
    {
        #region [ Varibale(s) ]

        #endregion

        #region [ Override Property(ies) ]
        public override ExternalServices serviceName
        {
            get { return ExternalServices.Company; }
        }
        #endregion

        #region [ Constructor(s) ]
        public CompanyJob() : base()
        {

        }
        #endregion

        #region [ Override Method(s) ]
        protected override async Task Execute()
        {
            DateTime endDate = DateTime.Now;
            DateTime? startDate = null;

            //call web service
            List<Company> lstEliteData = await ServiceWrapper.Instance.LoadCompaniesAsync(startDate, endDate);

            //compare & mapping the exist and just loaded data 
            var lst_existCompanies = await unitOfWork.CompanyDataService.GetAsync(where: x => x.RefId != 0);


            //seperation the new data 
            var companies = lstEliteData.Except(lst_existCompanies, x => x.RefId).ToList();

            //insert batch data
            var newDataInserted = await unitOfWork.CompanyDataService.InsertBulkAsync(companies);

            LogInsertedData(lstEliteData.Count, newDataInserted, lstEliteData.Count - newDataInserted);

            //insert/update data sync status
            await RecordDataSyncStatusAsync(newDataInserted, lstEliteData.Count);

        }
        #endregion
    }
}
