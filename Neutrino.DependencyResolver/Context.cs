using System;
using System.Configuration;
using System.Reflection;
using Ninject;
using Ninject.Modules;

namespace Neutrino.DependencyResolver
{
    public sealed class Context
    {
        private static volatile Context instance;
        private static readonly object padlock = new object();
        private readonly StandardKernel kernel = null;

        public static Context Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        try
                        {
                            instance = new Context();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    return instance;
                }

            }
        }

        internal Context()
        {
            string bindingModulesName = ConfigurationManager.AppSettings["BindingNinjectModule"];
            if (!String.IsNullOrWhiteSpace(bindingModulesName))
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                NinjectModule bindingModulesObj = (NinjectModule)assembly.CreateInstance(bindingModulesName, true);
                if (bindingModulesObj != null)
                {
                    kernel = new StandardKernel(bindingModulesObj);
                }
            }
            
        }

        public TService GetService<TService>()
        {
            if (kernel == null)
                return (TService)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(TService));
            return kernel.Get<TService>();
        }
    }

}
