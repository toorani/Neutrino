using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Espresso.BusinessService.Interfaces;
using Espresso.Portal;
using Neutrino.Entities;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;
using System.Linq;
using System.Net;
using Neutrino.Interfaces;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/positionTypeService")]
    public class PositionTypeServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IEntityListLoader<PositionType> businessService;
        private readonly IPositionMappingBS positionMappingBS;
        private readonly IEntityListLoader<ElitePosition> elitePositionListLoader;
        #endregion

        #region [ Constructor(s) ]
        public PositionTypeServiceController(IEntityListLoader<PositionType> businessService
            , IPositionMappingBS positionMappingBS
            , IEntityListLoader<ElitePosition> elitePositionListLoader)
        {
            this.businessService = businessService;
            this.positionMappingBS = positionMappingBS;
            this.elitePositionListLoader = elitePositionListLoader;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getValues"), HttpGet]
        public async Task<HttpResponseMessage> GetValuesAsync()
        {
            IBusinessResultValue<List<PositionType>> entities = await businessService.LoadListAsync(orderBy: ord => ord.OrderBy(x => x.Description));
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }
            var mapper = GetMapper();
            var result = mapper.Map<List<PositionType>, List<TypeEntityViewModel>>(entities.ResultValue);
            return CreateSuccessedListResponse<TypeEntityViewModel>(result);
        }

        [Route("getElitePositions"), HttpGet]
        public async Task<HttpResponseMessage> GetElitePositionsAsync()
        {
            var entities = await elitePositionListLoader.LoadListAsync(orderBy: x => x.OrderBy(c => c.Title));
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            var result = entities.ResultValue.Select(x => new ElitePositionViewModel
            {
                Id = x.Id,
                Title = x.Title,
                RefId = x.RefId
            });
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("getPositionMapping"), HttpGet]
        public async Task<HttpResponseMessage> GetPositionMappingAsync(int positionTypeId)
        {
            //var entities = await positionMappingBS.EntityListLoader.LoadListAsync(where: x => x.PositionTypeId == (PositionTypeEnum)positionTypeId
            //, includes: x => x.ElitePosition);

            //var elitePositions = await elitePositionListLoader.LoadListAsync(orderBy: x => x.OrderBy(c => c.Title));
            //if (elitePositions.ReturnStatus == false)
            //{
            //    return CreateErrorResponse(elitePositions);
            //}

            var entities = await positionMappingBS.LoadPositionMapping((PositionTypeEnum)positionTypeId);

            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }


            var result = new PositionMappingViewModel
            {
                PositionTypeId = positionTypeId,
                SelectedElitePoistions = entities.ResultValue.Item1.Select(x => new ElitePositionViewModel
                {
                    Id = x.ElitePositionId,
                    RefId = x.ElitePosition.RefId,
                    Title = x.ElitePosition.Title
                }).ToList(),
                ElitePoistions = entities.ResultValue.Item2.Select(x => new ElitePositionViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    RefId = x.RefId
                }).ToList()
            };

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("addOrEdit")]
        public async Task<HttpResponseMessage> AddOrEditAsync(PositionMappingViewModel postedViewModel)
        {
            var entitiesMapped = postedViewModel.SelectedElitePoistions.Select(x => new PositionMapping
            {
                ElitePositionId = x.Id,
                PositionTypeId = (PositionTypeEnum)postedViewModel.PositionTypeId,
                PositionRefId = x.RefId
            }).ToList();

            var entities = await positionMappingBS.CreateOrEditAsync(entitiesMapped);
            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            return Request.CreateResponse(HttpStatusCode.OK, entities);
        }


        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TypeEntityMapperProfile());

            });
            return config.CreateMapper();
        }
        #endregion

    }
}