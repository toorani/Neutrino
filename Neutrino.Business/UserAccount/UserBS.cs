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
using System.Security.Claims;

namespace Neutrino.Business
{
    public class ApplicationClaimTypes
    {
        public const string BranchId = "branchId";
        public const string FullName = "fullName";
        public const string HtmlUrl = "htmlUrl";
        public const string ActionUrl = "actionUrl";
    }

    public class UserBS : NeutrinoBusinessService, IUserBS
    {

        #region [ Constructor(s) ]
        public UserBS(NeutrinoUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
        #endregion

        #region [ Public Method(s) ]
        public async Task<IBusinessLoadByPagingResult<User>> LoadAsync(string searchExpr
           , Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null
           , int pageNumber = 0
           , int pageSize = 15)
        {
            var entites = new BusinessLoadByPagingResult<User>();

            try
            {
                var query = unitOfWork.UserDataService.GetQuery()
                    .IncludeFilter(x => x.Roles.Where(r => r.Deleted == false))
                    .IncludeFilter(x => x.Roles.Select(r => r.Role))
                    .Where(x => (x.UserName.Contains(searchExpr) || x.Email.Contains(searchExpr)
                    || x.Name.Contains(searchExpr) || x.LastName.Contains(searchExpr)
                    || x.Roles.Any(r => r.Role.FaName.Contains(searchExpr))) && x.Deleted == false && x.Roles.Any(r => r.Role.IsUsingBySystem == false));

                entites.TotalRows = await query.CountAsync();

                query = orderBy(query);
                query = query.Skip(pageNumber).Take(pageSize);
                entites.ResultValue = await query.ToListAsync();

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
            var userExist = await unitOfWork.UserDataService.GetQuery()
                .IncludeFilter(x => x.Roles.Where(r => r.Deleted == false))
                .IncludeFilter(x => x.Claims.Where(r => r.Deleted == false && r.ClaimType == ApplicationClaimTypes.BranchId))
                .FirstOrDefaultAsync(x => x.Id == user.Id);

            userExist.Name = user.Name;
            userExist.LastName = user.LastName;
            userExist.UserName = user.UserName;
            userExist.Email = user.Email;
            userExist.PhoneNumber = user.PhoneNumber;

            var lst_newrole = user.Roles.Except(userExist.Roles, x => x.RoleId);
            var lst_removerole = userExist.Roles.Except(user.Roles, x => x.RoleId);

            foreach (var item in lst_newrole)
            {
                item.DateCreated = DateTime.Now;
                item.LastUpdated = DateTime.Now;
                unitOfWork.UserRoleDataService.Insert(item);
            }
            foreach (var item in lst_removerole)
            {
                item.LastUpdated = DateTime.Now;
                item.Deleted = true;
                unitOfWork.UserRoleDataService.Delete(item);
            }

            var lst_newclaims = user.Claims.Except(userExist.Claims, x => x.ClaimValue);
            var lst_removeclaims = userExist.Claims.Except(user.Claims, x => x.ClaimValue);
            foreach (var item in lst_newclaims)
            {
                item.DateCreated = DateTime.Now;
                item.LastUpdated = DateTime.Now;
                unitOfWork.UserClaimDataService.Insert(item);
            }

            foreach (var item in lst_removeclaims)
            {
                item.Deleted = true;
                item.LastUpdated = DateTime.Now;
                unitOfWork.UserClaimDataService.Delete(item);
            }

            user.LastUpdated = DateTime.Now;
            unitOfWork.UserDataService.Update(userExist);

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
            user.Deleted = true;
            foreach (var item in user.Claims)
            {
                item.Deleted = true;
                item.LastUpdated = DateTime.Now;
                unitOfWork.UserClaimDataService.Delete(item);
            }

            foreach (var item in user.Roles)
            {
                item.Deleted = true;
                item.LastUpdated = DateTime.Now;
                unitOfWork.UserRoleDataService.Delete(item);
            }

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
                .IncludeFilter(x => x.Claims.Where(cl => cl.Deleted == false))
                .IncludeFilter(x => x.Roles.Where(r => r.Deleted == false))
                .AsNoTracking().SingleOrDefaultAsync(x => x.Id == userId);
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
            else
            {
                user.PasswordHash = passwordHash;
            }
        }
        public async Task<IList<string>> GetRolesAsync(User user)
        {
            var userFind = await (from usr in unitOfWork.UserDataService.GetQuery()
                                  .IncludeFilter(x => x.Roles.Where(r => r.Deleted == false))
                                  .IncludeFilter(x => x.Roles.Select(r => r.Role))
                                  .AsNoTracking()

                                  where usr.Id == user.Id
                                  select usr).FirstOrDefaultAsync();

            return userFind != null ? userFind.Roles.Select(x => x.Role.Name).ToList() : new List<string>();
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
            else
            {
                user.Email = email;
            }
        }
        public async Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            var userloaded = await unitOfWork.UserDataService.FirstOrDefaultAsync(x => x.Id == user.Id);
            return userloaded != null ? user.LockoutEndDateUtc.Value : user.LockoutEndDateUtc.Value;
        }

        public async Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            var dbuser = await unitOfWork.UserDataService.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (dbuser != null)
            {
                dbuser.LockoutEndDateUtc = lockoutEnd.DateTime;
                unitOfWork.UserDataService.Update(dbuser);
                await unitOfWork.CommitAsync();
            }
            else
            {
                user.LockoutEndDateUtc = lockoutEnd.DateTime;
            }
        }

