using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Espresso.DataAccess.Interfaces;
using Espresso.DataAccess;
using Neutrino.Data.EntityFramework.DataServices;
using Neutrino.Interfaces;
using Espresso.Entites;
using Neutrino.Entities;
using Espresso.Core.Ninject;
using Espresso.Core.Ninject.Http;

namespace Neutrino.Data.EntityFramework
{
    public class NeutrinoUnitOfWork : IUnitOfWork
    {
        #region [ Varibale(s) ]
        private DbContextTransaction transaction;
        private bool disposed = false;
        public readonly NeutrinoContext context;

        #endregion

        #region [ Public Property(ies) ]
        public IEntityRepository<AppMenu> AppMenuDataService { get; private set; }
        public IEntityRepository<AppSetting> AppSettingDataService { get; private set; }
        public IEntityRepository<Branch> BranchDataService { get; private set; }
        public IBranchGoalDS BranchGoalDataService { get; private set; }
        public IBranchReceiptDS BranchReceiptDataService { get; private set; }
        public IBranchReceiptGoalPercentDS BranchReceiptGoalPercentDataService { get; private set; }
        public IBranchSalesDS BranchSalesDataService { get; private set; }
        public ICostCoefficientDS CostCoefficientDataService { get; private set; }
        public IGoalDS GoalDataService { get; private set; }
        public IFulfillmentPercentDS FulfillmentPercentDataService { get; private set; }
        public IGoalGoodsCategoryDS GoalGoodsCategoryDataService { get; private set; }
        public IEntityRepository<GoalStep> GoalStepDataService { get; private set; }
        public IInvoiceDS InvoiceDataService { get; private set; }
        public IEntityRepository<Member> MemberDataService { get; private set; }
        public IMemberPayrollDS MemberPayrollDataService { get; private set; }
        public IMemberReceiptDS MemberReceiptDataService { get; private set; }
        public IEntityRepository<OrgStructure> OrgStructureDataService { get; private set; }
        public IOrgStructureShareDS OrgStructureShareDataService { get; private set; }
        public IPermissionDS PermissionDataService { get; private set; }
        public IEntityRepository<FulfillmentPromotionCondition> TotalFulfillPromotionPercentDataService { get; private set; }
        public IEntityRepository<Promotion> PromotionDataService { get; private set; }
        public IQuantityConditionDS QuantityConditionDataService { get; private set; }
        public IEntityRepository<BranchQuntityGoal> BranchQuntityGoalDataService { get; private set; }
        public INeutrinoRoleDS RoleDataService { get; private set; }
        public INeutrinoUserDS UserDataService { get; private set; }
        public IEntityRepository<GoalStepItemInfo> GoalStepItemInfoDataService { get; private set; }
        public IEntityRepository<BranchPromotion> BranchPromotionDataService { get; private set; }
        public IEntityRepository<Company> CompanyDataService { get; private set; }
        public IEntityRepository<DataSyncStatus> DataSyncStatusDataService { get; private set; }
        public IEntityRepository<Goods> GoodsDataService { get; private set; }
        public IEntityRepository<GoodsCategoryType> GoodsCategoryTypeDataService { get; private set; }
        public IEntityRepository<GoodsCategory> GoodsCategoryDataService { get; private set; }
        public IEntityRepository<GoalNonFulfillmentPercent> GoalNonFulfillmentPercentDataService { get; private set; }
        public IEntityRepository<GoalNonFulfillmentBranch> GoalNonFulfillmentBranchDataService { get; private set; }

        #endregion

