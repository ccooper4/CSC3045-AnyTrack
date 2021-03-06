﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.PlanningPoker.BackendPlanningPokerManagerService;

namespace AnyTrack.PlanningPoker.ServiceGateways
{
    /// <summary>
    /// Outlines the interface for the planning poker manager service gateway.
    /// </summary>
    public interface IPlanningPokerManagerServiceGateway
    {
        #region Events

        /// <summary>
        /// Notifies the client of a change in the sessions of the sprint group they are in.
        /// </summary>
        event EventHandler<ServiceSessionChangeInfo> NotifyClientOfSessionEvent;

        /// <summary>
        /// Notifies the client that their session has been terminated on the server side.
        /// </summary>
        event EventHandler NotifyClientOfTerminatedSessionEvent;

        /// <summary>
        /// Notifies the client that they have been sent a message from the server.
        /// </summary>
        event EventHandler<ServiceChatMessage> NotifyClientOfNewMessageFromServerEvent;

        /// <summary>
        /// Notifies the client that the session has changed.
        /// </summary>
        event EventHandler<ServicePlanningPokerSession> NotifyClientOfSessionUpdateEvent;

        /// <summary>
        /// Notifies the client that the session has started.
        /// </summary>
        event EventHandler NotifyClientOfSessionStartEvent; 

        #endregion

        #region Methods 

        /// <summary>
        /// Allows the client to subscribe to messages about new sessions for the given project and sprint ids. 
        /// </summary>
        /// <param name="sprintId">The sprint id.</param>
        /// <returns>Any currently available session.</returns>
        ServiceSessionChangeInfo SubscribeToNewSessionMessages(Guid sprintId);

        /// <summary>
        /// Allows the scrum master to start a new planning poker session.
        /// </summary>
        /// <param name="sprintId">The sprint id.</param>
        /// <returns>The session id for the new poker session.</returns>
        Guid StartNewPokerSession(Guid sprintId);

        /// <summary>
        /// Allows the scrum master to cancel a pending planning poker session.
        /// </summary>
        /// <param name="sessionId">The session.</param>
        void EndPokerSession(Guid sessionId);

        /// <summary>
        /// Allows a client to join an active session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns>The details of the server side planning poker session.</returns>
        ServicePlanningPokerSession JoinSession(Guid sessionId);

        /// <summary>
        /// Allows a user to exit the session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        void LeaveSession(Guid sessionId);

        /// <summary>
        /// Allows a scrum master to start the session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        void StartSession(Guid sessionId);

        /// <summary>
        /// Allows the client to pull an up to date session state. 
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns>The current session.</returns>
        ServicePlanningPokerSession RetrieveSessionInfo(Guid sessionId);

        /// <summary>
        /// Allows a client to submit message to a session on the server
        /// </summary>
        /// <param name="msg">The message to send.</param>
        void SubmitMessageToServer(ServiceChatMessage msg);

        /// <summary>
        /// Sumbits the client's estimate to the server
        /// </summary>
        /// <param name="estimate">The estimate to submit</param>
        void SubmitEstimateToServer(ServicePlanningPokerEstimate estimate);

        /// <summary>
        /// Allows a scrum master to show the estimates. 
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        void ShowEstimates(Guid sessionId); 

        /// <summary>
        /// Allows a scrum master to submit the final estimate.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="sprintStoryId">The sprint story id.</param>
        /// <param name="estimate">The selected estimate.</param>
        void SubmitFinalEstimate(Guid sessionId, Guid sprintStoryId, double estimate);

        #endregion
    }
}
