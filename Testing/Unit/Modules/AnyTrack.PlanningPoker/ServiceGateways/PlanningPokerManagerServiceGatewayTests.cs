using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using NSubstitute;
using Microsoft.Practices.Unity;
using AnyTrack.PlanningPoker.BackendPlanningPokerManagerService;
using AnyTrack.PlanningPoker.ServiceGateways;
using System.Threading;

namespace Unit.Modules.AnyTrack.PlanningPoker.ServiceGateways.PlanningPokerManagerServiceGatewayTests
{
    #region SetUp

    public class Context
    {
        public static IUnityContainer container;
        public static IPlanningPokerManagerService client;
        public static PlanningPokerManagerServiceGateway serviceGateway;

        [SetUp]
        public void Setup()
        {
            container = Substitute.For<IUnityContainer>();
            client = Substitute.For<IPlanningPokerManagerService>();

            container.Resolve<IPlanningPokerManagerService>(Arg.Any<ResolverOverride[]>()).Returns(client);

            serviceGateway = new PlanningPokerManagerServiceGateway(container);
        }
    }

    #endregion 

    #region Tests 

    public class PlanningPokerManagerServiceGatewayTests: Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoContainer()
        {
            serviceGateway = new PlanningPokerManagerServiceGateway(null);
        }

        #endregion 

        #region SubscribeToNewSessionMessages(Guid sprintId) Tests 

        [Test]
        public void CallSubscribeToNewSessionMessages()
        {
            var sprintId = Guid.NewGuid();

            serviceGateway.SubscribeToNewSessionMessages(sprintId);

            client.Received().SubscribeToNewSessionMessages(sprintId);
        }

        #endregion 

        #region StartNewPokerSession(Guid sprintId) Tests 
        
        [Test]
        public void CallStartNewPokerSession()
        {
            var sessionId = Guid.NewGuid();
            var sprintId = Guid.NewGuid();

            client.StartNewPokerSession(sprintId).Returns(sessionId);

            var res = serviceGateway.StartNewPokerSession(sprintId);

            client.Received().StartNewPokerSession(sprintId);
            res.Should().Be(sessionId);
        }

        #endregion 

        #region CancelPendingPokerSession(Guid sessionId) Tests 
        
        [Test]
        public void CallCancelPendingPokerSession()
        {
            var sessionId = Guid.NewGuid();

            serviceGateway.CancelPendingPokerSession(sessionId);

            client.Received().CancelPendingPokerSession(sessionId);
        }

        #endregion 

        #region JoinSession(Guid sessionId) Tests 

        [Test]
        public void JoinSession()
        {
            var sessionId = Guid.NewGuid();
            var serviceResult = new ServicePlanningPokerSession();
            client.JoinSession(sessionId).Returns(serviceResult);

            var res = serviceGateway.JoinSession(sessionId);
            client.Received().JoinSession(sessionId);
            res.Equals(serviceResult).Should().BeTrue();
        }

        #endregion 

        #region NotifyClientOfSession(ServiceSessionChangeInfo sessionInfo) Tests 
        
        [Test]
        public void CallNotifyClientOfSession()
        {
            var waitObject = new ManualResetEvent(false);

            var sessionInfo = new ServiceSessionChangeInfo();

            serviceGateway.NotifyClientOfSessionEvent += (sender, eh) =>
            {
                sender.Equals(serviceGateway).Should().BeTrue();
                eh.Equals(sessionInfo).Should().BeTrue();

                waitObject.Set();
                
            };

            serviceGateway.NotifyClientOfSession(sessionInfo);

            waitObject.WaitOne();
        }

        #endregion 

        #region NotifyClientOfTerminatedSession() Tests 

        [Test]
        public void NotifyClientOfTerminatedSession()
        {
            var waitObject = new ManualResetEvent(false);

            serviceGateway.NotifyClientOfTerminatedSessionEvent += (sender, args) =>
            {
                sender.Equals(serviceGateway).Should().BeTrue();
                args.Should().BeNull();

                waitObject.Set();
            };

            serviceGateway.NotifyClientOfTerminatedSession();

            waitObject.WaitOne();
        }

        #endregion 
    }

    #endregion 
}
