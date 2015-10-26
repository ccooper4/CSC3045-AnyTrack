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

namespace Unit.Modules.AnyTrack.Accounting.Views.RegistrationViewModelTests
{
    #region Context

    public class Context
    {
        public static IRegionManager regionManager;
        public static RegistrationViewModel registrationViewModel;

        [SetUp]
        public void ContextSetup()
        {
            regionManager = Substitute.For<IRegionManager>();
            registrationViewModel = new RegistrationViewModel(regionManager);
        }
    }

    #endregion

    #region Tests

    public class RegistrationViewModelTests : Context
    {
        [Test]
        [ExpectedException (typeof(ArgumentNullException))]
        public void TestNullConstructor()
        {
            registrationViewModel = new RegistrationViewModel(null);
        }

        [Test]
        public void TestCanRegister()
        {
            var result = registrationViewModel.Call<bool>("CanRegister");
            result.Should().BeTrue();
        }

        [Test]
        public void TestRegisterUser()
        {
            //registrationViewModel.Call("RegisterUser");
            //regionManager.Received().RequestNavigate(RegionNames.MainRegion, "Login");
        }

    }

    #endregion
}
