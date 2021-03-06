﻿using System;
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
            var session = new ServiceSessionChangeInfo();
            var sprintId = Guid.NewGuid();

            client.SubscribeToNewSessionMessages(sprintId).Returns(session);

            var res = serviceGateway.SubscribeToNewSessionMessages(sprintId);

            client.Received().SubscribeToNewSessionMessages(sprintId);
            res.Equals(session).Should().BeTrue();
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

        #region EndPokerSession(Guid sessionId) Tests

        [Test]
        public void EndPokerSession()
        {
            var sessionId = Guid.NewGuid();

            serviceGateway.EndPokerSession(sessionId);

            client.Received().EndPokerSession(sessionId);
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

        #region NotifyClientOfSessionUpdate(ServicePlanningPokerSession newSession) Tests

        [Test]
        public void NotifyClientOfSessionUpdate()
        {
            var waitObject = new ManualResetEvent(false);

            var session = new ServicePlanningPokerSession();

            serviceGateway.NotifyClientOfSessionUpdateEvent += (sender, args) =>
            {
                sender.Equals(serviceGateway).Should().BeTrue();
                args.Equals(session).Should().BeTrue();

                waitObject.Set();
            };

            serviceGateway.NotifyClientOfSessionUpdate(session);

            waitObject.WaitOne();
        }

        #endregion 

        #region RetrieveSessionInfo(Guid sessionId) Tests 

        [Test]
        public void CallRetrieveSessionInfo()
        {
            var sessionId = Guid.NewGuid();
            var session = new ServicePlanningPokerSession();

            client.RetrieveSessionInfo(sessionId).Returns(session);

            var res = serviceGateway.RetrieveSessionInfo(sessionId);

            client.Received().RetrieveSessionInfo(sessionId);

            res.Equals(session).Should().BeTrue();
        }

        #endregion 

        #region LeaveSession(Guid sessionId) Tests 

        [Test]
        public void LeaveSession()
        {
            var sessionId = Guid.NewGuid();

            serviceGateway.LeaveSession(sessionId);

            client.Received().LeaveSession(sessionId);
        }

        #endregion 

        #region StartSession(Guid sessionId) Tests 

        [Test]
        public void CallStartSession()
        {
            var sessionId = Guid.NewGuid();

            serviceGateway.StartSession(sessionId);

            client.Received().StartSession(sessionId);
        }

        #endregion 

        #region NotifyClientOfSessionStart() Tests

        [Test]
        public void NotifyClientOfSessionStart()
        {
            var waitObject = new ManualResetEvent(false);

            serviceGateway.NotifyClientOfSessionStartEvent += (sender, args) =>
            {
                sender.Equals(serviceGateway).Should().BeTrue();

                waitObject.Set();
            };

            serviceGateway.NotifyClientOfSessionStart();

            waitObject.WaitOne();
        }

        #endregion 

        #region SubmitMessageToServer(ServiceChatMessage msg) Tests 

        [Test]
        public void CallSubmitMessageToServer()
        {
            var newMessage = new ServiceChatMessage();

            serviceGateway.SubmitMessageToServer(newMessage);
            client.Received().SubmitMessageToServer(newMessage);
        }

        #endregion 

        #region SendMessageToClient() Tests

        [Test]
        public void SendMessageToClientTest()
        {
            var message = new ServiceChatMessage();

            var waitObject = new ManualResetEvent(false);

            serviceGateway.NotifyClientOfNewMessageFromServerEvent += (sender, args) =>
            {
                sender.Equals(serviceGateway).Should().BeTrue();
                args.Equals(message).Should().BeTrue();

                waitObject.Set();
            };

            serviceGateway.SendMessageToClient(message);

            waitObject.WaitOne();
        }

        #endregion 

        #region ShowEstimates(Guid sessionId) Tests 

        [Test]
        public void CallShowEstimates()
        {
            var sessionId = Guid.NewGuid();

            serviceGateway.ShowEstimates(sessionId);

            client.Received().ShowEstimates(sessionId);
        }

        #endregion 

        #region SubmitFinalEstimate(Guid sessionId, Guid sprintStoryId, double estimate) Tests 

        [Test]
        public void CallSubmitFinalEstimate()
        {
            var sessionId = Guid.NewGuid();
            var storyId = Guid.NewGuid();
            double estimate = 10;

            serviceGateway.SubmitFinalEstimate(sessionId, storyId, estimate);

            client.Received().SubmitFinalEstimate(sessionId, storyId, estimate);
        }

        #endregion 

    }

    #endregion 
}
