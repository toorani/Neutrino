using Espresso.Core.Ninject.Http;
using Espresso.DataAccess.Interfaces;
using Espresso.Entites;
using Neutrino.Entities;
using Neutrino.Interfaces;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

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
        public IEntityBaseRepository<AppMenu> AppMenuDataService { get; private set; }
        public IEntityBaseRepository<ApplicationAction> ApplicationActionDataService { get; private set; }
        public IEntityBaseRepository<AppSetting> AppSettingDataService { get; private set; }
        public IEntityBaseRepository<Branch> BranchDataService { get; private set; }
        public IBranchGoalDS BranchGoalDataService { get; private set; }
        public IBranchReceiptDS BranchReceiptDataService { get; private set; }
        public IBranchReceiptGoalPercentDS BranchReceiptGoalPercentDataService { get; private set; }
        public IBranchSalesDS BranchSalesDataService { get; private set; }
        public ICostCoefficientDS CostCoefficientDataService { get; private set; }
        public IGoalDS GoalDataService { get; private set; }
        public IFulfillmentPercentDS FulfillmentPercentDataService { get; private set; }
        public IGoalGoodsCategoryDS GoalGoodsCategoryDataService { get; private set; }
        public IEntityBaseRepository<GoalStep> GoalStepDataService { get; private set; }
        public IInvoiceDS InvoiceDataService { get; private set; }
        public IEntityBaseRepository<Member> MemberDataService { get; private set; }
        public IMemberPayrollDS MemberPayrollDataService { get; private set; }
        public IMemberReceiptDS MemberReceiptDataService { get; private set; }
        public IEntityBaseRepository<OrgStructure> OrgStructureDataService { get; private set; }
        public IOrgStructureShareDS OrgStructureShareDataService { get; private set; }
        public IEntityBaseRepository<Permission> PermissionDataService { get; private set; }
        public IEntityBaseRepository<FulfillmentPromotionCondition> TotalFulfillPromotionPercentDataService { get; private set; }
        public IEntityBaseRepository<Promotion> PromotionDataService { get; private set; }
        public IQuantityConditionDS QuantityConditionDataService { get; private set; }
        public IEntityBaseRepository<BranchQuntityGoal> BranchQuntityGoalDataService { get; private set; }
        public IDataRepository<User> UserDataService { get; private set; }
        public IDataRepository<UserRole> UserRoleDataService { get; private set; }
        public IDataRepository<UserClaim> UserClaimDataService { get; private set; }
        public IEntityBaseRepository<GoalStepItemInfo> GoalStepItemInfoDataService { get; private set; }
        public IEntityBaseRepository<BranchPromotion> BranchPromotionDataService { get; private set; }
        public IEntityBaseRepository<Company> CompanyDataService { get; private set; }
        public IEntityBaseRepository<DataSyncStatus> DataSyncStatusDataService { get; private set; }
        public IEntityBaseRepository<Goods> GoodsDataService { get; private set; }
        public IEntityBaseRepository<GoodsCategoryType> GoodsCategoryTypeDataService { get; private set; }
        public IEntityBaseRepository<GoodsCategory> GoodsCategoryDataService { get; private set; }
        public IEntityBaseRepository<GoalNonFulfillmentPercent> GoalNonFulfillmentPercentDataService { get; private set; }
        public IEntityBaseRepository<GoalNonFulfillmentBranch> GoalNonFulfillmentBranchDataService { get; private set; }
        public IEntityBaseRepository<GoalGoodsCategoryGoods> GoalGoodsCategoryGoodsDataService { get; private set; }
        public IEntityBaseRepository<BranchGoalPromotion> BranchGoalPromotionDataService { get; private set; }
        public IEntityBaseRepository<PositionReceiptPromotion> PositionReceiptPromotionDataService { get; private set; }
        public IEntityBaseRepository<PositionType> PositionTypeDataService { get; private set; }
        public IEntityBaseRepository<PositionMapping> PositionMappingDataService { get; private set; }
        public IEntityBaseRepository<SellerPromotion> SellerPromotionDataService { get; private set; }
        public IEntityBaseRepository<MemberPromotion> MemberPromotionDataService { get; private set; }
        public IEntityBaseRepository<MemberPenalty> MemberPenaltyDataService { get; private set; }
        public IEntityBaseRepository<ElitePosition> ElitePositionDataService { get; private set; }
        public IEntityBaseRepository<Department> DepartmentDataService { get; private set; }
        public IEntityBaseRepository<MemberPromotionDetail> MemberPromotionDetailDataService { get; private set; }
        public IEntityBaseRepository<OperationPromotion> OperationPromotionDataService { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public NeutrinoUnitOfWork(NeutrinoContext context
            , IEntityBaseRepository<AppMenu> appMenuDS
            , IEntityBaseRepository<ApplicationAction> applicationActionDS
            , IEntityBaseRepository<AppSetting> appSettingDS
            , IBranchReceiptDS branchReceiptDS
            , IBranchGoalDS branchGoalDS
            , IEntityBaseRepository<Branch> branchDS
            , IBranchReceiptGoalPercentDS branchReceiptGoalPercentDS
            , IBranchSalesDS branchSalesDS
            , ICostCoefficientDS costCoefficientDS
            , IGoalDS goalDS
            , IFulfillmentPercentDS goalFulfillmentDS
            , IGoalGoodsCategoryDS goalGoodsCategoryDS
            , IEntityBaseRepository<GoalStep> goalStepDS
            , IInvoiceDS invoiceDS
            , IMemberPayrollDS memberPayrollDS
            , IMemberReceiptDS memberReceiptDS
            , IEntityBaseRepository<OrgStructure> orgStructureDS
            , IOrgStructureShareDS orgStructureShareDS
            , IEntityBaseRepository<Permission> permissionDS
            , IEntityBaseRepository<FulfillmentPromotionCondition> totalFulfillPromotionPercentDS
            , IEntityBaseRepository<Promotion> promotionDS
            , IQuantityConditionDS quantityConditionDS
            , IEntityBaseRepository<BranchQuntityGoal> branchQuntityGoalDS
            , IDataRepository<User> userDS
            , IDataRepository<UserRole> userRoleDS
            , IDataRepository<UserClaim> userClaimDS
            , IEntityBaseRepository<GoalStepItemInfo> goalStepItemInfoDS
            , IEntityBaseRepository<BranchPromotion> branchPromotionDS
            , IEntityBaseRepository<Member> memberDS
            , IEntityBaseRepository<Company> companyDS
            , IEntityBaseRepository<DataSyncStatus> dataSyncStatusDS
            , IEntityBaseRepository<Goods> goodsDS
            , IEntityBaseRepository<GoodsCategoryType> goodsCategoryTypeDS
            , IEntityBaseRepository<GoodsCategory> goodsCategoryDS
            , IEntityBaseRepository<GoalNonFulfillmentPercent> goalNonFulfillmentPercentDS
            , IEntityBaseRepository<GoalNonFulfillmentBranch> goalNonFulfillmentBranchDS
            , IEntityBaseRepository<GoalGoodsCategoryGoods> goalGoodsCategoryGoodsDS
            , IEntityBaseRepository<BranchGoalPromotion> branchGoalPromotionDS
            , IEntityBaseRepository<PositionReceiptPromotion> positionReceiptPromotionDS
            , IEntityBaseRepository<PositionType> positionTypeDS
            , IEntityBaseRepository<PositionMapping> postionMappingDS
            , IEntityBaseRepository<SellerPromotion> sellerPromotionDS
            , IEntityBaseRepository<MemberPromotion> memberPromotionDS
            , IEntityBaseRepository<MemberPenalty> memberPenaltyDS
            , IEntityBaseRepository<ElitePosition> elitePostionDS
            , IEntityBaseRepository<Department> departmentDS
            , IEntityBaseRepository<MemberPromotionDetail> memberPromotionDetailDS
            , IEntityBaseRepository<OperationPromotion> operationPromotionDS
           )
        {
            this.context = context;
            AppMenuDataService = appMenuDS;
            AppSettingDataService = appSettingDS;
            ApplicationActionDataService = applicationActionDS;
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
            UserDataService = userDS;
            UserRoleDataService = userRoleDS;
            UserClaimDataService = userClaimDS;
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
            GoalGoodsCategoryGoodsDataService = goalGoodsCategoryGoodsDS;
            BranchGoalPromotionDataService = branchGoalPromotionDS;
            PositionReceiptPromotionDataService = positionReceiptPromotionDS;
            PositionTypeDataService = positionTypeDS;
            PositionMappingDataService = postionMappingDS;
            SellerPromotionDataService = sellerPromotionDS;
            MemberPromotionDataService = memberPromotionDS;
            MemberPenaltyDataService = memberPenaltyDS;
            ElitePositionDataService = elitePostionDS;
            DepartmentDataService = departmentDS;
            MemberPromotionDetailDataService = memberPromotionDetailDS;
            OperationPromotionDataService = operationPromotionDS;
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
        public IEntityBaseRepository<TEntity> GetRepository<TEntity>()
            where TEntity : EntityBase, new()
        {
            //return new NeutrinoRepositoryBase<TEntity>(context);

            return NinjectHttpContainer.Resolve<IEntityBaseRepository<TEntity>>();

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