using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Espresso.Core;
using Espresso.Portal;
using jQuery.DataTables.WebApi;

using Neutrino.Entities;
using Neutrino.Interfaces;

using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/costCoefficientService")]
    public class CostCoefficientServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly ICostCoefficientBS businessService;
        #endregion

        #region [ Constructor(s) ]
        public CostCoefficientServiceController(ICostCoefficientBS businessService)
        {
            this.businessService = businessService;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getCostCoefficientList")]
        public async Task<HttpResponseMessage> GetCostCoefficientListAsync()
        {
            var entities = await businessService.LoadCoefficientList();
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }
            var mapper = GetMapper();
            var result = mapper.Map<List<CostCoefficient>, List<CostCoefficientViewModel>>(entities.ResultValue);
            return CreateSuccessedListResponse(result);
        }

        [Route("add"), HttpPost]
        public async Task<HttpResponseMessage> Add(CostCoefficientViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var entityCreating = mapper.Map<CostCoefficient>(postedViewModel);
            var entityCreated = await businessService.AddOrUpdateAsync(entityCreating);

            if (entityCreated.ReturnStatus == false)
            {
                return CreateErrorResponse(entityCreated);
            }
            return CreateViewModelResponse(postedViewModel, entityCreated);
        }

        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CostCoefficientMapperProfile());
                //cfg.AddProfile(new TypeEntityMapperProfile());
            });

            return config.CreateMapper();
        }
        
        #endregion
    }

}