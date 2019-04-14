using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System.Data.Entity;

namespace Neutrino.Data.EntityFramework.DataServices
{
    public class OrgStructureDataService : RepositoryBase<OrgStructure>
    {
        #region [ Constructor(s) ]
        public OrgStructureDataService(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }
        #endregion

        //public override void Insert(OrgStructure entity)
        //{
        //    UnitOfWork.Context.Entry(entity.Branch).State = EntityState.Unchanged;
            
        //    base.Insert(entity);
        //}
    }
}
