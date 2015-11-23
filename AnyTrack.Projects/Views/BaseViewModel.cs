using System;
using AnyTrack.Infrastructure;
using AnyTrack.Projects.ServiceGateways;

namespace AnyTrack.Projects.Views
{
    /// <summary>
    /// BaseViewModel class
    /// </summary>
    public abstract class BaseViewModel : ValidatedBindableBase
    {
        #region Fields

        /// <summary>
        /// The project service gateway
        /// </summary>
        protected readonly IProjectServiceGateway ServiceGateway;

        #endregion
        
        /// <summary>
        /// Constructor for BaseViewModel
        /// </summary>
        /// <param name="serviceGateway">The Project Service Gateway</param>
        protected BaseViewModel(IProjectServiceGateway serviceGateway)
        {
            if (serviceGateway == null)
            {
                throw new ArgumentNullException("serviceGateway");
            }

            this.ServiceGateway = serviceGateway;
        }
    }
}
