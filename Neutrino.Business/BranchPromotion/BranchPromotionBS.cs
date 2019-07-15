using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Neutrino.Business
{
    public class BranchPromotionBS : NeutrinoBusinessService, IBranchPromotionBS
    {

        #region [ Constructor(s) ]
        public BranchPromotionBS(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork) { }
        #endregion

        #region [ Public Method(s) ]

        public async Task<IBusinessResult> AddOrUpdateAsync(List<BranchPromotion> lstEntities)
        {
            var result = new BusinessResult();

            try
            {
                lstEntities.ForEach(brp => unitOfWork.BranchPromotionDataService.Update(brp));
                await unitOfWork.CommitAsync();
                result.ReturnMessage.Add(MESSAGE_ADD_ENTITY);
                result.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }

        public async Task<IBusinessResultValue<BranchPromotion>> LoadBranchPromotionAsync(int branchId, PromotionReviewStatusEnum promotionReviewStatusId)
        {
            var result = new BusinessResultValue<BranchPromotion>();
            try
            {
                result.ResultValue = await unitOfWork.BranchPromotionDataService.GetQuery()
                    .IncludeFilter(x => x.BranchGoalPromotions.Where(y => y.Deleted == false))
                    .IncludeFilter(x => x.BranchGoalPromotions.Select(y => y.PositionReceiptPromotions.Where(z => z.Deleted == false)))
                    .IncludeFilter(x => x.BranchGoalPromotions.Select(y => y.PositionReceiptPromotions.Select(z => z.PositionType)))
                    .IncludeFilter(x => x.BranchGoalPromotions.Select(y => y.Goal))
                    .IncludeFilter(x => x.BranchGoalPromotions.Select(y => y.Branch))
                    .Where(x => x.BranchId == branchId && x.PromotionReviewStatusId == promotionReviewStatusId)
                    .FirstOrDefaultAsync();

               
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }

        public async Task<IBusinessResultValue<PromotionReviewStatusEnum>> ConfirmCompensatoryAsync(List<BranchPromotion> lstEntities)
        {
            var result = new BusinessResultValue<PromotionReviewStatusEnum>();

            try
            {
                lstEntities.ForEach(brp =>
                {
                    brp.PromotionReviewStatusId = PromotionReviewStatusEnum.WaitingForStep1BranchManagerReview;
                    unitOfWork.BranchPromotionDataService.Update(brp);
                });

                await unitOfWork.CommitAsync();
                result.ResultValue = PromotionReviewStatusEnum.WaitingForStep1BranchManagerReview;
                result.ReturnMessage.Add("پورسانت تایید و به مراکز ارسال شد");
                result.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }

        public async Task<IBusinessResultValue<List<BranchPromotion>>> LoadListAsync(int promotionId)
        {
            var result = new BusinessResultValue<List<BranchPromotion>>();
            try
            {
                result.ResultValue = await unitOfWork.BranchPromotionDataService.GetAsync(c => c.PromotionId == promotionId, includes: c => c.Branch);
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
