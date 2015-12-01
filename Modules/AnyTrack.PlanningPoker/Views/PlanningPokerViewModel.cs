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
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Regions;

namespace AnyTrack.PlanningPoker.Views
{
    /// <summary>
    /// ChatViewModel class
    /// </summary>
    public class PlanningPokerViewModel : ValidatedBindableBase, INavigationAware, IRegionMemberLifetime
    {
        #region Fields 

        /// <summary>
        /// The project service gateway
        /// </summary>
        private readonly IProjectServiceGateway projectServiceGateway;

        /// <summary>
        /// The planning poker service gateway.
        /// </summary>
        private readonly IPlanningPokerManagerServiceGateway serviceGateway;
        
        /// <summary>
        /// The specified message to send.
        /// </summary>
        private string messageToSend;
        
        /// <summary>
        /// The specified sessionID for this chat.
        /// </summary>
        private Guid sessionId;        

        #endregion

        #region Constructor 
        /// <summary>
        /// ChatViewModel constructor.
        /// </summary>
        public PlanningPokerViewModel()
        {
            this.SprintStoriesCollection = new ObservableCollection<ServiceStorySummary>();

            this.SprintStories.Clear();
            this.SprintStories.AddRange<ServiceStorySummary>(projectServiceGateway.GetProjectStories(Guid.NewGuid()));

            serviceGateway.NotifyClientToClearStoryPointEstimateFromServerEvent += ServiceGateway_NotifyClientToClearStoryPointEstimateFromServerEvent;
            serviceGateway.NotifyClientOfNewMessageFromServerEvent += ServiceGateway_NotifyClientOfNewMessageFromServerEvent;

            SendMessageCommand = new DelegateCommand(this.SubmitMessageToServer);

            SendEstimateCommand = new DelegateCommand<string>(SubmitEstimateToServer);

            SendFinalEstimateCommand = new DelegateCommand<double>(SubmitFinalEstimateToServer);
        }               

        #endregion

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
        public ObservableCollection<string> RecievedEstimates { get; set; }

        /// <summary>
        /// Gets the command used to send a message from a user. 
        /// </summary>
        public DelegateCommand SendMessageCommand { get; private set; }

        /// <summary>
        /// Gets the command used to send a an estimate from client to server. 
        /// </summary>
        public DelegateCommand<string> SendEstimateCommand { get; private set; }

        /// <summary>
        /// Gets the command used to send and set the final story point estimate. 
        /// </summary>
        public DelegateCommand<double> SendFinalEstimateCommand { get; private set; }

        #region Methods         

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
                messageToSend = value;
            }
        }

        public bool KeepAlive
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Duplex binding for recieving messages from server
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="msg">the message that has been sent</param>
        private void ServiceGateway_NotifyClientOfNewMessageFromServerEvent(object sender, ServiceChatMessage msg)
        {
            this.MessageHistories.Add(msg.Message);
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
            this.RecievedEstimates.Add(msg.Name + " estimates " + msg.Message);
        }              

        /// <summary>
        /// Submit a message to the server to be relayed to other session members.
        /// </summary>
        private void SubmitMessageToServer()
        {
            ServiceChatMessage msg = new ServiceChatMessage();

            ////Needs to be changed to the sessionID of planning poker session
            msg.SessionID = Guid.NewGuid();

            msg.Message = messageToSend;
            
            serviceGateway.SubmitMessageToServer(msg);
        }

        /// <summary>
        /// Submit a story estimate to the server to be relayed to other session members.
        /// </summary>
        /// <param name="estimate">estimate submited by clients</param>
        private void SubmitEstimateToServer(string estimate)
        {
            ServiceChatMessage msg = new ServiceChatMessage();

            ////Needs to be changed to the sessionID of planning poker session
            msg.SessionID = Guid.NewGuid();

            msg.Message = estimate;

            this.ShowMetroDialog("Going to submit estimate", msg.Message, MessageDialogStyle.Affirmative);

            serviceGateway.SubmitMessageToServer(msg);
        }

        /// <summary>
        /// Submit a story estimate to the server to be relayed to other session members.
        /// </summary>
        /// <param name="finalEstimate">Final estimate for storys to be stored</param>
        private void SubmitFinalEstimateToServer(double finalEstimate)
        {
            ServiceChatMessage msg = new ServiceChatMessage();

            ////Needs to be changed to the sessionID of planning poker session
            msg.SessionID = Guid.NewGuid();

            msg.Message = finalEstimate.ToString();

            serviceGateway.SubmitMessageToServer(msg);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
