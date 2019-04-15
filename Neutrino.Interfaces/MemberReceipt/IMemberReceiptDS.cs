using System.Collections.Generic;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IMemberReceiptDS : IEntityRepository<MemberReceipt>
    {
        Task<MemberReceipt> GetLatestYearMonthAsync();
        Task<List<MemberReceipt>> GetMonthYearListAsync();
    }
}
