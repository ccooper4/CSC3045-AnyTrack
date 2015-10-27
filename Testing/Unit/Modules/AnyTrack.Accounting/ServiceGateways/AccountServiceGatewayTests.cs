using AnyTrack.Accounting.BackendAccountService;
using AnyTrack.Accounting.ServiceGateways;
using AnyTrack.Accounting.ServiceGateways.Models;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit.Modules.AnyTrack.Accounting.ServiceGateways.AccountServiceGatewayTests
{
    #region Context

    public class Context
    {
        public static IAccountService accountService;

        public static AccountServiceGateway gateway;

        [SetUp]
        public void ContextSetup()
        {
            accountService = Substitute.For<IAccountService>();
            gateway = new AccountServiceGateway(accountService);
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
            gateway = new AccountServiceGateway(null);
        }

        #endregion

        #region RegisterAccount(NewUserRegistration registration) Tests 

        [Test]
        public void RegisterAnAccount()
        {
            NewUser sentModel = null;
            accountService.CreateAccount(Arg.Do<NewUser>(n => sentModel = n));

            var registration = new NewUserRegistration
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


        #endregion 
    }

    #endregion 
}
