using AnyTrack.Backend.Data;
using Microsoft.Practices.Unity;
using Unity.Wcf;

namespace AnyTrack.Backend.WebHost
{
    /// <summary>
    /// Configures the factory used for WCF service Dependancy Injection.
    /// </summary>
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        /// <summary>
        /// Configures the unity container to resolve dependencies.
        /// </summary>
        /// <param name="container">The unity container.</param>
        protected override void ConfigureContainer(IUnityContainer container)
        {
            // register all your components with the container here
            // container
            //    .RegisterType<IService1, Service1>()
            //    .RegisterType<DataContext>(new HierarchicalLifetimeManager());

            // AnyTrack.Backend.Data 
            container.RegisterType<IUnitOfWork, EntityUnitOfWork>();
        }
    }    
}