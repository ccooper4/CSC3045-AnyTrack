using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.Providers;
using AnyTrack.Infrastructure.Security;
using AnyTrack.Infrastructure.Service;
using AnyTrack.Infrastructure.ServiceGateways;
using AnyTrack.Projects.Views;
using AnyTrack.Sprints.Views;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using Prism.Regions;

namespace Unit.Modules.AnyTrack.Sprints.Views.SprintManagerViewModelTests
{
    public class Context
    {
        public static IRegionManager regionManager;
        public static IProjectService gateway;
        public static SprintManagerViewModel vm;
        public static List<ServiceProjectRoleSummary> listOfProjects;
            
        [SetUp]
        public static void SetUp()
        {
            regionManager = Substitute.For<IRegionManager>();
            gateway = Substitute.For<IProjectService>();

            listOfProjects = new List<ServiceProjectRoleSummary>()
            {
                new ServiceProjectRoleSummary()
                {
                    ProjectId = new Guid(),
                    Sprints = new List<ServiceSprintSummary>()
                    {
                        new ServiceSprintSummary()
                    }
                }
            };

            UserDetailsStore.LoggedInUserPrincipal = new ServiceUserPrincipal(new ServiceLoginResult { EmailAddress = "test@agile.local" }, "");
            gateway.GetUserProjectRoleSummaries("test@agile.local").Returns(listOfProjects); 

            vm = new SprintManagerViewModel(gateway);
            vm.RegionManager = regionManager;

                    
        }
    }

    public class SprintManagerViewModelTests : Context
    {
        #region Constructor Tests

        [Test]
        public void ConstructSprintManager()
        {
            UserDetailsStore.LoggedInUserPrincipal = new ServiceUserPrincipal(new ServiceLoginResult { EmailAddress = "test@agile.local" }, "");
            gateway.GetUserProjectRoleSummaries("test@agile.local").Returns(listOfProjects);

            vm = new SprintManagerViewModel(gateway);
            vm.RegionManager = regionManager;

            vm = new SprintManagerViewModel(gateway);
            vm.Projects.Should().Contain(listOfProjects);
            vm.AddSprintCommand.Should().NotBeNull();
            vm.UpdateProjectDisplayedCommand.Should().NotBeNull();
            vm.OpenSprintOptionsCommand.Should().NotBeNull();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructSprintManagerNullGateway()
        {
            gateway = null;
            vm = new SprintManagerViewModel(gateway);
        }

        #endregion

        #region IsNavigationTarget(NavigationContext navigationContext) Tests

        [Test]
        public void CallIsNavTarget()
        {
            var context = new NavigationContext(Substitute.For<IRegionNavigationService>(), new Uri("SprintManager", UriKind.Relative));
            vm.IsNavigationTarget(context).Should().BeFalse();
        }

        #endregion 

        #region OnNavigatedTo(NavigationContext navigationContext) Tests

        [Test]
        public void CallOnNav()
        {
            var projectId = new Guid();

            var context = new NavigationContext(Substitute.For<IRegionNavigationService>(), new Uri("SprintManager", UriKind.Relative));
            context.Parameters.Add("ProjectId", projectId);

            vm.OnNavigatedTo(context);

            vm.SelectedProject.Should().Be(listOfProjects[0]);
            vm.CurrentlyShowingProject.Should().Be(listOfProjects[0]);
        }

        #endregion 

        #region GotToCreateSprint() Tests

        [Test]
        public void GoToCreateSprint()
        {
            NavigationParameters sentParams = null;
            vm.ProjectId = Guid.NewGuid();

            var regionManager = Substitute.For<IRegionManager>();
            regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.RegionManager = regionManager;

            vm.Call("GoToCreateSprint");
            sentParams.Should().NotBeNull();
            sentParams.ContainsKey("ProjectId").Should().BeTrue();
            sentParams["ProjectId"].Should().Be(vm.ProjectId);
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "CreateSprint", sentParams);
        }

        #endregion

        #region CanUpdateProject() Tests

        [Test]
        public void CanUpdateTrue()
        {
            vm.SelectedProject = new ServiceProjectRoleSummary();

            var result = vm.Call<bool>("CanUpdateProject");

            result.Should().Be(true);
        }

        [Test]
        public void CanUpdateFalse()
        {
            vm.SelectedProject = null;

            var result = vm.Call<bool>("CanUpdateProject");

            result.Should().Be(false);
        }

        #endregion

        #region UpdateProjectDisplayed() Tests

        [Test]
        public void UpdateProjectDisplayed()
        {
            vm.SelectedProject = new ServiceProjectRoleSummary()
            {
                ProjectId = Guid.NewGuid(),
                Sprints = new List<ServiceSprintSummary>()
                {
                    new ServiceSprintSummary()
                },
                ScrumMaster = true
            };

            vm.Call("UpdateProjectDisplayed");

            vm.CurrentlyShowingProject.Should().Be(vm.SelectedProject);
            vm.ProjectId.Should().Be(vm.SelectedProject.ProjectId);
            vm.ShowAddButton.Should().Be(vm.SelectedProject.ScrumMaster);
            vm.Sprints.Should().Contain(vm.SelectedProject.Sprints);
        }

        #endregion

        #region ShowSprintOptions(ServiceSprintSummary sprintSummary) Tests

        [Test]
        public void ShowSprintOptions()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;

            NavigationParameters sentParams = null;
            vm.CurrentlyShowingProject = new ServiceProjectRoleSummary();
            var sprintSummary = new ServiceSprintSummary();

            var flyoutService = Substitute.For<IFlyoutService>();
            flyoutService.ShowMetroFlyout(Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.FlyoutService = flyoutService;

            vm.Call("ShowSprintOptions", sprintSummary);
            sentParams.Should().NotBeNull();
            sentParams.ContainsKey("projectRoleInfo").Should().BeTrue();
            sentParams.ContainsKey("sprintSummary").Should().BeTrue();
            sentParams["projectRoleInfo"].Should().Be(vm.CurrentlyShowingProject);
            sentParams["sprintSummary"].Should().Be(sprintSummary);
            flyoutService.Received().ShowMetroFlyout("SprintOptions", sentParams);
        }

        #endregion
    }
}
