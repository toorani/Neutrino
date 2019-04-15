using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Espresso.Portal;
using Neutrino.Entities;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/CompanyService")]
    public class CompanyServiceController : ApiControllerBase
    {

        #region [ Varibale(s) ]
        private readonly IEntityListLoader<Company> businessService;
        #endregion
        
        #region [ Constructor(s) ]
        public CompanyServiceController(IEntityListLoader<Company> businessService)
        {
            this.businessService = businessService;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getCompany"), HttpGet]
        public HttpResponseMessage GetCompanies(string faName = null)
        {

            IBusinessResultValue<List<Company>> entities = businessService.LoadList(
               where: co => co.FaName.StartsWith(faName)
               || co.Code.StartsWith(faName)
               || co.NationalCode.StartsWith(faName)
               || faName == null || faName == ""
               , orderBy: Utilities.GetOrderBy<Company>("FaName", "asc"));


            if (entities.ReturnStatus == false)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, entities.ReturnMessage);
            }

            var mapper = GetMapper();
            var result = mapper.Map<List<Company>, List<CompanyViewModel>>(entities.ResultValue);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CompanyMapperProfile());
            });
            return config.CreateMapper();
        }
        #endregion

    }

}