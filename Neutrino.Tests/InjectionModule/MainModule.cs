using System.Collections.Generic;
using Espresso.DataAccess.Interfaces;
using FluentValidation;
using Neutrino.Business;
using Neutrino.Data.EntityFramework.DataServices;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Ninject.MockingKernel.Moq;

namespace Neutrino.Portal.Tests
{
    //Main Module For Application
    public static class MainModule 
    {
        public static void Bind(MoqMockingKernel _kernel)
        {
            //Goal
            _kernel.Bind<IGoalBS>().To<GoalBS>();
            _kernel.Bind(typeof(AbstractValidator<Goal>)).To(typeof(GoalBR));
            _kernel.Bind<IGoalDS>().To<GoalDataService>();


            //GoalStep
            _kernel.Bind<IGoalStepBS>().To<GoalStepBS>();
            _kernel.Bind(typeof(IEntityRepository<GoalStep>)).To(typeof(GoalStepDataService));
            _kernel.Bind(typeof(AbstractValidator<GoalStep>)).To(typeof(GoalStepBusinessRules));



            //BranchGoal
            _kernel.Bind<IBranchGoalDS>().To<BranchGoalDataService>();
            _kernel.Bind<IBranchGoalBS>().To<BranchGoalBS>();
            _kernel.Bind(typeof(AbstractValidator<BranchGoalDTO>)).To(typeof(BranchGoalBatchRules));

            //CostCoefficient
            _kernel.Bind<ICostCoefficientDS>().To<CostCoefficientDataService>();
            _kernel.Bind<ICostCoefficientBS>().To<CostCoefficientBS>();
            _kernel.Bind(typeof(AbstractValidator<CostCoefficient>)).To(typeof(CostCoefficientBusinessRules));


            //Permission
            _kernel.Bind<IPermissionDS>().To<PermissionDataService>();
            _kernel.Bind<IPermissionBS>().To<PermissionBS>();

            //IGoalGoodsCategory
            _kernel.Bind<IGoalGoodsCategoryDS>().To<GoalGoodsCategoryDataService>();
            _kernel.Bind<IGoalGoodsCategoryBS>().To<GoalGoodsCategoryBS>();
            _kernel.Bind(typeof(AbstractValidator<GoalGoodsCategory>)).To(typeof(GoalGoodsCategoryBR));


            //QuantityCondition
            _kernel.Bind(typeof(AbstractValidator<QuantityCondition>)).To(typeof(QuantityConditionBR));
            _kernel.Bind<IQuantityConditionDS>().To(typeof(QuantityConditionDataService));
            _kernel.Bind<IQuantityConditionBS>().To<QuantityConditionBS>();

            //GoalFulfillment
            _kernel.Bind<IFulfillmentPercentDS>().To<FulfillmentPercentDataService>();
            _kernel.Bind<IFulfillmentPercentBS>().To<FulfillmentPercentBS>();
            _kernel.Bind(typeof(AbstractValidator<List<FulfillmentPercent>>)).To(typeof(FulfillmentPercentRules));


            //OrgStructure
            _kernel.Bind<IOrgStructureBS>().To<OrgStructureBS>();
            _kernel.Bind(typeof(AbstractValidator<List<OrgStructure>>)).To(typeof(OrgStructureCollectionBR));
            _kernel.Bind(typeof(AbstractValidator<OrgStructure>)).To(typeof(OrgStructureBR));


            //IPersonelShare
            _kernel.Bind<IOrgStructureShareDS>().To<OrgStructureShareDataService>();
            _kernel.Bind<IOrgStructureShareBS>().To<OrgStructureShareBS>();
            _kernel.Bind(typeof(AbstractValidator<OrgStructureShareDTO>)).To(typeof(OrgStructureShareDTOBR));

            //User account
            _kernel.Bind<INeutrinoUserDS>().To<UserDataService>();
            _kernel.Bind<INeutrinoUserBS>().To<NeutrinoUserBS>();
            _kernel.Bind(typeof(AbstractValidator<NeutrinoUser>)).To(typeof(UserBusinessRules));
            _kernel.Bind<INeutrinoRoleDS>().To<RoleDataService>();
            _kernel.Bind<INeutrinoRoleBS>().To<NeutrinoRoleBS>();

            //TotalFulfillPromotionPercent
            _kernel.Bind<IFulfillmentPromotionConditionBS>().To<FulfillmentPromotionConditionBS>();
            _kernel.Bind(typeof(AbstractValidator<List<FulfillmentPromotionCondition>>)).To(typeof(FulfillmentPromotionConditionListBR));

            //Promotion
            _kernel.Bind<IPromotionBS>().To<PromotionBS>();
            _kernel.Bind(typeof(AbstractValidator<Promotion>)).To(typeof(PromotionBR));

            //BranchReceiptGoalPercent
            _kernel.Bind<IBranchReceiptGoalPercentBS>().To<BranchReceiptGoalPercentBS>();
            _kernel.Bind(typeof(AbstractValidator<BranchReceiptGoalPercentDTO>)).To(typeof(BranchReceiptGoalPercentBR));
            _kernel.Bind<IBranchReceiptGoalPercentDS>().To<BranchReceiptGoalPercentDataService>();
            
        }
    }
}
