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
using Prism.Commands;
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

        /// <summary>
        /// A flag indicating if this user is the scrum master in this lobby. 
        /// </summary>
        private bool scrumMaster;

        /// <summary>
        /// A flag indicating if this user is a developer in this lobby. 
        /// </summary>
        private bool developer; 

        /// <summary>
        /// The session id.
        /// </summary>
        private Guid sessionId; 

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
            this.EndPokerSession = new DelegateCommand(EndCurrentPokerSession);
            this.ExitSession = new DelegateCommand(ExitCurrentPokerSession);
            this.StartSession = new DelegateCommand(StartCurrentSession);
        }

        #endregion 

        #region Properties 

        /// <summary>
        /// Gets or sets the users in the session.
        /// </summary>
        public ObservableCollection<ServicePlanningPokerUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the command used to end a poker session.
        /// </summary>
        public DelegateCommand EndPokerSession { get; set; }

        /// <summary>
        /// Gets or sets the exit session command.
        /// </summary>
        public DelegateCommand ExitSession { get; set; }

        /// <summary>
        /// Gets or sets the start session command.
        /// </summary>
        public DelegateCommand StartSession { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating whether or not this user is a developer.
        /// </summary>
        public bool Developer
        {
            get
            {
                return developer;
            }

            set
            {
                SetProperty(ref developer, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this user is a scrum master.
        /// </summary>
        public bool ScrumMaster
        {
            get
            {
                return scrumMaster;
            }
            
            set
            {
                SetProperty(ref scrumMaster, value);
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
            gateway.NotifyClientOfTerminatedSessionEvent -= HandleSessionTerminatedEvent;
            gateway.NotifyClientOfSessionStartEvent -= HandleNotifyClientOfSessionStartEvent;
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
                gateway.NotifyClientOfTerminatedSessionEvent += HandleSessionTerminatedEvent;
                gateway.NotifyClientOfSessionStartEvent += HandleNotifyClientOfSessionStartEvent;

                UpdateVmGivenSession(session);
                SessionJoined = true;
                PendingSessionJoin = false; 
            }
        }

        /// <summary>
        /// Handles the session started event. 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Any event args.</param>
        private void HandleNotifyClientOfSessionStartEvent(object sender, EventArgs e)
        {
            var navParams = new NavigationParameters();
            navParams.Add("sessionId", sessionId);

            this.NavigateToItem("PlanningPokerSession", navParams);
        }

        /// <summary>
        /// Handles the session termination event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void HandleSessionTerminatedEvent(object sender, EventArgs e)
        {
            this.ShowMetroDialog("Planning Poker Session Terminated!", "The planning poker session has been terminated.");
            this.NavigateToItem("MyProjects");
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

        /// <summary>
        /// Terminates the current planning poker session.
        /// </summary>
        private void EndCurrentPokerSession()
        {
            gateway.EndPokerSession(sessionId); 
            this.ShowMetroDialog("Planning poker session terminated!", "The planning poker session has been terminated!"); 
            this.NavigateToItem("StartPlanningPokerSession", null);
        }

        /// <summary>
        /// Leaves the current session.
        /// </summary>
        private void ExitCurrentPokerSession()
        {
            gateway.LeaveSession(sessionId);
            this.ShowMetroDialog("Left the Planning poker session!", "You have now left the session.");
            this.NavigateToItem("MyProjects", null);
        }

        /// <summary>
        /// Starts the current session.
        /// </summary>
        private void StartCurrentSession()
        {
            gateway.StartSession(sessionId);
            var navParams = new NavigationParameters();
            navParams.Add("sessionId", sessionId);

            this.NavigateToItem("PlanningPokerSession", navParams);
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

            this.sessionId = session.SessionID;

            this.LobbyHeaderText = "{0} - {1}".Substitute(session.SprintName, session.ProjectName);

            var myUserEmail = UserDetailsStore.LoggedInUserPrincipal.Identity.Name;

            ScrumMaster = session.Users.SingleOrDefault(u => u.EmailAddress == myUserEmail && u.UserID == session.HostID) != null;
            Developer = !ScrumMaster;
        }

        #endregion 
    }
}
