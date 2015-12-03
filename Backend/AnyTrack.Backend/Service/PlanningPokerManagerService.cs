﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.SessionState;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Providers;
using AnyTrack.Backend.Security;
using AnyTrack.Backend.Service.Model;
using AnyTrack.SharedUtilities.Extensions;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// The implementation of the planning poker manager service. 
    /// </summary>
    [CreatePrincipal]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerCall)]
    public class PlanningPokerManagerService : IPlanningPokerManagerService
    {
        #region Fields 

        /// <summary>
        /// The database unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The operation context provider.
        /// </summary>
        private readonly OperationContextProvider contextProvider;

        /// <summary>
        /// The currently connected clients.
        /// </summary>
        private readonly AvailableClientsProvider availableClients;

        /// <summary>
        /// The currently active sessions. 
        /// </summary>
        private readonly ActivePokerSessionsProvider activeSessions;

        #endregion 

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the planning poker manager service.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="context">The operation context provider.</param>
        /// <param name="availableClients">The connected clients provider.</param>
        /// <param name="activeSessions">The currently active sessions.</param>
        public PlanningPokerManagerService(IUnitOfWork unitOfWork, OperationContextProvider context, AvailableClientsProvider availableClients, ActivePokerSessionsProvider activeSessions)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (availableClients == null)
            {
                throw new ArgumentNullException("availableClients");
            }

            if (activeSessions == null)
            {
                throw new ArgumentNullException("activeSessions");
            }

            this.unitOfWork = unitOfWork;
            this.contextProvider = context;
            this.availableClients = availableClients;
            this.activeSessions = activeSessions;
        }

        #endregion 

        #region Methods 

        /// <summary>
        /// Allows the client to subscribe to messages about new sessions for the given project and sprint ids. 
        /// </summary>
        /// <param name="sprintId">The sprint id.</param>
        /// <returns>A current session, if there is one.</returns>
        public ServiceSessionChangeInfo SubscribeToNewSessionMessages(Guid sprintId)
        {
            var sprint = unitOfWork.SprintRepository.Items.SingleOrDefault(s => s.Id == sprintId);

            if (sprint == null)
            {
                throw new ArgumentException("The specified sprint could not be found", "sprintId");
            }

            var currentUser = unitOfWork.UserRepository.Items.Single(u => u.EmailAddress == Thread.CurrentPrincipal.Identity.Name);
            var currentUserId = currentUser.Id;

            if (!sprint.Team.Select(u => u.Id).Contains(currentUserId))
            {
                throw new SecurityException("User is not a developer on the specified sprint.");
            }

            var clientChannel = contextProvider.GetClientChannel<IPlanningPokerClientService>();

            var connectedClientsList = availableClients.GetListOfClients();

            if (!connectedClientsList.ContainsKey(sprintId))
            {
                connectedClientsList.Add(sprintId, new List<ServicePlanningPokerPendingUser>());
            }

            var thisSprintLst = connectedClientsList.Single(k => k.Key == sprintId);

            var pendingUser = new ServicePlanningPokerPendingUser
            {
                ClientChannel = clientChannel,
                EmailAddress = currentUser.EmailAddress,
                Name = "{0} {1}".Substitute(currentUser.FirstName, currentUser.LastName),
                UserID = currentUserId,
                UserRoles = currentUser.Roles.Where(r => r.SprintId == sprintId && r.ProjectId == sprint.Project.Id).Select(r => r.RoleName).ToList()
            };

            thisSprintLst.Value.Add(pendingUser);

            // Notify clients in this sprint group. 
            var sessions = activeSessions.GetListOfSessions();

            var sprintSession = sessions.Where(s => s.Value.SprintID == sprintId && s.Value.State == ServicePlanningPokerSessionState.Pending).Select(s => s.Value).SingleOrDefault();

            if (sprintSession != null)
            {
                var sessionInfo = new ServiceSessionChangeInfo
                {
                    SprintId = sprintId,
                    SessionAvailable = true,
                    SessionId = sprintSession.SessionID,
                    SprintName = sprint.Name,
                    ProjectName = sprint.Project.Name
                };
                return sessionInfo; 
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Starts a new planning poker session. 
        /// </summary>
        /// <param name="sprintId">The sprint id.</param>
        /// <returns>The new session id.</returns>
        public Guid StartNewPokerSession(Guid sprintId)
        {
            var sprint = unitOfWork.SprintRepository.Items.SingleOrDefault(s => s.Id == sprintId);

            if (sprint == null)
            {
                throw new ArgumentException("The specified sprint could not be found", "sprintId");
            }

            var currentUser = unitOfWork.UserRepository.Items.Single(u => u.EmailAddress == Thread.CurrentPrincipal.Identity.Name);
            var currentUserId = currentUser.Id;

            if (!sprint.Project.ScrumMasters.Select(u => u.Id).Contains(currentUserId))
            {
                throw new SecurityException("This user is not a scrum master in this project.");
            }

            var sessions = activeSessions.GetListOfSessions(); 

            if (sessions.Count(s => s.Value.SprintID == sprintId) != 0)
            {
                throw new ArgumentException("A planning poker session is already active for this sprint.", "sprintId");
            }

            var clientSocket = contextProvider.GetClientChannel<IPlanningPokerClientService>();

            var newSession = new ServicePlanningPokerSession
            {
                SessionID = Guid.NewGuid(),
                HostID = currentUserId,
                SprintID = sprintId,
                SprintName = sprint.Name,
                ProjectName = sprint.Project.Name,
                Users = new List<ServicePlanningPokerUser>()
                {
                    new ServicePlanningPokerUser
                    {
                        ClientChannel = clientSocket,
                        EmailAddress = currentUser.EmailAddress,
                        Name = "{0} {1}".Substitute(currentUser.FirstName, currentUser.LastName),
                        UserID = currentUserId,
                        UserRoles = currentUser.Roles.Where(r => r.SprintId == sprintId && r.ProjectId == sprint.Project.Id).Select(r => r.RoleName).ToList(),
                        Estimate = new ServicePlanningPokerEstimate()
                    }
                },
                State = ServicePlanningPokerSessionState.Pending
            };

            sessions.Add(newSession.SessionID, newSession);

            // Notify clients in this sprint group. 
            var availableClientsList = availableClients.GetListOfClients();
            if (availableClientsList.ContainsKey(sprintId))
            {
                var clientList = availableClientsList[sprintId];
                var sessionInfo = new ServiceSessionChangeInfo
                {
                    SprintId = sprintId,
                    SessionAvailable = true,
                    SessionId = newSession.SessionID,
                    SprintName = sprint.Name,
                    ProjectName = sprint.Project.Name
                };
                foreach (var client in clientList)
                {
                    client.ClientChannel.NotifyClientOfSession(sessionInfo);
                }
            }

            return newSession.SessionID;
        }

        /// <summary>
        /// Allows the scrum master to end a poker session.
        /// </summary>
        /// <param name="sessionId">The session.</param>
        public void EndPokerSession(Guid sessionId)
        {
            var sessions = activeSessions.GetListOfSessions();

            if (!sessions.ContainsKey(sessionId))
            {
                throw new ArgumentException("No session can be found", "sessionId");
            }

            var thisSession = sessions.Single(s => s.Key == sessionId).Value;
            var sprintId = thisSession.SprintID;

            if (thisSession.State == ServicePlanningPokerSessionState.Pending)
            {
                // Notify clients in this sprint group. 
                var availableClientsList = availableClients.GetListOfClients();
                if (availableClientsList.ContainsKey(sprintId))
                {
                    var clientList = availableClientsList[sprintId];

                    var sessionInfo = new ServiceSessionChangeInfo
                    {
                        SprintId = sprintId,
                        SessionAvailable = false,
                        SessionId = null
                    };

                    foreach (var client in clientList)
                    {
                        client.ClientChannel.NotifyClientOfSession(sessionInfo);
                    }
                }
            }

            var currentUserEmail = Thread.CurrentPrincipal.Identity.Name;

            // Notify and connected clients in the group that a session is no longer available 
            var connectedUsers = thisSession.Users.Where(u => u.EmailAddress != currentUserEmail);
            foreach (var user in connectedUsers)
            {
                user.ClientChannel.NotifyClientOfTerminatedSession();
            }

            sessions.Remove(sessionId);
        }

        /// <summary>
        /// Allows a client to join an active session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns>Information on the server side session.</returns>
        public ServicePlanningPokerSession JoinSession(Guid sessionId)
        {
            var sessions = activeSessions.GetListOfSessions();

            if (!sessions.ContainsKey(sessionId))
            {
                throw new ArgumentException("No session could be found", "sessionId");
            }

            var thisSession = sessions[sessionId];

            var currentUser = unitOfWork.UserRepository.Items.Single(u => u.EmailAddress == Thread.CurrentPrincipal.Identity.Name);

            var pendingUsers = availableClients.GetListOfClients();

            if (!pendingUsers.ContainsKey(thisSession.SprintID))
            {
                throw new InvalidOperationException("No clients for this sprint are currently connected");
            }

            var pendingUserEntry = pendingUsers[thisSession.SprintID].SingleOrDefault(u => u.UserID == currentUser.Id); 

            if (pendingUserEntry == null)
            {
                throw new InvalidOperationException("Current user is not currently in the pending clients list.");
            }

            var newUser = new ServicePlanningPokerUser
            {
                ClientChannel = pendingUserEntry.ClientChannel,
                EmailAddress = pendingUserEntry.EmailAddress,
                Name = pendingUserEntry.Name,
                UserID = pendingUserEntry.UserID,
                UserRoles = pendingUserEntry.UserRoles,
                Estimate = new ServicePlanningPokerEstimate()
            }; 

            thisSession.Users.Add(newUser);

            pendingUsers[thisSession.SprintID].Remove(pendingUserEntry);

            foreach (var user in thisSession.Users.Where(u => u != newUser))
            {
                user.ClientChannel.NotifyClientOfSessionUpdate(thisSession);
            }

            return thisSession;
        }

        /// <summary>
        /// Allows a user to exit the session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public void LeaveSession(Guid sessionId)
        {
            var sessions = activeSessions.GetListOfSessions();

            if (!sessions.ContainsKey(sessionId))
            {
                throw new ArgumentException("No session found", "sessionId");
            }

            var session = sessions[sessionId];

            var thisUserEmail = Thread.CurrentPrincipal.Identity.Name;

            var userInSession = session.Users.SingleOrDefault(u => u.EmailAddress == thisUserEmail);

            if (userInSession == null)
            {
                throw new InvalidOperationException("User not found in session");
            }

            if (userInSession.UserID == session.HostID)
            {
                throw new InvalidOperationException("User is the scrum master");
            }

            session.Users.Remove(userInSession);

            foreach (var user in session.Users)
            {
                user.ClientChannel.NotifyClientOfSessionUpdate(session);
            }            
        }

        /// <summary>
        /// Allows a scrum master to start the session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public void StartSession(Guid sessionId)
        {
            var sessions = activeSessions.GetListOfSessions();

            if (!sessions.ContainsKey(sessionId))
            {
                throw new ArgumentException("No session found", "sessionId");
            }

            var session = sessions[sessionId];

            var thisUserEmail = Thread.CurrentPrincipal.Identity.Name;

            var userInSession = session.Users.SingleOrDefault(u => u.EmailAddress == thisUserEmail);

            if (userInSession == null)
            {
                throw new InvalidOperationException("User not found in session");
            }

            if (userInSession.UserID != session.HostID)
            {
                throw new InvalidOperationException("User is not the scrum master");
            }

            session.State = ServicePlanningPokerSessionState.GettingEstimates;

            foreach (var user in session.Users.Where(u => u != userInSession))
            {
                user.ClientChannel.NotifyClientOfSessionStart();
            }
        }

        /// <summary>
        /// Allows the client to pull an up to date session state. 
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns>The current session.</returns>
        public ServicePlanningPokerSession RetrieveSessionInfo(Guid sessionId)
        {
            var sessions = activeSessions.GetListOfSessions();

            if (!sessions.ContainsKey(sessionId))
            {
                throw new ArgumentException("Session could not be found!", "sessionId");
            }

            var session = sessions[sessionId];

            var currentUserEmail = Thread.CurrentPrincipal.Identity.Name; 

            if (session.Users.SingleOrDefault(u => u.EmailAddress == currentUserEmail) == null)
            {
                throw new InvalidOperationException("User is not in the session"); 
            }

            return session;
        }

        /// <summary>
        /// Method to submit message to chat channel
        /// </summary>
        /// <param name="msg">The chatmessage object which is to be sent</param>
        public void SubmitMessageToServer(ServiceChatMessage msg)
        {
            Guid sessionId = msg.SessionID;

            var sessions = activeSessions.GetListOfSessions();

            if (!sessions.ContainsKey(sessionId))
            {
                throw new ArgumentException("No session could be found", "sessionId");
            }

            SendMessageToClients(msg);                      
        }

        /// <summary>
        /// Method to submit estimate
        /// </summary>
        /// <param name="estimate">The estimate object which is to be sent</param>
        public void SubmitEstimateToServer(ServicePlanningPokerEstimate estimate)
        {
            Guid sessionId = estimate.SessionID;
            var currentUser = unitOfWork.UserRepository.Items.Single(u => u.EmailAddress == Thread.CurrentPrincipal.Identity.Name);
            var sessions = activeSessions.GetListOfSessions();

            if (!sessions.ContainsKey(sessionId))
            {
                throw new ArgumentException("No session could be found", "sessionId");
            }

            ServicePlanningPokerUser user = sessions[sessionId].Users.FirstOrDefault(x => x.UserID == currentUser.Id);

            if (user == null)
            {
                throw new ArgumentException("User could not be found", "currentUser");
            }

            user.Estimate = estimate;

            var connectedClientsList = availableClients.GetListOfClients();
            var clientList = connectedClientsList[sessionId];

            foreach (var client in clientList)
            {
                client.ClientChannel.SendSessionToClient(sessions[sessionId]);
            }
        }

        /// <summary>
        /// Method for sending a message
        /// </summary>
        /// <param name="msg">the message object to be sent</param>
        public void SendMessageToClients(ServiceChatMessage msg)
        {
            var thisSession = msg.SessionID;

            var currentUser = unitOfWork.UserRepository.Items.Single(u => u.EmailAddress == Thread.CurrentPrincipal.Identity.Name);

            var connectedClientsList = availableClients.GetListOfClients();
            var clientList = connectedClientsList[msg.SessionID];

            msg.Name = "{0} {1}".Substitute(currentUser.FirstName, currentUser.LastName);

            foreach (var client in clientList)
            {
                client.ClientChannel.SendMessageToClient(msg);
            }
        }

        /// <summary>
        /// Method for telling clients to clear recieved estimates
        /// </summary>
        /// <param name="msg">the message object used to find the sessionID</param>
        public void ClearRecievedEstimatesToClients(ServiceChatMessage msg)
        {
            var thisSession = msg.SessionID;

            var currentUser = unitOfWork.UserRepository.Items.Single(u => u.EmailAddress == Thread.CurrentPrincipal.Identity.Name);

            var connectedClientsList = availableClients.GetListOfClients();
            var clientList = connectedClientsList[msg.SessionID];
            
            foreach (var client in clientList)
            {
                client.ClientChannel.NotifyClientToClearStoryPointEstimateFromServer();
            }
        }

        #endregion 
    }
}
