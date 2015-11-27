using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Outlines the interface for the Server hosted component of the planning poker system.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IPlanningPokerClientService))]
    public interface IPlanningPokerManagerService
    {
        /// <summary>
        /// Allows the client to subscribe to messages about new sessions for the given project and sprint ids. 
        /// </summary>
        /// <param name="sprintId">The sprint id.</param>
        [OperationContract(IsOneWay = true)]
        void SubscribeToNewSessionMessages(Guid sprintId);

        /// <summary>
        /// Allows the scrum master to start a new planning poker session.
        /// </summary>
        /// <param name="sprintId">The sprint id.</param>
        /// <returns>The session id for the new poker session.</returns>
        Guid StartNewPokerSession(Guid sprintId);
    }    

    /// <summary>
    /// Interface for Chat
    /// </summary>
    [ServiceContract]
    public interface IChat
    {
        /// <summary>
        /// Method for sending a message
        /// </summary>
        /// <param name="msg">the message object to be sent</param>
        [OperationContract(IsOneWay = true)]
        void SendMessage(ChatMessage msg);
    }

    /// <summary>
    /// interface for chat manager
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IChat))]
    public interface IChatManager
    {
        /// <summary>
        /// Method to register a user to chat
        /// </summary>
        /// <param name="sessionID">the session that they want to chat in</param>
        /// <param name="name">the name of the user registering</param>
        [OperationContract(IsOneWay = true)]
        void RegisterChatUser(Guid sessionID, string name);

        /// <summary>
        /// Method to submit message to chat channel
        /// </summary>
        /// <param name="msg">The chatmessage object which is to be sent</param>
        [OperationContract(IsOneWay = true)]
        void SubmitMessage(ChatMessage msg);
    }

    /// <summary>
    /// Chat message class
    /// </summary>
    ////[DataContract]
    public class ChatMessage
    {
        /// <summary>
        /// the sessionID to say what session this message should be sent along
        /// </summary>
        private Guid sessionID;

        /// <summary>
        /// The users name
        /// </summary>
        private string name;

        /// <summary>
        /// The actual message text to be sent
        /// </summary>
        private string message;

        /// <summary>
        /// Constructor for a chat message
        /// </summary>
        /// <param name="sessionID">Session that this message belongs to</param>
        /// <param name="name">name of user which this message is from</param>
        /// <param name="message">the contents of the message</param>
        public ChatMessage(Guid sessionID, string name, string message)
        {
            this.sessionID = sessionID;
            this.name = name;
            this.message = message;
        }        
    }

    /// <summary>
    /// The chat manager services
    /// </summary>
    ////[ServiceBehaviour(InstanceContextMode = InstanceContextMode.Single)]
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Makes sense to have multiple in here")]
    public class ChatManagerService : IChatManager
    {
        /// <summary>
        /// A list containing all clients in chat
        /// </summary>
        private Dictionary<string, IChat> clients = new Dictionary<string, IChat>();

        /// <summary>
        /// Used to register a user to a chat channel
        /// </summary>
        /// <param name="sessionID">The chat session ID</param>
        /// <param name="name">The clients name</param>
        public void RegisterChatUser(Guid sessionID, string name)
        {
            IChat client = OperationContext.Current.GetCallbackChannel<IChat>();
            if (client != null)
            {
                clients.Add(name, client);
                ////output that client has joined
            }
        }

        /// <summary>
        /// method used to push message to all registered clients
        /// </summary>
        /// <param name="msg">Chat message</param>
        public void SubmitMessage(ChatMessage msg)
        {
            foreach (string key in clients.Keys)
            {
                try
                {
                    clients[key].SendMessage(msg);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
