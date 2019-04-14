using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Espresso.Identity.Models;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IServiceLog : IDataRepository<ServiceLog>
    {
    }
}
