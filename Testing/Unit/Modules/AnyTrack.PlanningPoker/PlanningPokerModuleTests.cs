using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.Service;
using AnyTrack.Infrastructure.Service.Model;
using AnyTrack.PlanningPoker;
using AnyTrack.PlanningPoker.BackendPlanningPokerManagerService;
using AnyTrack.PlanningPoker.ServiceGateways;
using AnyTrack.PlanningPoker.Views;
using AnyTrack.Projects;
using AnyTrack.Projects.Views;
using Microsoft.Practices.Unity;
using NSubstitute;
using NUnit.Framework;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit.Modules.AnyTrack.Projects.PlanningPokerModuleTests
{
    #region Context

    public class Context
    {
        public static IUnityContainer container; 
        public static IRegionManager regionManager;
        public static IMenuService menuService;
        public static PlanningPokerModule module; 

        [SetUp]
        public void Setup()
        {
            container = Substitute.For<IUnityContainer>();
            regionManager = Substitute.For<IRegionManager>();
            menuService = Substitute.For<IMenuService>();

            module = new PlanningPokerModule(container, regionManager, menuService);
        }
    }

    #endregion 

    #region Tests 

    public class PlanningPokerModuleTests : Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoContainer()
        {
            module = new PlanningPokerModule(null, regionManager, menuService); 
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoRegionManager()
        {
            module = new PlanningPokerModule(container, null, menuService);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoMenuService()
        {
            module = new PlanningPokerModule(container, regionManager, null);
        }

        #endregion 

        #region Initialize Tests 

        [Test]
        public void CallInitalize()
        {
            module.Initialize();

            container.Received().RegisterType<IPlanningPokerManagerService, PlanningPokerManagerServiceClient>(Arg.Any<LifetimeManager>(), Arg.Any<InjectionMember[]>());

            container.Received().RegisterType<IPlanningPokerManagerServiceGateway, PlanningPokerManagerServiceGateway>(Arg.Any<LifetimeManager>());

            container.Received().RegisterType<object, StartPlanningPokerSession>("StartPlanningPokerSession");
            container.Received().RegisterType<object, SearchForPlanningPokerSession>("SearchForPlanningPokerSession");
            container.Received().RegisterType<object, PokerLobby>("PokerLobby");
            container.Received().RegisterType<object, PlanningPokerSession>("PlanningPokerSession");
        }

        #endregion 
    }

    #endregion 
}
