using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.ServiceGateways;
using AnyTrack.PlanningPoker.BackendPlanningPokerManagerService;
using AnyTrack.PlanningPoker.ServiceGateways;
using AnyTrack.SharedUtilities.Extensions;
using Prism.Commands;
using Prism.Regions;
using SprintModels = AnyTrack.Infrastructure.BackendSprintService;

namespace AnyTrack.PlanningPoker.Views
{
    /// <summary>
    /// The view model that allows a developer to join a planning poker session.
    /// </summary>
    public class SearchForPlanningPokerSessionViewModel : ValidatedBindableBase, INavigationAware, IRegionMemberLifetime
    {
        #region Fields 

        /// <summary>
        /// The planning poker service gateway.
        /// </summary>
        private readonly IPlanningPokerManagerServiceGateway serviceGateway;

        /// <summary>
        /// The selected sprint id.
        /// </summary>
        private Guid sprintId;

        /// <summary>
        /// The session id.
        /// </summary>
        private Guid? sessionId;

        /// <summary>
        /// The session available message.
        /// </summary>
        private string sessionAvailableMessage; 

        /// <summary>
        /// The visibility field that controls the display of searching for a session. 
        /// </summary>
        private Visibility showSearchingForSession = Visibility.Visible;

        /// <summary>
        /// The visibility field that controls the dispaly of the session info.
        /// </summary>
        private Visibility showFoundSession = Visibility.Collapsed;

        #endregion 

        #region Constructor 

        /// <summary>
        /// Constructs a new instance of the search for planning poker view model with the specified dependencies.
        /// </summary>
        /// <param name="gateway">The service gateway.</param>
        public SearchForPlanningPokerSessionViewModel(IPlanningPokerManagerServiceGateway gateway)
        {
            if (gateway == null)
            {
                throw new ArgumentNullException("gateway");
            }

            this.serviceGateway = gateway;

            this.JoinPokerSession = new DelegateCommand(JoinAndNavigateToPokerSession);
        }

        #endregion 

        #region Properties 

        /// <summary>
        /// Gets or sets the visibility value for the searching section.
        /// </summary>
        public Visibility ShowSearchingForSesion
        {
            get
            {
                return showSearchingForSession; 
            }

            set
            {
                SetProperty(ref showSearchingForSession, value);
            }
        }

        /// <summary>
        /// Gets or sets the visibility value for the session found section.
        /// </summary>
        public Visibility ShowSessionFound
        {
            get
            {
                return showFoundSession; 
            }

            set
            {
                SetProperty(ref showFoundSession, value);
            }
        }

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

        /// <summary>
        /// Gets or sets the session available text.
        /// </summary>
        public string SessionAvailableText
        {
            get
            {
                return sessionAvailableMessage;
            }

            set
            {
                SetProperty(ref sessionAvailableMessage, value);
            }
        }

        /// <summary>
        /// Gets or sets the command used to join the poker session.
        /// </summary>
        public DelegateCommand JoinPokerSession { get; set; }

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
            serviceGateway.NotifyClientOfSessionEvent -= HandleNotifyClientOfSessionEvent;
        }

        /// <summary>
        /// Allows this view model to run any custom logic when the region system navigates to it.
        /// </summary>
        /// <param name="navigationContext">The current navigation context.</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("sprintId"))
            {
                sprintId = (Guid)navigationContext.Parameters["sprintId"];
                serviceGateway.SubscribeToNewSessionMessages(sprintId);
                serviceGateway.NotifyClientOfSessionEvent += HandleNotifyClientOfSessionEvent;
            }
        }

        /// <summary>
        /// Joins the selected poker session.
        /// </summary>
        private void JoinAndNavigateToPokerSession()
        {
        }

        /// <summary>
        /// Handles the "NotifyClientOfSessionEvent" event from the server. 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void HandleNotifyClientOfSessionEvent(object sender, ServiceSessionChangeInfo e)
        {
            if (e.SessionAvailable)
            {
                ShowSearchingForSesion = Visibility.Collapsed;
                ShowSessionFound = Visibility.Visible;
                SessionAvailableText = "There is a poker session available for project {0} and sprint {1} that you can now join.".Substitute(e.ProjectName, e.SprintName);
                sessionId = e.SessionId;
            }
            else
            {
                ShowSearchingForSesion = Visibility.Visible;
                ShowSessionFound = Visibility.Collapsed;
            }
        }

        #endregion 
    }
}
