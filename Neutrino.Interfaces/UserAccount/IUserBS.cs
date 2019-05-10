using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Espresso.BusinessService.Interfaces;
using Microsoft.AspNet.Identity;
using Neutrino.Entities;

namespace Neutrino.Interfaces
{
    public interface IUserBS : IBusinessService, IUserRoleStore<User, int>,
        IUserStore<User, int>,
        IUserPasswordStore<User, int>,
        IUserEmailStore<User, int>
    {
        Task<IBusinessResultValue<User>> LoadUserAsync(int userId);
        Task<IBusinessLoadByPagingResult<User>> LoadAsync(string searchExpr
           , Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null
           , int pageNumber = 0
           , int pageSize = 15);
        

    }
}
