﻿
using AnyTrack.PlanningPoker.Views;
using NUnit.Framework;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.PlanningPoker.ServiceGateways;
using Prism.Regions;
using AnyTrack.PlanningPoker.BackendPlanningPokerManagerService;
using System.Windows;
using AnyTrack.SharedUtilities.Extensions;
using System.Reflection;
using AnyTrack.Infrastructure;

namespace Unit.Modules.AnyTrack.PlanningPoker.Views.SearchForPlanningPokerSessionViewModelTests
{
    #region Context 

    public class Context
    {
        public static IPlanningPokerManagerServiceGateway gateway;
        public static SearchForPlanningPokerSessionViewModel vm; 

        [SetUp]
        public void SetUp()
        {
            gateway = Substitute.For<IPlanningPokerManagerServiceGateway>();
            vm = new SearchForPlanningPokerSessionViewModel(gateway);
        }
    }

    #endregion 

    #region Tests 

    public class SearchForPlanningPokerSessionViewModelTests : Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoGateway()
        {
            vm = new SearchForPlanningPokerSessionViewModel(null);
        }

        [Test]
        public void ConstructVm()
        {
            vm.JoinPokerSession.Should().NotBeNull();
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
        public void CallOnNavigatedToNoSprintId()
        {
            var navService = Substitute.For<IRegionNavigationService>();
            var navContext = new NavigationContext(navService, new Uri("Test", UriKind.Relative));

            vm.OnNavigatedTo(navContext);

            gateway.DidNotReceive().SubscribeToNewSessionMessages(Arg.Any<Guid>());
        }

        [Test]
        public void CallOnNavigatedTo()
        {
            var sprintId = Guid.NewGuid();

            gateway.SubscribeToNewSessionMessages(sprintId).Returns(a => { return null; });

            var navService = Substitute.For<IRegionNavigationService>();
            var navContext = new NavigationContext(navService, new Uri("Test", UriKind.Relative));
            navContext.Parameters.Add("sprintId", sprintId);

            vm.OnNavigatedTo(navContext);

            gateway.Received().SubscribeToNewSessionMessages(sprintId);
        }

        [Test]
        public void CallOnNavigatedToAndASessionIsReturned()
        {
            var sprintId = Guid.NewGuid();

            var newSessionInfo = new ServiceSessionChangeInfo
            {
                SessionAvailable = true,
                SessionId = Guid.NewGuid(),
                SprintId = sprintId,
                SprintName = "Test",
                ProjectName = "Test"
            };

            gateway.SubscribeToNewSessionMessages(sprintId).Returns(newSessionInfo);

            var navService = Substitute.For<IRegionNavigationService>();
            var navContext = new NavigationContext(navService, new Uri("Test", UriKind.Relative));
            navContext.Parameters.Add("sprintId", sprintId);

            vm.OnNavigatedTo(navContext);

            gateway.Received().SubscribeToNewSessionMessages(sprintId);

            vm.ShowSearchingForSesion.Should().Be(Visibility.Collapsed);
            vm.ShowSessionFound.Should().Be(Visibility.Visible);
            vm.SessionAvailableText.Should().Be("There is a poker session available for project {0} and sprint {1} that you can now join.".Substitute(newSessionInfo.ProjectName, newSessionInfo.SprintName));
        }

        #endregion 

        #region HandleNotifyClientOfSessionEvent(object sender, ServiceSessionChangeInfo e) Tests 

        [Test]
        public void CallHandleNotifyClientOfSessionEventWithSessionAvailable()
        {
            var model = new ServiceSessionChangeInfo
            {
                SessionAvailable = true,
                SprintName = "Sprint",
                ProjectName = "Project",
                SessionId = Guid.NewGuid()
            };

            vm.Call("HandleNotifyClientOfSessionEvent", this, model);

            vm.ShowSearchingForSesion.Should().Be(Visibility.Collapsed);
            vm.ShowSessionFound.Should().Be(Visibility.Visible);
            vm.SessionAvailableText.Should().Be("There is a poker session available for project {0} and sprint {1} that you can now join.".Substitute(model.ProjectName, model.SprintName));
        }

        [Test]
        public void CallHandleNotifyClientOfSessionEventWithNo()
        {
            var model = new ServiceSessionChangeInfo
            {
                SessionAvailable = false,
                SprintName = "Sprint",
                ProjectName = "Project",
                SessionId = Guid.NewGuid()
            };

            vm.Call("HandleNotifyClientOfSessionEvent", this, model);

            vm.ShowSearchingForSesion.Should().Be(Visibility.Visible);
            vm.ShowSessionFound.Should().Be(Visibility.Collapsed);
        }

        #endregion 

        #region JoinAndNavigateToPokerSession() Tests 

        [Test]
        public void CallJoinAndNavigateToPokerSession()
        {
            var sessionId = Guid.NewGuid();
            vm.GetType().GetField("sessionId", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(vm, sessionId);

            var regionManager = Substitute.For<IRegionManager>();
            vm.RegionManager = regionManager;

            NavigationParameters sentParams = null;
            vm.RegionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(n => sentParams = n));

            vm.Call("JoinAndNavigateToPokerSession");

            sentParams.Should().NotBeNull();
            sentParams.ContainsKey("sessionId").Should().BeTrue();
            sentParams["sessionId"].Should().Be(sessionId);
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "PokerLobby", sentParams);
        }

        #endregion 
    }

    #endregion 
}