        #region [ Constructor(s) ]
        public NeutrinoUnitOfWork(NeutrinoContext context
            , IEntityRepository<AppMenu> appMenuDS
            , IEntityRepository<AppSetting> appSettingDS
            , IBranchReceiptDS branchReceiptDS
            , IBranchGoalDS branchGoalDS
            , IEntityRepository<Branch> branchDS
            , IBranchReceiptGoalPercentDS branchReceiptGoalPercentDS
            , IBranchSalesDS branchSalesDS
            , ICostCoefficientDS costCoefficientDS
            , IGoalDS goalDS
            , IFulfillmentPercentDS goalFulfillmentDS
            , IGoalGoodsCategoryDS goalGoodsCategoryDS
            , IEntityRepository<GoalStep> goalStepDS
            , IInvoiceDS invoiceDS
            , IMemberPayrollDS memberPayrollDS
            , IMemberReceiptDS memberReceiptDS
            , IEntityRepository<OrgStructure> orgStructureDS
            , IOrgStructureShareDS orgStructureShareDS
            , IPermissionDS permissionDS
            , IEntityRepository<FulfillmentPromotionCondition> totalFulfillPromotionPercentDS
            , IEntityRepository<Promotion> promotionDS
            , IQuantityConditionDS quantityConditionDS
            , IEntityRepository<BranchQuntityGoal> branchQuntityGoalDS
            , INeutrinoRoleDS roleDS
            , INeutrinoUserDS userDS
            , IEntityRepository<GoalStepItemInfo> goalStepItemInfoDS
            , IEntityRepository<BranchPromotion> branchPromotionDS
            , IEntityRepository<Member> memberDS
            , IEntityRepository<Company> companyDS
            , IEntityRepository<DataSyncStatus> dataSyncStatusDS
            , IEntityRepository<Goods> goodsDS
            , IEntityRepository<GoodsCategoryType> goodsCategoryTypeDS
            , IEntityRepository<GoodsCategory> goodsCategoryDS
            , IEntityRepository<GoalNonFulfillmentPercent> goalNonFulfillmentPercentDS
            , IEntityRepository<GoalNonFulfillmentBranch> goalNonFulfillmentBranchDS
           )
        {
            this.context = context;
            AppMenuDataService = appMenuDS;
            AppSettingDataService = appSettingDS;
            BranchReceiptDataService = branchReceiptDS;
            BranchGoalDataService = branchGoalDS;
            BranchDataService = branchDS;
            BranchReceiptGoalPercentDataService = branchReceiptGoalPercentDS;
            BranchSalesDataService = branchSalesDS;
            CostCoefficientDataService = costCoefficientDS;
            GoalDataService = goalDS;
            FulfillmentPercentDataService = goalFulfillmentDS;
            GoalGoodsCategoryDataService = goalGoodsCategoryDS;
            GoalStepDataService = goalStepDS;
            InvoiceDataService = invoiceDS;
            MemberPayrollDataService = memberPayrollDS;
            MemberReceiptDataService = memberReceiptDS;
            OrgStructureDataService = orgStructureDS;
            OrgStructureShareDataService = orgStructureShareDS;
            PermissionDataService = permissionDS;
            TotalFulfillPromotionPercentDataService = totalFulfillPromotionPercentDS;
            PromotionDataService = promotionDS;
            QuantityConditionDataService = quantityConditionDS;
            BranchQuntityGoalDataService = branchQuntityGoalDS;
            RoleDataService = roleDS;
            UserDataService = userDS;
            GoalStepItemInfoDataService = goalStepItemInfoDS;
            BranchPromotionDataService = branchPromotionDS;
            MemberDataService = memberDS;
            CompanyDataService = companyDS;
            DataSyncStatusDataService = dataSyncStatusDS;
            GoodsDataService = goodsDS;
            GoodsCategoryTypeDataService = goodsCategoryTypeDS;
            GoodsCategoryDataService = goodsCategoryDS;
            GoalNonFulfillmentPercentDataService = goalNonFulfillmentPercentDS;
            GoalNonFulfillmentBranchDataService = goalNonFulfillmentBranchDS;
        }
        #endregion

        #region [ Public Method(s) ]
        public int Commit(bool isCommitedDbTransaction = true)
        {
            if (transaction != null)
            {
                if (isCommitedDbTransaction)
                {
                    transaction.Commit();
                    transaction.Dispose();
                    return 0;
                }
                else
                {
                    context.ChangeTracker.DetectChanges();
                    return context.SaveChanges();
                }
            }
            context.ChangeTracker.DetectChanges();
            return context.SaveChanges();

        }
        public async Task<int> CommitAsync(bool isCommitedDbTransaction = true)
        {
            if (transaction != null)
            {
                if (isCommitedDbTransaction)
                {
                    await Task.Factory.StartNew(() =>
                    {
                        transaction.Commit();
                        transaction.Dispose();
                        return 0;
                    });

                }
                else
                {
                    context.ChangeTracker.DetectChanges();
                    return await context.SaveChangesAsync();
                }
            }
            context.ChangeTracker.DetectChanges();
            return await context.SaveChangesAsync();
        }
        public void RollBack()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
            }
        }
        public void StartTransction()
        {
            transaction = context.Database.BeginTransaction();
        }
        public IEntityRepository<TEntity> GetRepository<TEntity>()
            where TEntity : EntityBase, new()
        {
            //return new NeutrinoRepositoryBase<TEntity>(context);
            return NinjectHttpContainer.Resolve<IEntityRepository<TEntity>>();

        }
        #endregion

        #region [ IDisposable ]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                    if (transaction != null)
                    {
                        transaction.Dispose();
                    }
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }



        #endregion

    }
}