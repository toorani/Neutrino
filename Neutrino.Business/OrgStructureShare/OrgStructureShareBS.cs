
using System;
using System.Linq;
using System.Threading.Tasks;
using Espresso.BusinessService;
using FluentValidation;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System.Data.Entity.Validation;
using FluentValidation.Results;
using Espresso.BusinessService.Interfaces;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Business
{
    public class OrgStructureShareBS : NeutrinoBusinessService, IOrgStructureShareBS
    {
        #region [ Varibale(s) ]
        private readonly AbstractValidator<OrgStructureShareDTO> valdiator;
        #endregion

        #region [ Constructor(s) ]
        public OrgStructureShareBS(NeutrinoUnitOfWork unitOfWork
            , AbstractValidator<OrgStructureShareDTO> valdiator)
            : base(unitOfWork)
        {
            this.valdiator = valdiator;
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessLoadByPagingResult<OrgStructureShareDTO>> LoadOrgStructureShareDTOByPagingAsync(string brandName
            , Func<IQueryable<OrgStructureShareDTO>, IOrderedQueryable<OrgStructureShareDTO>> orderBy
            , int pageNumber = 0
            , int pageSize = 15)
        {
            var result = new BusinessLoadByPagingResult<OrgStructureShareDTO>();
            try
            {
                var queryResult = await unitOfWork.OrgStructureShareDataService.GetOrgStructureShareDTOByPagingAsync(brandName, orderBy, pageNumber, pageSize);
                result.ResultValue = queryResult.Item1;
                result.TotalRows = queryResult.Item2;
                result.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }

            return result;
        }
        public async Task<IBusinessResultValue<OrgStructureShareDTO>> LoadOrgStructureShareDTOAsync(int branchId)
        {
            var result = new BusinessResultValue<OrgStructureShareDTO>();
            try
            {
                result.ResultValue = await unitOfWork.OrgStructureShareDataService.GetOrgStructureShareDTOAsync(branchId);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }

            return result;
        }
        public async Task<IBusinessResultValue<OrgStructureShareDTO>> CreateOrgStructureShareDTOAsync(OrgStructureShareDTO personelShareDTO)
        {
            var result = new BusinessResultValue<OrgStructureShareDTO>();
            try
            {
                ValidationResult validationResult = valdiator.Validate(personelShareDTO);
                if (validationResult.IsValid == false)
                {
                    result.PopulateValidationErrors(validationResult.Errors);
                    return result;
                }
                personelShareDTO.Items
                    .Where(x => x.PrivateReceiptPercent.HasValue || x.SalesPercent.HasValue || x.TotalReceiptPercent.HasValue
                    || x.MinimumPromotion.HasValue || x.MaximumPromotion.HasValue)
                    .ToList()
                    .ForEach(x =>
                    {
                        x.BranchId = personelShareDTO.Branch.Id;
                        unitOfWork.OrgStructureShareDataService.InsertOrUpdateOrgStructureShare(x);
                    });
                await unitOfWork.CommitAsync();
                result.ReturnMessage.Add(MESSAGE_ADD_ENTITY);
                result.ResultValue = personelShareDTO;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }

            return result;
        }
        public async Task<IBusinessResult> DeleteOrgStructureShareDTOAsync(OrgStructureShareDTO personelShareDTO)
        {
            var result = new BusinessResult();

            try
            {
                personelShareDTO.Items
                    .Where(x => x.Id != 0)
                    .ToList()
                    .ForEach(x =>
                    {
                        unitOfWork.OrgStructureShareDataService.Delete(x);
                    });
                await unitOfWork.CommitAsync();
                result.ReturnMessage.Add(" اطلاعات با موفقیت حذف شد");
            }
            catch (DbEntityValidationException ex)
            {
                result.PopulateValidationErrors(ex);
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        #endregion
    }
}
