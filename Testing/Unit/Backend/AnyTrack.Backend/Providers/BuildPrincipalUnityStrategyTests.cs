using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using FluentAssertions;
using AnyTrack.Backend.Security;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Providers;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using NSubstitute;
using Microsoft.Practices.ObjectBuilder2;
using System.ServiceModel;
using System.ServiceModel.Channels;
using AnyTrack.Backend.Data.Model;
using System.Web.Security;
using System.Threading;
using System.Security.Principal;

namespace Unit.Backend.AnyTrack.Backend.Providers.BuildPrincipalUnityStrategyTests
{
    #region Supporting Types

    [CreatePrincipal]
    public class TestServiceWithAttribute
    {
        public TestServiceWithAttribute()
        {
        }

        public void DoStuff()
        {
        }
    }

    public class TestServiceWithMethodAttribute
    {
        public TestServiceWithMethodAttribute()
        {
        }

        [CreatePrincipal]
        public void DoStuff()
        {
        }

        public void Other()
        {
        }
    }

    #endregion 

    #region Context

    public class Context
    {
        public static IUnitOfWork unitOfWork;
        public static FormsAuthenticationProvider provider;
        public static OperationContextProvider context;
        public static IUnityContainer container;

        public static BuildPrincipalUnityStrategy strategy; 

        [SetUp]
        public void SetUp()
        {
            unitOfWork = Substitute.For<IUnitOfWork>();
            provider = Substitute.For<FormsAuthenticationProvider>();
            context = Substitute.For<OperationContextProvider>();
            container = Substitute.For<IUnityContainer>();

            container.Resolve<IUnitOfWork>().Returns(unitOfWork);
            container.Resolve<FormsAuthenticationProvider>().Returns(provider);
            container.Resolve<OperationContextProvider>().Returns(context);

            strategy = new BuildPrincipalUnityStrategy(container);
        }
    }

    #endregion

    #region Tests  

    public class BuildPrincipalUnityStrategyTests : Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildWithNoContainer()
        {
            strategy = new BuildPrincipalUnityStrategy(null);
        }

        #endregion 

        #region PostBuildUp(IBuilderContext context) Tests 

        [Test]
        public void CallWithNoAttribute()
        {
            var obj = new TestServiceWithMethodAttribute();
            var methodInfo = obj.GetType().GetMethod("Other");

            context.GetMethodInfoForServiceCall().Returns(methodInfo);

            var unityContext = Substitute.For<IBuilderContext>();
            unityContext.Existing.Returns(obj);

            strategy.PostBuildUp(unityContext);

            container.Received().Resolve<OperationContextProvider>();
            context.Received().GetMethodInfoForServiceCall();
            container.DidNotReceive().Resolve<FormsAuthenticationProvider>();
            container.DidNotReceive().Resolve<IUnitOfWork>();

        }

        [Test]
        public void CallWithHttpContextPrincipalAvailable()
        {
            var obj = new TestServiceWithAttribute();
            var methodInfo = obj.GetType().GetMethod("DoStuff");

            context.GetMethodInfoForServiceCall().Returns(methodInfo);

            var unityContext = Substitute.For<IBuilderContext>();
            unityContext.Existing.Returns(obj);

            var user = new User { EmailAddress = "tester@agile.local", Roles = new List<Role>() };
            unitOfWork.UserRepository.Items.Returns(new List<User>() { user }.AsQueryable());

            var contextPrincipal = new GeneratedServiceUserPrincipal(user);
            context.GetHttpContextUser().Returns(contextPrincipal);

            strategy.PostBuildUp(unityContext);

            context.Received().GetHttpContextUser();
            Thread.CurrentPrincipal.Equals(contextPrincipal).Should().BeTrue();
        }

        [Test]
        public void CallWithAttributeForNonExpiredToken()
        {
            var obj = new TestServiceWithAttribute();
            var methodInfo = obj.GetType().GetMethod("DoStuff");

            context.GetMethodInfoForServiceCall().Returns(methodInfo);

            var unityContext = Substitute.For<IBuilderContext>();
            unityContext.Existing.Returns(obj);

            var authCookie = "test";
            var user = new User { EmailAddress = "tester@agile.local", Roles = new List<Role>() };

            context.GetHttpContextUser().Returns(c => { return null; });

            var headers = new MessageHeaders(MessageVersion.Soap11);

            var header = MessageHeader.CreateHeader("authCookie", "http://anytrack", authCookie);

            headers.Add(header);

            context.IncomingMessageHeaders.Returns(headers);

            var decryptedTicket = new FormsAuthenticationTicket("tester@agile.local", false, 100);
            provider.Decrypt(authCookie).Returns(decryptedTicket);

            unitOfWork.UserRepository.Items.Returns(new List<User>() { user }.AsQueryable());

            strategy.PostBuildUp(unityContext);

            provider.Received().Decrypt(authCookie);
            Thread.CurrentPrincipal.Should().NotBeNull();
            Thread.CurrentPrincipal.Should().BeOfType<GeneratedServiceUserPrincipal>();
            Thread.CurrentPrincipal.Identity.Should().NotBeNull();
            Thread.CurrentPrincipal.Identity.Name.Should().Be(user.EmailAddress);
            Thread.CurrentPrincipal.Identity.IsAuthenticated.Should().BeTrue();
            Thread.CurrentPrincipal.Identity.AuthenticationType.Should().Be("FormsAuthentication");
        }

