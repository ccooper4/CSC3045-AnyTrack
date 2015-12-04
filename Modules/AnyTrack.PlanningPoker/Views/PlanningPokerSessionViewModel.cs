using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.Extensions;
using AnyTrack.Infrastructure.ServiceGateways;
using AnyTrack.PlanningPoker;
using AnyTrack.PlanningPoker.BackendPlanningPokerManagerService;
using AnyTrack.PlanningPoker.ServiceGateways;
using AnyTrack.SharedUtilities.Extensions;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Regions;

namespace AnyTrack.PlanningPoker.Views
{
    /// <summary>
    /// PlanningPokerSession view model class
    /// </summary>
    public class PlanningPokerSessionViewModel : ValidatedBindableBase, INavigationAware, IRegionMemberLifetime
    {
        #region Fields 

        /// <summary>
        /// The planning poker service gateway.
        /// </summary>
        private readonly IPlanningPokerManagerServiceGateway serviceGateway;

        /// <summary>
        /// The specified message to send.
        /// </summary>
        private string messageToSend;
        
        /// <summary>
        /// Is the user the scrum master
        /// </summary>
        private bool isScrumMaster = false;

        /// <summary>
        /// Is the user a developer? 
        /// </summary>
        private bool isDeveloper = false; 

        /// <summary>
        /// Should the estimates be shown
        /// </summary>
        private bool showEstimates = false;

        /// <summary>
        /// Should the estimates be hidden
        /// </summary>
        private bool hideEstimates = true;
        
        /// <summary>
        /// The specified sessionID for this chat.
        /// </summary>
        private Guid sessionId;

        /// <summary>
        /// The specified estimate to send.
        /// </summary>
        private string estimateToSend;

        /// <summary>
        /// A value indicating if estimates can be shown.
        /// </summary>
        private bool canShowEstimates = true;

        /// <summary>
        /// A value indicating if a final estimate can be provided. 
        /// </summary>
        private bool canGiveFinalEstimate = false;

        /// <summary>
        /// The active story.
        /// </summary>
        private ServiceSprintStory activeStory;

        /// <summary>
        /// The progress label. 
        /// </summary>
        private string totalStoriesLabel;

        /// <summary>
        /// The final selected estimate.
        /// </summary>
        private string selectedFinalEstimate;

        /// <summary>
        /// The final active story index.
        /// </summary>
        private int activeStoryIndex; 

        #endregion

        #region Constructor 

        /// <summary>
        /// ChatViewModel constructor.
        /// </summary>
        /// <param name="gateway">The service gateway.</param>
        public PlanningPokerSessionViewModel(IPlanningPokerManagerServiceGateway gateway)
        {
            if (gateway == null)
            {
                throw new ArgumentNullException("gateway");
            }
            
            this.serviceGateway = gateway;

            this.SprintStoriesCollection = new ObservableCollection<ServiceSprintStory>();

            this.MessageHistories = new ObservableCollection<string>();

            this.RecievedEstimates = new ObservableCollection<ServicePlanningPokerEstimate>();

            this.Users = new ObservableCollection<ServicePlanningPokerUser>();

            SendMessageCommand = new DelegateCommand(SubmitMessageToServer);

            ShowEstimatesCommand = new DelegateCommand(ShowUserEstimates);

            SendEstimateCommand = new DelegateCommand<string>(SubmitEstimateToServer);

            SubmitFinalEstimateCommand = new DelegateCommand(SubmitFinalEstimateToServer);

            EndSessionCommand = new DelegateCommand(EndCurrentSession);

            LeaveSessionCommand = new DelegateCommand(LeaveCurrentSession);
        }               

        #endregion

        #region Properties 

        /// <summary>
        /// Gets or sets the stories
        /// </summary>
        public ObservableCollection<ServiceSprintStory> SprintStoriesCollection { get; set; }

        /// <summary>
        /// Gets or sets the history of messages.
        /// </summary>
        public ObservableCollection<string> MessageHistories { get; set; }