        public async Task<int> IncrementAccessFailedCountAsync(User user)
        {
            var dbuser = await unitOfWork.UserDataService.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (dbuser != null)
            {
                dbuser.AccessFailedCount++;
                unitOfWork.UserDataService.Update(dbuser);
                await unitOfWork.CommitAsync();
                return dbuser.AccessFailedCount;
            }
            else
            {
                user.AccessFailedCount++;
                return user.AccessFailedCount;
            }
        }

        public async Task ResetAccessFailedCountAsync(User user)
        {
            var dbuser = await unitOfWork.UserDataService.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (dbuser != null)
            {
                dbuser.AccessFailedCount = 0;
                unitOfWork.UserDataService.Update(dbuser);
                await unitOfWork.CommitAsync();
            }

        }

        public async Task<int> GetAccessFailedCountAsync(User user)
        {
            var dbuser = await unitOfWork.UserDataService.FirstOrDefaultAsync(x => x.Id == user.Id);
            return dbuser != null ? dbuser.AccessFailedCount : user.AccessFailedCount;
        }

        public async Task<bool> GetLockoutEnabledAsync(User user)
        {
            var dbuser = await unitOfWork.UserDataService.FirstOrDefaultAsync(x => x.Id == user.Id);
            return dbuser != null ? dbuser.LockoutEnabled : user.LockoutEnabled;
        }

        public async Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            var dbuser = await unitOfWork.UserDataService.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (dbuser != null)
            {
                dbuser.LockoutEnabled = enabled;
                unitOfWork.UserDataService.Update(dbuser);
                await unitOfWork.CommitAsync();
            }
        }

        public async Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            var dbuser = await unitOfWork.UserDataService.FirstOrDefaultAsync(x => x.Id == user.Id);
            dbuser.TwoFactorEnabled = enabled;
            unitOfWork.UserDataService.Update(dbuser);
            await unitOfWork.CommitAsync();
        }

        public async Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            var dbuser = await unitOfWork.UserDataService.FirstOrDefaultAsync(x => x.Id == user.Id);
            return dbuser != null ? dbuser.TwoFactorEnabled : user.TwoFactorEnabled;
        }
        public async Task<IList<Claim>> GetClaimsAsync(User user)
        {
            var dbuser = await unitOfWork.UserDataService.GetQuery()
                .IncludeFilter(x => x.Claims.Where(cl => cl.Deleted == false))
                .IncludeFilter(x => x.Roles.Where(cl => cl.Deleted == false))
                .SingleAsync(x => x.Id == user.Id);

            var roleIds = dbuser.Roles.Select(x => x.RoleId);
            var permissions = await (from per in unitOfWork.PermissionDataService.GetQuery()
                                     join act in unitOfWork.ApplicationActionDataService.GetQuery()
                                     on per.ApplicationActionId equals act.Id
                                     where roleIds.Contains(per.RoleId)
                                     select new
                                     {
                                         act.HtmlUrl,
                                         act.ActionUrl
                                     }).ToListAsync();

            var result = new List<Claim>() { new Claim(ApplicationClaimTypes.FullName, user.Name + " " + user.LastName) };
            result.AddRange(permissions.Select(x => x.HtmlUrl).Distinct().Select(x => new Claim(ApplicationClaimTypes.HtmlUrl, x)));
            result.AddRange(permissions.Select(x => new Claim(ApplicationClaimTypes.ActionUrl, x.ActionUrl)));
            result.AddRange(dbuser.Claims.Select(x => new Claim(x.ClaimType, x.ClaimValue)));
            return result;
        }

        public async Task AddClaimAsync(User user, Claim claim)
        {
            UserClaim userClaim = new UserClaim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                UserId = user.Id
            };
            unitOfWork.UserClaimDataService.Insert(userClaim);
            await unitOfWork.CommitAsync();
        }

        public async Task RemoveClaimAsync(User user, Claim claim)
        {
            var dbData = await unitOfWork.UserClaimDataService.FirstOrDefaultAsync(x => x.UserId == user.Id
            && x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            if (dbData != null)
            {
                unitOfWork.UserClaimDataService.Delete(dbData);
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
