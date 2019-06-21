using Espresso.BusinessService.Interfaces;
using Neutrino.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neutrino.Interfaces
{
    public interface IMemberPromotionBS : IBusinessService
    {
        Task<IBusinessResultValue<decimal>> LoadTotalPromotionAsync(int memberId, int month, int year);
    }
}
