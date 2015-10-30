﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
using AnyTrack.Infrastructure.Providers;
using System.Windows.Threading;
using MahApps.Metro.Controls.Dialogs;
using System.ServiceModel;
using AnyTrack.Backend.Faults;
using AnyTrack.Infrastructure.BackendAccountService;

namespace Unit.Modules.AnyTrack.Accounting.Views.RegistrationViewModelTests
{
    #region Context

    public class Context
    {
        public static IRegionManager regionManager;
        public static IAccountServiceGateway gateway;
        public static RegistrationViewModel registrationViewModel;

        [SetUp]
        public void ContextSetup()
        {
            regionManager = Substitute.For<IRegionManager>();
            gateway = Substitute.For<IAccountServiceGateway>();
            registrationViewModel = new RegistrationViewModel(regionManager, gateway);
        }
    }

    #endregion

    #region Tests

    public class RegistrationViewModelTests : Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException (typeof(ArgumentNullException))]
        public void TestNullConstructor()
        {
            registrationViewModel = new RegistrationViewModel(null, gateway);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullConstructorForGateway()
        {
            registrationViewModel = new RegistrationViewModel(regionManager, null);
        }

        #endregion

        #region CanRegister() Tests 

        [Test]
        public void TestCanRegister()
        {
            var result = registrationViewModel.Call<bool>("CanRegister");
            result.Should().BeTrue();
        }

        [Test]
        public void TestCanRegisterWithValidationError()
        {
            registrationViewModel.Email = "wrong";
            var result = registrationViewModel.Call<bool>("CanRegister");
            result.Should().BeFalse();
        }

        #endregion

        #region CanCancel() Tests

        [Test]
        public void TestCanCancel()
        {
            var result = registrationViewModel.Call<bool>("CanCancel");
            result.Should().BeTrue();
        }

        #endregion

        #region CanAddSkill() Tests

        [Test]
        public void TestCanAddSkill()
        {
            var result = registrationViewModel.Call<bool>("CanAddSkill");
            result.Should().BeTrue();
        }

        #endregion

        #region AddSkill() Tests 

        [Test]
        public void CallAddSkill()
        {
            var skill = "Test";

            registrationViewModel.CurrentSkill = skill;

            registrationViewModel.Call("AddSkill");

            registrationViewModel.Skills.Count.Should().Be(1);
            registrationViewModel.Skills.Single().Should().Be(skill);
            registrationViewModel.CurrentSkill.Should().BeEmpty();
        }

        #endregion 

        #region CancelRegisterUser() Tests

        [Test]
        public void TestCancelRegisterUser()
        {
            registrationViewModel.Call("CancelRegisterUser");

            regionManager.Received().RequestNavigate(RegionNames.AppContainer, "Login");
        }

        #endregion 

        #region RegisterUser() Tests

        [Test]
        public void TestRegisterUser()
        {
            NewUser registration = null;

            gateway.RegisterAccount(Arg.Do<NewUser>(r => registration = r));
            registrationViewModel.MainWindow = Substitute.For<WindowProvider>();
            registrationViewModel.MainWindow.ShowMessageAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(MessageDialogResult.Affirmative);
            registrationViewModel.MainWindow.InvokeAction(Arg.Do<Action>(a => a()));

            registrationViewModel.Email = "test@agile.local";
            registrationViewModel.Password = "Password";
            registrationViewModel.FirstName = "Test";
            registrationViewModel.LastName = "Test";
            registrationViewModel.ProductOwner = false;
            registrationViewModel.ScrumMaster = false;
            registrationViewModel.Developer = false;

            registrationViewModel.Call("RegisterUser");

            registration.Should().NotBeNull();
            registration.EmailAddress.Should().Be(registrationViewModel.Email);
            registration.Password.Should().Be(registrationViewModel.Password);
            registration.LastName.Should().Be(registrationViewModel.LastName);
            registration.FirstName.Should().Be(registrationViewModel.FirstName);
            registration.ProductOwner.Should().Be(registrationViewModel.ProductOwner);
            registration.ScrumMaster.Should().Be(registrationViewModel.ScrumMaster);
            registration.Developer.Should().Be(registrationViewModel.Developer);
            gateway.Received().RegisterAccount(registration);

            regionManager.Received().RequestNavigate(RegionNames.AppContainer, "Login");
        }

        #endregion

    }

    #endregion
}
