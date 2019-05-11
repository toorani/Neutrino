
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using FluentValidation;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Business
{
    public class FulfillmentPromotionConditionBS : NeutrinoBusinessService, IFulfillmentPromotionConditionBS
    {
        #region [ Varibale(s) ]
        private readonly AbstractValidator<List<FulfillmentPromotionCondition>> validator;
        #endregion

        #region [ Public Property(ies) ]
        [Inject]
        public IEntityListLoader<FulfillmentPromotionCondition> EntityListLoader { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public FulfillmentPromotionConditionBS(NeutrinoUnitOfWork unitOfWork
            , AbstractValidator<List<FulfillmentPromotionCondition>> validator):base(unitOfWork)
        {
            this.validator = validator;
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResult> SaveAsync(List<FulfillmentPromotionCondition> lstGeneralGoal)
        {
            var result = new BusinessResult();
            try
            {
                lstGeneralGoal = lstGeneralGoal.Where(x => x.ManagerPromotion.HasValue || x.TotalSalesFulfilledPercent.HasValue
                || x.PrivateReceiptFulfilledPercent.HasValue || x.SellerPromotion.HasValue
                || x.TotalReceiptFulfilledPercent.HasValue).ToList();
                var validatorResult = await validator.ValidateAsync(lstGeneralGoal);
                if (validatorResult.IsValid == false)
                {
                    result.PopulateValidationErrors(validatorResult.Errors);
                    return result;
                }
                var totalFulfillEnity = lstGeneralGoal.FirstOrDefault();
                lstGeneralGoal.ForEach(x => unitOfWork.TotalFulfillPromotionPercentDataService.InsertOrUpdate(x));
                await unitOfWork.CommitAsync();
                result.ReturnMessage.Add(MESSAGE_ADD_ENTITY);
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
