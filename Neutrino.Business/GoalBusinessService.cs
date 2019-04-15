
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Espresso.Core;
using Espresso.BusinessService;
using FluentValidation;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Neutrino.Data.EntityFramework;
using FluentValidation.Results;
using Espresso.Entites;

namespace Neutrino.Business
{
    public class GoalBusinessService : NeutrinoBSBase<Goal, IGoalDS>
    {
        #region [ Varibale(s) ]
        private GoalBusinessRules goalBusinessRules;
        #endregion

        #region [ Protected Property(ies) ]
        protected override AbstractValidator<Goal> businessRulesService
        {
            get { return goalBusinessRules; }
        }
        #endregion

        #region [ Constructor(s) ]
        public GoalBusinessService(ITransactionalData transactionalData)
            : base(transactionalData)
        {
            goalBusinessRules = new GoalBusinessRules(dataService);
        }
        #endregion

        #region [ Override Method(s) ]
        public override async Task DeleteAsync(Goal entity)
        {
            //Goal perviousGoal = await dataService.FirstOrDefaultAsync(where:
            //    x => !x.Deleted
            //    && x.EndDate < entity.StartDate
            //    && x.IsActive == false
            //    && x.GoalGoodsCategoryId == entity.GoalGoodsCategoryId
            //    && x.Id != entity.Id
            //    && x.GoalTypeId == entity.GoalTypeId
            //    , includes: x => new { x.GoalGoodsCategory, x.GoalSteps }
            //    , orderBy: x => x.OrderByDescending(y => y.DateCreated)
            //    );

            Goal dbGaol = dbContext.Goals
                        .Include(x => x.GoalGoodsCategory)
                        .Include(x => x.GoalSteps.Select(y => y.Items))
                        .Where(x => x.Id == entity.Id)
                        .FirstOrDefault();
            dbGaol.GoalGoodsCategory.Deleted = true;
            dbGaol.GoalSteps.ToList().ForEach(x =>
            {
                x.Deleted = true;
                x.Items.ToList().ForEach(i => { i.Deleted = true; });
            });
            dbGaol.Deleted = true;

            

            //if (perviousGoal != null)
            //{
            //    perviousGoal.IsActive = true;
            //    perviousGoal.GoalGoodsCategory.IsActive = true;
            //    perviousGoal.GoalSteps
            //        .ToList()
            //        .ForEach(x => x.IsActive = true);
            //}

            await base.DeleteAsync(dbGaol);
        }
        protected override async Task BeforeCreateAsync(ITransactionalData transactionalData, Goal entity)
        {
            try
            {
                if (entity.GoalTypeId == GoalTypeEnum.Supplier)
                {
                    AddSupplierGoalGoodsCategory(entity);
                }
                else if (entity.GoalGoodsCategoryTypeId == GoalGoodsCategoryTypeEnum.GeneralGoal)
                {
                    AddGeneralGoalGoodsCategory(entity);
                }
                await CheckPerviousGoalAsync(entity);
            }
            catch (Exception ex)
            {
                transactionalData.ReturnMessage.Clear();
                transactionalData.ReturnMessage.Add(ex.ToString());
                transactionalData.ReturnStatus = false;
            }

        }
        protected override async Task BeforeUpdateAsync(ITransactionalData transactionalData, Goal entity)
        {
            try
            {
                await CheckPerviousGoalAsync(entity);
            }
            catch (Exception ex)
            {
                transactionalData.ReturnMessage.Clear();
                transactionalData.ReturnMessage.Add(ex.ToString());
                transactionalData.ReturnStatus = false;
            }

        }

        #endregion

        #region [ Public Method(s) ]
        public async Task<Goal> LoadGoalAync(int goalId)
        {
            TransactionalData = TransactionalData.CreateInstance();
            Goal result = new Goal();
            try
            {
                result = await dbContext.Goals.Where(x => x.Id == goalId)
                    .Include(x => x.GoalSteps.Select(st => st.Items))
                    .Include(x => x.GoalGoodsCategory.GoodsCollection)
                    .Include(x => x.Company)
                    .FirstOrDefaultAsync();
                TransactionalData.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                TransactionalData.ReturnMessage.Add(errorMessage);
                TransactionalData.ReturnStatus = false;
            }
            return result;
        }

        #endregion

        #region [ Private Method(s) ]

