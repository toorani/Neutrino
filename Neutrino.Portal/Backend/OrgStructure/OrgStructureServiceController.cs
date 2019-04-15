using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Espresso.Portal;
using jQuery.DataTables.WebApi;

using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Portal.Models;
using Neutrino.Portal.ProfileMapper;

namespace Neutrino.Portal.WebApiControllers
{
    [RoutePrefix("api/orgStructureService")]
    public class OrgStructureServiceController : ApiControllerBase
    {

        #region [ Varibale(s) ]
        private readonly IOrgStructureBS businessService;

        #endregion

        #region [ Constructor(s) ]
        public OrgStructureServiceController(IOrgStructureBS businessService)
        {
            this.businessService = businessService;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getDataGrid"), HttpPost]
        public virtual async Task<HttpResponseMessage> GetDataGrid(JQueryDataTablesModel dataTablesModel)
        {
            var entities = await businessService.EntityListByPagingLoader.LoadAsync(
                    pageNumber: dataTablesModel.iDisplayStart
                    , pageSize: dataTablesModel.iDisplayLength
                    , includes: x => new { x.Branch, x.PositionType }
                    , orderBy: UIHelper.GetOrderBy<OrgStructure, OrgStructureIndexViewModel>(dataTablesModel.GetSortedColumns())
                    ,
                    where: x =>
                x.PositionType.Description.Contains(dataTablesModel.sSearch) ||
                x.Branch.Name.Contains(dataTablesModel.sSearch)
                || dataTablesModel.sSearch == null);

            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            var mapper = GetMapper();
            List<OrgStructureIndexViewModel> dataSource = mapper.Map<List<OrgStructure>, List<OrgStructureIndexViewModel>>(entities.ResultValue);

            return Request.CreateResponse(HttpStatusCode.OK
                , DataTablesJson(items: dataSource
                , totalRecords: entities.TotalRows
                , totalDisplayRecords: entities.TotalRows
                , sEcho: dataTablesModel.sEcho));

        }

        [Route("getDataItem")]
        public async Task<HttpResponseMessage> GetDataItem(int positionTypeId)
        {
            var lstEntities = await businessService.EntityListLoader.LoadListAsync(where: x => x.PositionTypeId == (PositionTypeEnum)positionTypeId);
            if (lstEntities.ReturnStatus == false)
            {
                return CreateErrorResponse(lstEntities);
            }

            var dataModelView = new OrgStructureViewModel();
            dataModelView.Id = positionTypeId;
            dataModelView.Branches.AddRange(lstEntities.ResultValue.Select(x => x.BranchId));


            return CreateViewModelResponse(dataModelView, lstEntities);

        }

        [Route("addOrEdit"), HttpPost]
        public async Task<HttpResponseMessage> AddOrEdit(OrgStructureViewModel postedViewModel)
        {
            var entityCreating = postedViewModel.Branches.Select(x=>new OrgStructure
            {
                BranchId = x,
                PositionTypeId = (PositionTypeEnum)postedViewModel.PositionTypeId
            })
            .ToList();
            
            var entityCreated = await businessService.CreateOrEditAsync(entityCreating);

            if (entityCreated.ReturnStatus == false)
            {
                return CreateErrorResponse(entityCreated);
            }

            return CreateViewModelResponse(postedViewModel, entityCreated);
        }

        [Route("delete"), HttpPost]
        public async Task<HttpResponseMessage> Delete(OrgStructureViewModel postedViewModel)
        {
            var result = await businessService.DeleteAsync(positionTypeId: postedViewModel.Id);

            if (result.ReturnStatus == false)
            {
                return CreateErrorResponse(result);
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new OrgStructureMapperProfile());
                cfg.AddProfile(new BranchMapperProfile());
                cfg.AddProfile(new TypeEntityMapperProfile());
            });

            return config.CreateMapper();
        }
        #endregion

    }

}