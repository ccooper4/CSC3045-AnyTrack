using AnyTrack.Backend.Service;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Providers;
using NSubstitute;
using AnyTrack.Backend.Data.Model;
using System.Security;
using AnyTrack.Backend.Security;
using System.Threading;
using AnyTrack.Backend.Service.Model;

namespace Unit.Backend.AnyTrack.Backend.Service.PlanningPokerManagerServiceTests
{
    #region Setup

    public class Context
    {
        public static IUnitOfWork unitOfWork;
        public static OperationContextProvider contextProvider;
        public static ActivePokerSessionsProvider activeSessionProvider;
        public static AvailableClientsProvider pendingClientsProvider; 
        public static PlanningPokerManagerService service; 

        [SetUp]
        public void SetUp()
        {
            unitOfWork = Substitute.For<IUnitOfWork>();
            contextProvider = Substitute.For<OperationContextProvider>();
            activeSessionProvider = Substitute.For<ActivePokerSessionsProvider>();
            pendingClientsProvider = Substitute.For<AvailableClientsProvider>();

            service = new PlanningPokerManagerService(unitOfWork, contextProvider, pendingClientsProvider, activeSessionProvider);
        }
    }

    #endregion 

    #region Tests 

    public class PlanningPokerManagerServiceTests : Context
    {
        #region Constructor Tests 

        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void ConstructWithNoUnitOfWork()
        {
            service = new PlanningPokerManagerService(null, contextProvider, pendingClientsProvider, activeSessionProvider);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void ConstructWithNoOperationContextProvider()
        {
            service = new PlanningPokerManagerService(unitOfWork, null, pendingClientsProvider, activeSessionProvider);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void ConstructWithPendingClientsProvider()
        {
            service = new PlanningPokerManagerService(unitOfWork, contextProvider, null, activeSessionProvider);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void ConstructWithNoActiveSessionProvider()
        {
            service = new PlanningPokerManagerService(unitOfWork, contextProvider, pendingClientsProvider, null);
        }

        #endregion 

        #region SubscribeToNewSessionMessages(Guid sprintId) Tests 

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void SubscribeWithNoSprint()
        {
            var sprintId = Guid.NewGuid();

            var sprints = new List<Sprint>().AsQueryable();
            unitOfWork.SprintRepository.Items.Returns(sprints);

            service.SubscribeToNewSessionMessages(sprintId);
        }

        [Test]
        [ExpectedException(typeof(SecurityException))]
        public void SubscribeButNotInSprintTeam()
        {
            var thisUser = new User
            {
                Id = Guid.NewGuid(),
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
            };

            unitOfWork.UserRepository.Items.Returns(new List<User> { thisUser }.AsQueryable());

            var generatedPrincipal = new GeneratedServiceUserPrincipal(thisUser);
            Thread.CurrentPrincipal = generatedPrincipal;

            var sprintId = Guid.NewGuid();
            var sprint = new Sprint
            {
                Id = sprintId,
                Team = new List<User>
                {
                    new User
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };

            unitOfWork.SprintRepository.Items.Returns(new List<Sprint>() { sprint }.AsQueryable());

            service.SubscribeToNewSessionMessages(sprintId);
        }

        [Test]
        public void Subscribe()
        {
            var sprintId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            var thisUser = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "David",
                LastName = "Tester",
                EmailAddress = "test@agile.local",
                Roles = new List<Role>() 
                {  
                    new Role  { RoleName = "Developer", ProjectId = projectId, SprintId = sprintId }
                }
            };

            unitOfWork.UserRepository.Items.Returns(new List<User> { thisUser }.AsQueryable());

            var generatedPrincipal = new GeneratedServiceUserPrincipal(thisUser);
            Thread.CurrentPrincipal = generatedPrincipal;
            
            var sprint = new Sprint
            {
                Id = sprintId,
                Team = new List<User>
                {
                    thisUser
                },
                Project = new Project
                {
                    Id = projectId
                }
            };

            unitOfWork.SprintRepository.Items.Returns(new List<Sprint>() { sprint }.AsQueryable());

            var clientChannel = Substitute.For<IPlanningPokerClientService>();
            contextProvider.GetClientChannel<IPlanningPokerClientService>().Returns(clientChannel);

            var pendingClients = new Dictionary<Guid, List<ServicePlanningPokerPendingUser>>();
            pendingClientsProvider.GetListOfClients().Returns(pendingClients);

            activeSessionProvider.GetListOfSessions().Returns(new Dictionary<Guid, ServicePlanningPokerSession>());

            var res = service.SubscribeToNewSessionMessages(sprintId);

            contextProvider.Received().GetClientChannel<IPlanningPokerClientService>();
            pendingClients.ContainsKey(sprintId).Should().BeTrue();
            pendingClients[sprintId].SingleOrDefault().Should().NotBeNull();
            var newPendingClient = pendingClients[sprintId].SingleOrDefault();
            newPendingClient.ClientChannel.Should().Be(clientChannel);
            newPendingClient.EmailAddress.Should().Be(thisUser.EmailAddress);
            newPendingClient.Name.Should().Be("David Tester");
            newPendingClient.UserRoles.SingleOrDefault().Should().Be("Developer");
            newPendingClient.UserID.Should().Be(thisUser.Id);
            res.Should().BeNull();
        }

        [Test]
        public void SubscribeWithASuitableSession()
        {
            var sprintId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            var thisUser = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "David",
                LastName = "Tester",
                EmailAddress = "test@agile.local",
                Roles = new List<Role>() 
                {  
                    new Role  { RoleName = "Developer", ProjectId = projectId, SprintId = sprintId }
                }
            };

            unitOfWork.UserRepository.Items.Returns(new List<User> { thisUser }.AsQueryable());

            var generatedPrincipal = new GeneratedServiceUserPrincipal(thisUser);
            Thread.CurrentPrincipal = generatedPrincipal;

            var sprint = new Sprint
            {
                Id = sprintId,
                Name = "Test",
                Project = new Project
                {
                    Id = projectId,
                    Name = "Test"
                },
                Team = new List<User>
                {
                    thisUser
                }
            };

            unitOfWork.SprintRepository.Items.Returns(new List<Sprint>() { sprint }.AsQueryable());

            var clientChannel = Substitute.For<IPlanningPokerClientService>();
            contextProvider.GetClientChannel<IPlanningPokerClientService>().Returns(clientChannel);

            var pendingClients = new Dictionary<Guid, List<ServicePlanningPokerPendingUser>>();
            pendingClientsProvider.GetListOfClients().Returns(pendingClients);

            var session = new ServicePlanningPokerSession
            {
                SessionID = Guid.NewGuid(),
                State = ServicePlanningPokerSessionState.Pending,
                SprintID = sprintId
            };

            var sessionDictionary = new Dictionary<Guid, ServicePlanningPokerSession>();
            sessionDictionary.Add(session.SessionID, session);

            activeSessionProvider.GetListOfSessions().Returns(sessionDictionary);

            var res = service.SubscribeToNewSessionMessages(sprintId);

            contextProvider.Received().GetClientChannel<IPlanningPokerClientService>();
            pendingClients.ContainsKey(sprintId).Should().BeTrue();
            pendingClients[sprintId].SingleOrDefault().Should().NotBeNull();
            var newPendingClient = pendingClients[sprintId].SingleOrDefault();
            newPendingClient.ClientChannel.Should().Be(clientChannel);
            newPendingClient.EmailAddress.Should().Be(thisUser.EmailAddress);
            newPendingClient.Name.Should().Be("David Tester");
            newPendingClient.UserRoles.SingleOrDefault().Should().Be("Developer");
            newPendingClient.UserID.Should().Be(thisUser.Id);
            res.Should().NotBeNull();
            res.SprintId.Should().Be(sprintId);
            res.SessionAvailable.Should().Be(true);
            res.SessionId.Should().Be(session.SessionID);
            res.SprintName.Should().Be(sprint.Name);
            res.ProjectName.Should().Be(sprint.Project.Name);
        }

        #endregion 

        #region StartNewPokerSession(Guid sprintId) Tests 

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void StartNewPokerSessionWithNoSprint()
        {
            var sprintId = Guid.NewGuid();

            var sprints = new List<Sprint>().AsQueryable();
            unitOfWork.SprintRepository.Items.Returns(sprints);

            service.StartNewPokerSession(sprintId);
        }

        [Test]
        [ExpectedException(typeof(SecurityException))]
        public void StartNewPokerSessionButNotInSprintTeam()
        {
            var thisUser = new User
            {
                Id = Guid.NewGuid(),
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
            };

            unitOfWork.UserRepository.Items.Returns(new List<User> { thisUser }.AsQueryable());

            var generatedPrincipal = new GeneratedServiceUserPrincipal(thisUser);
            Thread.CurrentPrincipal = generatedPrincipal;

            var sprintId = Guid.NewGuid();
            var sprint = new Sprint
            {
                Id = sprintId,
                Project = new Project
                {
                    ScrumMasters = new List<User>()
                }
            };

            unitOfWork.SprintRepository.Items.Returns(new List<Sprint>() { sprint }.AsQueryable());

            service.StartNewPokerSession(sprintId);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void StartNewPokerSessionSessionAlreadyExists()
        {
            var sprintId = Guid.NewGuid();

            var thisUser = new User
            {
                Id = Guid.NewGuid(),
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
                {
                    new Role { RoleName = "Scrum Master", SprintId = sprintId}
                }
            };

            unitOfWork.UserRepository.Items.Returns(new List<User> { thisUser }.AsQueryable());

            var generatedPrincipal = new GeneratedServiceUserPrincipal(thisUser);
            Thread.CurrentPrincipal = generatedPrincipal;

            var sprint = new Sprint
            {
                Id = sprintId,
                Project = new Project
                {
                    ScrumMasters = new List<User>()
                    {
                        thisUser
                    }
                }
            };

            unitOfWork.SprintRepository.Items.Returns(new List<Sprint>() { sprint }.AsQueryable());

            var currentSessionId = Guid.NewGuid();
            var currentSession = new ServicePlanningPokerSession
            {
                SprintID = sprintId,
                SessionID = currentSessionId
            };

            var sessionList = new Dictionary<Guid, ServicePlanningPokerSession>();
            sessionList.Add(currentSessionId, currentSession);
            activeSessionProvider.GetListOfSessions().Returns(sessionList);

            service.StartNewPokerSession(sprintId);
        }

        [Test]
        public void StartNewPokerSession()
        {
            var sprintId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            var thisUser = new User
            {
                Id = Guid.NewGuid(),
                EmailAddress = "test@agile.local",
                FirstName = "David",
                LastName = "Tester",
                Roles = new List<Role>()
                {
                    new Role { RoleName = "Scrum Master", ProjectId = projectId, SprintId = sprintId}
                }
            };

            unitOfWork.UserRepository.Items.Returns(new List<User> { thisUser }.AsQueryable());

            var generatedPrincipal = new GeneratedServiceUserPrincipal(thisUser);
            Thread.CurrentPrincipal = generatedPrincipal;

            var sprint = new Sprint
            {
                Id = sprintId,
                Project = new Project
                {
                    Id = projectId,
                    ScrumMasters = new List<User>()
                    {
                        thisUser
                    },
                    Name = "Test",
                },
                Name = "Test"
            };

            unitOfWork.SprintRepository.Items.Returns(new List<Sprint>() { sprint }.AsQueryable());

            var clientSocket = Substitute.For<IPlanningPokerClientService>();
            contextProvider.GetClientChannel<IPlanningPokerClientService>().Returns(clientSocket);

            var sessionList = new Dictionary<Guid, ServicePlanningPokerSession>();
            activeSessionProvider.GetListOfSessions().Returns(sessionList);

            ServiceSessionChangeInfo sentSessionInfo = null;

            var clientList = new Dictionary<Guid, List<ServicePlanningPokerPendingUser>>();
            var currentPendingClients = new List<ServicePlanningPokerPendingUser>();
            var pendingUserSocket = Substitute.For<IPlanningPokerClientService>();

            pendingUserSocket.NotifyClientOfSession(Arg.Do<ServiceSessionChangeInfo>(a => sentSessionInfo = a));

            currentPendingClients.Add(new ServicePlanningPokerPendingUser
            {
                ClientChannel = pendingUserSocket
            });

            clientList.Add(sprintId, currentPendingClients);
            pendingClientsProvider.GetListOfClients().Returns(clientList);

            var result = service.StartNewPokerSession(sprintId);

            contextProvider.Received().GetClientChannel<IPlanningPokerClientService>();
            sessionList.Count.Should().Be(1);
            var newSession = sessionList.Single().Value;
            newSession.HostID.Should().Be(thisUser.Id);
            newSession.SprintID.Should().Be(sprintId);
            newSession.Users.Should().NotBeNull();
            newSession.Users.Count.Should().Be(1);
            newSession.ProjectName.Should().Be(sprint.Project.Name);
            newSession.SprintName.Should().Be(sprint.Name);
            var singleUser = newSession.Users.Single();
            singleUser.ClientChannel.Should().Be(clientSocket);
            singleUser.EmailAddress.Should().Be(thisUser.EmailAddress);
            singleUser.Name.Should().Be("David Tester");
            singleUser.UserID.Should().Be(thisUser.Id);
            singleUser.UserRoles.Single().Should().Be("Scrum Master");
            newSession.State.Should().Be(ServicePlanningPokerSessionState.Pending);
            sentSessionInfo.Should().NotBeNull();
            sentSessionInfo.SprintId.Should().Be(sprintId);
            sentSessionInfo.SessionAvailable.Should().BeTrue();
            sentSessionInfo.SessionId.Should().Be(newSession.SessionID);
            sentSessionInfo.ProjectName.Should().Be(sprint.Project.Name);
            sentSessionInfo.SprintName.Should().Be(sprint.Name);
            pendingUserSocket.Received().NotifyClientOfSession(sentSessionInfo);

            result.Should().Be(newSession.SessionID);
        }

        #endregion 

        #region EndPokerSession(Guid sessionId) Tests

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void EndPokerSessionWithNoSession()
        {
            var sessionId = Guid.NewGuid();

            var activeSessions = new Dictionary<Guid, ServicePlanningPokerSession>();
            activeSessionProvider.GetListOfSessions().Returns(activeSessions);

            service.EndPokerSession(sessionId);
        }

        [Test]
        public void EndPokerSession()
        {
            var sessionId = Guid.NewGuid();
            var sprintId = Guid.NewGuid();

            var thisUser = new User
            {
                Id = Guid.NewGuid(),
                EmailAddress = "test@agile.local",
                FirstName = "David",
                LastName = "Tester",
                Roles = new List<Role>()
                {
                    new Role { RoleName = "Scrum Master", SprintId = sprintId}
                }
            };

            unitOfWork.UserRepository.Items.Returns(new List<User> { thisUser }.AsQueryable());

            var generatedPrincipal = new GeneratedServiceUserPrincipal(thisUser);
            Thread.CurrentPrincipal = generatedPrincipal;

            var thisUserChannel = Substitute.For<IPlanningPokerClientService>();
            var otherUserChannel = Substitute.For<IPlanningPokerClientService>();

            var activeSessions = new Dictionary<Guid, ServicePlanningPokerSession>();
            var session = new ServicePlanningPokerSession
            {
                SessionID = sessionId,
                SprintID = sprintId,
                State = ServicePlanningPokerSessionState.Pending,
                Users = new List<ServicePlanningPokerUser>
                {
                    new ServicePlanningPokerUser
                    {
                        EmailAddress = thisUser.EmailAddress,
                        ClientChannel = thisUserChannel
                    },
                    new ServicePlanningPokerUser 
                    {
                        EmailAddress = "other@agile.local",
                        ClientChannel = otherUserChannel
                    }
                }                
            };
            activeSessions.Add(sessionId, session);
            activeSessionProvider.GetListOfSessions().Returns(activeSessions);

            var pendingUserChannel = Substitute.For<IPlanningPokerClientService>();

            ServiceSessionChangeInfo sentSessionInfo = null; 
            pendingUserChannel.NotifyClientOfSession(Arg.Do<ServiceSessionChangeInfo>(s => sentSessionInfo = s));

            var pendingClients = new Dictionary<Guid, List<ServicePlanningPokerPendingUser>>();
            var pendingClientList = new List<ServicePlanningPokerPendingUser>();
            pendingClientList.Add(new ServicePlanningPokerPendingUser
            {
                ClientChannel = pendingUserChannel
            });
            pendingClients.Add(sprintId, pendingClientList);

            pendingClientsProvider.GetListOfClients().Returns(pendingClients);

            service.EndPokerSession(sessionId);

            activeSessionProvider.Received().GetListOfSessions();
            pendingClientsProvider.Received().GetListOfClients();
            sentSessionInfo.Should().NotBeNull();
            pendingUserChannel.Received().NotifyClientOfSession(sentSessionInfo);
            sentSessionInfo.SprintId.Should().Be(sprintId);
            sentSessionInfo.SessionAvailable.Should().BeFalse();
            sentSessionInfo.SessionId.HasValue.Should().BeFalse();

            thisUserChannel.DidNotReceive().NotifyClientOfTerminatedSession();
            otherUserChannel.Received().NotifyClientOfTerminatedSession();
        }

        #endregion 

        #region JoinSession(Guid sessionId) Tests 

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void JoinASessionWithNoSession()
        {
            var sessionId = Guid.NewGuid();

            var activeSessions = new Dictionary<Guid, ServicePlanningPokerSession>();
            activeSessionProvider.GetListOfSessions().Returns(activeSessions);

            service.JoinSession(sessionId);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void JoinASessionWithNoPendingSprintGroup()
        {
            var thisUser = new User
            {
                Id = Guid.NewGuid(),
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
            };

            unitOfWork.UserRepository.Items.Returns(new List<User> { thisUser }.AsQueryable());

            var generatedPrincipal = new GeneratedServiceUserPrincipal(thisUser);
            Thread.CurrentPrincipal = generatedPrincipal;

            var sprintId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();

            var session = new ServicePlanningPokerSession
            {
                SessionID = sessionId
            };

            var sessions = new Dictionary<Guid, ServicePlanningPokerSession>();
            sessions.Add(sessionId, session);
            activeSessionProvider.GetListOfSessions().Returns(sessions);

            var pendingUsersDictionary = new Dictionary<Guid, List<ServicePlanningPokerPendingUser>>();

            pendingClientsProvider.GetListOfClients().Returns(pendingUsersDictionary);

            service.JoinSession(sessionId);
            
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void JoinASessionAndUserNotInPendingList()
        {
            var thisUser = new User
            {
                Id = Guid.NewGuid(),
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
            };

            unitOfWork.UserRepository.Items.Returns(new List<User> { thisUser }.AsQueryable());

            var generatedPrincipal = new GeneratedServiceUserPrincipal(thisUser);
            Thread.CurrentPrincipal = generatedPrincipal;

            var sprintId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();

            var session = new ServicePlanningPokerSession
            {
                SessionID = sessionId
            };

            var sessions = new Dictionary<Guid, ServicePlanningPokerSession>();
            sessions.Add(sessionId, session);
            activeSessionProvider.GetListOfSessions().Returns(sessions);

            var pendingUsersDictionary = new Dictionary<Guid, List<ServicePlanningPokerPendingUser>>();
            pendingUsersDictionary.Add(sprintId, new List<ServicePlanningPokerPendingUser>());
            pendingClientsProvider.GetListOfClients().Returns(pendingUsersDictionary);

            service.JoinSession(sessionId);
        }

        [Test]
        public void JoinASessionWithExistingUsers()
        {
            var sprintId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();

            var thisUser = new User
            {
                Id = Guid.NewGuid(),
                EmailAddress = "test@agile.local",
                FirstName = "David",
                LastName = "Tester",
                Roles = new List<Role>()
                {
                    new Role
                    {
                        RoleName = "Developer", SprintId = sprintId
                    }
                }
            };

            unitOfWork.UserRepository.Items.Returns(new List<User> { thisUser }.AsQueryable());

            var generatedPrincipal = new GeneratedServiceUserPrincipal(thisUser);
            Thread.CurrentPrincipal = generatedPrincipal;

            var session = new ServicePlanningPokerSession
            {
                SprintID = sprintId,
                SessionID = sessionId,
                Users = new List<ServicePlanningPokerUser>()
            };

            var sessions = new Dictionary<Guid, ServicePlanningPokerSession>();
            sessions.Add(sessionId, session);
            activeSessionProvider.GetListOfSessions().Returns(sessions);

            var userChannel = Substitute.For<IPlanningPokerClientService>();

            var pendingUsersDictionary = new Dictionary<Guid, List<ServicePlanningPokerPendingUser>>();
            var pendingUserList = new List<ServicePlanningPokerPendingUser>();
            var pendingUser = new ServicePlanningPokerPendingUser
            {
                ClientChannel = userChannel,
                UserID = thisUser.Id,
                EmailAddress = thisUser.EmailAddress,
                Name = thisUser.FirstName + " " + thisUser.LastName,
                UserRoles = thisUser.Roles.Select(u => u.RoleName).ToList()
            };
            pendingUserList.Add(pendingUser);
            pendingUsersDictionary.Add(sprintId, pendingUserList);

            pendingClientsProvider.GetListOfClients().Returns(pendingUsersDictionary);

            var returnSession = service.JoinSession(sessionId);

            activeSessionProvider.Received().GetListOfSessions();
            pendingClientsProvider.Received().GetListOfClients();
            pendingUsersDictionary[sprintId].Count.Should().Be(0);
            session.Users.Count.Should().Be(1);
            var singleSessionUser = session.Users.Single();
            singleSessionUser.ClientChannel.Should().Be(pendingUser.ClientChannel);
            singleSessionUser.EmailAddress.Should().Be(pendingUser.EmailAddress);
            singleSessionUser.Name.Should().Be(pendingUser.Name);
            singleSessionUser.UserID.Should().Be(pendingUser.UserID);
            singleSessionUser.UserRoles.Equals(pendingUser.UserRoles).Should().BeTrue();
            returnSession.Should().Be(session);

        }

        [Test]
        public void JoinASession()
        {
            var sprintId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();

            var thisUser = new User
            {
                Id = Guid.NewGuid(),
                EmailAddress = "test@agile.local",
                FirstName = "David",
                LastName = "Tester",
                Roles = new List<Role>()
                {
                    new Role
                    {
                        RoleName = "Developer", SprintId = sprintId
                    }
                }
            };

            unitOfWork.UserRepository.Items.Returns(new List<User> { thisUser }.AsQueryable());

            var generatedPrincipal = new GeneratedServiceUserPrincipal(thisUser);
            Thread.CurrentPrincipal = generatedPrincipal;

            var existingUserChannel = Substitute.For<IPlanningPokerClientService>();

            var session = new ServicePlanningPokerSession
            {
                SprintID = sprintId,
                SessionID = sessionId,
                Users = new List<ServicePlanningPokerUser>()
                {
                    new ServicePlanningPokerUser
                    {
                        ClientChannel = existingUserChannel,
                        EmailAddress = "other@agile.local"
                    }
                }
            };

            var sessions = new Dictionary<Guid, ServicePlanningPokerSession>();
            sessions.Add(sessionId, session);
            activeSessionProvider.GetListOfSessions().Returns(sessions);

            var userChannel = Substitute.For<IPlanningPokerClientService>();

            var pendingUsersDictionary = new Dictionary<Guid, List<ServicePlanningPokerPendingUser>>();
            var pendingUserList = new List<ServicePlanningPokerPendingUser>();
            var pendingUser = new ServicePlanningPokerPendingUser
            {
                ClientChannel = userChannel,
                UserID = thisUser.Id,
                EmailAddress = thisUser.EmailAddress,
                Name = thisUser.FirstName + " " + thisUser.LastName,
                UserRoles = thisUser.Roles.Select(u => u.RoleName).ToList()
            };
            pendingUserList.Add(pendingUser);
            pendingUsersDictionary.Add(sprintId, pendingUserList);

            pendingClientsProvider.GetListOfClients().Returns(pendingUsersDictionary);

            var returnSession = service.JoinSession(sessionId);

            activeSessionProvider.Received().GetListOfSessions();
            pendingClientsProvider.Received().GetListOfClients();
            pendingUsersDictionary[sprintId].Count.Should().Be(0);
            session.Users.Count.Should().Be(2);
            var singleSessionUser = session.Users.Last();
            singleSessionUser.ClientChannel.Should().Be(pendingUser.ClientChannel);
            singleSessionUser.EmailAddress.Should().Be(pendingUser.EmailAddress);
            singleSessionUser.Name.Should().Be(pendingUser.Name);
            singleSessionUser.UserID.Should().Be(pendingUser.UserID);
            singleSessionUser.UserRoles.Equals(pendingUser.UserRoles).Should().BeTrue();
            returnSession.Should().Be(session);

            existingUserChannel.Received().NotifyClientOfSessionUpdate(session);
            pendingUser.ClientChannel.DidNotReceive().NotifyClientOfSessionUpdate(session);

        }

        #endregion 

        #region ServicePlanningPokerSession RetrieveSessionInfo(Guid sessionId) Tests 

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CallForASessionThatDoesntExist()
        {
            var sessionId = Guid.NewGuid();
            activeSessionProvider.GetListOfSessions().Returns(new Dictionary<Guid, ServicePlanningPokerSession>());

            service.RetrieveSessionInfo(sessionId);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CallForASessionThatIAmNotAUserIn()
        {
            var user = new User
            {
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
            };

            var principal = new GeneratedServiceUserPrincipal(user);
            Thread.CurrentPrincipal = principal;

            var sessionId = Guid.NewGuid();

            var session = new ServicePlanningPokerSession()
            {
                SessionID = sessionId,
                Users = new List<ServicePlanningPokerUser>()
            };

            var sessionDictionary = new Dictionary<Guid, ServicePlanningPokerSession>();
            sessionDictionary.Add(session.SessionID, session);

            activeSessionProvider.GetListOfSessions().Returns(sessionDictionary);

            service.RetrieveSessionInfo(sessionId);
        }

        [Test]
        public void CallForASessionThatIAmIn()
        {
            var user = new User
            {
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
            };

            var principal = new GeneratedServiceUserPrincipal(user);
            Thread.CurrentPrincipal = principal;

            var sessionId = Guid.NewGuid();

            var session = new ServicePlanningPokerSession()
            {
                SessionID = sessionId,
                Users = new List<ServicePlanningPokerUser>()
                {
                    new ServicePlanningPokerUser
                    {
                        EmailAddress = "test@agile.local"
                    }
                }
            };

            var sessionDictionary = new Dictionary<Guid, ServicePlanningPokerSession>();
            sessionDictionary.Add(session.SessionID, session);

            activeSessionProvider.GetListOfSessions().Returns(sessionDictionary);

            var res = service.RetrieveSessionInfo(sessionId);
            res.Equals(session).Should().BeTrue();
        }

        #endregion 

        #region LeaveSession(Guid sessionId) Tests 

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CallLeaveSessionWithNoSession()
        {
            var sessionId = Guid.NewGuid();
            activeSessionProvider.GetListOfSessions().Returns(new Dictionary<Guid, ServicePlanningPokerSession>());

            service.LeaveSession(sessionId);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CallLeaveSessionAndUserNotInSession()
        {
            var sessionId = Guid.NewGuid();
            
            var session = new ServicePlanningPokerSession
            {
                SessionID = sessionId,
                Users = new List<ServicePlanningPokerUser>()
            }; 

            var dictionary = new Dictionary<Guid, ServicePlanningPokerSession>(); 
            dictionary.Add(sessionId, session);

            activeSessionProvider.GetListOfSessions().Returns(dictionary);

            var user = new User
            {
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
            };

            Thread.CurrentPrincipal = new GeneratedServiceUserPrincipal(user);

            service.LeaveSession(sessionId);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CallLeaveSessionAndUserIsScrumMaster()
        {
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var session = new ServicePlanningPokerSession
            {
                SessionID = sessionId,
                Users = new List<ServicePlanningPokerUser>()
                {
                    new ServicePlanningPokerUser
                    {
                        UserID = userId,
                        EmailAddress = "test@agile.local"
                    }
                },
                HostID = userId
            };

            var dictionary = new Dictionary<Guid, ServicePlanningPokerSession>();
            dictionary.Add(sessionId, session);

            activeSessionProvider.GetListOfSessions().Returns(dictionary);

            var user = new User
            {
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
            };

            Thread.CurrentPrincipal = new GeneratedServiceUserPrincipal(user);

            service.LeaveSession(sessionId);
        }

        [Test]
        public void CallLeaveSession()
        {
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var otherClientChannel = Substitute.For<IPlanningPokerClientService>();

            var thisUser = new ServicePlanningPokerUser
            {
                UserID = userId,
                EmailAddress = "test@agile.local"
            };

            var session = new ServicePlanningPokerSession
            {
                SessionID = sessionId,
                Users = new List<ServicePlanningPokerUser>()
                {
                    thisUser,
                    new ServicePlanningPokerUser
                    {
                        ClientChannel = otherClientChannel,
                        UserID = Guid.NewGuid(),
                        EmailAddress = "other@agile.local"
                    }
                },
            };

            var dictionary = new Dictionary<Guid, ServicePlanningPokerSession>();
            dictionary.Add(sessionId, session);

            activeSessionProvider.GetListOfSessions().Returns(dictionary);

            var user = new User
            {
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
            };

            Thread.CurrentPrincipal = new GeneratedServiceUserPrincipal(user);

            service.LeaveSession(sessionId);

            session.Users.Contains(thisUser).Should().BeFalse();
            otherClientChannel.Received().NotifyClientOfSessionUpdate(session);

            
        }

        #endregion 

        #region StartSession(Guid sessionId) Tests

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CallStartSessionWithNoSession()
        {
            var sessionId = Guid.NewGuid();
            activeSessionProvider.GetListOfSessions().Returns(new Dictionary<Guid, ServicePlanningPokerSession>());

            service.StartSession(sessionId);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CallStartSessionAndUserNotInSession()
        {
            var sessionId = Guid.NewGuid();

            var session = new ServicePlanningPokerSession
            {
                SessionID = sessionId,
                Users = new List<ServicePlanningPokerUser>()
            };

            var dictionary = new Dictionary<Guid, ServicePlanningPokerSession>();
            dictionary.Add(sessionId, session);

            activeSessionProvider.GetListOfSessions().Returns(dictionary);

            var user = new User
            {
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
            };

            Thread.CurrentPrincipal = new GeneratedServiceUserPrincipal(user);

            service.StartSession(sessionId);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CallStartSessionAndUserIsNotScrumMaster()
        {
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var session = new ServicePlanningPokerSession
            {
                SessionID = sessionId,
                Users = new List<ServicePlanningPokerUser>()
                {
                    new ServicePlanningPokerUser
                    {
                        UserID = userId,
                        EmailAddress = "test@agile.local"
                    }
                },
                HostID = Guid.NewGuid()
            };

            var dictionary = new Dictionary<Guid, ServicePlanningPokerSession>();
            dictionary.Add(sessionId, session);

            activeSessionProvider.GetListOfSessions().Returns(dictionary);

            var user = new User
            {
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
            };

            Thread.CurrentPrincipal = new GeneratedServiceUserPrincipal(user);

            service.StartSession(sessionId);
        }

        [Test]
        public void CallStartSession()
        {
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var otherClientChannel = Substitute.For<IPlanningPokerClientService>();

            var thisUser = new ServicePlanningPokerUser
            {
                UserID = userId,
                EmailAddress = "test@agile.local"
            };

            var session = new ServicePlanningPokerSession
            {
                SessionID = sessionId,
                Users = new List<ServicePlanningPokerUser>()
                {
                    thisUser,
                    new ServicePlanningPokerUser
                    {
                        ClientChannel = otherClientChannel,
                        UserID = Guid.NewGuid(),
                        EmailAddress = "other@agile.local"
                    }
                },
                HostID = userId
            };

            var dictionary = new Dictionary<Guid, ServicePlanningPokerSession>();
            dictionary.Add(sessionId, session);

            activeSessionProvider.GetListOfSessions().Returns(dictionary);

            var user = new User
            {
                EmailAddress = "test@agile.local",
                Roles = new List<Role>()
            };

            Thread.CurrentPrincipal = new GeneratedServiceUserPrincipal(user);

            service.StartSession(sessionId);

            session.State.Should().Be(ServicePlanningPokerSessionState.GettingEstimates);
            otherClientChannel.Received().NotifyClientOfSessionStart();
        }

        #endregion 
    }

    #endregion 

}
