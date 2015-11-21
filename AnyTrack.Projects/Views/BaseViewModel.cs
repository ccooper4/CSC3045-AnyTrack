using System;
using AnyTrack.Projects.ServiceGateways;

namespace AnyTrack.Infrastructure
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
        public BaseViewModel(IProjectServiceGateway serviceGateway)
        {
            if (serviceGateway == null)
            {
                throw new ArgumentNullException("serviceGateway");
            }

            this.ServiceGateway = serviceGateway;
        }
    }
}
