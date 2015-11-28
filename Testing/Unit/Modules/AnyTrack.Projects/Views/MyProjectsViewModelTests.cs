using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.Providers;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.ServiceGateways;
using AnyTrack.Projects.Views;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Prism.Regions;
using AnyTrack.Infrastructure.Security;
using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Infrastructure.Service;

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

            UserDetailsStore.LoggedInUserPrincipal = new ServiceUserPrincipal(new ServiceLoginResult { EmailAddress = "test@agile.local" }, "");
            gateway.GetLoggedInUserProjectRoleSummaries("test@agile.local").Returns(listOfProjects);

            vm = new MyProjectsViewModel(gateway);
            vm.RegionManager = regionManager;
        }
    }

    #endregion

    #region Tests

    public class MyProjectsViewModelTests : Context
    {
        #region Constructor Tests 

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
            vm.ViewProjectOptions.Should().NotBeNull();
        }

        #endregion 

        #region AddProjectView Tests 

        [Test]
        public void AddProject()
        {
            vm.Call("AddProjectView");
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "Project");
        }

        #endregion 

        #region ShowProjectOptions(ServiceProjectRoleSummary summary) Tests 

        [Test]
        public void TestShowProjectOptions()
        {
            var projectOptions = new ServiceProjectRoleSummary();
            NavigationParameters sentParams = null;

            var flyoutService = Substitute.For<IFlyoutService>();
            flyoutService.ShowMetroFlyout(Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.FlyoutService = flyoutService;

            vm.Call("ShowProjectOptions", projectOptions);

            sentParams.Should().NotBeNull();
            sentParams.ContainsKey("projectInfo").Should().BeTrue();
            flyoutService.Received().ShowMetroFlyout("ProjectOptions", sentParams);
        }

        #endregion 
    }

    #endregion 
}
