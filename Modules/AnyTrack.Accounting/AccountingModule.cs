using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Accounting.ServiceGateways;
using AnyTrack.Accounting.Views;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Infrastructure.Service;
using AnyTrack.Infrastructure.Service.Model;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace AnyTrack.Accounting
{
    /// <summary>
    /// Configures the Accounting Module. 
    /// </summary>
    public class AccountingModule : IModule
    { 
        #region Fields 

        /// <summary>
        /// The Unity Container. 
        /// </summary>
        private readonly IUnityContainer container;

        /// <summary>
        /// The region Manager.
        /// </summary>
        private readonly IRegionManager regionManager;

        /// <summary>
        /// The menu service.
        /// </summary>
        private readonly IMenuService menuService; 

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs the overall Accounting Module. 
        /// </summary>
        /// <param name="container">The Unity container</param>
        /// <param name="regionManager">The Prism region manager</param>
        /// <param name="menuService">The menu service</param>
        public AccountingModule(IUnityContainer container, IRegionManager regionManager, IMenuService menuService)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }

            if (menuService == null)
            {
                throw new ArgumentNullException("menuService");
            }

            this.container = container;
            this.regionManager = regionManager;
            this.menuService = menuService;
        }

        #endregion

        #region Methods 

        /// <summary>
        /// Performs any neccessary logic to initalize the module. 
        /// </summary>
        public void Initialize()
        {
            // AnyTrack.Accountng.ServiceReferences.BackendAccountService 
            container.RegisterType<IAccountService, AccountServiceClient>(new InjectionConstructor());

            // AnyTrack.Accounting.ServiceGateways
            container.RegisterType<IAccountServiceGateway, AccountServiceGateway>();

            // AnyTrack.Accounting.Views
            container.RegisterType<object, Registration>("Registration");
            container.RegisterType<object, Login>("Login");

            regionManager.RequestNavigate(RegionNames.AppContainer, "Login");
        }

        #endregion 
    }
}
