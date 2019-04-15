[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Neutrino.Portal.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Neutrino.Portal.App_Start.NinjectWebCommon), "Stop")]

namespace Neutrino.Portal.App_Start
{
    using System;
    using System.Web;
    using Espresso.Core;
    using Espresso.DataAccess.Interfaces;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Settings.InjectNonPublic = true;
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                
                RegisterServices(kernel);
                System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new Ninject.Web.WebApi.NinjectDependencyResolver(kernel);
                
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IUnitOfWork>().To<Data.EntityFramework.NeutrinoUnitOfWork>();
            kernel.Bind<ITransactionalData>().To<Core.TransactionalData>();
            //kernel.Bind<AppSettingManager>().ToProvider(new AppSettingManagerProvider());
            //kernel.Bind<PermissionManager>().ToProvider(new PermissionManagerProvider());
            //kernel.Load(new List<INinjectModule>() { new DataServiceModule() });

            kernel.Bind<Interfaces.IAppMenu>().To<Data.EntityFramework.DataServices.AppMenuDataService>();
            kernel.Bind<Interfaces.ICondemnationType>().To<Data.EntityFramework.DataServices.CondemnationTypeDataService>();
            kernel.Bind<Interfaces.ICompany>().To<Data.EntityFramework.DataServices.CompanyDataService>();
            kernel.Bind<Interfaces.IGoal>().To<Data.EntityFramework.DataServices.GoalDataService>();
            kernel.Bind<Interfaces.IGoalStep>().To<Data.EntityFramework.DataServices.GoalStepDataService>();
            kernel.Bind<Interfaces.IGoods>().To<Data.EntityFramework.DataServices.GoodsDataService>();
            kernel.Bind<Interfaces.IGoalGoodsCategory>().To<Data.EntityFramework.DataServices.GoalGoodsCategoryDataService>();
            kernel.Bind<Interfaces.IOtherRewardType>().To<Data.EntityFramework.DataServices.OtherRewardTypeDataService>();
            kernel.Bind<Interfaces.IRewardType>().To<Data.EntityFramework.DataServices.RewardTypeDataService>();
            kernel.Bind<Interfaces.IBranch>().To<Data.EntityFramework.DataServices.BranchDataService>();
            kernel.Bind<Interfaces.IBranchBenefit>().To<Data.EntityFramework.DataServices.BranchBenefitDataService>();
            kernel.Bind<Interfaces.INeutrinoUser>().To<Data.EntityFramework.DataServices.UserDataService>();
            kernel.Bind<Interfaces.INeutrinoRole>().To<Data.EntityFramework.DataServices.RoleDataService>();
            kernel.Bind<Interfaces.IAppSetting>().To<Data.EntityFramework.DataServices.AppSettingDataService>();
            kernel.Bind<Interfaces.IOrgStructure>().To<Data.EntityFramework.DataServices.OrgStructureDataService>();
            kernel.Bind<Interfaces.IPersonelShare>().To<Data.EntityFramework.DataServices.PersonelShareDataService>();
            kernel.Bind<Interfaces.ICostCoefficient>().To<Data.EntityFramework.DataServices.CostCoefficientDataService>();
            kernel.Bind<Interfaces.IApplicationAction>().To<Data.EntityFramework.DataServices.ApplicationActionDataService>();
            kernel.Bind<Interfaces.IPermission>().To<Data.EntityFramework.DataServices.PermissionDataService>();
            //kernel.Bind<Interfaces.IServiceLog>().To<Data.EntityFramework.DataServices.ServiceLogDataService>();
            kernel.Bind<Interfaces.IGoodsCategoryType>().To<Data.EntityFramework.DataServices.GoodsCategoryTypeDataService>();
            kernel.Bind<Interfaces.IGoodsCategory>().To<Data.EntityFramework.DataServices.GoodsCategoryDataService>();
            kernel.Bind<Interfaces.IGoalFulfillment>().To<Data.EntityFramework.DataServices.GoalFulfillmentDataService>();
            kernel.Bind<Interfaces.IGoodsScore>().To<Data.EntityFramework.DataServices.GoodsScoreDataService>();
            kernel.Bind<Interfaces.IGoodsPromotion>().To<Data.EntityFramework.DataServices.GoodsPromotionDataService>();
            kernel.Bind<Interfaces.ITherapeuticType>().To<Data.EntityFramework.DataServices.TherapeuticTypeDataService>();
            kernel.Bind<Interfaces.ICompanyType>().To<Data.EntityFramework.DataServices.CompanyTypeDataService>();
            kernel.Bind<Interfaces.IStockInventory>().To<Data.EntityFramework.DataServices.StockInventoryDataService>();
            kernel.Bind<Interfaces.ISalesLedger>().To<Data.EntityFramework.DataServices.SalesLedgerDataService>();
            kernel.Bind<Interfaces.IUploadedFile>().To<Data.EntityFramework.DataServices.UploadedFileDataService>();
        }
    }
}
