using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neutrino.Interfaces
{
    public interface IPositionMappingBS : IBusinessService
        , IEnabledEntityListLoader<PositionMapping>
    {
        Task<IBusinessResultValue<Tuple<List<PositionMapping>,List<ElitePosition>>>> LoadPositionMapping(PositionTypeEnum positionTypeId);
        Task<IBusinessResult> CreateOrEditAsync(List<PositionMapping> lstPositionMappings);
    }
}
