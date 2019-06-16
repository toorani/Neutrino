
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using Espresso.Core;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Business
{
    public class PositionMappingBS : NeutrinoBusinessService, IPositionMappingBS
    {
        #region [ Varibale(s) ]

        #endregion

        #region [ Public Property(ies) ]
        [Inject]
        public IEntityListLoader<PositionMapping> EntityListLoader { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public PositionMappingBS(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }


        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessResult> CreateOrEditAsync(List<PositionMapping> lstPositionMappings)
        {
            var result = new BusinessResult();
            try
            {
                var positionTypeId = lstPositionMappings.FirstOrDefault().PositionTypeId;
                var lst_ExistsData = await unitOfWork.PositionMappingDataService.GetAsync(x => x.PositionTypeId == positionTypeId);
                var lst_newData = lstPositionMappings.Except(lst_ExistsData, x => x.PositionRefId);

                // add new orgsturctures 
                foreach (var item in lst_newData)
                    unitOfWork.PositionMappingDataService.Insert(item);

                var lst_removeData = lst_ExistsData.Except(lstPositionMappings, x => x.PositionTypeId);

                // delete new orgsturctures 
                foreach (var item in lst_removeData)
                    unitOfWork.PositionMappingDataService.Delete(item);

                await unitOfWork.CommitAsync();
                result.ReturnMessage.Add(MESSAGE_ADD_ENTITY);
                result.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "", "");
            }
            return result;
        }

        public async Task<IBusinessResultValue<Tuple<List<PositionMapping>, List<ElitePosition>>>> LoadPositionMapping(PositionTypeEnum positionTypeId)
        {
            var result = new BusinessResultValue<Tuple<List<PositionMapping>, List<ElitePosition>>>();
            try
            {
                var lst_positionMappings = await unitOfWork.PositionMappingDataService.GetAsync(where: x => x.PositionTypeId == positionTypeId, includes: x => x.ElitePosition);

                var eliteMapped = unitOfWork.PositionMappingDataService.GetQuery()
                    .Where(x => x.PositionTypeId != positionTypeId)
                    .Select(x => x.ElitePositionId);

                var query = unitOfWork.ElitePositionDataService.GetQuery()
                    .Where(x => !eliteMapped.Any(c => c == x.Id)).ToList();


                result.ResultValue = new Tuple<List<PositionMapping>, List<ElitePosition>>(lst_positionMappings, query);
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
