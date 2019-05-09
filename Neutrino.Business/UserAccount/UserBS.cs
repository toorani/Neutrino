using FluentValidation;
using Neutrino.Interfaces;
using Neutrino.Entities;
using System.Threading.Tasks;
using Espresso.BusinessService;
using Espresso.BusinessService.Interfaces;
using System;
using Neutrino.Data.EntityFramework;
using Z.EntityFramework.Plus;
using System.Linq;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq.Expressions;
using Espresso.Core;

namespace Neutrino.Business
{
    public class UserBS : NeutrinoBusinessService, IUserBS
    {

        #region [ Constructor(s) ]
        public UserBS(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessLoadByPagingResult<User>> LoadAsync(Expression<Func<User, bool>> where = null
           , Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null
           , int pageNumber = 0
           , int pageSize = 15)
        {
            var entites = new BusinessLoadByPagingResult<User>();

            try
            {
                if (where != null)
                    where = where.And(x => x.Deleted == false);
                else
                    where = x => x.Deleted == false;

                var query = unitOfWork.UserDataService.GetQuery()
                    .IncludeFilter(x => x.Roles.Where(r => r.Deleted == false))
                    .IncludeFilter(x => x.Roles.Select(r => r.Role))
                    .Where(where);
                query = orderBy(query);


                query = query.Skip(pageNumber).Take(pageSize);
                entites.ResultValue = await query.ToListAsync();

                entites.TotalRows = await unitOfWork.UserDataService.GetCountAsync(where);
                entites.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                CatchException(ex, entites, "");
            }

            return entites;
        }
        public async Task<IBusinessResultValue<User>> LoadUserAsync(int userId)
        {
            var result = new BusinessResultValue<User>();
            try
            {
                result.ResultValue = await (from us in unitOfWork.UserDataService.GetQuery()
                                           .IncludeFilter(x => x.Roles.Where(r => r.Deleted == false))
                                           .IncludeFilter(x => x.Claims.Where(r => r.Deleted == false))
                                            where us.Id == userId
                                            select us).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                CatchException(ex, result, "");
            }
            return result;
        }
        public async Task UpdateAsync(User user)
        {
            var roleExists = await unitOfWork.UserRoleDataService.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Deleted == false);
            if (user.Roles.Any(x => x.RoleId == roleExists.RoleId) == false)
            {
                roleExists.Deleted = false;
                roleExists.LastUpdated = DateTime.Now;
                unitOfWork.UserRoleDataService.Delete(roleExists);
            }
            var lst_claimExists = await unitOfWork.UserClaimDataService.GetAsync(x => x.UserId == user.Id && x.ClaimType == "branch" && x.Deleted == false);
            var lst_newclaims = user.Claims.Except(lst_claimExists, x => x.ClaimValue);
            var lst_removeclaims = lst_claimExists.Except(user.Claims, x => x.ClaimValue);
            foreach (var item in lst_newclaims)
            {
                item.DateCreated = DateTime.Now;
                item.LastUpdated = DateTime.Now;
                unitOfWork.UserClaimDataService.Insert(item);
            }

            foreach (var item in lst_removeclaims)
            {
                item.Deleted = false;
                item.LastUpdated = DateTime.Now;
                unitOfWork.UserClaimDataService.Delete(item);
            }
            
            user.LastUpdated = DateTime.Now;
            unitOfWork.UserDataService.Update(user);
            
            await unitOfWork.CommitAsync();
        }
        public async Task CreateAsync(User user)
        {
            user.DateCreated = DateTime.Now;
            user.LastUpdated = DateTime.Now;
            unitOfWork.UserDataService.Insert(user);
            await unitOfWork.CommitAsync();
        }
        public async Task DeleteAsync(User user)
        {
            user.LastUpdated = DateTime.Now;
            user.Deleted = false;
            unitOfWork.UserDataService.Delete(user);
            await unitOfWork.CommitAsync();
        }
        public Task<User> FindByEmailAsync(string email)
        {
            return unitOfWork.UserDataService.GetQuery()
                .AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
        }
        public Task<User> FindByIdAsync(int userId)
        {
            return unitOfWork.UserDataService.GetQuery()
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
        }
        public Task<User> FindByNameAsync(string userName)
        {
            return unitOfWork.UserDataService.GetQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName == userName);
        }
        public async Task<string> GetPasswordHashAsync(User user)
        {
            var userFind = await unitOfWork.UserDataService.GetQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == user.Id);
            return userFind != null ? user.PasswordHash : null;
        }
        public async Task<bool> HasPasswordAsync(User user)
        {
            var userFind = await unitOfWork.UserDataService.GetQuery()
                .AsNoTracking().FirstOrDefaultAsync(u => u.Id == user.Id);
            return userFind != null ? string.IsNullOrWhiteSpace(userFind.PasswordHash) : false;
        }
        public async Task SetPasswordHashAsync(User user, string passwordHash)
        {
            var userFind = await unitOfWork.UserDataService.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (userFind != null)
            {
                userFind.PasswordHash = passwordHash;
                userFind.LastUpdated = DateTime.Now;
                unitOfWork.UserDataService.Update(userFind);
                await unitOfWork.CommitAsync();
            }
        }
        public async Task<IList<string>> GetRolesAsync(User user)
        {
            var userFind = await (from usr in unitOfWork.UserDataService.GetQuery()
                                  .AsNoTracking()
                                  .IncludeFilter(x => x.Roles.Where(r => r.Deleted == false))
                                  .IncludeFilter(x => x.Roles.Select(r => r.Role))
                                  where usr.Id == user.Id
                                  select usr).FirstOrDefaultAsync();

            return userFind != null ? user.Roles.Select(x => x.Role.Name).ToList() : new List<string>();
        }
        public async Task<bool> IsInRoleAsync(User user, string roleName)
        {
            var roles = await GetRolesAsync(user);
            return roles.Contains(roleName);
        }
        public async Task<string> GetEmailAsync(User user)
        {
            var userFind = await unitOfWork.UserDataService.GetQuery()
                .AsNoTracking().FirstOrDefaultAsync(u => u.Id == user.Id);
            return userFind?.Email;
        }
        public async Task SetEmailAsync(User user, string email)
        {
            var userFind = await unitOfWork.UserDataService.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (userFind != null)
            {
                userFind.Email = email;
                userFind.LastUpdated = DateTime.Now;
                unitOfWork.UserDataService.Update(userFind);
                await unitOfWork.CommitAsync();
            }
        }


        public void Dispose()
        {
            //
        }

        #endregion

        #region [ NotImplemented ]
        public Task AddToRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            throw new NotImplementedException();
        }
        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }
        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
