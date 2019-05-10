using System.Web;
using System.Web.Optimization;

namespace Neutrino.Portal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/fickle/bootstrap-rtl.css",
                       "~/Content/fickle/style-rtl.css",
                       "~/Content/fickle/responsive-rtl.css",
                      "~/Content/site.css",
                      "~/Content/angular/angular-ui.min.css",
                      "~/Content/angular/angular-block-ui.min.css",
                      "~/Content/bootstrap/font-awesome.css",
                      "~/Content/ngToast/ngToast.css"
                      ));


            bundles.Add(new StyleBundle("~/Content/validation").Include(
                      "~/Content/validation/bootstrapValidator-rtl.css",
                      "~/Content/validation/validationEngine.jquery-rtl.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/plugins").Include(
                "~/Content/datatable/jquery.dataTables.min.css"
                , "~/Content/datatable/responsive.dataTables.min.css"
                , "~/Content/datatable/dataTables.bootstrap.min.css"
                , "~/Content/datatable/fixedColumns.dataTables.min.css"
                , "~/Content/datatable/fixedColumns.bootstrap.min.css"
                , "~/Content/datatable/fixedHeader.dataTables.min.css"
                , "~/Content/datatable/rowGroup.dataTables.min.css"
                , "~/Content/plugins/amaranjs/jquery.amaran-rtl.css"
                , "~/Content/plugins/jstree/style.min.css"
                , "~/Content/plugins/multiSelect/multiple-select.min.css"
                , "~/Content/plugins/datePicker/ADM-dateTimePicker.min.css"
                , "~/Content/plugins/ui-select/select.css"
                , "~/Content/plugins/ui-select/selectize.css"
                , "~/Content/plugins/slider/rzslider.min.css"
                , "~/Content/plugins/wizard/angular-wizard.min.css"
                ));



            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                     "~/Scripts/jquery/jquery-{version}.js"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/jquery/bootstrap.js",
                      "~/Scripts/jquery/respond.js"));


            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
               "~/Scripts/angular/angular.js",
               "~/Scripts/angular/angular-animate.min.js",
               "~/Scripts/angular/angular-sanitize.min.js",
               "~/Scripts/angular/angular-route.min.js",
               "~/Scripts/angular/angular-ui.min.js",
               "~/Scripts/angular/angular-block-ui.js",
               "~/Scripts/angular/angular-filter.js",
               "~/Scripts/ui-router/ui-router.min.js",
               "~/Scripts/ui-bootstrap/ui-bootstrap.min.js",
               "~/Scripts/ui-bootstrap/ui-bootstrap-tpls.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/jDataTables").Include(
               "~/Scripts/jquery.dataTables/media/js/jquery.dataTables.js"
               , "~/Scripts/jquery.dataTables/media/js/dataTables.bootstrap.js"
               , "~/Scripts/jquery.dataTables/media/js/dataTable.Plugin.fnReloadAjax.js"
               , "~/Scripts/jquery.dataTables/media/js/dataTables.responsive.min.js"
               , "~/Scripts/jquery.dataTables/media/js/dataTables.fixedColumns.min.js"
               , "~/Scripts/jquery.dataTables/media/js/dataTables.rowGroup.min.js"
               , "~/Scripts/jquery.dataTables/bootstrapPager.min.js"
               , "~/Scripts/jquery.dataTables/dataTables.fixedHeader.min.js"
            ));


            bundles.Add(new ScriptBundle("~/bundles/validation").Include(
                "~/Scripts/validationEngine/jquery/languages/jquery.validationEngine-fa.js"
                , "~/Scripts/validationEngine/jquery/jquery.validationEngine.js"
                , "~/Scripts/validationEngine/bootstrap/bootstrapValidator.js"
                ));



            bundles.Add(new ScriptBundle("~/bundles/fickle").Include(
               "~/Scripts/color.js",
               "~/Scripts/multipleAccordion.js",
               "~/Scripts/layout.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/jPlugins").Include(
               "~/Scripts/plugins/ngJsTree/jstree.min.js"
               , "~/Scripts/plugins/ngJsTree/ngJsTree.min.js"
               , "~/Scripts/plugins/datepicker/ADM-dateTimePicker.min.js"
               , "~/Scripts/plugins/number/dynamic-number.min.js"
               , "~/Scripts/plugins/multiSelect/multiple-select.min.js"
               , "~/Scripts/plugins/ngstrap/angular-strap.min.js"
               , "~/Scripts/plugins/ngstrap/angular-strap.tpl.min.js"
               , "~/Scripts/plugins/jslazyloading/ocLazyLoad.min.js"
               , "~/Scripts/plugins/ngToast/ngToast.min.js"
               , "~/Scripts/plugins/ui-select/select.js"
               , "~/Scripts/plugins/slider/rzslider.min.js"
               , "~/Scripts/plugins/ngsticky/sticky.min.js"
               , "~/Scripts/plugins/uploader/angular-file-upload.min.js"
               , "~/Scripts/plugins/wizard/angular-wizard.min.js"
               , "~/Scripts/plugins/persianDate/persian-date.min.js"
               , "~/Scripts/plugins/icheck/angular-icheck.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/shared").Include(
               "~/Views/Shared/fn.extend.js",
               "~/Views/Shared/neutrinoBootstrap.js",
               "~/Views/Shared/neutrinoFactories.js",
               "~/Views/Shared/neutrinoServices.js",
               "~/Views/Shared/neutrinoFilters.js",
               "~/Views/Shared/neutrinoDirectives.js",
               "~/Views/Shared/neutrinoFilters.js",
                "~/Views/Shared/espresso.angular.dataTabels.js",
                "~/Views/Shared/fn.validation.js",
               "~/Views/Shared/layoutController.js"
           ));



            bundles.Add(new ScriptBundle("~/bundles/routing-debug").Include(
              "~/Views/Shared/neutrinoRouting-debug.js"
          ));

            bundles.Add(new ScriptBundle("~/bundles/routing-production").Include(
             "~/Views/Shared/neutrinoRouting-production.js"
         ));

            //   bundles.Add(new ScriptBundle("~/bundles/login").Include(
            //       "~/Views/Account/LoginController.js"
            //));

            //bundles.Add(new ScriptBundle("~/bundles/home").Include(
            //    "~/Views/Home/IndexController.js"));


            //bundles.Add(new ScriptBundle("~/bundles/registration").Include(
            // "~/Views/account/index.registrationController.js",
            // "~/Views/account/item.registrationController.js"));

            //bundles.Add(new ScriptBundle("~/bundles/goal").Include(
            //   "~/Views/goal/index.distributorController.js",
            //   "~/Views/goal/item.distributorController.js",
            //   "~/Views/goal/index.supplierController.js",
            //   "~/Views/goal/item.supplierController.js"));

            //bundles.Add(new ScriptBundle("~/bundles/goalGoodsCategory").Include(
            //   "~/Views/goalGoodsCategory/index.GroupController",
            //   "~/Views/goalGoodsCategory/item.GroupController",
            //   "~/Views/goalGoodsCategory/item.SingleController"));

            //bundles.Add(new ScriptBundle("~/bundles/branchBenefit").Include(
            //   "~/Views/branchBenefit/index.multiController.js",
            //   "~/Views/branchBenefit/item.multiController.js",
            //   "~/Views/branchBenefit/index.singleController.js",
            //   "~/Views/branchBenefit/item.singleController.js"
            //   ));

            //bundles.Add(new ScriptBundle("~/bundles/orgStructure").Include(
            //   "~/Views/OrgStructure/itemController.js",
            //   "~/Views/OrgStructure/indexController.js"
            //   ));
            //bundles.Add(new ScriptBundle("~/bundles/personelShare").Include(
            //   "~/Views/personelShare/itemController.js",
            //   "~/Views/personelShare/indexController.js"
            //   ));

            //bundles.Add(new ScriptBundle("~/bundles/goalFulfillment").Include(
            //   "~/Views/goalFulfillment/indexController.js"
            //   ));

            //bundles.Add(new ScriptBundle("~/bundles/personelShare").Include(
            //    "~/Views/costCoefficient/indexController.js"));

            //bundles.Add(new ScriptBundle("~/bundles/servicesMonitoring").Include(
            //    "~/Views/servicesMonitoring/indexController.js"));

            //bundles.Add(new ScriptBundle("~/bundles/salesLegder").Include(
            //    "~/Views/salesLegder/Pharmacy/index.excelController.js"));
            //bundles.Add(new ScriptBundle("~/bundles/orderSchedule").Include(
            //    "~/Views/orderSchedule/index.pharmacyController.js"));
            //bundles.Add(new ScriptBundle("~/bundles/order").Include(
            //    "~/Views/order/index.pharmacyController.js"));

            //bundles.Add(new ScriptBundle("~/bundles/supplier").Include(
            //    "~/Views/supplier/index.promotionController.js"
            //    , "~/Views/supplier/index.pharmacyCreditController.js"
            //    , "~/Views/supplier/index.stockShareController.js"));
            ////
            //bundles.Add(new ScriptBundle("~/bundles/secuirty").Include(
            //    "~/Views/secuirty/item.actionController",
            //    "~/Views/secuirty/index.actionController"));

            //bundles.Add(new ScriptBundle("~/bundles/quantityConditions").Include(
            //   "~/Views/quantityConditions/indexController.js"
            //   , "~/Views/quantityConditions/qcGridDirective.js"
            //   ));
        }
    }
}
