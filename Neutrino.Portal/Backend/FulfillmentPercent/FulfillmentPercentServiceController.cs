using System.Net;
using System.Net.Http;
using System.Web.Http;
using Neutrino.Entities;
using Espresso.Core;
using Neutrino.Portal.Models;
using AutoMapper;
using Espresso.Portal;
using Neutrino.Portal.ProfileMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Neutrino.Interfaces;
using Espresso.BusinessService.Interfaces;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/fulfillmentPercentService")]
    public class FulfillmentPercentServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IFulfillmentPercentBS businessService;
        #endregion

        #region [ Constructor(s) ]
        public FulfillmentPercentServiceController(IFulfillmentPercentBS businessService)
        {
            this.businessService = businessService;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getFulfillmentList")]
        public async Task<HttpResponseMessage> GetFulfillmentList(int year,int month)
        {
            IBusinessResultValue<List<FulfillmentPercent>> lstGoalFullfillments = await businessService.LoadFulfillmentListAsync(year,month);

            if (!lstGoalFullfillments.ReturnStatus)
            {
                return CreateErrorResponse(lstGoalFullfillments);
            }
            List<FulfillmentPercentViewModel> result = new List<FulfillmentPercentViewModel>();
            var mapper = GetMapper();

            lstGoalFullfillments.ResultValue.ForEach(x =>
            {
                result.Add(mapper.Map<FulfillmentPercent, FulfillmentPercentViewModel>(x));
            });

            if (result.Count == 0)
            {
                lstGoalFullfillments.ReturnMessage.Add("اطلاعات مراکز وجود ندارد");
                return CreateErrorResponse(lstGoalFullfillments);
            }

            return CreateSuccessedListResponse<FulfillmentPercentViewModel>(result);


        }

        [Route("addFulfillment"), HttpPost]
        public async Task<HttpResponseMessage> AddFulfillment(List<FulfillmentPercentViewModel> postedViewModel)
        {
            var mapper = GetMapper();
            var lstGeneralGoalFullfil = mapper.Map<List<FulfillmentPercentViewModel>, List<FulfillmentPercent>>(postedViewModel);
   

            var businessResult = await businessService.SubmitDataAsync(lstGeneralGoalFullfil);

            if (!businessResult.ReturnStatus)
            {
                return CreateErrorResponse(businessResult);
            }

            postedViewModel.Where(x => (x.TouchedGoalPercent != 0 || x.EncouragePercent != 0) && x.Id == 0)
                .ToList()
                .ForEach(vm =>
                {
                    vm.Id = businessResult.ResultValue.FirstOrDefault(x => x.BranchId == vm.BranchId).Id;
                });


            return Request.CreateResponse(HttpStatusCode.OK, new { ReturnMessage = businessResult.ReturnMessage, Items = postedViewModel });

        }
        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FulfillmentPercentMapperProfile());
                cfg.AddProfile(new BranchMapperProfile());
                cfg.AddProfile(new GoalStepMapperProfile());
                cfg.AddProfile(new GoalMapperProfile());


            });
            return config.CreateMapper();
        }
        #endregion

    }
}
