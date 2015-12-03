using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AnyTrack.Infrastructure;
using AnyTrack.PlanningPoker.BackendPlanningPokerManagerService;
using AnyTrack.PlanningPoker.ServiceGateways;
using AnyTrack.SharedUtilities.Extensions;
using Prism.Regions;

namespace AnyTrack.PlanningPoker.Views
{
    /// <summary>
    /// The view model for the planning poker lobby.
    /// </summary>
    public class PokerLobbyViewModel : ValidatedBindableBase, INavigationAware
    {
        #region Fields 

        /// <summary>
        /// The planning poker service gateway.
        /// </summary>
        private readonly IPlanningPokerManagerServiceGateway gateway;

        /// <summary>
        /// A flag to indicate if the session has been joined.
        /// </summary>
        private bool sessionJoined = false; 

        /// <summary>
        /// A flag to indicate if the session has not been joined.
        /// </summary>
        private bool pendingSessionJoin = true;

        /// <summary>
        /// The lobby header text.
        /// </summary>
        private string lobbyHeaderText; 

        #endregion 

        #region Constructor 

        /// <summary>
        /// Constructs a new poker lobby view model
        /// </summary>
        /// <param name="gateway">The service gateway.</param>
        public PokerLobbyViewModel(IPlanningPokerManagerServiceGateway gateway)
        {
            if (gateway == null)
            {
                throw new ArgumentNullException("gateway");
            }

            this.gateway = gateway;

            this.Users = new ObservableCollection<ServicePlanningPokerUser>();
        }

        #endregion 

        #region Properties 

        /// <summary>
        /// Gets or sets the users in the session.
        /// </summary>
        public ObservableCollection<ServicePlanningPokerUser> Users { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the session has been joined.
        /// </summary>
        public bool SessionJoined
        {
            get
            {
                return sessionJoined; 
            }

            set
            {
                SetProperty(ref sessionJoined, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the session has been joined.
        /// </summary>
        public bool PendingSessionJoin
        {
            get
            {
                return pendingSessionJoin;
            }

            set
            {
                SetProperty(ref pendingSessionJoin, value);
            }
        }

        /// <summary>
        /// Gets or sets the lobby header text.
        /// </summary>
        public string LobbyHeaderText
        {
            get
            {
                return lobbyHeaderText;
            }

            set
            {
                SetProperty(ref lobbyHeaderText, value);
            }
        }

        #endregion 

        #region Methods

        /// <summary>
        /// Returns a true or false value indicating if this view model can be re-used in a navigation request.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns>A true or false value indicating whether or not the view can be re-used.</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false; 
        }

        /// <summary>
        /// Allows the view model to execute any logic required before leaving the view.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            gateway.NotifyClientOfSessionUpdateEvent -= HandleNotifyClientOfSessionUpdate;
        }

        /// <summary>
        /// Allows the view model to execute any required logic when it is first navigated to. 
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("sessionId"))
            {
                SessionJoined = false;
                PendingSessionJoin = true; 
                var sessionId = (Guid)navigationContext.Parameters["sessionId"];

                var needToJoin = navigationContext.Parameters.ContainsKey("joinRequired") ? (bool)navigationContext.Parameters["joinRequired"] : true;

                var session = needToJoin ? gateway.JoinSession(sessionId) : gateway.RetrieveSessionInfo(sessionId);

                gateway.NotifyClientOfSessionUpdateEvent += HandleNotifyClientOfSessionUpdate;

                UpdateVmGivenSession(session);
                SessionJoined = true;
                PendingSessionJoin = false; 
            }
        }

        /// <summary>
        /// Updates the view in resposne to session changes on the server.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args. </param>
        private void HandleNotifyClientOfSessionUpdate(object sender, ServicePlanningPokerSession e)
        {
            UpdateVmGivenSession(e);
        }

        #endregion 

        #region Helpers 

        /// <summary>
        /// Updates the view model state given the session.
        /// </summary>
        /// <param name="session">The session</param>
        private void UpdateVmGivenSession(ServicePlanningPokerSession session)
        {
            this.Users.Clear();
            this.Users.AddRange(session.Users);

            this.LobbyHeaderText = "{0} - {1}".Substitute(session.SprintName, session.ProjectName);
        }

        #endregion 
    }
}
