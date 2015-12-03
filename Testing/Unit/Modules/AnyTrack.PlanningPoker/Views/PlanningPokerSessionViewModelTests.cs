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
using System.Reflection;
using AnyTrack.Infrastructure;
using System.Security.Principal;

namespace Unit.Modules.AnyTrack.PlanningPoker.Views.PlanningPokerSessionViewModelTests
{
    #region Context

    public class Context
    {
        public static IPlanningPokerManagerServiceGateway gateway;

        public static PlanningPokerSessionViewModel vm; 

        [SetUp]
        public void SetUp()
        {
            gateway = Substitute.For<IPlanningPokerManagerServiceGateway>();
            vm = new PlanningPokerSessionViewModel(gateway);
        }
    }

    #endregion 

    #region Tests 

    public class PlanningPokerSessionViewModelTests : Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CallConstructorWithNull()
        {
            vm = new PlanningPokerSessionViewModel(null);
        }

        [Test]
        public void CallConstructor()
        {
            vm = new PlanningPokerSessionViewModel(gateway);

            vm.SprintStoriesCollection.Should().NotBeNull();

            vm.MessageHistories.Should().NotBeNull();

            vm.SendMessageCommand.Should().NotBeNull();

            vm.ShowEstimatesCommand.Should().NotBeNull();

            vm.SendEstimateCommand.Should().NotBeNull();

            vm.SendFinalEstimateCommand.Should().NotBeNull();
        }

        #endregion 

        #region IsNavigationTarget(NavigationContext navigationContext) Tests 

        [Test]
        public void CallIsNavTarget()
        {
            var sessionId = Guid.NewGuid();
            var navParams = new NavigationParameters();
            navParams.Add("sessionId", sessionId);

            var navService = Substitute.For<IRegionNavigationService>();

            var navContext = new NavigationContext(navService, new Uri("Test", UriKind.Relative), navParams);

            var res = vm.IsNavigationTarget(navContext);

            res.Should().BeFalse();

        }

        #endregion 

        #region ServiceGateway_NotifyClientOfNewMessageFromServerEvent(object sender, ServiceChatMessage msg) Tests 

        public void NotifyClientOfNewMessage()
        {
            var newMessage = new ServiceChatMessage
            {
                Name = "Test",
                Message = "Msg"
            };

            vm.Call("ServiceGateway_NotifyClientOfNewMessageFromServerEvent", null, newMessage);

            vm.MessageHistories.Count.Should().Be(1);
            vm.MessageHistories.Single().Contains(newMessage.Name).Should().BeTrue();
            vm.MessageHistories.Single().Contains(newMessage.Message).Should().BeTrue();
        }

        #endregion 

        #region SubmitMessageToServer() Tests 

        [Test]
        public void CallSubmitToServer()
        {
            var sessionId = Guid.NewGuid();

            vm.GetType().GetField("sessionId", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(vm, sessionId);

            ServiceChatMessage sentMessage = null;

            gateway.SubmitMessageToServer(Arg.Do<ServiceChatMessage>(a => sentMessage = a));

            vm.MessageToSend = "Test"; 

            vm.Call("SubmitMessageToServer");

            sentMessage.Should().NotBeNull();
            sentMessage.Message.Should().Be(vm.MessageToSend);
            sentMessage.Name.Should().Be("Me");
            gateway.Received().SubmitMessageToServer(sentMessage);
            vm.MessageHistories.Count.Should().Be(1);
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
        public void CallOnNavigatedToAndUserIsScrumMasterAndStateIsShowEstimates()
        {
            vm.RecievedEstimates = new System.Collections.ObjectModel.ObservableCollection<ServicePlanningPokerEstimate>()
            {
                new ServicePlanningPokerEstimate
                {
                    Name = "Old"
                }
            };

            var sessionId = Guid.NewGuid();

            var navParams = new NavigationParameters();
            navParams.Add("sessionId", sessionId);

            var navService = Substitute.For<IRegionNavigationService>();
            var navContext = new NavigationContext(navService, new Uri("Test", UriKind.Relative), navParams);            

            var userId = Guid.NewGuid();
            UserDetailsStore.LoggedInUserPrincipal = new GenericPrincipal(new GenericIdentity("test@agile.local"), null);

            var session = new ServicePlanningPokerSession
            {
                SessionID = sessionId,
                Users = new List<ServicePlanningPokerUser>
                {
                    new ServicePlanningPokerUser
                    {
                        UserID = userId,
                        EmailAddress = "test@agile.local",
                        Estimate = new ServicePlanningPokerEstimate
                        {
                            Estimate = 10,
                            Name = "New"
                        }
                    }
                }.ToArray(),
                State = ServicePlanningPokerSessionState.GettingEstimates,
                HostID = userId
            };

            gateway.RetrieveSessionInfo(sessionId).Returns(session);

            vm.OnNavigatedTo(navContext);

            vm.IsScrumMaster.Should().BeTrue();
            vm.RecievedEstimates.Count.Should().Be(1);
            vm.RecievedEstimates.Single().Name.Should().Be("New");
            vm.ShowEstimates.Should().BeFalse();
            vm.HideEstimates.Should().BeTrue();
        }

        [Test]
        public void CallOnNavigatedToAndUserIsScrumMaster()
        {
            vm.RecievedEstimates = new System.Collections.ObjectModel.ObservableCollection<ServicePlanningPokerEstimate>()
            {
                new ServicePlanningPokerEstimate
                {
                    Name = "Old"
                }
            };

            var sessionId = Guid.NewGuid();

            var navParams = new NavigationParameters();
            navParams.Add("sessionId", sessionId);

            var navService = Substitute.For<IRegionNavigationService>();
            var navContext = new NavigationContext(navService, new Uri("Test", UriKind.Relative), navParams);

            var userId = Guid.NewGuid();
            UserDetailsStore.LoggedInUserPrincipal = new GenericPrincipal(new GenericIdentity("test@agile.local"), null);

            var session = new ServicePlanningPokerSession
            {
                SessionID = sessionId,
                Users = new List<ServicePlanningPokerUser>
                {
                    new ServicePlanningPokerUser
                    {
                        UserID = userId,
                        EmailAddress = "test@agile.local",
                        Estimate = new ServicePlanningPokerEstimate
                        {
                            Estimate = 10,
                            Name = "New"
                        }
                    }
                }.ToArray(),
                State = ServicePlanningPokerSessionState.ShowingEstimates,
                HostID = userId
            };

            gateway.RetrieveSessionInfo(sessionId).Returns(session);

            vm.OnNavigatedTo(navContext);

            vm.IsScrumMaster.Should().BeTrue();
            vm.RecievedEstimates.Count.Should().Be(1);
            vm.RecievedEstimates.Single().Name.Should().Be("New");
            vm.ShowEstimates.Should().BeTrue();
            vm.HideEstimates.Should().BeFalse();
        }


        #endregion

    }


    #endregion 
}
