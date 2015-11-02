using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Providers;
using AnyTrack.Backend.Service;
using NUnit.Framework;
using NSubstitute;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web.Security;
using AnyTrack.Backend.Data.Model;
using System.Threading;
using FluentAssertions;
using AnyTrack.Backend.Security;

namespace Unit.Backend.AnyTrack.Backend.Service.PrincipalBuilderServiceTests
{
    #region Supporting Types 

    public class TestService : PrincipalBuilderService
    {
        public TestService(IUnitOfWork unitOfWork, FormsAuthenticationProvider formsAuth, OperationContextProvider context) : base(unitOfWork, formsAuth, context)
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

        public static TestService service; 

        [SetUp]
        public void SetUp()
        {
            unitOfWork = Substitute.For<IUnitOfWork>();
            provider = Substitute.For<FormsAuthenticationProvider>();
            context = Substitute.For<OperationContextProvider>();
        }
    }

    #endregion

    #region Tests
    
    public class PrincipalBuilderServiceTests : Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoUnitOfWork()
        {
            service = new TestService(null, provider, context);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoProvider()
        {
            service = new TestService(unitOfWork, null, context);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoContext()
        {
            service = new TestService(unitOfWork, provider, null);
        }

        [Test]
        public void ConstructAndSetPrincipalForNonExpiredToken()
        {
            var channel = Substitute.For<IContextChannel>();
            var requestMessage = new HttpRequestMessageProperty();
            var authCookie = "test";
            var user = new User { EmailAddress = "tester@agile.local", Roles = new List<Role>() };
            requestMessage.Headers.Set("Set-Cookie", "AuthCookie=" + authCookie + ";other=other");

            var properties = new MessageProperties();
            properties.Add(HttpRequestMessageProperty.Name, requestMessage);
            context.IncomingMessageProperties.Returns(properties);

            var decryptedTicket = new FormsAuthenticationTicket("tester@agile.local", false, 100);
            provider.Decrypt(authCookie).Returns(decryptedTicket);

            unitOfWork.UserRepository.Items.Returns(new List<User>() { user }.AsQueryable());

            service = new TestService(unitOfWork, provider, context);

            provider.Received().Decrypt(authCookie);
            Thread.CurrentPrincipal.Should().NotBeNull();
            Thread.CurrentPrincipal.Should().BeOfType<GeneratedServiceUserPrincipal>();
            Thread.CurrentPrincipal.Identity.Should().NotBeNull();
            Thread.CurrentPrincipal.Identity.Name.Should().Be(user.EmailAddress);
            Thread.CurrentPrincipal.Identity.IsAuthenticated.Should().BeTrue();
            Thread.CurrentPrincipal.Identity.AuthenticationType.Should().Be("FormsAuthentication");
        }

        [Test]
        public void ConstructAndSetPrincipalForExpiredToken()
        {
            var channel = Substitute.For<IContextChannel>();
            var requestMessage = new HttpRequestMessageProperty();
            var authCookie = "test";
            var user = new User { EmailAddress = "tester@agile.local", Roles = new List<Role>() };
            requestMessage.Headers.Set("Set-Cookie", "AuthCookie=" + authCookie + ";other=other");

            var properties = new MessageProperties();
            properties.Add(HttpRequestMessageProperty.Name, requestMessage);
            context.IncomingMessageProperties.Returns(properties);

            var decryptedTicket = new FormsAuthenticationTicket("tester@agile.local", false, 100);
            provider.Decrypt(authCookie).Returns(decryptedTicket);

            unitOfWork.UserRepository.Items.Returns(new List<User>() { user }.AsQueryable());

            service = new TestService(unitOfWork, provider, context);

            provider.Received().Decrypt(authCookie);
            Thread.CurrentPrincipal.Should().NotBeNull();
            Thread.CurrentPrincipal.Should().BeOfType<GeneratedServiceUserPrincipal>();
            Thread.CurrentPrincipal.Identity.Should().NotBeNull();
            Thread.CurrentPrincipal.Identity.Name.Should().Be(user.EmailAddress);
            Thread.CurrentPrincipal.Identity.IsAuthenticated.Should().BeTrue();
            Thread.CurrentPrincipal.Identity.AuthenticationType.Should().Be("FormsAuthentication");
        }

        #endregion 
    }

    #endregion 
    
}
