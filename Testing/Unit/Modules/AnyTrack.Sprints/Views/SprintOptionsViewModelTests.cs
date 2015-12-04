using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Projects.Views;
using AnyTrack.Sprints.Views;
using FluentAssertions;
using MahApps.Metro.Controls;
using NSubstitute;
using NUnit.Framework;
using Prism.Regions;
using AnyTrack.Infrastructure.Security;
using AnyTrack.Infrastructure.BackendAccountService;

namespace Unit.Modules.AnyTrack.Sprints.Views.SprintOptionsViewModelTests
{
    #region Context

    public class Context
    {
        public static SprintOptionsViewModel vm;

        [SetUp]
        public void Setup()
        {
            vm = new SprintOptionsViewModel();
        }
    }

    #endregion

    public class SprintOptionsViewModelTests : Context
    {
        #region Constructor Tests

        [Test]
        public void ConstructVm()
        {
            vm = new SprintOptionsViewModel();
            vm.Header.Should().Be("Sprint Options");
            vm.IsModal.Should().BeTrue();
            vm.Position.Should().Be(Position.Right);
            vm.Theme.Should().Be(FlyoutTheme.Accent);

            vm.OpenProjectManager.Should().NotBeNull();
            vm.OpenPlanningPoker.Should().NotBeNull();
            vm.OpenBurndown.Should().NotBeNull();
            vm.OpenEditSprint.Should().NotBeNull();
            vm.OpenManageSprintBacklog.Should().NotBeNull();
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
        public void CallOnNavToWithSummaryAsScrumMaster()
        {

            var projectInfo = new ServiceProjectRoleSummary();
            projectInfo.ProjectId = Guid.NewGuid();
            projectInfo.Name = "Test";
            projectInfo.Description = "Test";
            projectInfo.ScrumMaster = true;

            var sprintInfo = new ServiceSprintSummary();
            sprintInfo.SprintId = Guid.NewGuid();
            sprintInfo.Description = "Description";
            sprintInfo.Name = "Sprint";

             var loginResult = new ServiceLoginResult
             {
                 AssignedRoles = new List<ServiceRoleInfo>()
                 {
                     new ServiceRoleInfo
                     {
                         ProjectId = projectInfo.ProjectId,
                         SprintId = sprintInfo.SprintId,
                         Role = "Scrum Master"
                     }
                 }.ToArray()
             };
             var principal = new ServiceUserPrincipal(loginResult, "");

             UserDetailsStore.LoggedInUserPrincipal = principal;

            var context = new NavigationContext(Substitute.For<IRegionNavigationService>(), new Uri("SprintManager", UriKind.Relative));
            context.Parameters.Add("projectRoleInfo", projectInfo);
            context.Parameters.Add("sprintSummary", sprintInfo);

            vm.IsOpen = true;

            vm.OnNavigatedTo(context);

            vm.IsOpen.Should().BeTrue();

            vm.ProjectId.Should().Be(projectInfo.ProjectId);
            vm.ProjectName.Should().Be(projectInfo.Name);
            vm.IsScrumMaster.Should().Be(projectInfo.ScrumMaster);
            vm.SprintId.Should().Be(sprintInfo.SprintId);
            vm.SprintName.Should().Be(sprintInfo.Name);
            vm.SprintDescription.Should().Be(sprintInfo.Description);
        }

         [Test]
         public void CallOnNavToWithSummaryAsDev()
         {

             var projectInfo = new ServiceProjectRoleSummary();
             projectInfo.ProjectId = Guid.NewGuid();
             projectInfo.Name = "Test";
             projectInfo.Description = "Test";
             projectInfo.ScrumMaster = true;

             var sprintInfo = new ServiceSprintSummary();
             sprintInfo.SprintId = Guid.NewGuid();
             sprintInfo.Description = "Description";
             sprintInfo.Name = "Sprint";

             var loginResult = new ServiceLoginResult
             {
                 AssignedRoles = new List<ServiceRoleInfo>()
                 {
                     new ServiceRoleInfo
                     {
                         ProjectId = projectInfo.ProjectId,
                         SprintId = sprintInfo.SprintId,
                         Role = "Developer"
                     }
                 }.ToArray()
             };
             var principal = new ServiceUserPrincipal(loginResult, "");

             UserDetailsStore.LoggedInUserPrincipal = principal;

             var context = new NavigationContext(Substitute.For<IRegionNavigationService>(), new Uri("SprintManager", UriKind.Relative));
             context.Parameters.Add("projectRoleInfo", projectInfo);
             context.Parameters.Add("sprintSummary", sprintInfo);

             vm.IsOpen = true;

             vm.OnNavigatedTo(context);

             vm.IsOpen.Should().BeTrue();

             vm.ProjectId.Should().Be(projectInfo.ProjectId);
             vm.ProjectName.Should().Be(projectInfo.Name);
             vm.IsScrumMaster.Should().BeFalse();
             vm.IsDeveloper.Should().BeTrue();
             vm.SprintId.Should().Be(sprintInfo.SprintId);
             vm.SprintName.Should().Be(sprintInfo.Name);
             vm.SprintDescription.Should().Be(sprintInfo.Description);
         }

        #endregion 

        #region DisplayPlanningPoker() Tests

        [Test]
        public void DisplayPlanningPokerInSMMode()
        {
            NavigationParameters sentParams = null;
            vm.ProjectId = Guid.NewGuid();
            vm.SprintId = Guid.NewGuid();

            vm.IsOpen = true;

            vm.IsScrumMaster = true; 

            var regionManager = Substitute.For<IRegionManager>();
            regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.RegionManager = regionManager;

            vm.Call("DisplayPlanningPoker", "ScrumMaster");
            sentParams.Should().NotBeNull();
            sentParams.ContainsKey("projectId").Should().BeTrue();
            sentParams.ContainsKey("sprintId").Should().BeTrue();
            sentParams["projectId"].Should().Be(vm.ProjectId);
            sentParams["sprintId"].Should().Be(vm.SprintId);
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "StartPlanningPokerSession", sentParams);
            vm.IsOpen.Should().BeFalse();
        }

