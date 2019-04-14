using System.Collections.Generic;
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
    [RoutePrefix("api/orgStructureShareService")]
    public class OrgStructureShareServiceController : ApiControllerBase
    {
        #region [ Varibale(s) ]
        private readonly IOrgStructureShareBS businessService;
        #endregion

        #region [ Constructor(s) ]
        public OrgStructureShareServiceController(IOrgStructureShareBS businessService)
        {
            this.businessService = businessService;
        }
        #endregion

        #region [ Public Method(s) ]
        [Route("getDataGrid"), HttpPost]
        public virtual async Task<HttpResponseMessage> GetDataGrid(JQueryDataTablesModel dataTablesModel)
        {
            var entities = await businessService.LoadOrgStructureShareDTOByPagingAsync(
                 pageNumber: dataTablesModel.iDisplayStart
                , pageSize: dataTablesModel.iDisplayLength
                , orderBy: UIHelper.GetOrderBy<OrgStructureShareDTO>(dataTablesModel.GetSortedColumns())
                , brandName: dataTablesModel.sSearch
                );  

            if (entities.ReturnStatus == false)
            {
                return CreateErrorResponse(entities);
            }

            var mapper = GetMapper();
            List<OrgStructureShareDTOViewModel> dataSource = mapper.Map<List<OrgStructureShareDTO>, List<OrgStructureShareDTOViewModel>>(entities.ResultValue);

            return Request.CreateResponse(HttpStatusCode.OK
                , DataTablesJson(items: dataSource
                , totalRecords: entities.TotalRows
                , totalDisplayRecords: entities.TotalRows
                , sEcho: dataTablesModel.sEcho));

        }

        [Route("getDataItem")]
        public async Task<HttpResponseMessage> GetDataItem(int id)
        {
            var entity = await businessService.LoadOrgStructureShareDTOAsync(id);
            if (entity.ReturnStatus == false)
            {
                return CreateErrorResponse(entity);
            }

            var mapper = GetMapper();
            var dataModelView = mapper.Map<OrgStructureShareDTOViewModel>(entity.ResultValue);

            return CreateViewModelResponse(dataModelView, entity);

        }

        [Route("add"), HttpPost]
        public async Task<HttpResponseMessage> Add(OrgStructureShareDTOViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var entityCreating = mapper.Map<OrgStructureShareDTO>(postedViewModel);
            var entityCreated = await businessService.CreateOrgStructureShareDTOAsync(entityCreating);

            if (entityCreated.ReturnStatus == false)
            {
                return CreateErrorResponse(entityCreated);
            }
            postedViewModel = mapper.Map<OrgStructureShareDTOViewModel>(entityCreated.ResultValue);
            return CreateViewModelResponse(postedViewModel, entityCreated);
        }


        [Route("delete"), HttpPost]
        public async Task<HttpResponseMessage> Delete(OrgStructureShareDTOViewModel postedViewModel)
        {
            var mapper = GetMapper();
            var entityDeleteing = mapper.Map<OrgStructureShareDTO>(postedViewModel);
            var result = await businessService.DeleteOrgStructureShareDTOAsync(entityDeleteing);

            if (result.ReturnStatus == false)
            {
                return CreateErrorResponse(result);
            }

            return CreateViewModelResponse(postedViewModel, result);
        }
        #endregion

        #region [ Private Method(s) ]
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new OrgStructureShareMapperProfile());
                cfg.AddProfile(new OrgStructureMapperProfile());
                cfg.AddProfile(new TypeEntityMapperProfile());
                cfg.AddProfile(new BranchMapperProfile());
            });
            return config.CreateMapper();
        }
        #endregion

    }

}