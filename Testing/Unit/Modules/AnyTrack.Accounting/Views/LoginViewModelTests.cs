using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Accounting.Views;
using AnyTrack.Backend.Data;
using AnyTrack.Infrastructure;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Prism.Regions;
using Unit.Backend.AnyTrack.Backend.Data.EntityRepositoryTests;
using AnyTrack.Accounting.ServiceGateways;
using AnyTrack.Accounting.ServiceGateways.Models;
using AnyTrack.Infrastructure.BackendAccountService;

namespace Unit.Modules.AnyTrack.Accounting.Views
{
    #region Context

    public class Context
    {
        public static IRegionManager regionManager;
        public static IAccountServiceGateway gateway;
        public static LoginViewModel loginViewModel;

        [SetUp]
        public void ContextSetup()
        {
            regionManager = Substitute.For<IRegionManager>();
            gateway = Substitute.For<IAccountServiceGateway>();
            loginViewModel = new LoginViewModel(regionManager, gateway);
        }
    }

    #endregion

    #region Tests

    public class LoginViewModelTests : Context
    {
        #region Constructor Tests

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullConstructor()
        {
            loginViewModel = new LoginViewModel(null, gateway);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullConstructorForGateway()
        {
            loginViewModel = new LoginViewModel(regionManager, null);
        }

        #endregion

        #region CanLogin() Tests

        [Test]
        public void TestCanLogin()
        {
            var result = loginViewModel.Call<bool>("CanLogin");
            result.Should().BeTrue();
        }

        #endregion

        #region CanSignUp() Tests

        [Test]
        public void TestCanSignUp()
        {
            var result = loginViewModel.Call<bool>("CanSignUp");
            result.Should().BeTrue();
        }

        #endregion

        #region LoginUser() Tests 

        [Test]
        public void CallLoginUser()
        {
            UserCredential cred = null;
            gateway.LoginAccount(Arg.Do<UserCredential>(c => cred = c));

            loginViewModel.Email = "test@agile.local";
            loginViewModel.Password = "test";

            loginViewModel.Call("LoginUser");

            cred.Should().NotBeNull();
            cred.EmailAddress.Should().Be(loginViewModel.Email);
            cred.Password.Should().Be(loginViewModel.Password);
            gateway.Received().LoginAccount(cred);

            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "Login");

        }

        #endregion 

        #region SignUp() Tests

        [Test]
        public void CallSignUp()
        {
            loginViewModel.Call("SignUp");

            regionManager.Received().RequestNavigate(RegionNames.AppContainer, "Registration");

        }

        #endregion 

    }

    #endregion
}
