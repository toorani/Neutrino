using Espresso.Core;
using Neutrino.Entities;
using Neutrino.External.Sevices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Data.Synchronization.ServiceJobs
{
    class DepartmentJob : ServiceJob
    {
        #region [ Varibale(s) ]

        #endregion

        #region [ Override Property(ies) ]
        public override ExternalServices serviceName
        {
            get { return ExternalServices.Department; }
        }
        #endregion

        #region [ Constructor(s) ]
        public DepartmentJob() : base()
        {

        }
        #endregion

        #region [ Override Method(s) ]
        protected override async Task Execute()
        {
            //call web service
            List<Department> lstEliteData = await ServiceWrapper.Instance.LoadDepartmentInfo();

            //compare & mapping the exist and just loaded data 
            var lst_existData = await unitOfWork.DepartmentDataService.GetAsync(where: x => x.RefId != 0);


            //seperation the new data 
            var lst_newData = lstEliteData.Except(lst_existData, x => x.RefId).ToList();

            //insert batch data
            var newDataInserted = await unitOfWork.DepartmentDataService.InsertBulkAsync(lst_newData);

            LogInsertedData(lstEliteData.Count, newDataInserted, lstEliteData.Count - newDataInserted);

            //insert/update data sync status
            await RecordDataSyncStatusAsync(newDataInserted, lstEliteData.Count);

        }
        #endregion
    }
}