        private void AddSupplierGoalGoodsCategory(Goal entity)
        {
            entity.GoalGoodsCategoryTypeId = GoalGoodsCategoryTypeEnum.Group;
            entity.GoalGoodsCategory = new GoalGoodsCategory();
            entity.GoalGoodsCategory.GoalGoodsCategoryTypeId = GoalGoodsCategoryTypeEnum.Group;
            entity.GoalGoodsCategory.GoalTypeId = GoalTypeEnum.Supplier;
            entity.GoalGoodsCategory.GoodsCollection = entity.GoodsSelectionList;
            if (entity.Company != null)
            {
                entity.GoalGoodsCategory.Name = entity.Company.FaName;
            }

            //entity.GoalGoodsCategory.GoodsCollection.ToList().ForEach(x =>
            //    dbContext.Entry<Goods>(x).State = EntityState.Unchanged);
            //dbContext.Entry<Company>(entity.Company).State = EntityState.Unchanged;
        }

        private void AddGeneralGoalGoodsCategory(Goal entity)
        {
            entity.GoalGoodsCategory = new GoalGoodsCategory();
            entity.GoalGoodsCategory.GoalGoodsCategoryTypeId = entity.GoalGoodsCategoryTypeId;
            entity.GoalGoodsCategory.GoalTypeId = GoalTypeEnum.Distributor;
            entity.GoalGoodsCategory.Name =  GoalGoodsCategoryTypeEnum.GeneralGoal.GetEnumDescription<GoalGoodsCategoryTypeEnum>();
        }

        private async Task CheckPerviousGoalAsync(Goal entity)
        {
            if (entity.GoalGoodsCategory == null)
            {
                entity.GoalGoodsCategory = await dbContext.GoalGoodsCategories
                    //.AsNoTracking()
                    .Where(x => x.Id == entity.GoalGoodsCategoryId)
                    .FirstOrDefaultAsync();
            }
            Goal perviousGoal = await dataService.FirstOrDefaultAsync(where: x => !x.Deleted
                && (x.EndDate == null || x.EndDate < entity.EndDate)
                && x.StartDate < entity.StartDate
                && x.IsActive
                && x.GoalGoodsCategoryId == entity.GoalGoodsCategoryId
                && x.GoalGoodsCategoryTypeId == entity.GoalGoodsCategoryTypeId
                && x.Id != entity.Id
                && x.GoalTypeId == entity.GoalTypeId
                , includes: x => new { x.GoalGoodsCategory, x.GoalSteps }
                );

            if (perviousGoal != null)
            {
                if (perviousGoal.EndDate.HasValue == false)
                    perviousGoal.EndDate = entity.StartDate.AddDays(-1);
                perviousGoal.IsActive = false;
                perviousGoal.GoalGoodsCategory.IsActive = false;
                perviousGoal.GoalSteps
                    .ToList()
                    .ForEach(x => x.IsActive = false);
            }
        }

