
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Business
{
    public class MemberBS : NeutrinoBusinessService, IMemberBS
    {
        #region [ Constructor(s) ]
        public MemberBS(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResult> ToggleActivationAsync(int memberId)
        {
            var result = new BusinessResult();
            try
            {
                var entity = await unitOfWork.MemberDataService.GetByIdAsync(memberId);
                if (entity != null)
                {
                    entity.IsActive = !entity.IsActive;
                    await unitOfWork.CommitAsync();
                    result.ReturnMessage.Add(MESSAGE_MODIFY_ENTITY);
                }
                else
                {
                    result.ReturnStatus = false;
                    result.ReturnMessage.Add("اطلاعاتی یافت نشد");
                }
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }

        public async Task<IBusinessResultValue<List<Member>>> LoadMembersAsync(int brandId,bool? isActive = null)
        {
            var result = new BusinessResultValue<List<Member>>();
            try
            {
                result.ResultValue = await unitOfWork.MemberDataService.GetAsync(where: x => x.BranchId == brandId && (isActive == null || x.IsActive)
                , orderBy: x => x.OrderBy(c => c.LastName)
                , includes: x => x.PositionType);
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