        [Test]
        public void CallWithMethodAttributeForNonExpiredToken()
        {
            var obj = new TestServiceWithMethodAttribute();
            var methodInfo = obj.GetType().GetMethod("DoStuff");

            context.GetMethodInfoForServiceCall().Returns(methodInfo);

            var unityContext = Substitute.For<IBuilderContext>();
            unityContext.Existing.Returns(obj);

            var authCookie = "test";
            var user = new User { EmailAddress = "tester@agile.local", Roles = new List<Role>() };

            context.GetHttpContextUser().Returns(c => { return null; });

            var headers = new MessageHeaders(MessageVersion.Soap11);

            var header = MessageHeader.CreateHeader("authCookie", "http://anytrack", authCookie);

            headers.Add(header);

            context.IncomingMessageHeaders.Returns(headers);

            var decryptedTicket = new FormsAuthenticationTicket("tester@agile.local", false, 100);
            provider.Decrypt(authCookie).Returns(decryptedTicket);

            unitOfWork.UserRepository.Items.Returns(new List<User>() { user }.AsQueryable());

            strategy.PostBuildUp(unityContext);

            provider.Received().Decrypt(authCookie);
            Thread.CurrentPrincipal.Should().NotBeNull();
            Thread.CurrentPrincipal.Should().BeOfType<GeneratedServiceUserPrincipal>();
            Thread.CurrentPrincipal.Identity.Should().NotBeNull();
            Thread.CurrentPrincipal.Identity.Name.Should().Be(user.EmailAddress);
            Thread.CurrentPrincipal.Identity.IsAuthenticated.Should().BeTrue();
            Thread.CurrentPrincipal.Identity.AuthenticationType.Should().Be("FormsAuthentication");
        }

        [Test]
        public void CallWithAttributeForExpiredToken()
        {
            var obj = new TestServiceWithAttribute();
            var methodInfo = obj.GetType().GetMethod("DoStuff");

            context.GetMethodInfoForServiceCall().Returns(methodInfo);

            var unityContext = Substitute.For<IBuilderContext>();
            unityContext.Existing.Returns(obj);

            var authCookie = "test";
            var user = new User { EmailAddress = "tester@agile.local", Roles = new List<Role>() };

            context.GetHttpContextUser().Returns(c => { return null; });

            var headers = new MessageHeaders(MessageVersion.Soap11);

            var header = MessageHeader.CreateHeader("authCookie", "http://anytrack", authCookie);

            headers.Add(header);

            context.IncomingMessageHeaders.Returns(headers);

            var decryptedTicket = new FormsAuthenticationTicket("tester@agile.local", false, -100);
            provider.Decrypt(authCookie).Returns(decryptedTicket);

            unitOfWork.UserRepository.Items.Returns(new List<User>() { user }.AsQueryable());

            strategy.PostBuildUp(unityContext);

            provider.Received().Decrypt(authCookie);
            Thread.CurrentPrincipal.GetType().ToString().Should().NotContain("GeneratedServiceUserPrincipal");
        }

        [Test]
        public void CallWithMethodAttributeForExpiredToken()
        {
            var obj = new TestServiceWithMethodAttribute();
            var methodInfo = obj.GetType().GetMethod("DoStuff");

            context.GetMethodInfoForServiceCall().Returns(methodInfo);

            var unityContext = Substitute.For<IBuilderContext>();
            unityContext.Existing.Returns(obj);

            var authCookie = "test";
            var user = new User { EmailAddress = "tester@agile.local", Roles = new List<Role>() };

            context.GetHttpContextUser().Returns(c => { return null; });

            var headers = new MessageHeaders(MessageVersion.Soap11);

            var header = MessageHeader.CreateHeader("authCookie", "http://anytrack", authCookie);

            headers.Add(header);

            context.IncomingMessageHeaders.Returns(headers);

            var decryptedTicket = new FormsAuthenticationTicket("tester@agile.local", false, -100);
            provider.Decrypt(authCookie).Returns(decryptedTicket);

            unitOfWork.UserRepository.Items.Returns(new List<User>() { user }.AsQueryable());

            strategy.PostBuildUp(unityContext);

            provider.Received().Decrypt(authCookie);
            Thread.CurrentPrincipal.GetType().ToString().Should().NotContain("GeneratedServiceUserPrincipal");
        }

        #endregion 
    }

    #endregion 
}
