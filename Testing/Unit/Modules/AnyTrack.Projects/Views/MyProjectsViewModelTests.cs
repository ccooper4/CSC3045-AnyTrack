using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.Providers;
using AnyTrack.Projects.BackendProjectService;
using AnyTrack.Projects.ServiceGateways;
using AnyTrack.Projects.Views;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Prism.Regions;

namespace Unit.Modules.AnyTrack.Projects.Views
{
    #region Context
    public class Context
    {
        public static IRegionManager regionManager;
        public static IProjectServiceGateway gateway;
        public static MyProjectsViewModel vm;

        [SetUp]
        public static void SetUp()
        {
            regionManager = Substitute.For<IRegionManager>();
            gateway = Substitute.For<IProjectServiceGateway>();

            vm = new MyProjectsViewModel(regionManager, gateway);
        }
    }

    #endregion

    public class MyProjectsViewModelTests : Context
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructNoRegionManager()
        {
            vm = new MyProjectsViewModel(null, gateway);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructNoGateway()
        {
            vm = new MyProjectsViewModel(regionManager, null);
        }

        [Test]
        public void ConstructVm()
        {
            vm = new MyProjectsViewModel(regionManager, gateway);
            vm.CreateProjectCommand.Should().NotBeNull();
            vm.BurndownChartsCommand.Should().NotBeNull();
            vm.ManageBacklogCommand.Should().NotBeNull();
            vm.ManageSprintsCommand.Should().NotBeNull();
            vm.OfflineModeCommand.Should().NotBeNull();
            vm.ManageProjectCommand.Should().NotBeNull();
            vm.ProjectSettingsCommand.Should().NotBeNull();
        }

        [Test]
        public void AddProject()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.Call("AddProjectView");
            regionManager.Received().RequestNavigate(RegionNames.AppContainer, "Project");      
        }
    }
}