        private void CheckforDelete(Goal entity)
        {
            if (entity.EndDate.HasValue)
            {
                var perviousGoal = dataService.Get(where: x => x.Deleted == false
                && x.StartDate < entity.StartDate
                && x.GoalGoodsCategoryTypeId == entity.GoalGoodsCategoryTypeId
               , orderBy: Utilities.GetOrderBy<Goal>("StartDate", "asc")).FirstOrDefault();

                var nextGoal = dataService.Get(where: x => x.Deleted == false
                   && x.StartDate > entity.EndDate
                   && x.GoalGoodsCategoryTypeId == entity.GoalGoodsCategoryTypeId
                   , orderBy: Utilities.GetOrderBy<Goal>("StartDate", "asc")).FirstOrDefault();

                if (perviousGoal != null && nextGoal != null)
                {
                    TransactionalData.ReturnStatus = false;
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("هدف انتخاب شده از جهت تاریخ در بین دو هدف زیر قرار دارد");
                    strBuilder.Append("</br>");
                    strBuilder.Append("  هدف قبل از هدف انتخاب شده ");
                    strBuilder.Append("</br>");
                    strBuilder.Append(String.Format(" تاریخ شروع :{0} -- تاریخ پایان :{1}"
                        , Utilities.ToPersianDate(perviousGoal.StartDate)
                        , Utilities.ToPersianDate(perviousGoal.EndDate)));
                    strBuilder.Append(" هدف بعد از هدف انتخاب شده ");
                    strBuilder.Append("</br>");
                    strBuilder.Append(String.Format(" تاریخ شروع :{0} -- تاریخ پایان :{1}"
                        , Utilities.ToPersianDate(nextGoal.StartDate)
                        , Utilities.ToPersianDate(nextGoal.EndDate)));
                    strBuilder.Append("<div class='row ls_divider'></div>");
                    strBuilder.Append(String.Format("اگر شما این هدف را حذف کنید ،محدوده تاریخی {0} تا  {1} هیچگونه هدفی برای دسته دارویی {0} وجود نخواهد داشت"
                        , Utilities.ToPersianDate(entity.StartDate)
                        , Utilities.ToPersianDate(entity.EndDate)
                        , new GoalGoodsCategoryType(entity.GoalGoodsCategoryTypeId).Description));
                    strBuilder.Append("<div class='row ls_divider'></div>");
                    strBuilder.Append("");
                    TransactionalData.ReturnMessage.Add(strBuilder.ToString());
                }
            }

        }
        private void CreateDistributor(Goal entity)
        {
            var checkBeforeStartDate = dataService.FirstOrDefault(where: x => x.Deleted == false
                            && x.StartDate > entity.StartDate
                            && x.GoalGoodsCategoryTypeId == entity.GoalGoodsCategoryTypeId
                            && x.CompanyId == null
                            && x.GoalTypeId == GoalTypeEnum.Distributor
                            , orderBy: Utilities.GetOrderBy<Goal>("StartDate", "asc"));
            if (checkBeforeStartDate != null)
            {
                if (entity.EndDate == null)
                {
                    //هدفی در بانک اطلاعاتی وجود دارد که تاریخ شروع هدف در حال ثبت کوچکتر از تاریخ شروع آن میباشد

                    entity.EndDate = checkBeforeStartDate.StartDate.AddDays(-1);
                    TransactionalData.ReturnMessage.Add(String.Format("تاریخ پایان هدف به دلیل وجود هدف دیگری بعد از تاریخ شروع با مقدار {0} مشخص شد "
                        , Utilities.ToPersianDate(entity.EndDate)));
                }
                else if (entity.EndDate > checkBeforeStartDate.StartDate)
                {
                    // هدف در حال ثبت با هدفی از سیستم محدوده تاریخی مشترکی دارند
                    TransactionalData.ReturnStatus = false;
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("هدف با تاریخ شروع " + Utilities.ToPersianDate(checkBeforeStartDate.StartDate));
                    if (checkBeforeStartDate.EndDate.HasValue)
                    {
                        strBuilder.Append(" و تاریخ پایان" + Utilities.ToPersianDate(checkBeforeStartDate.EndDate));
                    }
                    strBuilder.AppendLine("<br/>");
                    strBuilder.AppendLine("با هدف وارد شده دارای محدوده تاریخی مشترک میباشند .امکان ثبت وجود ندارد");

                    TransactionalData.ReturnMessage.Add(strBuilder.ToString());
                    return;
                }
            }

            var middelChecking = dataService.Get(where: x => x.Deleted == false
                && x.StartDate < entity.StartDate
                && x.EndDate > entity.StartDate
                && x.GoalGoodsCategoryTypeId == entity.GoalGoodsCategoryTypeId
                && x.CompanyId == null
                && x.GoalTypeId == GoalTypeEnum.Distributor
                , orderBy: Utilities.GetOrderBy<Goal>("StartDate", "asc")).FirstOrDefault();

            if (middelChecking != null)
            {
                middelChecking.EndDate = entity.StartDate.AddDays(-1);
                //dataService.Update(middelChecking);
            }

            var perviousGoal = dataService.Get(where: x => x.Deleted == false && x.EndDate == null
                && x.StartDate < entity.StartDate
                && x.GoalTypeId == GoalTypeEnum.Distributor
                && x.CompanyId == null).SingleOrDefault();

            if (perviousGoal != null)
            {
                perviousGoal.EndDate = entity.StartDate.AddDays(-1);
            }

            dataRepository.Insert(entity);
        }
        private async Task CheckforInsert(Goal entity)
        {
            var checkBeforeStartDate = await dataService.FirstOrDefaultAsync(where: x =>
                x.Deleted == false
                && x.StartDate > entity.StartDate
                && x.IsActive
                && x.GoalGoodsCategory.Name == entity.GoalGoodsCategory.Name
                , orderBy: Utilities.GetOrderBy<Goal>("StartDate", "asc")
                , includes: x => new { x.GoalGoodsCategory });
            if (checkBeforeStartDate != null)
            {
                TransactionalData.ReturnMessage.Add("به دلیل وجود هدفگذاری با تاریخ شروع بزرگتر از هدف وارد شده امکان ثبت وجود ندارد.");
                TransactionalData.ReturnStatus = false;
                return;
            }





            dataRepository.Insert(entity);
        }
        private async Task UpdateSupplier(ITransactionalData transactionalData, Goal entity)
        {
            var perviousEntity = await dataRepository.FirstOrDefaultAsync(where: x =>
                               entity.StartDate >= x.StartDate
                               && entity.StartDate <= x.EndDate
                               && x.Id != entity.Id
                               && x.CompanyId == entity.CompanyId
                               && x.Deleted == false);
            if (perviousEntity != null)
            {

                transactionalData.ReturnStatus = false;
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("هدف گذاری نمیتواند اشتراک تاریخی با اهداف دیگر داشته باشد ");
                strBuilder.Append("</br>");
                strBuilder.Append(string.Format("تاریخ شروع انتخاب شده در محدوده هدفگذاری با تاریخ شروع {0} و پایان {1} میباشد"
                    , Utilities.ToPersianDate(perviousEntity.StartDate), Utilities.ToPersianDate(perviousEntity.EndDate)));
                transactionalData.ReturnMessage.Add(strBuilder.ToString());
            }

            var nextEntity = await dataRepository.FirstOrDefaultAsync(where: x => entity.EndDate >= x.StartDate
            && entity.EndDate <= x.EndDate
            && x.Id != entity.Id
            && x.CompanyId == entity.CompanyId
            && x.Deleted == false);

            if (nextEntity != null)
            {
                transactionalData.ReturnStatus = false;
                StringBuilder strBuilder = new StringBuilder();
                if (perviousEntity == null)
                {
                    strBuilder.Append("هدف گذاری نمیتواند اشتراک تاریخی با اهداف دیگر داشته باشد ");
                    strBuilder.Append("</br>");
                }

                strBuilder.Append(string.Format("تاریخ پایان انتخاب شده در محدوده هدفگذاری با تاریخ شروع {0} و پایان {1} میباشد"
                    , Utilities.ToPersianDate(nextEntity.StartDate), Utilities.ToPersianDate(nextEntity.EndDate)));
                transactionalData.ReturnMessage.Add(strBuilder.ToString());

            }
        }
        private async Task UpdateDistributor(ITransactionalData transactionalData, Goal entity)
        {
            var perviousEntity = await dataRepository.FirstOrDefaultAsync(where: x =>
                               entity.StartDate >= x.StartDate
                               && entity.StartDate <= x.EndDate
                               && x.Id != entity.Id
                               && x.GoalGoodsCategoryTypeId == entity.GoalGoodsCategoryTypeId
                               && x.Deleted == false);

            if (perviousEntity != null)
            {
                transactionalData.ReturnStatus = false;
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("هدف گذاری نمیتواند اشتراک تاریخی با اهداف دیگر داشته باشد ");
                strBuilder.Append("</br>");
                strBuilder.Append(string.Format("تاریخ شروع انتخاب شده در محدوده هدفگذاری با تاریخ شروع {0} و پایان {1} میباشد"
                    , Utilities.ToPersianDate(perviousEntity.StartDate), Utilities.ToPersianDate(perviousEntity.EndDate)));
                transactionalData.ReturnMessage.Add(strBuilder.ToString());

            }

            var nextEntity = await dataRepository.FirstOrDefaultAsync(where: x => entity.EndDate >= x.StartDate
            && entity.EndDate <= x.EndDate
            && x.Id != entity.Id
            && x.GoalGoodsCategoryTypeId == entity.GoalGoodsCategoryTypeId
            && x.Deleted == false);

            if (nextEntity != null)
            {
                transactionalData.ReturnStatus = false;
                StringBuilder strBuilder = new StringBuilder();
                if (perviousEntity == null)
                {
                    strBuilder.Append("هدف گذاری نمیتواند اشتراک تاریخی با اهداف دیگر داشته باشد ");
                    strBuilder.Append("</br>");
                }

                strBuilder.Append(string.Format("تاریخ پایان انتخاب شده در محدوده هدفگذاری با تاریخ شروع {0} و پایان {1} میباشد"
                    , Utilities.ToPersianDate(nextEntity.StartDate), Utilities.ToPersianDate(nextEntity.EndDate)));
                transactionalData.ReturnMessage.Add(strBuilder.ToString());

            }
        }
        #endregion

    }
}
