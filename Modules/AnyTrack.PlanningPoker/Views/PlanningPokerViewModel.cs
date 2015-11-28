using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.PlanningPoker;
using Prism.Commands;

namespace AnyTrack.PlanningPoker.Views
{
    /// <summary>
    /// ChatViewModel class
    /// </summary>
    public class PlanningPokerViewModel
    {
        #region Fields 

        /// <summary>
        /// The specified message to send.
        /// </summary>
        private string messageToSend;

        /// <summary>
        /// The specified messages timestamp.
        /// </summary>
        private string messageTimeStamp;

        /// <summary>
        /// The specified sessionID for this chat.
        /// </summary>
        private Guid sessionID;

        #endregion

        #region Constructor 
        /// <summary>
        /// ChatViewModel constructor.
        /// </summary>
        public PlanningPokerViewModel()
        {
            SendMessageCommand = new DelegateCommand(this.SendMessage);
        }

        #endregion
        
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

        ////private static ChatMessage localService = new ChatMessage();
        ////private static DuplexChannelFactory<> dcf = new DuplexChannelFactory<IChatManager>(localService, "mgr");
        ////IPl mgr = dcf.CreateChannel();
        ////mgr.("tester");

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
        /// Perform a send message.
        /// </summary>
        private void SendMessage()
        {
            this.messageTimeStamp = DateTime.Now.ToString();
            this.messageToSend = this.sessionID + ":USERNAME" + this.messageTimeStamp + ":" + this.messageToSend;

            ////mgr.SubmitMessage(new ChatMessage("Test", this.messageTimeStamp, this.messageToSend));

            MessageToSend = string.Empty;
        }

        #endregion
    }
}
