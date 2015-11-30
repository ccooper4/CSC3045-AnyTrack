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
        /// Method to submit message to chat channel
        /// </summary>
        /// <param name="msg">The chatmessage object which is to be sent</param>
        [OperationContract(IsOneWay = true)]
        void SubmitMessageToServer(ServiceChatMessage msg);
    }        
}
