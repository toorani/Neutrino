using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using FluentValidation;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Neutrino.Business
{
    public class MemberPromotionBS : NeutrinoBusinessService, IMemberPromotionBS
    {
        public MemberPromotionBS(NeutrinoUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IBusinessResultValue<decimal>> LoadTotalPromotionAsync(int memberId, int month, int year)
        {
            var result = new BusinessResultValue<decimal>();
            try
            {
                // پورسانت عوامل فروش
                result.ResultValue = await (from mep in unitOfWork.MemberPromotionDataService.GetQuery()
                                            join brp in unitOfWork.BranchPromotionDataService.GetQuery()
                                            on mep.BranchPromotionId equals brp.Id
                                            where mep.MemberId == memberId && brp.Month == month && brp.Year == year && mep.Deleted == false
                                            group mep by mep.MemberId into grp_memp
                                            select grp_memp.Sum(x => x.Promotion)).FirstOrDefaultAsync();


            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
    }
}
