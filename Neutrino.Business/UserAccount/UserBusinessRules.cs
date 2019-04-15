using System.Data.Entity;
using System.Linq;
using FluentValidation;
using Neutrino.Data.EntityFramework;
using Neutrino.Entities;
using Neutrino.Interfaces;

namespace Neutrino.Business
{
    public class UserBusinessRules : NeutrinoValidator<NeutrinoUser>
    {
        #region [ Protected Property(ies) ]
        
        #endregion

        #region [ Constructor(s) ]

        public UserBusinessRules(NeutrinoUnitOfWork unitOfWork)
            :base(unitOfWork)
        {
            RuleFor(x => x.Email)
               .NotNull()
               .WithMessage(".آدرس ایمیل مشخص نشده است");

            RuleFor(x => x.LastName)
                .NotNull()
                .WithMessage("نام خانوادگی مشخص نشده است");

            RuleFor(entity => entity.MobileNumber)
                .NotNull()
                .WithMessage(".شماره موبایل مشخص نشده است");

            RuleFor(entity => entity.Name)
                .NotNull()
                .WithMessage(".نام مشخص نشده است");

            RuleFor(entity => entity.UserName)
                .NotNull()
                .WithMessage(".نام کاربری مشخص نشده است");

            RuleFor(entity => entity.UserRoles)
                .Must(x => x.Count != 0)
                .WithMessage(".نقش کاربری مشخص نشده است");

            RuleFor(entity => entity)
                .Must(entity => !IsEmailDuplicate(entity))
                .WithMessage(".ایمیل انتخاب شده قبلا در سیستم ثبت شده است");

            RuleFor(entity => entity)
                .Must(entity => !IsUserNameDuplicate(entity))
                .WithMessage(".نام کاربری انتخاب شده قبلا در سیستم ثبت شده است");

            RuleFor(entity => entity)
                .Must(entity => !IsMobileDuplicate(entity))
                .WithMessage(".شماره موبایل انتخاب شده قبلا در سیستم ثبت شده است");

        }

        private bool IsMobileDuplicate(NeutrinoUser entity)
        {
            var dbEntity = unitOfWork.UserDataService
                .GetQuery()
                .AsNoTracking()
                .Where(
                    gal => gal.MobileNumber == entity.MobileNumber
                    && gal.Deleted == false).FirstOrDefault();


            if (dbEntity == null)
                return false;
            return !(dbEntity.Id == entity.Id);
        }
        private bool IsUserNameDuplicate(NeutrinoUser entity)
        {
            //TODO shift to repo
            var dbEntity = unitOfWork.UserDataService
                .GetQuery()
                .AsNoTracking()
                .Where(
                    gal => gal.UserName == entity.UserName
                    && gal.Deleted == false).FirstOrDefault();


            if (dbEntity == null)
                return false;
            return !(dbEntity.Id == entity.Id);
        }

        #endregion

        #region [ Private Method(s) ]
        private bool IsEmailDuplicate(NeutrinoUser user)
        {
            var dbEntity = unitOfWork.UserDataService
                .GetQuery()
                .AsNoTracking()
                .Where(
                    gal => gal.Email == user.Email
                    && gal.Deleted == false).FirstOrDefault();


            if (dbEntity == null)
                return false;
            return !(dbEntity.Id == user.Id);
        }


        #endregion




    }
}
