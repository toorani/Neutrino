using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Espresso.Core;

using Neutrino.Entities;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;
using Neutrino.Core.SecurityManagement;
using Espresso.Portal;
using Neutrino.Interfaces;
using Espresso.BusinessService.Interfaces;
using Neutrino.Data.EntityFramework;
using System.Data.Entity;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/penaltyService")]
    public class PenaltyServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly NeutrinoUnitOfWork unitOfWork;
        #endregion

        #region [ Constructor(s) ]
        public PenaltyServiceController(NeutrinoUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getData")]
        public async Task<HttpResponseMessage> GetData(int year, int month, int branchId)
        {
            if (year != 0 && month != 0 && branchId != 0)
            {
                var query = await (from me in unitOfWork.MemberDataService
                                   .GetQuery()
                                   .Include(x => x.PositionType)
                                   .OrderBy(x => x.PositionTypeId)
                                   where me.BranchId == branchId
                                   select new PenaltyViewModel
                                   {
                                       Id = me.Id,
                                       EmployeeCode = me.Code,
                                       EmployeeName = me.Name + " " + me.LastName,
                                       Position = me.PositionType.Description
                                   }).ToListAsync();
                return CreateSuccessedListResponse(query);
            }

            return await Task.Run(() => Request.CreateResponse(HttpStatusCode.NotFound));
        }

        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {

            });
            return config.CreateMapper();
        }
        #endregion

    }

}