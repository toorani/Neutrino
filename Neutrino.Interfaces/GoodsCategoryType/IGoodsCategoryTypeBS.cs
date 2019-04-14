using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IGoodsCategoryTypeBS : IBusinessService
    {
        Task<int> AddGoodsCategoryTypesAsync(List<GoodsCategoryType> lstGoodsCategoryTypes)
    }
}
