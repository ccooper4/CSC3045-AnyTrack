using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Accounting.Views;
using AnyTrack.Infrastructure;
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

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs the overall Accounting Module. 
        /// </summary>
        /// <param name="container">The Unity container</param>
        /// <param name="regionManager">The Prism region manager</param>
        public AccountingModule(IUnityContainer container, IRegionManager regionManager)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }

            this.container = container;
            this.regionManager = regionManager;
        }

        #endregion

        #region Methods 

        /// <summary>
        /// Performs any neccessary logic to initalize the module. 
        /// </summary>
        public void Initialize()
        {
            container.RegisterType<object, Registration>("Registration");

            regionManager.RequestNavigate(RegionNames.AppContainer, "Registration");
        }

        #endregion 
    }
}
