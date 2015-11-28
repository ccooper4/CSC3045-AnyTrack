using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.Service;
using AnyTrack.Infrastructure.Service.Model;
using AnyTrack.Projects;
using AnyTrack.Projects.Views;
using AnyTrack.Sprints;
using AnyTrack.Sprints.BackendSprintService;
using AnyTrack.Sprints.ServiceGateways;
using Microsoft.Practices.Unity;
using NSubstitute;
using NUnit.Framework;
using Prism.Regions;

namespace Unit.Modules.AnyTrack.Sprints
{
    #region Context

    public class Context
    {
        public static IUnityContainer container;
        public static IRegionManager regionManager;
        public static IMenuService menuService;
        public static SprintModule module;

        [SetUp]
        public void Setup()
        {
            container = Substitute.For<IUnityContainer>();
            regionManager = Substitute.For<IRegionManager>();
            menuService = Substitute.For<IMenuService>();

            module = new SprintModule(container, regionManager, menuService);
        }
    }

    #endregion 

    class SprintModuleTests : Context
    {
        #region Constructor Tests

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoContainer()
        {
            module = new SprintModule(null, regionManager, menuService);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoRegionManager()
        {
            module = new SprintModule(container, null, menuService);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoMenuService()
        {
            module = new SprintModule(container, regionManager, null);
        }

        #endregion 

        #region Initialize Tests

        [Test]
        public void CallInitalize()
        {
            module.Initialize();
            container.Received().RegisterType<ISprintService, SprintServiceClient>(Arg.Any<InjectionMember[]>());
            container.Received().RegisterType<ISprintServiceGateway, SprintServiceGateway>();

            menuService.Received().AddMenuItem(Arg.Any<MenuItem>());

        }

        #endregion 
    }
}
