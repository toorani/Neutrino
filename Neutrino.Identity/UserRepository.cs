using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Espresso.Identity.Models;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Identity;
using Espresso.Core;
using System.Data.Entity;


namespace Neutrino.Identity
{
    class UserRepository<TIdentityUser> : IDisposable
        where TIdentityUser : IdentityUser
    {
        private readonly NeutrinoContext _databaseContext;

        public UserRepository(NeutrinoContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        internal TIdentityUser GeTByName(string userName)
        {
            var user = _databaseContext.Users.SingleOrDefault(u => u.UserName == userName);

            if (user != null)
            {
                return getIdentityUser(user);

            }
            return null;
        }
        internal TIdentityUser GeTByEmail(string email)
        {
            var user = _databaseContext.Users.SingleOrDefault(u => u.Email == email);
            if (user != null)
            {
                TIdentityUser result = (TIdentityUser)Activator.CreateInstance(typeof(TIdentityUser));

                return getIdentityUser(user);
            }
            return null;
        }
        internal int Insert(TIdentityUser user)
        {
            int result = -1;
            try
            {
                NeutrinoUser newUser = new NeutrinoUser
                {
                    UserName = user.UserName,
                    PasswordHash = user.PasswordHash,
                    SecurityStamp = user.SecurityStamp,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    MobileNumber = user.PhoneNumber,
                    MobileNumberConfirmed = user.PhoneNumberConfirmed,
                    LockoutEnabled = user.LockoutEnabled,
                    LockoutEndDateUtc = user.LockoutEndDateUtc,
                    AccessFailedCount = user.AccessFailedCount,
                    Name = user.Name,
                    LastName = user.LastName
                };
                user.Roles.ToList().ForEach((item) =>
                {
                    //NeutrinoRole role = new NeutrinoRole() { Id = item.RoleId };
                    //_databaseContext.Entry<NeutrinoRole>(role).State = System.Data.Entity.EntityState.Unchanged;
                    UserRole userRole = new UserRole();
                    userRole.RoleId = item.RoleId;
                    userRole.UserId = item.UserId;
                    newUser.UserRoles.Add(userRole);
                });
                _databaseContext.Users.Add(newUser);

                result = _databaseContext.SaveChanges();
                user.Id = newUser.Id;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                StringBuilder message = new StringBuilder();
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        message.Append(string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage));
                        // raise a new exception nesting
                        // the current instance as InnerException

                    }
                }
                throw new Exception(message.ToString(), ex);
            }
            return result;
        }

        /// <summary>
        /// Returns an T given the user’s id
        /// </summary>
        /// <param name=”userId”>The user’s id</param>
        /// <returns></returns>
        public TIdentityUser GeTById(int userId)
        {
            var user = _databaseContext.Users.Find(userId);
            return getIdentityUser(user);

        }

        /// <summary>
        /// Return the user’s password hash
        /// </summary>
        /// <param name=”userId”>The user’s id</param>
        /// <returns></returns>
        public string GetPasswordHash(int userId)
        {
            var user = _databaseContext.Users.FirstOrDefault(u => u.Id == userId);
            var passHash = user != null ? user.PasswordHash : null;
            return passHash;
        }

        /// <summary>
        /// Updates a user in the Users table
        /// </summary>
        /// <param name=”updatingUser”></param>
        /// <returns></returns>
        public int Update(TIdentityUser updatingUser)
        {
            int id = Convert.ToInt32(updatingUser.Id);
            var dbUser = _databaseContext.Users
                .Include(x => x.UserRoles)
                .FirstOrDefault(u => u.Id == id);
            if (dbUser != null)
            {
                dbUser.UserName = updatingUser.UserName;
                //result.PasswordHash = user.PasswordHash;
                //result.SecurityStamp = user.SecurityStamp;
                dbUser.Email = dbUser.Email;
                dbUser.EmailConfirmed = updatingUser.EmailConfirmed;
                dbUser.MobileNumber = updatingUser.PhoneNumber;
                dbUser.MobileNumberConfirmed = updatingUser.PhoneNumberConfirmed;
                dbUser.Name = updatingUser.Name;
                dbUser.LastName = updatingUser.LastName;


                //deleted role
                dbUser.UserRoles
                    .Where(x => x.Deleted == false)
                    .ToList()
                    .ForEach(x =>
                    {
                        if (!updatingUser.Roles.Any(y => y.RoleId == x.Id))
                        {
                            x.Deleted = true;
                        }
                    });

                //added role
                updatingUser.Roles.ToList().ForEach(x =>
                {
                    UserRole userRole = new UserRole();
                    if (dbUser.UserRoles.Any(y => y.RoleId == x.RoleId && y.Deleted))
                    {
                        userRole = dbUser.UserRoles.SingleOrDefault(y => y.RoleId == x.RoleId);
                        userRole.Deleted = false;
                    }
                    else if (!dbUser.UserRoles.Any(y => y.RoleId == x.RoleId && !y.Deleted))
                    {
                        userRole.UserId = x.UserId;
                        userRole.RoleId = x.RoleId;
                        dbUser.UserRoles.Add(userRole);
                    }
                });

                return _databaseContext.SaveChanges();

            }
            return 0;
        }
        public void Dispose()
        {
            if (_databaseContext != null)
                _databaseContext.Dispose();
        }
        private TIdentityUser getIdentityUser(NeutrinoUser user)
        {
            var mapper = getMapper();
            var result = mapper.Map<NeutrinoUser, TIdentityUser>(user);
            result.LastName = user.LastName;
            result.Name = user.Name;
            return result;

        }

        private IMapper getMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TIdentityUser, User>()
                .ForMember(x => x.MobileNumber, opt => opt.ResolveUsing(x => x.PhoneNumber))
                .ForMember(x => x.MobileNumberConfirmed, opt => opt.ResolveUsing(x => x.PhoneNumberConfirmed))
                .ReverseMap()
                .ForMember(x => x.PhoneNumber, opt => opt.ResolveUsing(x => x.MobileNumber))
                .ForMember(x => x.PhoneNumberConfirmed, opt => opt.ResolveUsing(x => x.MobileNumberConfirmed));
            });

            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}
