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
using AnyTrack.Infrastructure.Security;
using AnyTrack.Infrastructure.BackendAccountService;

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

            var listOfProjects = new List<ServiceProjectRoleSummary>()
            {
                new ServiceProjectRoleSummary()
            };

            UserDetailsStore.LoggedInUserPrincipal = new ServiceUserPrincipal(new LoginResult { EmailAddress = "test@agile.local" }, "");
            gateway.GetLoggedInUserProjectRoleSummaries("test@agile.local").Returns(listOfProjects);

            vm = new MyProjectsViewModel(gateway);
            vm.RegionManager = regionManager;
        }
    }

    #endregion

    #region Tests

    public class MyProjectsViewModelTests : Context
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructNoGateway()
        {
            vm = new MyProjectsViewModel(null);
        }

        [Test]
        public void ConstructVm()
        {
            vm = new MyProjectsViewModel(gateway);
            vm.CreateProjectCommand.Should().NotBeNull();
            vm.ManageBacklogCommand.Should().NotBeNull();
        }

        [Test]
        public void AddProject()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.Call("AddProjectView");
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "Project");      
        }
    }

    #endregion 
}
