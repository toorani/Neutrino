using System.Web;
using System.Web.Mvc;
using Neutrino.Portal.Attributes;
using Neutrino.Portal.WebApiControllers;

namespace Neutrino.Portal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            
            //filters.Add(new LocalizationAttribute("fa"), 0);
        }
    }
}
