using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using FluentAssertions;
using AnyTrack.PlanningPoker.ServiceGateways;
using AnyTrack.PlanningPoker.Views;
using Prism.Regions;
using AnyTrack.PlanningPoker.BackendPlanningPokerManagerService;
using AnyTrack.SharedUtilities.Extensions;
using AnyTrack.Infrastructure.Providers;
using AnyTrack.Infrastructure;
using System.Reflection;

namespace Unit.Modules.AnyTrack.PlanningPoker.Views.PokerLobbyViewModelTests
{
    #region Context

    public class Context
    {
        public static IPlanningPokerManagerServiceGateway gateway;
        public static PokerLobbyViewModel vm;

        [SetUp]
        public void Setup()
        {
            gateway = Substitute.For<IPlanningPokerManagerServiceGateway>();
            vm = new PokerLobbyViewModel(gateway);
        }
    }

    #endregion 

    #region Tests 

    public class PokerLobbyViewModelTests : Context
    {
        #region Constructor Tests 
        
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructNoGateway()
        {
            vm = new PokerLobbyViewModel(null);
        }

        [Test]
        public void Construct()
        {
            vm = new PokerLobbyViewModel(gateway);
            vm.Users.Should().NotBeNull();
            vm.EndPokerSession.Should().NotBeNull();
            vm.ExitSession.Should().NotBeNull();
            vm.StartSession.Should().NotBeNull();
        }

        #endregion 

        #region IsNavigationTarget(NavigationContext navigationContext) Tests 

        [Test]
        public void CallIsNavigationTarget()
        {
            var navService = Substitute.For<IRegionNavigationService>();
            var navContext = new NavigationContext(navService, new Uri("Test", UriKind.Relative));

            var res = vm.IsNavigationTarget(navContext);
            res.Should().BeFalse();
        }

        #endregion 

        #region OnNavigatedTo(NavigationContext navigationContext) Tests

        [Test]
        public void CallOnNavigatedToNoSprntId()
        {
            var navService = Substitute.For<IRegionNavigationService>();
            var navContext = new NavigationContext(navService, new Uri("Test", UriKind.Relative));

            vm.OnNavigatedTo(navContext);

            gateway.DidNotReceive().JoinSession(Arg.Any<Guid>());
            gateway.DidNotReceive().RetrieveSessionInfo(Arg.Any<Guid>());
        }

        [Test]
        public void CallOnNavigatedToAndNeedToJoin()
        {
            var sessionId = Guid.NewGuid();

            var navParams = new NavigationParameters();
            navParams.Add("sessionId", sessionId);

            var navService = Substitute.For<IRegionNavigationService>();
            var navContext = new NavigationContext(navService, new Uri("Test", UriKind.Relative), navParams);

            var session = new ServicePlanningPokerSession
            {
                Users = new List<ServicePlanningPokerUser>().ToArray(),
                SprintName = "Test",
                ProjectName = "Test"
            };

            gateway.JoinSession(sessionId).Returns(session);

            vm.Users = new System.Collections.ObjectModel.ObservableCollection<ServicePlanningPokerUser>()
            {
                new ServicePlanningPokerUser
                {
                    EmailAddress = "old@agile.local"
                }
            };

            vm.OnNavigatedTo(navContext);

            gateway.Received().JoinSession(sessionId);
            gateway.DidNotReceive().RetrieveSessionInfo(sessionId);
            vm.Users.Count.Should().Be(0);
            vm.LobbyHeaderText.Should().Be("{0} - {1}".Substitute(session.SprintName, session.ProjectName));
        }

        [Test]
        public void CallOnNavigatedToAndNoNeedToJoin()
        {
            var sessionId = Guid.NewGuid();

            var navParams = new NavigationParameters();
            navParams.Add("sessionId", sessionId);
            navParams.Add("joinRequired", false);

            var navService = Substitute.For<IRegionNavigationService>();
            var navContext = new NavigationContext(navService, new Uri("Test", UriKind.Relative), navParams);

            var session = new ServicePlanningPokerSession
            {
                Users = new List<ServicePlanningPokerUser>().ToArray(),
                SprintName = "Test",
                ProjectName = "Test"
            };

            gateway.RetrieveSessionInfo(sessionId).Returns(session);

            vm.Users = new System.Collections.ObjectModel.ObservableCollection<ServicePlanningPokerUser>()
            {
                new ServicePlanningPokerUser
                {
                    EmailAddress = "old@agile.local"
                }
            };

            vm.OnNavigatedTo(navContext);

            gateway.DidNotReceive().JoinSession(sessionId);
            gateway.Received().RetrieveSessionInfo(sessionId);
            vm.Users.Count.Should().Be(0);
            vm.LobbyHeaderText.Should().Be("{0} - {1}".Substitute(session.SprintName, session.ProjectName));
        }

