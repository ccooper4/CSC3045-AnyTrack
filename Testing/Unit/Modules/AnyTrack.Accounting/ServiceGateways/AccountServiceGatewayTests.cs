using AnyTrack.Accounting.ServiceGateways;
using AnyTrack.Accounting.ServiceGateways.Models;
using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Infrastructure.Security;
using FluentAssertions;
using Microsoft.Practices.Unity;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Unit.Modules.AnyTrack.Accounting.ServiceGateways.AccountServiceGatewayTests
{
    #region Context

    public class Context
    {
        public static IAccountService accountService;

        public static IUnityContainer container;

        public static AccountServiceGateway gateway;

        [SetUp]
        public void ContextSetup()
        {
            accountService = Substitute.For<IAccountService>();
            container = Substitute.For<IUnityContainer>();
            gateway = new AccountServiceGateway(container, accountService);
        }
    }

    #endregion

    #region AccountServiceGatewayTests

    public class AccountServiceGatewayTests: Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoClient()
        {
            gateway = new AccountServiceGateway(container, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoContainer()
        {
            gateway = new AccountServiceGateway(null, accountService);
        }

        #endregion

        #region RegisterAccount(NewUser registration) Tests 

        [Test]
        public void RegisterAnAccount()
        {
            NewUser sentModel = null;
            accountService.CreateAccount(Arg.Do<NewUser>(n => sentModel = n));

            var registration = new NewUser
            {
                EmailAddress = "test@agile.local",
                FirstName = "Test",
                LastName = "Test",
                Password = "Test",
                ProductOwner = false,
                ScrumMaster = false,
                Developer = false
            };

            gateway.RegisterAccount(registration);
            accountService.Received().CreateAccount(sentModel);
            sentModel.Should().NotBeNull();
            sentModel.EmailAddress.Should().Be("test@agile.local");
            sentModel.FirstName.Should().Be("Test");
            sentModel.LastName.Should().Be("Test");
            sentModel.Password.Should().Be("Test");
            sentModel.ProductOwner.Should().Be(registration.ProductOwner);
            sentModel.ScrumMaster.Should().Be(registration.ScrumMaster);
            sentModel.Developer.Should().Be(registration.Developer);

        }

        [Test]
        [ExpectedException(typeof( FaultException<UserAlreadyExistsFault>))]
        public void RegisterAnAccountWithADuplicateEmailAddress()
        {
            NewUser sentModel = null;
            var exception = new FaultException<UserAlreadyExistsFault>(new UserAlreadyExistsFault());
            accountService.CreateAccount(Arg.Do<NewUser>(n => sentModel = n));

            accountService.When(a => a.CreateAccount(Arg.Any<NewUser>())).Do(a => { throw exception;} );

            var registration = new NewUser
            {
                EmailAddress = "test@agile.local",
                FirstName = "Test",
                LastName = "Test",
                Password = "Test",
                ProductOwner = false,
                ScrumMaster = false,
                Developer = false
            };

            gateway.RegisterAccount(registration);

        }

        #endregion 

        #region LoginAccount(UserCredential login) Tests 

        [Test]
        public void CallLoginForAValidLogin()
        {
            var creds = new UserCredential { EmailAddress = "test@agile.local", Password = "Letmein" }; 
            var result = new LoginResult { Success = true};

            accountService.LogIn(creds).Returns(result);

            var gatewayResult = gateway.LoginAccount(creds);

            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeSameAs(result);
            accountService.Received().LogIn(creds);
            Thread.CurrentPrincipal.Should().NotBeNull();
            Thread.CurrentPrincipal.Should().BeOfType<ServiceUserPrincipal>();
            container.Received().RegisterInstance<IPrincipal>(Arg.Any<ServiceUserPrincipal>(), Arg.Any < ContainerControlledLifetimeManager>());
        }

        public void CallLoginForInvalidLogin()
        {
            var creds = new UserCredential { EmailAddress = "test@agile.local", Password = "Letmein" };
            var result = new LoginResult { Success = false };

            accountService.LogIn(creds).Returns(result);

            var gatewayResult = gateway.LoginAccount(creds);

            gatewayResult.Should().NotBeNull();
            gatewayResult.Should().BeSameAs(result);
            accountService.Received().LogIn(creds);
            Thread.CurrentPrincipal.Should().BeNull();
        }

        #endregion 
    }

    #endregion 
}
