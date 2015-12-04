using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure.Providers;
using AnyTrack.PlanningPoker.BackendPlanningPokerManagerService;
using Microsoft.Practices.Unity;

namespace AnyTrack.PlanningPoker.ServiceGateways
{
    /// <summary>
    /// Provides an implementation for the Planning Poker Manager service gateway.
    /// </summary>
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class PlanningPokerManagerServiceGateway : IPlanningPokerManagerServiceGateway, IPlanningPokerManagerServiceCallback
    {
        #region Fields 

        /// <summary>
        /// The Unity Container.
        /// </summary>
        private readonly IUnityContainer container;

        /// <summary>
        /// The web service client.
        /// </summary>
        private readonly IPlanningPokerManagerService client; 
        
        #endregion 

        #region Constructor 

        /// <summary>
        /// Constructs a new planning poker manager service gateway with the specified dependencies. 
        /// </summary>
        /// <param name="container">The unity container.</param>
        public PlanningPokerManagerServiceGateway(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;

            var clientContext = new InstanceContext(this);

            client = container.Resolve<IPlanningPokerManagerService>(new ParameterOverride("callbackInstance", clientContext));
        }

        #endregion 
    
        #region Events

        /// <summary>
        /// Notifies the client of a change in the sessions of the sprint group they are in.
        /// </summary>
        public event EventHandler<ServiceSessionChangeInfo> NotifyClientOfSessionEvent;

        /// <summary>
        /// Notifies the client that their session has been terminated on the server side.
        /// </summary>
        public event EventHandler NotifyClientOfTerminatedSessionEvent;

        /// <summary>
        /// Notifies the client that they have been sent a message from the server.
        /// </summary>
        public event EventHandler<ServiceChatMessage> NotifyClientOfNewMessageFromServerEvent;

        /// <summary>
        /// Notifies the client that they should clear their recieved estimates
        /// </summary>
        public event EventHandler NotifyClientToClearStoryPointEstimateFromServerEvent;

        /// <summary>
        /// Notifies the client that the session has changed.
        /// </summary>
        public event EventHandler<ServicePlanningPokerSession> NotifyClientOfSessionUpdateEvent;

        /// <summary>
        /// Notifies the client that the session has started.
        /// </summary>
        public event EventHandler NotifyClientOfSessionStartEvent;

        #endregion 

        #region Methods 

        #region Client Methods 

        /// <summary>
        /// Allows the client to subscribe to messages about new sessions for the given project and sprint ids. 
        /// </summary>
        /// <param name="sprintId">The sprint id.</param>
        /// <returns>Any currently available session.</returns>
        public ServiceSessionChangeInfo SubscribeToNewSessionMessages(Guid sprintId)
        {
            return client.SubscribeToNewSessionMessages(sprintId);
        }

        /// <summary>
        /// Allows the scrum master to start a new planning poker session.
        /// </summary>
        /// <param name="sprintId">The sprint id.</param>
        /// <returns>The session id for the new poker session.</returns>
        public Guid StartNewPokerSession(Guid sprintId)
        {
            return client.StartNewPokerSession(sprintId);
        }

        /// <summary>
        /// Allows the scrum master to cancel a pending planning poker session.
        /// </summary>
        /// <param name="sessionId">The session.</param>
        public void EndPokerSession(Guid sessionId)
        {
            client.EndPokerSession(sessionId);
        }
        
        /// <summary>
        /// Allows a client to join an active session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns>The details of the server side planning poker session.</returns>
        public ServicePlanningPokerSession JoinSession(Guid sessionId)
        {
            return client.JoinSession(sessionId);
        }

        /// <summary>
        /// Allows a user to exit the session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public void LeaveSession(Guid sessionId)
        {
            client.LeaveSession(sessionId);
        }

        /// <summary>
        /// Allows a scrum master to start the session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public void StartSession(Guid sessionId)
        {
            client.StartSession(sessionId);
        }

        /// <summary>
        /// Allows the client to pull an up to date session state. 
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns>The current session.</returns>
        public ServicePlanningPokerSession RetrieveSessionInfo(Guid sessionId)
        {
            return client.RetrieveSessionInfo(sessionId);
        }

        /// <summary>
        /// A callback to be raised when the client's current session is terminated. 
        /// </summary>
        /// <param name="msg">The message to submit.</param>        
        public void SubmitMessageToServer(ServiceChatMessage msg)
        {
            client.SubmitMessageToServer(msg);
        }

        /// <summary>
        /// Sends the client's estimate to the server
        /// </summary>
        /// <param name="estimate">The message to submit.</param>        
        public void SubmitEstimateToServer(ServicePlanningPokerEstimate estimate)
        {
            client.SubmitEstimateToServer(estimate);
        }

        /// <summary>
        /// Method for sending messages out to the client
        /// </summary>
        /// <param name="msg">The message to be sent</param>
        public void SendMessageToClient(ServiceChatMessage msg)
        {
            NotifyClientOfNewMessageFromServerEvent(this, msg);
        }

        /// <summary>
        /// Allows a scrum master to show the estimates. 
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public void ShowEstimates(Guid sessionId)
        {
            client.ShowEstimates(sessionId);
        }

        #endregion 

        #region Callback Methods

        /// <summary>
        /// A callback to be raised when a new session is available for this client. 
        /// </summary>
        /// <param name="sessionInfo">Information about the sesion change.</param>
        public void NotifyClientOfSession(ServiceSessionChangeInfo sessionInfo)
        {
            NotifyClientOfSessionEvent(this, sessionInfo);
        }

        /// <summary>
        /// A callback to be raised when the client's current session is terminated. 
        /// </summary>
        public void NotifyClientOfTerminatedSession()
        {
            NotifyClientOfTerminatedSessionEvent(this, null);
        }

        /// <summary>
        /// Notifies the client to clear their current story point estimate. 
        /// </summary>
        public void NotifyClientToClearStoryPointEstimateFromServer()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Notifies the client that the session has changed.
        /// </summary>
        /// <param name="newSession">The new session.</param>
        public void NotifyClientOfSessionUpdate(ServicePlanningPokerSession newSession)
        {
            NotifyClientOfSessionUpdateEvent(this, newSession);
        }

        /// <summary>
        /// Notifies the client that the session has started.
        /// </summary>
        public void NotifyClientOfSessionStart()
        {
            NotifyClientOfSessionStartEvent(this, new EventArgs());
        }

        #endregion

        #endregion
    }
}
