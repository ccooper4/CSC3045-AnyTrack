using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Service.Model;

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
        [OperationContract]
        void SubscribeToNewSessionMessages(Guid sprintId);

        /// <summary>
        /// Allows the scrum master to start a new planning poker session.
        /// </summary>
        /// <param name="sprintId">The sprint id.</param>
        /// <returns>The session id for the new poker session.</returns>
        [OperationContract]
        Guid StartNewPokerSession(Guid sprintId);

        /// <summary>
        /// Allows the scrum master to cancel a pending planning poker session.
        /// </summary>
        /// <param name="sessionId">The session.</param>
        [OperationContract]
        void CancelPendingPokerSession(Guid sessionId);

        /// <summary>
        /// Allows a client to join an active session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns>The details of the server side planning poker session.</returns>
        [OperationContract]
        ServicePlanningPokerSession JoinSession(Guid sessionId);

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
        void SubmitMessage(ServiceChatMessage msg);

        /// <summary>
        /// Method for sending a message
        /// </summary>
        /// <param name="msg">the message object to be sent</param>
        [OperationContract(IsOneWay = true)]
        void SendMessage(ServiceChatMessage msg);
    }    

    /////// <summary>
    /////// The chat manager services
    /////// </summary>
    ////[SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Makes sense to have multiple in here")]
    ////public class ChatManagerService
    ////{
    ////    /// <summary>
    ////    /// A list containing all clients in chat
    ////    /// </summary>
    ////    private Dictionary<string, IPlanningPokerManagerService> clients = new Dictionary<string, IPlanningPokerManagerService>();

    ////    /// <summary>
    ////    /// Used to register a user to a chat channel
    ////    /// </summary>
    ////    /// <param name="sessionID">The chat session ID</param>
    ////    /// <param name="name">The clients name</param>
    ////    public void RegisterChatUser(Guid sessionID, string name)
    ////    {
    ////        IPlanningPokerManagerService client = OperationContext.Current.GetCallbackChannel<IPlanningPokerManagerService>();
    ////        if (client != null)
    ////        {
    ////            clients.Add(name, client);
    ////            ////output that client has joined
    ////        }
    ////    }

    ////    /// <summary>
    ////    /// method used to push message to all registered clients
    ////    /// </summary>
    ////    /// <param name="msg">Chat message</param>
    ////    public void SubmitMessage(ChatMessage msg)
    ////    {
    ////        foreach (string key in clients.Keys)
    ////        {
    ////            try
    ////            {
    ////                clients[key].SendMessage(msg);
    ////            }
    ////            catch (Exception e)
    ////            {
    ////                throw e;
    ////            }
    ////        }
    ////    }
    ////}
}
