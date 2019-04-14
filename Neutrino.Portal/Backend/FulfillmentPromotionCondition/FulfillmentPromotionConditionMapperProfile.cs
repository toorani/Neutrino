using AutoMapper;
using Neutrino.Entities;
using Neutrino.Portal.Models;

namespace Neutrino.Portal.ProfileMapper
{
    public class FulfillmentPromotionConditionMapperProfile : Profile
    {
        #region [ Constructor(s) ]
        public FulfillmentPromotionConditionMapperProfile()
        {
            CreateMap<FulfillmentPromotionCondition, FulfillmentPromotionConditionViewModel>()
                .ReverseMap();
        }
        #endregion
        
    }
}