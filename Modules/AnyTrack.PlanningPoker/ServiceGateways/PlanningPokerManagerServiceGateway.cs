﻿using System;
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

        #endregion 

        #region Methods 

        #region Client Methods 

        /// <summary>
        /// Allows the client to subscribe to messages about new sessions for the given project and sprint ids. 
        /// </summary>
        /// <param name="sprintId">The sprint id.</param>
        public void SubscribeToNewSessionMessages(Guid sprintId)
        {
            client.SubscribeToNewSessionMessages(sprintId);
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
        public void CancelPendingPokerSession(Guid sessionId)
        {
            client.CancelPendingPokerSession(sessionId);
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

        #endregion 

        #endregion
    }
}