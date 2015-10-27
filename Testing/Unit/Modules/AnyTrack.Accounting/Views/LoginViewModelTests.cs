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

        #endregion

        #region CanLogin() Tests

        [Test]
        public void TestCanLogin()
        {
            var result = loginViewModel.Call<bool>("CanLogin");
            result.Should().BeTrue();
        }

        #endregion
        
    }

    #endregion
}
