using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Neutrino.Data.EntityFramework;

namespace Neutrino.Identity
{
    public class UserStore<TIdentityUser> : Espresso.Identity.IUserStore<TIdentityUser>
    where TIdentityUser : IdentityUser
    {
        private readonly UserRepository<TIdentityUser> _userRepository;
        private readonly UserRolesRepository _userRolesRepository;

        public UserStore(NeutrinoContext databaseContext)
        {
            _userRepository = new UserRepository<TIdentityUser>(databaseContext);
            _userRolesRepository = new UserRolesRepository(databaseContext);
        }
        public Task CreateAsync(TIdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.Run(() => _userRepository.Insert(user));
        }
        public Task UpdateAsync(TIdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.Run(() => _userRepository.Update(user));
        }
        public Task<TIdentityUser> FindByIdAsync(int userId)
        {
            if (userId == default(int))
            {
                throw new ArgumentException("Null argument: userId");
            }

            return Task.Run(() => _userRepository.GeTById(userId));
        }
        public Task<bool> GetTwoFactorEnabledAsync(TIdentityUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }
        public Task<TIdentityUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Null or empty argument: userName");
            }

            return Task.Run(() => _userRepository.GeTByName(userName));
        }
        public Task<IList<string>> GetRolesAsync(TIdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.Run(() => _userRolesRepository.FindByUserId(user.Id));
        }
        public async Task<IList<int>> GetRoleIdsAsync(TIdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.Run(() => _userRolesRepository.FindIdsByUserIdAsync(user.Id));
        }
        public Task<List<int>> GetRoleIdsAsync(int userId)
        {
            return Task.Run(() => _userRolesRepository.FindIdsByUserIdAsync(userId));
        }
        public Task<string> GetPasswordHashAsync(TIdentityUser user)
        {
            return Task.Run(() => _userRepository.GetPasswordHash(user.Id));
        }
        public Task SetPasswordHashAsync(TIdentityUser user, string passwordHash)
        {
            return Task.Run(() => user.PasswordHash = passwordHash);
        }
        public Task<TIdentityUser> FindByEmailAsync(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email");
            }

            return Task.Run(() => _userRepository.GeTByEmail(email));
        }
        public Task<string> GetEmailAsync(TIdentityUser user)
        {
            return Task.FromResult(user.Email);
        }
        public Task<int> GetAccessFailedCountAsync(TIdentityUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }
        public Task<bool> GetLockoutEnabledAsync(TIdentityUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }
        public Task<DateTimeOffset> GetLockoutEndDateAsync(TIdentityUser user)
        {
            return
            Task.FromResult(user.LockoutEndDateUtc.HasValue
            ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
            : new DateTimeOffset());
        }
        public Task SetLockoutEnabledAsync(TIdentityUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;

            return Task.Run(() => _userRepository.Update(user));
        }
        public Task SetLockoutEndDateAsync(TIdentityUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }
        public Task SetTwoFactorEnabledAsync(TIdentityUser user, bool enabled)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(TIdentityUser user)
        {
            throw new NotImplementedException();
        }
        public Task<int> IncrementAccessFailedCountAsync(TIdentityUser user)
        {
            throw new NotImplementedException();
        }
        public Task ResetAccessFailedCountAsync(TIdentityUser user)
        {
            throw new NotImplementedException();
        }
        public Task<bool> GetEmailConfirmedAsync(TIdentityUser user)
        {
            throw new NotImplementedException();
        }
        public Task SetEmailAsync(TIdentityUser user, string email)
        {
            throw new NotImplementedException();
        }
        public Task SetEmailConfirmedAsync(TIdentityUser user, bool confirmed)
        {
            throw new NotImplementedException();
        }
        public Task<bool> IsInRoleAsync(TIdentityUser user, string roleName)
        {
            throw new NotImplementedException();
        }
        public Task RemoveFromRoleAsync(TIdentityUser user, string roleName)
        {
            throw new NotImplementedException();
        }
        public Task<bool> HasPasswordAsync(TIdentityUser user)
        {
            throw new NotImplementedException();
        }
       
        public Task AddToRoleAsync(TIdentityUser user, string roleName)
        {
            throw new NotImplementedException();
        }
        public Task SetPhoneNumberAsync(TIdentityUser user, string phoneNumber)
        {
            throw new NotImplementedException();
        }
        public Task<string> GetPhoneNumberAsync(TIdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.Run(() => _userRepository.GeTById(user.Id).PhoneNumber);
        }
        public Task<bool> GetPhoneNumberConfirmedAsync(TIdentityUser user)
        {
            throw new NotImplementedException();
        }
        public Task SetPhoneNumberConfirmedAsync(TIdentityUser user, bool confirmed)
        {
            throw new NotImplementedException();
        }
        public Task AddLoginAsync(TIdentityUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }
        public Task RemoveLoginAsync(TIdentityUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }
        public Task<IList<UserLoginInfo>> GetLoginsAsync(TIdentityUser user)
        {
            throw new NotImplementedException();
        }
        public Task<TIdentityUser> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (_userRepository != null)
                _userRepository.Dispose();
            //throw new NotImplementedException();
        }
    }
}
