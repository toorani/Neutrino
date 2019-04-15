using Espresso.Core;
using Espresso.DataAccess.Interfaces;

namespace Neutrino.DependencyResolver
{
    public class NinjectBindings : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<Data.EntityFramework.NeutrinoUnitOfWork>();
            Bind<ITransactionalData>().To<Core.TransactionalData>();
            Bind<Interfaces.ICompany>().To<Data.EntityFramework.DataServices.CompanyDataService>();
            Bind<Interfaces.IGoal>().To<Data.EntityFramework.DataServices.GoalDataService>();
            Bind<Interfaces.IGoalStep>().To<Data.EntityFramework.DataServices.GoalStepDataService>();
            Bind<Interfaces.IGoods>().To<Data.EntityFramework.DataServices.GoodsDataService>();
            Bind<Interfaces.IGoodsCategory>().To<Data.EntityFramework.DataServices.GoodsCategoryDataService>();
            Bind<Interfaces.IOtherRewardType>().To<Data.EntityFramework.DataServices.OtherRewardTypeDataService>();
            Bind<Interfaces.IRewardType>().To<Data.EntityFramework.DataServices.RewardTypeDataService>();
            Bind<Interfaces.IBranch>().To<Data.EntityFramework.DataServices.BranchDataService>();
            Bind<Interfaces.IBranchBenefit>().To<Data.EntityFramework.DataServices.BranchBenefitDataService>();
            Bind<Interfaces.INeutrinoUser>().To<Data.EntityFramework.DataServices.UserDataService>();
            Bind<Interfaces.INeutrinoRole>().To<Data.EntityFramework.DataServices.RoleDataService>();
            Bind<Interfaces.IAppSetting>().To<Data.EntityFramework.DataServices.AppSettingDataService>();
            Bind<Interfaces.IOrgStructure>().To<Data.EntityFramework.DataServices.OrgStructureDataService>();
            Bind<Interfaces.IPersonelShare>().To<Data.EntityFramework.DataServices.PersonelShareDataService>();
            Bind<Interfaces.ICostCoefficient>().To<Data.EntityFramework.DataServices.CostCoefficientDataService>();
            Bind<Interfaces.IApplicationAction>().To<Data.EntityFramework.DataServices.ApplicationActionDataService>();
            Bind<Interfaces.IPermission>().To<Data.EntityFramework.DataServices.PermissionDataService>();
            Bind<Interfaces.IServiceLog>().To<Data.EntityFramework.DataServices.ServiceLogDataService>();
        }
    }
    
}