        #endregion 

        #region HandleNotifyClientOfSessionUpdate(object sender, ServicePlanningPokerSession e) Tests 

        [Test]
        public void CallHandleNotifyClientOfSessionUpdate()
        {
            var session = new ServicePlanningPokerSession
            {
                Users = new List<ServicePlanningPokerUser>().ToArray(),
                SprintName = "Test",
                ProjectName = "Test"
            };

            vm.Users = new System.Collections.ObjectModel.ObservableCollection<ServicePlanningPokerUser>()
            {
                new ServicePlanningPokerUser
                {
                    EmailAddress = "old@agile.local"
                }
            };

            vm.Call("HandleNotifyClientOfSessionUpdate", null, session);
            vm.Users.Count.Should().Be(0);
            vm.LobbyHeaderText.Should().Be("{0} - {1}".Substitute(session.SprintName, session.ProjectName));
        }

        #endregion 

        #region HandleSessionTerminatedEvent(object sender, EventArgs e) Tests 

        [Test]
        public void CallHandleSessionTerminatedEvent()
        {
            vm.MainWindow = Substitute.For<WindowProvider>();
            vm.RegionManager = Substitute.For<IRegionManager>();

            vm.Call("HandleSessionTerminatedEvent", null, new EventArgs());

            vm.MainWindow.Received().ShowMessageAsync("Planning Poker Session Terminated!", "The planning poker session has been terminated.");
            vm.RegionManager.Received().RequestNavigate(RegionNames.MainRegion, "MyProjects");
        }

        #endregion 

        #region EndCurrentPokerSession() Tests 

        [Test]
        public void CallEndCurrentPokerSession()
        {
            var sessionId = Guid.NewGuid();
            vm.MainWindow = Substitute.For<WindowProvider>();
            vm.RegionManager = Substitute.For<IRegionManager>();

            vm.GetType().GetField("sessionId", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(vm, sessionId);

            vm.Call("EndCurrentPokerSession");
            gateway.Received().EndPokerSession(sessionId);
            vm.MainWindow.Received().ShowMessageAsync("Planning poker session terminated!", "The planning poker session has been terminated!");
            vm.RegionManager.Received().RequestNavigate(RegionNames.MainRegion, "StartPlanningPokerSession");
        }

        #endregion 

        #region ExitCurrentPokerSession() Tests 

        [Test]
        public void ExitCurrentPokerSession()
        {
            vm.MainWindow = Substitute.For<WindowProvider>();
            vm.RegionManager = Substitute.For<IRegionManager>();

            vm.Call("ExitCurrentPokerSession");

            vm.MainWindow.Received().ShowMessageAsync("Left the Planning poker session!", "You have now left the session.");
            vm.RegionManager.Received().RequestNavigate(RegionNames.MainRegion, "MyProjects");
        }

        #endregion 

        #region StartCurrentSession() Tests 

        [Test]
        public void CallStartCurrentSession()
        {
            var sessionId = Guid.NewGuid();
            vm.GetType().GetField("sessionId", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(vm, sessionId);

            vm.RegionManager = Substitute.For<IRegionManager>();

            NavigationParameters sentParams = null;

            vm.RegionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(n => sentParams = n));

            vm.Call("StartCurrentSession");

            gateway.Received().StartSession(sessionId);
            sentParams.Should().NotBeNull();
            sentParams["sessionId"].Should().Be(sessionId);
            vm.RegionManager.Received().RequestNavigate(RegionNames.MainRegion, "PlanningPokerSession", sentParams);


        }

        #endregion 

        #region HandleNotifyClientOfSessionStartEvent(object sender, EventArgs e) Tests 

        [Test]
        public void CallHandleNotifyClientOfSessionStartEvent()
        {
            var sessionId = Guid.NewGuid();
            vm.GetType().GetField("sessionId", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(vm, sessionId);

            NavigationParameters sentParams = null;

            vm.RegionManager = Substitute.For<IRegionManager>();

            vm.RegionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(n => sentParams = n));

            vm.Call("HandleNotifyClientOfSessionStartEvent", null, null);

            sentParams["sessionId"].Should().Be(sessionId);
            vm.RegionManager.Received().RequestNavigate(RegionNames.MainRegion, "PlanningPokerSession", sentParams);
        }

        #endregion 
    }

    #endregion 
}
