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
using Prism.Commands;
using Prism.Regions;

namespace AnyTrack.PlanningPoker.Views
{
    /// <summary>
    /// ChatViewModel class
    /// </summary>
    public class PlanningPokerViewModel : ValidatedBindableBase
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
            serviceGateway.NotifyClientOfNewMessageFromServerEvent += ServiceGateway_NotifyClientOfNewMessageFromServerEvent;
            SendMessageCommand = new DelegateCommand(this.SubmitMessageToServer);
        }

        #endregion

        /// <summary>
        /// Gets or sets the history of messages.
        /// </summary>
        public ObservableCollection<string> MessageHistories { get; set; }

        /// <summary>
        /// Gets the command used to send a message from a user. 
        /// </summary>
        public DelegateCommand SendMessageCommand { get; private set; }              

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

        /// <summary>
        /// Duplex binding fro recieving messages from server
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="msg">the message that has been sent</param>
        private void ServiceGateway_NotifyClientOfNewMessageFromServerEvent(object sender, ServiceChatMessage msg)
        {
            this.MessageHistories.Add(msg.Message);
            throw new NotImplementedException();
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

        #endregion
    }
}
