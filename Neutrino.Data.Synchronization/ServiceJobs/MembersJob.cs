using System;
using System.Collections.Generic;
using System.Linq;
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
    class MembersJob : ServiceJob
    {
        #region [ Varibale(s) ]
        #endregion

        #region [ Override Property(ies) ]
        public override ExternalServices serviceName
        {
            get { return ExternalServices.Members; }
        }
        #endregion

        #region [ Constructor(s) ]
        public MembersJob() :base()
        {
        }
        #endregion

        #region [ Override Method(s) ]
       
        protected override async Task Execute()
        {
            DateTime? startDate = null;
            DateTime endDate = DateTime.Now;

            // load exist branches
            var lstBranches = await unitOfWork.BranchDataService.GetAsync(where: x => x.RefId != 0);


            //call web service 
            var lstEliteData = await ServiceWrapper.Instance.LoadMembersAsync(startDate, endDate, lstBranches);

            //seperation the new data
            var lstExistMembers = await unitOfWork.MemberDataService.GetAllAsync();

            var lstNewMembers = new List<Member>();
            if (lstExistMembers.Count != 0)
            {
                lstNewMembers.AddRange(lstEliteData.Except(lstExistMembers, x => x.Code));
            }
            else
            {
                lstNewMembers.AddRange(lstEliteData);
            }

            //insert batch data
            var newDataInserted = await unitOfWork.MemberDataService.InsertBulkAsync(lstNewMembers);

            LogInsertedData(lstEliteData.Count, newDataInserted);
            
            //insert/update data sync status
            await RecordDataSyncStatusAsync(newDataInserted, lstEliteData.Count);
        }
        #endregion
    }
}
