using System.Web.Security;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Providers;
using AnyTrack.Backend.Security;
using AnyTrack.Backend.Service;
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

            // AnyTrack.Backend.Services 
            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<IProjectService, ProjectService>();

            // AnyTrack.Backend.Providers
            container.RegisterType<FormsAuthenticationProvider, FormsAuthenticationProvider>();
            container.RegisterType<Providers.RoleProvider, Providers.RoleProvider>();
            container.RegisterType<OperationContextProvider, OperationContextProvider>();
            container.RegisterType<ConnectedClientsProvider, ConnectedClientsProvider>(new ContainerControlledLifetimeManager());

            container.AddExtension(new BuildPrincipalUnityExtension());

            ((Providers.RoleProvider)Roles.Provider).UnitOfWork = container.Resolve<IUnitOfWork>();
        }
    }    
}