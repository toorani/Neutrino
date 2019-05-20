using Espresso.Core;
using Neutrino.Entities;
using Neutrino.External.Sevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            //load exist postionmapping 
            List<PostionMapping> lstPostionMappings = await unitOfWork.PostionMappingDataService.GetAllAsync();

            //call web service 
            var tupleResultData = await ServiceWrapper.Instance.LoadMembersAsync(startDate, endDate, lstBranches,lstPostionMappings);

            var lstServerMembers = tupleResultData.Item1;
            var lstNewPostionMappings = tupleResultData.Item2;

            //seperation the new data
            var lstExistMembers = await unitOfWork.MemberDataService.GetAllAsync();

            var lstNewMembers = new List<Member>();
            if (lstExistMembers.Count != 0)
            {
                lstServerMembers.AddRange(lstServerMembers.Except(lstExistMembers, x => x.Code));
            }
            else
            {
                lstServerMembers.AddRange(lstServerMembers);
            }

            //insert batch data
            var newDataInserted = await unitOfWork.MemberDataService.InsertBulkAsync(lstNewMembers);
            
            //insert postion mappings
            var newPostionMappingInserted = await unitOfWork.PostionMappingDataService.InsertBulkAsync(lstNewPostionMappings);

            LogInsertedData(lstNewMembers.Count, newDataInserted);
            
            //insert/update data sync status
            await RecordDataSyncStatusAsync(newDataInserted, lstNewMembers.Count);
        }
        #endregion
    }
}
