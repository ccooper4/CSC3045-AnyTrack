using AnyTrack.Accounting;
using AnyTrack.Accounting.ServiceGateways;
using AnyTrack.Accounting.Views;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.Service;
using AnyTrack.Infrastructure.Service.Model;
using Microsoft.Practices.Unity;
using NSubstitute;
using NUnit.Framework;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure.BackendAccountService;

namespace Unit.Modules.AnyTrack.Accounting.AccountingModuleTests
{
    #region Context 

    public class Context
    {
        public static IUnityContainer container;
        public static IRegionManager regionManager;
        public static IMenuService menuService;
        public static AccountingModule module; 

        [SetUp]
        public void SetUp()
        {
            container = Substitute.For<IUnityContainer>();
            regionManager = Substitute.For<IRegionManager>();
            menuService = Substitute.For<IMenuService>();

            module = new AccountingModule(container, regionManager, menuService);
        }
    }

    #endregion

    #region Tests

    public class AccountingModuleTests: Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoContainer()
        {
            module = new AccountingModule(null, regionManager, menuService);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithRegionManager()
        {
            module = new AccountingModule(container, null, menuService);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithMenuService()
        {
            module = new AccountingModule(container, regionManager, null);
        }

        #endregion 

        #region Initialize() Tests 

        [Test]
        public void InitializeModuleTest()
        {
            module.Initialize();
            container.Received().RegisterType<IAccountService, AccountServiceClient>(Arg.Any<InjectionMember[]>());
            container.Received().RegisterType<IAccountServiceGateway, AccountServiceGateway>();
            container.Received().RegisterType<object, Registration>("Registration");
            container.Received().RegisterType<object, Login>("Login");

            regionManager.Received().RequestNavigate(RegionNames.AppContainer, "Login");
        }

        #endregion 
    }

    #endregion
}
