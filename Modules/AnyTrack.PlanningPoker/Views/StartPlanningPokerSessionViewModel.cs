using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.PlanningPoker.ServiceGateways;
using Prism.Regions;

namespace AnyTrack.PlanningPoker.Views
{
    /// <summary>
    /// The view model that allows a scrum master to start a planning poker session.
    /// </summary>
    public class StartPlanningPokerSessionViewModel : ValidatedBindableBase, INavigationAware, IRegionMemberLifetime
    {
        #region Fields 

        /// <summary>
        /// The planning poker service gateway.
        /// </summary>
        private IPlanningPokerManagerServiceGateway serviceGateway; 

        #endregion 

        #region Constructor 

        /// <summary>
        /// Constructs a new instance of the start planning poker view model with the specified dependencies.
        /// </summary>
        /// <param name="gateway">The service gateway.</param>
        public StartPlanningPokerSessionViewModel(IPlanningPokerManagerServiceGateway gateway)
        {
            if (gateway == null)
            {
                throw new ArgumentNullException("gateway");
            }

            this.serviceGateway = gateway;
        }

        #endregion 

        #region Properties 

        /// <summary>
        /// Gets a value indicating whether or not this view/view model should be reused.
        /// </summary>
        public bool KeepAlive
        {
            get
            {
                return false;
            }
        }

        #endregion 

        #region Methods 

        /// <summary>
        /// Allows this view model to specify if it can be re-used as a view model.
        /// </summary>
        /// <param name="navigationContext">The current navigation context.</param>
        /// <returns>A true or false value indicating if this view can be used as the new view.</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        /// <summary>
        /// Allows this view model to run any custom logic when the region system navigates from it.
        /// </summary>
        /// <param name="navigationContext">The current navigation context.</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows this view model to run any custom logic when the region system navigates to it.
        /// </summary>
        /// <param name="navigationContext">The current navigation context.</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        #endregion 
    }
}
