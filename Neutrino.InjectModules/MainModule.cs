using Espresso.DataAccess.Interfaces;
using FluentValidation;
using Neutrino.Business;
using Neutrino.Data.EntityFramework.DataServices;
using Neutrino.Entities;
using Neutrino.Interfaces;
using Ninject.Modules;
using System.Collections.Generic;

namespace Neutrino.InjectModules
{
    //Main Module For Application
    public class MainModule : NinjectModule
    {
        public override void Load()
        {
            //Goal
            Bind<IGoalBS>().To<GoalBS>();
            Bind(typeof(AbstractValidator<Goal>)).To(typeof(GoalBR));
            Bind<IGoalDS>().To<GoalDataService>();
            

            //GoalStep
            Bind<IGoalStepBS>().To<GoalStepBS>();
            Bind(typeof(IEntityBaseRepository<GoalStep>)).To(typeof(GoalStepDataService));
            Bind(typeof(AbstractValidator<GoalStep>)).To(typeof(GoalStepBusinessRules));



            //BranchGoal
            Bind<IBranchGoalDS>().To<BranchGoalDataService>();
            Bind<IBranchGoalBS>().To<BranchGoalBS>();
            Bind(typeof(AbstractValidator<BranchGoalDTO>)).To(typeof(BranchGoalBatchRules));

            //CostCoefficient
            Bind<ICostCoefficientDS>().To<CostCoefficientDataService>();
            Bind<ICostCoefficientBS>().To<CostCoefficientBS>();
            Bind(typeof(AbstractValidator<CostCoefficient>)).To(typeof(CostCoefficientBusinessRules));


            //Permission
            //Bind<IPermissionDS>().To<PermissionDataService>();
            Bind<IPermissionBS>().To<PermissionBS>();

            //IGoalGoodsCategory
            Bind<IGoalGoodsCategoryDS>().To<GoalGoodsCategoryDataService>();
            Bind<IGoalGoodsCategoryBS>().To<GoalGoodsCategoryBS>();
            Bind(typeof(AbstractValidator<GoalGoodsCategory>)).To(typeof(GoalGoodsCategoryBR));


            //QuantityCondition
            Bind(typeof(AbstractValidator<QuantityCondition>)).To(typeof(QuantityConditionBR));
            Bind<IQuantityConditionDS>().To(typeof(QuantityConditionDataService));
            Bind<IQuantityConditionBS>().To<QuantityConditionBS>();

            //GoalFulfillment
            Bind<IFulfillmentPercentDS>().To<FulfillmentPercentDataService>();
            Bind<IFulfillmentPercentBS>().To<FulfillmentPercentBS>();
            Bind(typeof(AbstractValidator<List<FulfillmentPercent>>)).To(typeof(FulfillmentPercentRules));


            //OrgStructure
            Bind<IOrgStructureBS>().To<OrgStructureBS>();
            Bind(typeof(AbstractValidator<List<OrgStructure>>)).To(typeof(OrgStructureCollectionBR));
            Bind(typeof(AbstractValidator<OrgStructure>)).To(typeof(OrgStructureBR));


            //IPersonelShare
            Bind<IOrgStructureShareDS>().To<OrgStructureShareDataService>();
            Bind<IOrgStructureShareBS>().To<OrgStructureShareBS>();
            Bind(typeof(AbstractValidator<OrgStructureShareDTO>)).To(typeof(OrgStructureShareDTOBR));

            //User account
            Bind<IUserBS>().To<UserBS>();
            Bind(typeof(AbstractValidator<User>)).To(typeof(UserBusinessRules));

            //TotalFulfillPromotionPercent
            Bind<IFulfillmentPromotionConditionBS>().To<FulfillmentPromotionConditionBS>();
            Bind(typeof(AbstractValidator<List<FulfillmentPromotionCondition>>)).To(typeof(FulfillmentPromotionConditionListBR));

            //Promotion
            Bind<IPromotionBS>().To<PromotionBS>();
            Bind(typeof(AbstractValidator<Promotion>)).To(typeof(PromotionBR));
            

            //BranchReceiptGoalPercent
            Bind<IBranchReceiptGoalPercentBS>().To<BranchReceiptGoalPercentBS>();
            Bind(typeof(AbstractValidator<BranchReceiptGoalPercentDTO>)).To(typeof(BranchReceiptGoalPercentBR));
            Bind<IBranchReceiptGoalPercentDS>().To<BranchReceiptGoalPercentDataService>();

            //GoalNonFulfillmentPercent
            Bind<IGoalNonFulfillmentPercentBS>().To<GoalNonFulfillmentPercentBS>();
            Bind(typeof(IEntityBaseRepository<GoalNonFulfillmentPercent>)).To(typeof(GoalNonFulfillmentPercentDataService));
            Bind(typeof(AbstractValidator<GoalNonFulfillmentPercent>)).To(typeof(GoalNonFulfillmentPercentBR));

            //AppMenu
            Bind<IAppMenuBS>().To<AppMenuBS>();

            //MemberSharePromotion
            Bind<IMemberSharePromotionBS>().To<MemberSharePromotionBS>();
            Bind(typeof(AbstractValidator<MemberSharePromotion>)).To(typeof(MemberSharePromotionBR));
            Bind(typeof(AbstractValidator<List<MemberSharePromotion>>)).To(typeof(MemberSharePromotionCollectionBR));
            

            //MemberPenalty
            Bind<IMemberPenaltyBS>().To<MemberPenaltyBS>();
            Bind(typeof(AbstractValidator<List<MemberPenalty>>)).To(typeof(MemberPenaltyCollectionBR));

            //PositionMapping
            Bind<IPositionMappingBS>().To<PositionMappingBS>();

            //Service data

            //BranchReceipt
            Bind<IBranchReceiptBS>().To<BranchReceiptBS>();
            Bind<IBranchReceiptDS>().To<BranchReceiptDataService>();

            //BranchSales
            Bind<IBranchSalesBS>().To<BranchSalesBS>();
            Bind<IBranchSalesDS>().To<BranchSalesDataService>();

            //Invoice
            Bind<IInvoiceBS>().To<InvoiceBS>();
            Bind<IInvoiceDS>().To<InvoiceDataService>();

            //MemberReceipt
            Bind<IMemberReceiptBS>().To<MemberReceiptBS>();
            Bind<IMemberReceiptDS>().To<MemberReceiptDataService>();

            //MemberPayroll
            Bind<IMemberPayrollBS>().To<MemberPayrollBS>();
            Bind<IMemberPayrollDS>().To<MemberPayrollDataService>();

            //ApplicationAction
            Bind<IApplicationActionBS>().To<ApplicationActionBS>();
            Bind(typeof(AbstractValidator<List<ApplicationAction>>)).To(typeof(ApplicationActionBR));


        }
    }
}
