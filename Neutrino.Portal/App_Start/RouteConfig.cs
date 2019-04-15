using System.Web.Mvc;
using System.Web.Routing;

namespace Neutrino.Portal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "HomeCatchAllRoute",
                url: "Home/{*.}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "RegistrationCatchAllRoute",
               url: "Account/registration/{*.}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "LoginCatchAllRoute",
                url: "Account/Login",
                defaults: new { controller = "Account", action = "Login", returnUrl = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "AccountCatchAllRoute",
               url: "Account/{*.}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "GoalCatchAllRoute",
                url: "Goal/{*.}",
                defaults: new { controller = "Home", action = "Index", returnUrl = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "GoalGoodsCategoryCatchAllRoute",
                url: "GoalGoodsCategory/{*.}",
                defaults: new { controller = "Home", action = "Index", returnUrl = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "BranchGoalCatchAllRoute",
                url: "BranchGoal/{*.}",
                defaults: new { controller = "Home", action = "Index", returnUrl = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "OrgStructureCatchAllRoute",
                url: "OrgStructure/{*.}",
                defaults: new { controller = "Home", action = "Index", returnUrl = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "OrgStructureShareCatchAllRoute",
                url: "orgStructureShare/{*.}",
                defaults: new { controller = "Home", action = "Index", returnUrl = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CostCoefficientCatchAllRoute",
                url: "CostCoefficient/{*.}",
                defaults: new { controller = "Home", action = "Index", returnUrl = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SecurityCatchAllRoute",
                url: "Security/{*.}",
                defaults: new { controller = "Home", action = "Index", returnUrl = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ServicesMonitoringCatchAllRoute",
                url: "servicesMonitoring/{*.}",
                defaults: new { controller = "Home", action = "Index", returnUrl = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "FulfillmentPercentCatchAllRoute",
                url: "fulfillmentPercent/{*.}",
                defaults: new { controller = "Home", action = "Index", returnUrl = UrlParameter.Optional });

            routes.MapRoute(
                name: "QuantityConditionsCatchAllRoute",
                url: "quantityConditions/{*.}",
                defaults: new { controller = "Home", action = "Index", returnUrl = UrlParameter.Optional });


            routes.MapRoute(
                name: "PromotionCatchAllRoute",
                url: "promotion/{*.}",
                defaults: new { controller = "Home", action = "Index", returnUrl = UrlParameter.Optional });

            routes.MapRoute(
                name: "BranchReceiptGoalPercentCatchAllRoute",
                url: "branchReceiptGoalPercent/{*.}",
                defaults: new { controller = "Home", action = "Index", returnUrl = UrlParameter.Optional });

            routes.MapRoute(
                name: "SharePromotionCatchAllRoute"
                , url: "sharePromotion/{*.}"
                , defaults: new { controller = "Home", action = "Index", returnUrl = UrlParameter.Optional });

            routes.MapRoute(
                name: "PenaltyCatchAllRoute"
                , url: "penalty/{*.}"
                , defaults: new { controller = "Home", action = "Index", returnUrl = UrlParameter.Optional });

            routes.MapRoute(
                name: "GoalNonFulfillmentCatchAllRoute"
                , url: "goalNonFulfillment/{*.}"
                , defaults: new { controller = "Home", action = "Index", returnUrl = UrlParameter.Optional });

            routes.MapRoute(
                name: "FulfillmentPromotionConditionCatchAllRoute"
                , url: "fulfillmentPromotionCondition/{*.}"
                , defaults: new { controller = "Home", action = "Index", returnUrl = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });


        }
    }
}
