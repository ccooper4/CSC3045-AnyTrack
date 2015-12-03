using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
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
        /// Should the estimates be shown
        /// </summary>
        private bool showEstimates = false;
        
        /// <summary>
        /// The specified sessionID for this chat.
        /// </summary>
        private Guid sessionId;

        /// <summary>
        /// The specified estimate to send.
        /// </summary>
        private string estimateToSend;

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

            this.SprintStoriesCollection = new ObservableCollection<ServiceStorySummary>();

            this.MessageHistories = new ObservableCollection<string>();

            SendMessageCommand = new DelegateCommand(SubmitMessageToServer);

            ShowEstimatesCommand = new DelegateCommand(ShowUserEstimates);

            SendEstimateCommand = new DelegateCommand(SubmitEstimateToServer);

            SendFinalEstimateCommand = new DelegateCommand<ServicePlanningPokerEstimate>(SubmitFinalEstimateToServer);
        }               

        #endregion

        #region Properties 

        /// <summary>
        /// Gets or sets the stories
        /// </summary>
        public ObservableCollection<ServiceStorySummary> SprintStoriesCollection { get; set; }

        /// <summary>
        /// Gets or sets a list of sprint stories
        /// </summary>
        public ObservableCollection<ServiceStorySummary> SprintStories { get; set; }

        /// <summary>
        /// Gets or sets the history of messages.
        /// </summary>
        public ObservableCollection<string> MessageHistories { get; set; }

        /// <summary>
        /// Gets or sets the history of recieved estimates for story points.
        /// </summary>
        public ObservableCollection<ServicePlanningPokerEstimate> RecievedEstimates { get; set; }

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
        public DelegateCommand SendEstimateCommand { get; private set; }

        /// <summary>
        /// Gets the command used to send and set the final story point estimate. 
        /// </summary>
        public DelegateCommand<ServicePlanningPokerEstimate> SendFinalEstimateCommand { get; private set; }

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
        /// Gets or sets message property.
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

        #endregion 

        #region Methods

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
                serviceGateway.NotifyClientToClearStoryPointEstimateFromServerEvent += ServiceGateway_NotifyClientToClearStoryPointEstimateFromServerEvent;
                serviceGateway.NotifyClientOfNewMessageFromServerEvent += ServiceGateway_NotifyClientOfNewMessageFromServerEvent;
            }
        }

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
            serviceGateway.NotifyClientToClearStoryPointEstimateFromServerEvent -= ServiceGateway_NotifyClientToClearStoryPointEstimateFromServerEvent;
            serviceGateway.NotifyClientOfNewMessageFromServerEvent -= ServiceGateway_NotifyClientOfNewMessageFromServerEvent;
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
        }

        /// <summary>
        /// Duplex binding for clearing estimates when server asks
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">the event</param>
        private void ServiceGateway_NotifyClientToClearStoryPointEstimateFromServerEvent(object sender, EventArgs e)
        {
            this.RecievedEstimates.Clear();
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
        }

        /// <summary>
        /// Shows the user estimates
        /// </summary>
        private void ShowUserEstimates()
        {
            var session = serviceGateway.RetrieveSessionInfo(this.sessionId);

            foreach (var user in session.Users)
            {
                RecievedEstimates.Add(user.Estimate);
            }

            ShowEstimates = true;
        }

        /// <summary>
        /// Submit a story estimate to the server to be relayed to other session members.
        /// </summary>
        private void SubmitEstimateToServer()
        {
            ServicePlanningPokerEstimate userEstimate = new ServicePlanningPokerEstimate
            {
                Estimate = double.Parse(this.EstimateToSend),
                SessionID = this.sessionId
            };

            this.ShowMetroDialog("Going to submit estimate", userEstimate.Estimate.ToString(), MessageDialogStyle.Affirmative);

            serviceGateway.SubmitEstimateToServer(userEstimate);
            serviceGateway.RetrieveSessionInfo(sessionId);
        }

        /// <summary>
        /// Submit a story estimate to the server to be relayed to other session members.
        /// </summary>
        /// <param name="finalEstimate">Final estimate for storys to be stored</param>
        private void SubmitFinalEstimateToServer(ServicePlanningPokerEstimate finalEstimate)
        {
            ServiceChatMessage msg = new ServiceChatMessage();

            ////Needs to be changed to the sessionID of planning poker session
            msg.SessionID = Guid.NewGuid();

            msg.Message = finalEstimate.ToString();

            serviceGateway.SubmitMessageToServer(msg);
        }

        #endregion
    }
}
