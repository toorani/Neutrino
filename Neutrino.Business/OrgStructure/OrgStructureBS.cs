
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.Core;
using Espresso.DataAccess.Interfaces;
using Espresso.BusinessService;
using FluentValidation;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Espresso.BusinessService.Interfaces;
using Ninject;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Business
{
    public class OrgStructureBS : NeutrinoBusinessService, IOrgStructureBS
    {
        #region [ Varibale(s) ]
        private readonly AbstractValidator<List<OrgStructure>> validator;
        #endregion

        #region [ Public Property(ies) ]
        [Inject]
        public IEntityListByPagingLoader<OrgStructure> EntityListByPagingLoader { get; set; }
        [Inject]
        public IEntityListLoader<OrgStructure> EntityListLoader { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public OrgStructureBS(NeutrinoUnitOfWork unitOfWork
            , AbstractValidator<List<OrgStructure>> validator)
            :base(unitOfWork)
        {
            this.validator = validator;
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResult> CreateOrEditAsync(List<OrgStructure> lstOrgStructure)
        {
            var result = new BusinessResult();
            try
            {
                var rslt_validator = validator.Validate(lstOrgStructure);
                if (rslt_validator.IsValid == false)
                {
                    result.PopulateValidationErrors(rslt_validator.Errors);
                    result.ReturnStatus = false;
                    return result;
                }
                var positionTypeId = lstOrgStructure.FirstOrDefault().PositionTypeId;
                var lst_ExistsData = await unitOfWork.OrgStructureDataService.GetAsync(x => x.PositionTypeId == positionTypeId);
                var lst_newData = lstOrgStructure.Except(lst_ExistsData, x => x.BranchId);
                
                // add new orgsturctures 
                foreach (var item in lst_newData)
                    unitOfWork.OrgStructureDataService.Insert(item);

                var lst_removeData = lst_ExistsData.Except(lstOrgStructure, x => x.BranchId);

                // delete new orgsturctures 
                foreach (var item in lst_removeData)
                    unitOfWork.OrgStructureDataService.Delete(item);

                await unitOfWork.CommitAsync();
                result.ReturnMessage.Add(MESSAGE_ADD_ENTITY);
                result.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "", "");
            }
            return result;
        }
        public async Task<IBusinessResult> DeleteAsync(int positionTypeId)
        {
            var result = new BusinessResult();
            try
            {
                var lst_ExistsData = await unitOfWork.OrgStructureDataService.GetAsync(x => x.PositionTypeId == (PositionTypeEnum)positionTypeId);
                // delete new orgsturctures 
                foreach (var item in lst_ExistsData)
                    unitOfWork.OrgStructureDataService.Delete(item);

                await unitOfWork.CommitAsync();
                result.ReturnMessage.Add(MESSAGE_DELETE_ENTITY);
                result.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "", "");
            }
            return result;
        }
        #endregion

    }
}
