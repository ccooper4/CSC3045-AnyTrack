﻿using AnyTrack.Client.Views;
using AnyTrack.Infrastructure.Service;
using NSubstitute;
using NUnit.Framework;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using AnyTrack.Infrastructure.Service.Model;
using System.Collections.ObjectModel;
using System.Threading;
using AnyTrack.Infrastructure.Security;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendAccountService;

namespace Unit.AnyTrack.Views.MainAppAreaViewModelTests
{
    #region Context

    public class Context
    {
        public static IMenuService menuService;
        public static IRegionManager regionManager;
        public static MainAppAreaViewModel vm; 

        [SetUp]
        public void SetUp()
        {
            menuService = Substitute.For<IMenuService>();
            regionManager = Substitute.For<IRegionManager>();

            vm = new MainAppAreaViewModel(menuService, regionManager);
            vm.RegionManager = regionManager;
        }
    }

    #endregion 

    #region Tests 

    public class MainAppAreaViewModelTests: Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoMenuService()
        {
            vm = new MainAppAreaViewModel(null, regionManager);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoRegionManager()
        {
            vm = new MainAppAreaViewModel(menuService, null);
        }

        [Test]
        public void ConstructViewModel()
        {
            vm.NavigateCommand.Should().NotBeNull();
        }

        #endregion 

        #region MenuItems Tests 

        [Test]
        public void GetMenuItems()
        {
            var menuItems = new ObservableCollection<MenuItem>()
            {
                new MenuItem()
            };

            menuService.MenuItems.Returns(menuItems);

            vm.MenuItems.Should().Equal(menuItems);
        }

        #endregion 

        #region FullName Tests 

        [Test]
        public void GetFullName()
        {
            var loginResult = new ServiceLoginResult
            {
                FirstName = "David",
                LastName = "Tester"
            };

            UserDetailsStore.LoggedInUserPrincipal = new ServiceUserPrincipal(loginResult, "");

            vm.FullName.Should().NotBeNull();
            vm.FullName.Should().Be(loginResult.FirstName + " " + loginResult.LastName);
        }

        #endregion 

        #region NavigateToItem(string view) Tests 

        [Test]
        public void NavigateToAView()
        {
            var view = "test";

            vm.Call("NavigateToItemFromMenu", view);

            regionManager.Received().RequestNavigate(RegionNames.MainRegion, view);
        }

        #endregion 

        #region Logout() Tests

        [Test]
        public void NavigateToLogin()
        {
            var view = "Login";

            vm.Call("NavigateToItemFromMenu", view);

            regionManager.Received().RequestNavigate(RegionNames.MainRegion, view);
        }

        [Test]
        public void CheckUserHasBeenLoggedOut()
        {
            vm.Call("Logout");

            vm.FullName.Should().BeNull();
            UserDetailsStore.AuthCookie.Should().BeNull();
            UserDetailsStore.LoggedInUserPrincipal.Should().BeNull();
        }

        #endregion
    }

    #endregion 
}
