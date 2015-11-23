using System;
using AnyTrack.Accounting.ServiceGateways;
using AnyTrack.Infrastructure;
using Prism.Regions;

namespace AnyTrack.Accounting.Views
{
    /// <summary>
    /// BaseViewModel class
    /// </summary>
    public abstract class BaseViewModel : ValidatedBindableBase
    {
        #region Fields

        /// <summary>
        /// The region manager.
        /// </summary>
        protected readonly IRegionManager RegionManager;

        /// <summary>
        /// The project service gateway
        /// </summary>
        protected readonly IAccountServiceGateway ServiceGateway;

        #endregion

        /// <summary>
        /// Constructor for BaseViewModel
        /// </summary>
        /// <param name="serviceGateway">The Account Service Gateway</param>
        /// <param name="regionManager">The Region Manager</param>
        protected BaseViewModel(IAccountServiceGateway serviceGateway, IRegionManager regionManager)
        {
            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }

            if (serviceGateway == null)
            {
                throw new ArgumentNullException("serviceGateway");
            }

            this.RegionManager = regionManager;
            this.ServiceGateway = serviceGateway;
        }
    }
}
