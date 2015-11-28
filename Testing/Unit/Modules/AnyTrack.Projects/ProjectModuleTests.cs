using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.Service;
using AnyTrack.Infrastructure.Service.Model;
using AnyTrack.Projects;
using AnyTrack.Infrastructure.BackendProjectService;
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

namespace Unit.Modules.AnyTrack.Projects.ProjectModuleTests
{
    #region Context

    public class Context
    {
        public static IUnityContainer container; 
        public static IRegionManager regionManager;
        public static IMenuService menuService;
        public static ProjectModule module; 

        [SetUp]
        public void Setup()
        {
            container = Substitute.For<IUnityContainer>();
            regionManager = Substitute.For<IRegionManager>();
            menuService = Substitute.For<IMenuService>();

            module = new ProjectModule(container, regionManager, menuService);
        }
    }

    #endregion 

    #region Tests 

    public class ProjectModuleTests: Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoContainer()
        {
            module = new ProjectModule(null, regionManager, menuService); 
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoRegionManager()
        {
            module = new ProjectModule(container, null, menuService);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoMenuService()
        {
            module = new ProjectModule(container, regionManager, null);
        }

        #endregion 

        #region Initialize Tests 

        [Test]
        public void CallInitalize()
        {
            module.Initialize();
            container.Received().RegisterType<object, ProductBacklog>("ProductBacklog");
            container.Received().RegisterType<object, Story>("Story");
            container.Received().RegisterType<object, CreateProject>("Project");
            container.Received().RegisterType<object, MyProjects>("MyProjects");
            container.Received().RegisterType<object, ProjectOptions>("ProjectOptions");

            menuService.Received().AddMenuItem(Arg.Any<MenuItem>());
            regionManager.RequestNavigate(RegionNames.MainRegion, "ProductBacklog");
        }

        #endregion 
    }

    #endregion 
}