        [Test]
        public void DisplayPlanningPokerInDevMode()
        {
            NavigationParameters sentParams = null;
            vm.ProjectId = Guid.NewGuid();
            vm.SprintId = Guid.NewGuid();

            vm.IsOpen = true;

            var regionManager = Substitute.For<IRegionManager>();
            regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.RegionManager = regionManager;

            vm.IsDeveloper = true; 

            vm.Call("DisplayPlanningPoker", "Developer");
            sentParams.Should().NotBeNull();
            sentParams.ContainsKey("projectId").Should().BeTrue();
            sentParams.ContainsKey("sprintId").Should().BeTrue();
            sentParams["projectId"].Should().Be(vm.ProjectId);
            sentParams["sprintId"].Should().Be(vm.SprintId);
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "SearchForPlanningPokerSession", sentParams);
            vm.IsOpen.Should().BeFalse();
        }

        #endregion

        #region DisplayBurnDownCharts()

        [Test]
        public void DisplayBurnDownCharts()
        {
            NavigationParameters sentParams = null;
            vm.SprintId = Guid.NewGuid();

            vm.IsOpen = true;

            var regionManager = Substitute.For<IRegionManager>();
            regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.RegionManager = regionManager;

            vm.Call("DisplayBurnDownCharts");
            sentParams.Should().NotBeNull();
            sentParams.ContainsKey("sprintId").Should().BeTrue();
            sentParams["sprintId"].Should().Be(vm.SprintId);
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "BurnDown", sentParams);
            vm.IsOpen.Should().BeFalse(); 
        }

        #endregion

        #region DisplayEditSprint()

        [Test]
        public void DisplayEditSprint()
        {
            NavigationParameters sentParams = null;
            vm.ProjectId = Guid.NewGuid();
            vm.SprintId = Guid.NewGuid();

            vm.IsOpen = true;

            var regionManager = Substitute.For<IRegionManager>();
            regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.RegionManager = regionManager;

            vm.Call("DisplayEditSprint");
            sentParams.Should().NotBeNull();
            sentParams.ContainsKey("ProjectId").Should().BeTrue();
            sentParams.ContainsKey("SprintId").Should().BeTrue();
            sentParams.ContainsKey("EditMode").Should().BeTrue();
            sentParams["ProjectId"].Should().Be(vm.ProjectId);
            sentParams["SprintId"].Should().Be(vm.SprintId);
            sentParams["EditMode"].Should().Be("true");
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "CreateSprint", sentParams);
            vm.IsOpen.Should().BeFalse();
        }

        #endregion

        #region DisplaySprintBacklog

        [Test]
        public void DisplaySprintBacklog()
        {
            NavigationParameters sentParams = null;
            vm.ProjectId = Guid.NewGuid();
            vm.SprintId = Guid.NewGuid();

            vm.IsOpen = true;

            var regionManager = Substitute.For<IRegionManager>();
            regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.RegionManager = regionManager;

            vm.Call("DisplaySprintBacklog");
            sentParams.Should().NotBeNull();
            sentParams.ContainsKey("ProjectId").Should().BeTrue();
            sentParams.ContainsKey("SprintId").Should().BeTrue();
            sentParams["ProjectId"].Should().Be(vm.ProjectId);
            sentParams["SprintId"].Should().Be(vm.SprintId);
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "ManageSprintBacklog", sentParams);
            vm.IsOpen.Should().BeFalse();
        }

        #endregion
    }
}