        /// <summary>
        /// Gets or sets the history of recieved estimates for story points.
        /// </summary>
        public ObservableCollection<ServicePlanningPokerEstimate> RecievedEstimates { get; set; }

        /// <summary>
        /// Gets or sets the collection of users in this session,
        /// </summary>
        public ObservableCollection<ServicePlanningPokerUser> Users { get; set; }

        /// <summary>
        /// Gets the command used to send a message from a user. 
        /// </summary>
        public DelegateCommand SendMessageCommand { get; private set; }

        /// <summary>
        /// Gets the command used to send a message from a user. 
        /// </summary>
        public DelegateCommand ShowEstimatesCommand { get; private set; }

        /// <summary>
        /// Gets the command used to send a an estimate from client to server. 
        /// </summary>
        public DelegateCommand<string> SendEstimateCommand { get; private set; }

        /// <summary>
        /// Gets the command used for a SM to end the session.
        /// </summary>
        public DelegateCommand EndSessionCommand { get; private set; }

        /// <summary>
        /// Gets the command used by a developer to leave the session
        /// </summary>
        public DelegateCommand LeaveSessionCommand { get; private set; }

        /// <summary>
        /// Gets the command used to submit a final estimate for a story.
        /// </summary>
        public DelegateCommand SubmitFinalEstimateCommand { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not this view can be re-used.
        /// </summary>
        public bool KeepAlive
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets message property.
        /// </summary>
        public string MessageToSend
        {
            get
            {
                return messageToSend;
            }

            set
            {
                SetProperty(ref messageToSend, value);
            }
        }

        /// <summary>
        /// Gets or sets value of the final estimate. 
        /// </summary>
        public string EstimateToSend
        {
            get
            {
                return estimateToSend;
            }

            set
            {
                SetProperty(ref estimateToSend, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user is scrum master
        /// </summary>
        public bool IsScrumMaster
        {
            get
            {
                return isScrumMaster;
            }

            set
            {
                SetProperty(ref isScrumMaster, value);
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether or not this user is a developer.
        /// </summary>
        public bool IsDeveloper
        {
            get
            {
                return isDeveloper;
            }

            set
            {
                SetProperty(ref isDeveloper, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the estimates
        /// </summary>
        public bool ShowEstimates
        {
            get
            {
                return showEstimates;
            }

            set
            {
                SetProperty(ref showEstimates, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to hide the estimates
        /// </summary>
        public bool HideEstimates
        {
            get
            {
                return hideEstimates;
            }

            set
            {
                SetProperty(ref hideEstimates, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the scrum master can show the estimates. 
        /// </summary>
        public bool CanShowEstimates
        {
            get
            {
                return canShowEstimates; 
            }

            set
            {
                SetProperty(ref canShowEstimates, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the scrum master can give a final estimate. 
        /// </summary>
        public bool CanGiveFinalEstimate
        {
            get
            {
                return canGiveFinalEstimate; 
            }

            set
            {
                SetProperty(ref canGiveFinalEstimate, value);
            }
        }

        /// <summary>
        /// Gets or sets the active story.
        /// </summary>
        public ServiceSprintStory ActiveStory
        {
            get
            {
                return activeStory;
            }

            set
            {
                SetProperty(ref activeStory, value);
            }
        }

        /// <summary>
        /// Gets or sets the display text for the story progress label.
        /// </summary>
        public string TotalStoriesLabel
        {
            get
            {
                return totalStoriesLabel;
            }

            set
            {
                SetProperty(ref totalStoriesLabel, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected final estimate.
        /// </summary>
        public string SelectedFinalEstimate
        {
            get
            {
                return selectedFinalEstimate;
            }

            set
            {
                SetProperty(ref selectedFinalEstimate, value);
            }
        }

        #endregion 

        #region Methods

        /// <summary>
        /// Allows the model to verify if it can be re-used.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns>Returns a boolean flag.</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        /// <summary>
        /// Allows the view model to handle the on navigated from event.
        /// </summary>
        /// <param name="navigationContext">The navigation context</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            serviceGateway.NotifyClientOfNewMessageFromServerEvent -= ServiceGateway_NotifyClientOfNewMessageFromServerEvent;
            serviceGateway.NotifyClientOfSessionUpdateEvent -= ServiceGateway_NotifyClientOfSessionUpdateEvent;
            serviceGateway.NotifyClientOfTerminatedSessionEvent -= ServiceGateway_NotifyClientOfTerminatedSessionEvent;
        }

        /// <summary>
        /// Allows the view model to handle the on navigated to event.
        /// </summary>
        /// <param name="navigationContext">The navigation context</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("sessionId"))
            {
                sessionId = (Guid)navigationContext.Parameters["sessionId"];
                var session = serviceGateway.RetrieveSessionInfo(sessionId);

                var userEmail = UserDetailsStore.LoggedInUserPrincipal.Identity.Name;
                var sessionUser = session.Users.Single(u => u.EmailAddress == userEmail);
                if (session.HostID == sessionUser.UserID)
                {
                    IsScrumMaster = true; 
                }
                else
                {
                    IsDeveloper = true; 
                }

                SprintStoriesCollection.AddRange(session.Stories);

                serviceGateway.NotifyClientOfNewMessageFromServerEvent += ServiceGateway_NotifyClientOfNewMessageFromServerEvent;
                serviceGateway.NotifyClientOfSessionUpdateEvent += ServiceGateway_NotifyClientOfSessionUpdateEvent;
                serviceGateway.NotifyClientOfTerminatedSessionEvent += ServiceGateway_NotifyClientOfTerminatedSessionEvent;

                UpdateViewGivenSession(session);
            }
        }

        /// <summary>
        /// Handles the session terminated event from the server. 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Any event args.</param>
        private void ServiceGateway_NotifyClientOfTerminatedSessionEvent(object sender, EventArgs e)
        {
            this.ShowMetroDialog("Planning Poker Session Terminated!", "The planning poker session has been terminated.");
            this.NavigateToItem("MyProjects");
        }

        /// <summary>
        /// Handles the session changed event from the server.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The session.</param>
        private void ServiceGateway_NotifyClientOfSessionUpdateEvent(object sender, ServicePlanningPokerSession e)
        {
            UpdateViewGivenSession(e);
        }

        /// <summary>
        /// Duplex binding for recieving messages from server
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="msg">the message that has been sent</param>
        private void ServiceGateway_NotifyClientOfNewMessageFromServerEvent(object sender, ServiceChatMessage msg)
        {
            var message = "{0} {1} - {2}".Substitute(msg.Name, DateTime.Now.ToString(), msg.Message);
            this.MessageHistories.Add(message);
            CollectionViewSource.GetDefaultView(this.MessageHistories).MoveCurrentTo(message);
        }

        /// <summary>
        /// Duplex binding for recieving story point estimates from server
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="msg">the message that has been sent, in the case the estimate</param>
        private void ServiceGateway_NotifyClientOfNewStoryPointEstimateFromServerEvent(object sender, ServiceChatMessage msg)
        {
            this.RecievedEstimates.Add(new ServicePlanningPokerEstimate
            {
                Estimate = Convert.ToDouble(msg.Message), SessionID = msg.SessionID
            }); 
        }              

        /// <summary>
        /// Submit a message to the server to be relayed to other session members.
        /// </summary>
        private void SubmitMessageToServer()
        {
            ServiceChatMessage msg = new ServiceChatMessage();

            msg.SessionID = sessionId;
            msg.Message = messageToSend;
            msg.Name = "Me";
            
            serviceGateway.SubmitMessageToServer(msg);

            ServiceGateway_NotifyClientOfNewMessageFromServerEvent(this, msg);

            MessageToSend = string.Empty;
        }

        /// <summary>
        /// Shows the user estimates
        /// </summary>
        private void ShowUserEstimates()
        {
            serviceGateway.ShowEstimates(sessionId);
            this.ShowEstimates = true;
            this.HideEstimates = false;
            this.CanShowEstimates = false;
            this.CanGiveFinalEstimate = true; 
        }

        /// <summary>
        /// Submit a story estimate to the server to be relayed to other session members.
        /// </summary>
        /// <param name="estimate">The estimate to send</param>
        private void SubmitEstimateToServer(string estimate)
        {
            ServicePlanningPokerEstimate userEstimate = new ServicePlanningPokerEstimate
            {
                Estimate = double.Parse(estimate),
                SessionID = this.sessionId
            };

            this.ShowMetroDialog("Going to submit estimate", userEstimate.Estimate.ToString(), MessageDialogStyle.Affirmative);

            serviceGateway.SubmitEstimateToServer(userEstimate);
        }

        /// <summary>
        /// Submit a story estimate to the server to be relayed to other session members.
        /// </summary>
        private void SubmitFinalEstimateToServer()
        {
            var finalEstimate = double.Parse(SelectedFinalEstimate);

            var sprintStoryId = SprintStoriesCollection[activeStoryIndex].SprintStoryId;

            serviceGateway.SubmitFinalEstimate(sessionId, sprintStoryId, finalEstimate);

            var updatedSession = serviceGateway.RetrieveSessionInfo(sessionId);
            UpdateViewGivenSession(updatedSession);

            if (updatedSession.State == ServicePlanningPokerSessionState.Complete)
            {
                this.ShowMetroDialog("Estimation complete!", "All stories have now been estimated! The session may now be ended.");
            }
        }

        /// <summary>
        /// Allows a scrum master to end the session.
        /// </summary>
        private void EndCurrentSession()
        {
            serviceGateway.EndPokerSession(sessionId);
            this.ShowMetroDialog("Poker session ended!", "Your planning poker session has been successfully ended.");
            this.NavigateToItem("StartPlanningPokerSession", null);
        }

        /// <summary>
        /// Allows a developer to leave the session.
        /// </summary>
        private void LeaveCurrentSession()
        {
            serviceGateway.LeaveSession(sessionId);
            this.ShowMetroDialog("Left the Planning poker session!", "You have now left the session.");
            this.NavigateToItem("MyProjects", null);
        }

        /// <summary>
        /// this scrools chat 
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">selections changed</param>
        private void BringSelectionIntoView(object sender, SelectionChangedEventArgs e)
        {
            Selector selector = sender as Selector;
            if (selector is ListBox)
            {
                (selector as ListBox).ScrollIntoView(selector.SelectedIndex = MessageHistories.Count);
            }
        }

        #endregion

        #region Helpers 

        /// <summary>
        /// Updates the view given the updated session.
        /// </summary>
        /// <param name="session">The session.</param>
        private void UpdateViewGivenSession(ServicePlanningPokerSession session)
        {
            this.RecievedEstimates.Clear();
            this.RecievedEstimates.AddRange(session.Users.Where(u => u.Estimate != null).Select(u => u.Estimate).ToList());

            this.Users.Clear();
            this.Users.AddRange(session.Users);

            this.SprintStoriesCollection.Clear();
            this.SprintStoriesCollection.AddRange(session.Stories);

            if (session.Stories.Count() > 0 && session.ActiveStoryIndex > -1)
            {
                var currentActiveStory = session.Stories.ToList()[session.ActiveStoryIndex];
                ActiveStory = currentActiveStory;
                activeStoryIndex = session.ActiveStoryIndex;
            }

            TotalStoriesLabel = "Stories - Estimating {0}/{1}".Substitute(session.ActiveStoryIndex + 1, session.Stories.Count());

            if (session.State == ServicePlanningPokerSessionState.GettingEstimates)
            {
                this.ShowEstimates = false;
                this.HideEstimates = true;
                this.CanGiveFinalEstimate = false;
                this.CanShowEstimates = true;
            }

            if (session.State == ServicePlanningPokerSessionState.ShowingEstimates)
            {
                this.ShowEstimates = true;
                this.HideEstimates = false;
                this.CanGiveFinalEstimate = true;
                this.CanShowEstimates = false;
            }

            if (session.State == ServicePlanningPokerSessionState.Complete)
            {
                this.CanGiveFinalEstimate = false;
                this.CanShowEstimates = false;
            }
        }

        #endregion 
    }
}
