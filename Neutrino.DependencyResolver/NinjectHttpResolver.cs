using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Ninject.Modules;


namespace Ninject.Http
{
    /// <summary>
    /// Resolves Dependencies Using Ninject
    /// </summary>
    public class NinjectHttpResolver : IDependencyResolver, IDependencyScope
    {
        public IKernel Kernel { get; private set; }
        public NinjectHttpResolver(params NinjectModule[] modules)
        {
            var settings = new NinjectSettings();
            settings.LoadExtensions = false;
            Kernel = new StandardKernel(settings, modules);
        }

        public NinjectHttpResolver(Assembly assembly)
        {
            var settings = new NinjectSettings();
            settings.LoadExtensions = false;
            Kernel = new StandardKernel(settings);
            Kernel.Load(assembly);
        }

        public object GetService(Type serviceType)
        {
            return Kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Kernel.GetAll(serviceType);
        }

        public void Dispose()
        {
            //Do Nothing
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }
    }


    /// <summary>
    /// Its job is to Register Ninject Modules and Resolve Dependencies
    /// </summary>
    public class NinjectHttpContainer
    {
        private static NinjectHttpResolver _resolver;

        //Register Ninject Modules
        public static void RegisterModules(NinjectModule[] modules)
        {
            _resolver = new NinjectHttpResolver(modules);
            GlobalConfiguration.Configuration.DependencyResolver = _resolver;
        }

        public static void RegisterAssembly()
        {
            _resolver = new NinjectHttpResolver(Assembly.GetExecutingAssembly());
            //This is where the actual hookup to the Web API Pipeline is done.
            GlobalConfiguration.Configuration.DependencyResolver = _resolver;
        }

        //Manually Resolve Dependencies
        public static T Resolve<T>()
        {
            return _resolver.Kernel.Get<T>();
        }
    }
}