namespace Neutrino.DataAccess.Migrations
{
    using Entities;
    using Espresso.DataAccess;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.SqlServer;
    using System.Linq;


    internal sealed class Configuration : DbMigrationsConfiguration<Neutrino.Data.EntityFramework.NeutrinoContext>
    {
        public Configuration()
        {

            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("System.Data.SqlClient", new CustomSqlServerMigrationSqlGenerator());
        }

        protected override void Seed(Neutrino.Data.EntityFramework.NeutrinoContext context)
        {
            //  This method will be called after migrating to the latest version.
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();


            context.GoalGoodsCategoryTypes.SeedEnumValues<GoalGoodsCategoryType, GoalGoodsCategoryTypeEnum>(e => new GoalGoodsCategoryType(e));
            context.ComputingTypes.SeedEnumValues<ComputingType, ComputingTypeEnum>(e => new ComputingType(e));
            context.RewardTypes.SeedEnumValues<RewardType, RewardTypeEnum>(e => new RewardType(e));
            context.CondemnationTypes.SeedEnumValues<CondemnationType, CondemnationTypeEnum>(e => new CondemnationType(e));
            context.OtherRewardTypes.SeedEnumValues<OtherRewardType, OtherRewardTypeEnum>(e => new OtherRewardType(e));
            context.GoalStepActionTypes.SeedEnumValues<GoalStepActionType, GoalStepActionTypeEnum>(e => new GoalStepActionType(e));
            context.GoalTypes.SeedEnumValues<GoalType, GoalTypeEnum>(e => new GoalType(e));
            context.SupplierTypes.SeedEnumValues<SupplierType, SupplierTypeEnum>(e => new SupplierType(e));
            context.PositionTypes.SeedEnumValues<PositionType, PositionTypeEnum>(e => new PositionType(e));
            context.TherapeuticTypes.SeedEnumValues<TherapeuticType, TherapeuticTypeEnum>(e => new TherapeuticType(e));
            context.CompanyTypes.SeedEnumValues<CompanyType, CompanyTypeEnum>(e => new CompanyType(e));
            context.PromotionStatus.SeedEnumValues<PromotionStatus, PromotionStatusEnum>(e => new PromotionStatus(e));
            context.ApprovePromotionTypes.SeedEnumValues<ApprovePromotionType, ApprovePromotionTypeEnum>(e => new ApprovePromotionType(e));
            context.QuantityConditionTypes.SeedEnumValues<QuantityConditionType, QuantityConditionTypeEnum>(e => new QuantityConditionType(e));
            context.PromotionReviewStatus.SeedEnumValues<PromotionReviewStatus, PromotionReviewStatusEnum>(e => new PromotionReviewStatus(e));




            context.AppMenuItems.AddOrUpdate(
                x => new { x.Title, x.Url },
                new AppMenu { Title = "هدف گذاری تامین کننده", Icon = "fa fa-line-chart", Url = "/goal/supplier/index", OrderId = 1 },
                new AppMenu { Title = "هدف گذاری الیت دارو", Icon = "fa fa-line-chart", OrderId = 2 },

                new AppMenu { Title = "پورسانت", Icon = "fa fa-flag-checkered", OrderId = 5 },


                new AppMenu { Title = "گزارشات", Icon = "fa fa-list-alt", OrderId = 9 },
                new AppMenu { Title = "ضریب نوع محصول", Icon = "fa fa-medkit", Url = "/costCoefficient/index", OrderId = 10, Deleted = true },


                new AppMenu { Title = "اطلاعات پایه", Icon = "fa fa-book", OrderId = 20 },
                new AppMenu { Title = "امنیت", Icon = "fa fa-key", OrderId = 21 }
                );



            AppMenu securityItem = context.AppMenuItems.SingleOrDefault(x => x.Title == "امنیت");
            if (securityItem == null)
            {
                securityItem = context.AppMenuItems.Local.SingleOrDefault(x => x.Title == "امنیت");
            }

            context.AppMenuItems.AddOrUpdate(
                x => new { x.Title, x.Url },
                    new AppMenu
                    {
                        Title = "لیست کاربران",
                        Icon = "fa fa-group",
                        Url = "/account/registration/index",
                        OrderId = 1,
                        Parent = securityItem,
                        ParentId = securityItem.Id
                    },
                    new AppMenu
                    {
                        Title = "تعیین سطوح دسترسی",
                        Icon = "fa fa-magic",
                        Url = "/security/permission/item",
                        OrderId = 2,
                        Parent = securityItem,
                        ParentId = securityItem.Id
                    },
                    new AppMenu
                    {
                        Title = "مدیریت فعالیت ها",
                        Icon = "fa fa-magic",
                        Url = "/security/action/item",
                        OrderId = 3,
                        Parent = securityItem,
                        ParentId = securityItem.Id
                    },
                    new AppMenu
                    {
                        Title = "تغییر رمز عبور",
                        Icon = "fa fa-key",
                        Url = "/account/changePassword/item",
                        OrderId = 3,
                        Parent = securityItem,
                        ParentId = securityItem.Id
                    }
                );

            AppMenu basicInfoItem = context.AppMenuItems.SingleOrDefault(x => x.Title == "اطلاعات پایه");
            if (basicInfoItem == null)
            {
                basicInfoItem = context.AppMenuItems.Local.SingleOrDefault(x => x.Title == "اطلاعات پایه");
            }
            context.AppMenuItems.AddOrUpdate(
               x => new { x.Title, x.Url },
                   new AppMenu
                   {
                       Title = "ساختار سازمانی",
                       Icon = "fa fa-sitemap",
                       Url = "/orgStructure/item",
                       OrderId = 1,
                       Parent = basicInfoItem,
                       ParentId = basicInfoItem.Id
                   },
                   new AppMenu
                   {
                       Title = "سهم سمت های سازمانی",
                       Icon = "fa fa-percent",
                       Url = "/orgStructureShare/item",
                       OrderId = 2,
                       Parent = basicInfoItem,
                       ParentId = basicInfoItem.Id
                   },
                   new AppMenu
                   {
                       Title = "شرط تحقق و پورسانت",
                       Icon = "fa fa-percent",
                       Url = "/fulfillmentPromotionCondition/item",
                       OrderId = 3,
                       Parent = basicInfoItem,
                       ParentId = basicInfoItem.Id
                   }
               );



            AppMenu reportItem = context.AppMenuItems.SingleOrDefault(x => x.Title == "گزارشات");
            if (reportItem == null)
            {
                reportItem = context.AppMenuItems.Local.SingleOrDefault(x => x.Title == "گزارشات");
            }

            context.AppMenuItems.AddOrUpdate(
                x => new { x.Title, x.Url },
                    new AppMenu
                    {
                        Title = "عملکرد نهایی",
                        Icon = "fa fa-file-text-o",
                        Url = "/promotion/overviewrpt/index",
                        OrderId = 1,
                        Parent = reportItem,
                        ParentId = reportItem.Id
                    },
                    new AppMenu
                    {
                        Title = "عملکرداهداف فروش مراکز ",
                        Icon = "fa fa-file-text-o",
                        Url = "/promotion/branchsalesrpt/index",
                        OrderId = 2,
                        Parent = reportItem,
                        ParentId = reportItem.Id
                    },
                    new AppMenu
                    {
                        Title = "پورسانت مراکز از اهداف فروش",
                        Icon = "fa fa-file-text-o",
                        Url = "/promotion/branchsalesoverviewrpt/index",
                        OrderId = 3,
                        Parent = reportItem,
                        ParentId = reportItem.Id
                    },
                    new AppMenu
                    {
                        Title = "عملکرداهداف وصول",
                        Icon = "fa fa-file-text-o",
                        Url = "/promotion/branchreceiptrpt/index",
                        OrderId = 4,
                        Parent = reportItem,
                        ParentId = reportItem.Id
                    }
                );

            AppMenu eliteGoalItem = context.AppMenuItems.SingleOrDefault(x => x.Title == "هدف گذاری الیت دارو");
            if (eliteGoalItem == null)
            {
                eliteGoalItem = context.AppMenuItems.Local.SingleOrDefault(x => x.Title == "هدف گذاری الیت دارو");
            }

            context.AppMenuItems.AddOrUpdate(
                x => new { x.Title, x.Url },
                    new AppMenu
                    {
                        Title = "هدف کل",
                        Icon = "fa fa-flag-checkered",
                        Url = "/goal/elite/totalsales/index/",
                        OrderId = 1,
                        Parent = eliteGoalItem,
                        ParentId = eliteGoalItem.Id
                    },
                    new AppMenu
                    {
                        Title = "هدف تجمیعی",
                        Icon = "fa fa-plus-circle",
                        Url = "/goal/elite/aggregation/index",
                        OrderId = 2,
                        Parent = eliteGoalItem,
                        ParentId = eliteGoalItem.Id
                    },
                    new AppMenu
                    {
                        Title = "هدف گروهی / تکی ",
                        Icon = "fa fa-th-large",
                        Url = "/goal/elite/groupSingle/index",
                        OrderId = 3,
                        Parent = eliteGoalItem,
                        ParentId = eliteGoalItem.Id
                    },
                    new AppMenu
                    {
                        Title = "هدف وصول کل",
                        Icon = "fa fa-money",
                        Url = "/goal/elite/receipt/index/total",
                        OrderId = 4,
                        Parent = eliteGoalItem,
                        ParentId = eliteGoalItem.Id
                    },
                    new AppMenu
                    {
                        Title = "وصول خصوصی/دولتی",
                        Icon = "fa fa-money",
                        Url = "/goal/elite/receipt/index/prgv",
                        OrderId = 5,
                        Parent = eliteGoalItem,
                        ParentId = eliteGoalItem.Id
                    }
                );

            AppMenu promosionlItem = context.AppMenuItems.SingleOrDefault(x => x.Title == "پورسانت");
            if (promosionlItem == null)
            {
                promosionlItem = context.AppMenuItems.Local.SingleOrDefault(x => x.Title == "پورسانت");
            }

            context.AppMenuItems.AddOrUpdate(
                x => new { x.Title, x.Url },
                    new AppMenu
                    {
                        Title = "محاسبه پورسانت",
                        Icon = "fa fa-tachometer",
                        Url = "/promotion/index/",
                        OrderId = 1,
                        Parent = promosionlItem,
                        ParentId = promosionlItem.Id
                    },
                    new AppMenu
                    {
                        Title = "ضریب تحقق",
                        OrderId = 2,
                        Url = "/fulfillmentPercent/item",
                        Icon = "fa fa-money",
                        Parent = promosionlItem,
                        ParentId = promosionlItem.Id
                    },
                    new AppMenu
                    {
                        Title = "پرداخت و کسورات",
                        Icon = "fa fa-percent",
                        Url = "/penalty/index",
                        OrderId = 3,
                        Parent = promosionlItem,
                        ParentId = promosionlItem.Id
                    },
                    new AppMenu
                    {
                        Title = "سهم پرسنل مراکز",
                        Icon = "fa fa-group",
                        Url = "/promotion/branchShare/ceo/index",
                        OrderId = 4,
                        Parent = promosionlItem,
                        ParentId = promosionlItem.Id
                    },
                    new AppMenu
                    {
                        Title = "سهم پرسنل",
                        Icon = "fa fa-group",
                        Url = "/promotion/branchShare/manager/index",
                        OrderId = 5,
                        Parent = promosionlItem,
                        ParentId = promosionlItem.Id
                    },
                    new AppMenu
                    {
                        Title = "تایید نهایی پورسانت",
                        Icon = "fa fa-group",
                        Url = "/promotion/branchShare/reviewFinalStep/index",
                        OrderId = 6,
                        Parent = promosionlItem,
                        ParentId = promosionlItem.Id
                    }
                );

            context.ApplicationActions.AddOrUpdate(
                x => new { x.HtmlUrl, x.ActionUrl },
                new ApplicationAction { HtmlUrl = "/home" });


            var roles = new List<Role>()
            {
                new Role{Name="Admin",FaName= "مدیر سیستم", IsUsingBySystem = true},
                new Role{Name="CEO",FaName= "مدیر عامل", IsUsingBySystem = false},
                new Role{Name="ViceCEO",FaName= "قائم مقام مدیرعامل", IsUsingBySystem = false},
                new Role{Name="CommercialManager",FaName= "مدیر بازرگانی", IsUsingBySystem = false},
                new Role{Name="CommercialEmployee",FaName= "پرسنل بازرگانی", IsUsingBySystem = false},
                new Role{Name="FinancialManager",FaName= "مدیر مالی", IsUsingBySystem = false},
                new Role{Name="FinancialEmployee",FaName= "پرسنل مالی", IsUsingBySystem = false},
                new Role{Name="SalesEmployee",FaName= "پرسنل فروش", IsUsingBySystem = false},
                new Role{Name="ZoneSalesManager",FaName= "مدیر فروش مناطق", IsUsingBySystem = false},
                new Role{Name="BranchManager",FaName= "رئیس مرکز", IsUsingBySystem = false}
            };


            //UserRole userRole = new UserRole
            //{
            //    Role = new Role { Name = "Admin", IsUsingBySystem = true },
            //    User = new User
            //    {
            //        UserName = "admin",
            //        PasswordHash = "AKT/sERhGF/k9CnbDnrWhNeG5nuQZRuLFQAqLrqq8cHBvAQcEFaKO6Yma3kn5qW86g==",
            //        PhoneNumber = "09125139301",
            //        Name = "admin",
            //        LastName = "admin",
            //        Email = "elit.neutrino@gmail.com"
            //    }
            //};
            roles.ForEach(x =>
            {
                context.Roles.AddOrUpdate(r => r.Name, x);
            });



            //NeutrinoUser adminUser = new NeutrinoUser
            //{
            //    UserName = "admin",
            //    PasswordHash = "AKT/sERhGF/k9CnbDnrWhNeG5nuQZRuLFQAqLrqq8cHBvAQcEFaKO6Yma3kn5qW86g==",
            //    MobileNumber = "09125139301",
            //    Name = "admin",
            //    LastName = "admin",
            //    Email = "elit.neutrino@gmail.com"
            //};

            //userRole.Role = adminRole
            //adminUser.UserRoles.Add(adminRole);
            //context.Users.AddOrUpdate(x => x.UserName, adminUser);


        }

        internal class CustomSqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
        {
            protected override void Generate(AddColumnOperation addColumnOperation)
            {
                SetSqlFuncColumn(addColumnOperation.Column);
                base.Generate(addColumnOperation);
            }
            protected override void Generate(CreateTableOperation createTableOperation)
            {
                SetSqlFuncColumn(createTableOperation.Columns);

                base.Generate(createTableOperation);
            }
            private static void SetSqlFuncColumn(IEnumerable<ColumnModel> columns)
            {
                foreach (var columnModel in columns)
                {
                    SetSqlFuncColumn(columnModel);
                }
            }
            private static void SetSqlFuncColumn(PropertyModel column)
            {
                //if (System.Diagnostics.Debugger.IsAttached == false)
                //    System.Diagnostics.Debugger.Launch();
                if (column.Name == "DateCreated")
                {
                    column.DefaultValueSql = "GetDate()";
                }
                else if (column.Name == "Deleted")
                {
                    column.DefaultValueSql = "0";
                }
            }
        }


    }
}

