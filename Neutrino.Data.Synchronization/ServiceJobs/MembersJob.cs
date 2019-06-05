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
        public MembersJob() : base()
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
            List<PositionMapping> lstPostionMappings = await unitOfWork.PositionMappingDataService.GetAllAsync();

            //load exist postionmapping 
            List<Department> lstDepartments = await unitOfWork.DepartmentDataService.GetAllAsync();

            //call web service 
            var lstServerMembers = await ServiceWrapper.Instance.LoadMembersAsync(startDate, endDate, lstBranches);



            //seperation the new data
            var lstExistMembers = await unitOfWork.MemberDataService.GetAllAsync();

            lstExistMembers.ForEach(entityToUpdate =>
            {
                var serverEntity = lstServerMembers.FirstOrDefault(en => en.Code == entityToUpdate.Code);
                if (serverEntity != null)
                {
                    var postionMapping = lstPostionMappings.FirstOrDefault(pm => pm.PositionRefId == serverEntity.PositionRefId);
                    if (postionMapping != null)
                    {
                        entityToUpdate.PositionTypeId = postionMapping.PositionTypeId.Value;
                    }
                    else
                    {
                        entityToUpdate.PositionTypeId = null;
                        logger.Warn(serviceName, $"could not be mapped position with Id : {serverEntity.PositionRefId}");
                    }

                    var department = lstDepartments.FirstOrDefault(pm => pm.RefId == serverEntity.DepartmentRefId);
                    if (department != null)
                    {
                        entityToUpdate.DepartmentId = department.Id;
                    }
                    else
                    {
                        logger.Warn(serviceName, $"could not be found department with Id :{serverEntity.DepartmentRefId}");
                    }
                    entityToUpdate.PositionRefId = serverEntity.PositionRefId;
                    entityToUpdate.DepartmentRefId = serverEntity.DepartmentRefId;
                    unitOfWork.MemberDataService.Update(entityToUpdate);
                }
                else
                {
                    unitOfWork.MemberDataService.Delete(entityToUpdate);
                }
                
            });


            await unitOfWork.CommitAsync();

            var lstNewMembers = new List<Member>();
            if (lstExistMembers.Count != 0)
            {
                lstNewMembers.AddRange(lstServerMembers.Except(lstExistMembers, x => x.Code));
            }
            else
            {
                lstNewMembers.AddRange(lstServerMembers);
            }

            //insert batch data
            var newDataInserted = await unitOfWork.MemberDataService.InsertBulkAsync(lstNewMembers);

            LogInsertedData(lstNewMembers.Count, newDataInserted);

            //insert/update data sync status
            await RecordDataSyncStatusAsync(newDataInserted, lstNewMembers.Count);
        }
        #endregion
    }

    
}
