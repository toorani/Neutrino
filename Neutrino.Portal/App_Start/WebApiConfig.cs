using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using jQuery.DataTables.WebApi;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using Espresso.Portal;
using Neutrino.Portal.WebApiControllers;
using Neutrino.Portal.Attributes;
using System.Web.Http.ExceptionHandling;
using Neutrino.Portal.ExceptionHandling;

namespace Neutrino.Portal
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var provider = new SimpleModelBinderProvider(typeof(JQueryDataTablesModel), new JQueryDataTablesModelBinder());
            var generalProvider = new SimpleModelBinderProvider(typeof(ViewModelBase), new GeneralModelBinder());
            config.Services.Insert(typeof(ModelBinderProvider), 0, generalProvider);
            config.Services.Insert(typeof(ModelBinderProvider), 1, provider);

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            config.Filters.Add(new WebApiValidateAntiForgeryTokenAttribute());
            //config.Filters.Add(new ExcptionHandelFilterAttribute());
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.Services.Replace(typeof(IExceptionLogger), new GlobalExceptionLogger());
            //config.Filters.Add(new ApiAuthorizeAttribute());
            
        }
    }
}
