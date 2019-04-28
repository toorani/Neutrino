using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Xml;
using System.Data.Entity.ModelConfiguration.Conventions;
using Espresso.Identity.Models;
using Neutrino.Entities;
using System.Linq;
using System.Data.Entity.Infrastructure.Interception;
using Espresso.Entites;
using System.Threading.Tasks;
using System;

namespace Neutrino.Data.EntityFramework
{
    public class NeutrinoContext : DbContext
    {
        #region [ Constructor(s) ]
        public NeutrinoContext()
            : this("name=DefaultConnection")
        {
            
        }

        public NeutrinoContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ValidateOnSaveEnabled = true;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<NeutrinoContext>(null);
            DbInterception.Add(new CommandInterceptor());
        }
        #endregion

        #region [ DbSets ]
        public DbSet<NeutrinoRole> Roles { get; set; }
        public DbSet<NeutrinoUser> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<AppMenu> AppMenuItems { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<GoalGoodsCategory> GoalGoodsCategories { get; set; }
        public DbSet<GoalGoodsCategoryType> GoalGoodsCategoryTypes { get; set; }
        public DbSet<Goods> Goods { get; set; }
        public DbSet<GoalStep> GoalSteps { get; set; }
        public DbSet<ComputingType> ComputingTypes { get; set; }
        public DbSet<RewardType> RewardTypes { get; set; }
        public DbSet<GoalStepItemInfo> GoalStepItemInfos { get; set; }
        public DbSet<GoalStepActionType> GoalStepActionTypes { get; set; }
        public DbSet<CondemnationType> CondemnationTypes { get; set; }
        public DbSet<OtherRewardType> OtherRewardTypes { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<BranchGoal> BranchGoals { get; set; }
        public DbSet<GoalType> GoalTypes { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<OrgStructure> OrgStructures { get; set; }
        public DbSet<OrgStructureShare> OrgStructureShare { get; set; }
        public DbSet<SupplierType> SupplierTypes { get; set; }
        public DbSet<CostCoefficient> CostCoefficients { get; set; }
        public DbSet<ApplicationAction> ApplicationActions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        //public DbSet<ServiceLog> ServiceLogs { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<PositionType> PositionTypes { get; set; }
        public DbSet<GoodsCategory> GoodsCategories { get; set; }
        public DbSet<GoodsCategoryType> GoodsCategoryTypes { get; set; }
        public DbSet<FulfillmentPercent> FulfillmentPercents { get; set; }
        public DbSet<BranchSales> BranchSales { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<MemberPayroll> Payrolls { get; set; }
        public DbSet<MemberReceipt> MemberReceipts { get; set; }
        public DbSet<BranchReceipt> BranchReceipts { get; set; }
        public DbSet<TherapeuticType> TherapeuticTypes { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DbSet<DataSyncStatus> DataSyncStatus { get; set; }
        public DbSet<QuantityCondition> QuantityConditions { get; set; }
        public DbSet<GoodsQuantityCondition> GoodsQuantityConditions { get; set; }
        public DbSet<BranchQuantityCondition> BranchQuantityConditions { get; set; }
        public DbSet<FulfillmentPromotionCondition> FulfillmentPromotionConditions { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionStatus> PromotionStatus { get; set; }
        public DbSet<BranchReceiptGoalPercent> BranchReceiptGoalPercents { get; set; }
        public DbSet<BranchQuntityGoal> BranchQuntityGoals { get; set; }
        public DbSet<BranchPromotion> BranchPromotions { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ApprovePromotionType> ApprovePromotionTypes { get; set; }
        public DbSet<QuantityConditionType> QuantityConditionTypes { get; set; }
        public DbSet<MemberSales> MemberSales { get; set; }
        public DbSet<MemberPromotion> MemberPromotions { get; set; }
        public DbSet<CustomerGoal> CustomerGoals { get; set; }
        #endregion

        #region [ Override Method(s) ]
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<AppMenu>()
                   .HasOptional(i => i.Parent)
                   .WithMany(i => i.ChildItems)
                   .HasForeignKey(i => i.ParentId);

            modelBuilder.Entity<AppSetting>()
                .HasOptional(i => i.Parent)
                .WithMany(i => i.Childern)
                .HasForeignKey(i => i.ParentId);


            modelBuilder.Entity<GoalStepItemInfo>()
                .HasRequired<GoalStep>(s => s.GoalStep)
                .WithMany(g => g.Items)
                .HasForeignKey<int>(i => i.GoalStepId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Goods>()
                .HasRequired<Company>(s => s.Company)
                .WithMany(g => g.GoodsCollection)
                .HasForeignKey<int>(i => i.CompanyId);

            modelBuilder.Entity<Goods>()
            .HasIndex(c => c.RefId)
            .IsUnique();



            //modelBuilder.Entity<User>()
            //    .HasMany<Role>(s => s.Roles)
            //    .WithMany(c => c.Users)
            //    .Map(cs =>
            //    {
            //        cs.MapLeftKey("UserId");
            //        cs.MapRightKey("RoleId");
            //    });

            modelBuilder.Entity<ApplicationAction>()
                   .HasOptional(i => i.Parent)
                   .WithMany(i => i.ChildActions)
                   .HasForeignKey(i => i.ParentId);

            
            

            modelBuilder.Entity<BranchGoal>()
                .Property(p => p.Percent)
                .HasPrecision(9, 5);

            modelBuilder.Entity<BranchReceiptGoalPercent>()
                .Property(p => p.NotReachedPercent)
                .HasPrecision(9, 5);

            modelBuilder.Entity<BranchReceiptGoalPercent>()
                .Property(p => p.ReachedPercent)
                .HasPrecision(9, 5);

            modelBuilder.Entity<BranchPromotion>()
                .Property(p => p.TotalSalesSpecifiedPercent)
                .HasPrecision(9, 5);

           
            modelBuilder.Entity<OrgStructureShare>()
                .Property(p => p.PrivateReceiptPercent)
                .HasPrecision(9, 5);
            modelBuilder.Entity<OrgStructureShare>()
                .Property(p => p.SalesPercent)
                .HasPrecision(9, 5);
            modelBuilder.Entity<OrgStructureShare>()
                .Property(p => p.TotalReceiptPercent)
                .HasPrecision(9, 5);
           




            modelBuilder.Entity<CondemnationType>().Ignore(enEn => enEn.Id);
            modelBuilder.Entity<RewardType>().Ignore(enEn => enEn.Id);
            modelBuilder.Entity<ComputingType>().Ignore(enEn => enEn.Id);
            modelBuilder.Entity<GoalGoodsCategoryType>().Ignore(enEn => enEn.Id);
            modelBuilder.Entity<OtherRewardType>().Ignore(enEn => enEn.Id);
            modelBuilder.Entity<GoalStepActionType>().Ignore(enEn => enEn.Id);
            modelBuilder.Entity<GoalType>().Ignore(enEn => enEn.Id);
            modelBuilder.Entity<SupplierType>().Ignore(enEn => enEn.Id);
            modelBuilder.Entity<PositionType>().Ignore(x => x.Id);
            modelBuilder.Entity<Goal>().Ignore(x => x.GoodsSelectionList);
            modelBuilder.Entity<CostCoefficient>().Ignore(x => x.Records);
            modelBuilder.Entity<TherapeuticType>().Ignore(x => x.Id);
            modelBuilder.Entity<CompanyType>().Ignore(enEn => enEn.Id);
            modelBuilder.Entity<PromotionStatus>().Ignore(x => x.Id);
            modelBuilder.Entity<ApprovePromotionType>().Ignore(x => x.Id);
            modelBuilder.Entity<QuantityConditionType>().Ignore(x => x.Id);
        }
        public override int SaveChanges()
        {
            SetLastUpdated();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync()
        {
            SetLastUpdated();
            return base.SaveChangesAsync();
        }

        private void SetLastUpdated()
        {
            ChangeTracker.Entries()
                .Where(t => t.State == EntityState.Modified)
                .ToList()
                .ForEach(x =>
                {
                    if (x.Entity is EntityBase)
                    {
                        ((EntityBase)x.Entity).LastUpdated = DateTime.Now;
                    }
                });
        }
        #endregion
    }


}