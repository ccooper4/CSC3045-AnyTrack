using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using NSubstitute;
using AnyTrack.PlanningPoker.Views;
using AnyTrack.PlanningPoker.ServiceGateways;
using Prism.Regions;
using AnyTrack.Infrastructure.BackendProjectService;
using SprintModels = AnyTrack.Infrastructure.BackendSprintService;
using AnyTrack.Infrastructure.ServiceGateways;
using AnyTrack.Infrastructure.Providers;

namespace Unit.Modules.AnyTrack.PlanningPoker.Views.StartPlanningPokerSessionViewModelTests
{
    #region Setup

    public class Context
    {
        public static Guid projectId = Guid.NewGuid();
        public static string projectName = "Test";
        public static IPlanningPokerManagerServiceGateway serviceGateway;
        public static IProjectServiceGateway projectServiceGateway;
        public static ISprintServiceGateway sprintServiceGateway;
        public static StartPlanningPokerSessionViewModel vm;

        [SetUp]
        public void SetUp()
        {
            var projectNames = new List<ServiceProjectSummary>()
            {
                new ServiceProjectSummary { ProjectId = projectId, ProjectName = projectName}
            }; 
            
            serviceGateway = Substitute.For<IPlanningPokerManagerServiceGateway>();
            projectServiceGateway = Substitute.For<IProjectServiceGateway>();
            sprintServiceGateway = Substitute.For<ISprintServiceGateway>();

            projectServiceGateway.GetProjectNames(true, false, false).Returns(projectNames);

            vm = new StartPlanningPokerSessionViewModel(serviceGateway, projectServiceGateway, sprintServiceGateway);
        }
    }

    #endregion 

    #region Tests 

    public class StartPlanningPokerSessionViewModelTests : Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoService()
        {
            vm = new StartPlanningPokerSessionViewModel(null, projectServiceGateway, sprintServiceGateway);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoProjectService()
        {
            vm = new StartPlanningPokerSessionViewModel(serviceGateway, null, sprintServiceGateway);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoSprintService()
        {
            vm = new StartPlanningPokerSessionViewModel(serviceGateway, projectServiceGateway, null);
        }

        [Test]
        public void ConstructViewModel()
        {
            vm = new StartPlanningPokerSessionViewModel(serviceGateway, projectServiceGateway, sprintServiceGateway);

            vm.StartPokerSession.Should().NotBeNull();
            vm.Sprints.Should().NotBeNull();
            vm.Projects.Should().NotBeNull();
        }

        #endregion 

        #region IsNavigationTarget(NavigationContext navigationContext) Tests 

        [Test]
        public void CallIsNavigationTarget()
        {
            var navService = Substitute.For<IRegionNavigationService>();
            var navContext = new NavigationContext(navService, new Uri("Test", UriKind.Relative));

            var res = vm.IsNavigationTarget(navContext);

            res.Should().BeFalse();
        }

        #endregion 

        #region OnNavigatedTo(NavigationContext navigationContext) Tests 

        [Test]
        public void CallOnNavigateTo()
        {
            var oldProjectId = Guid.NewGuid();

            vm.Projects = new System.Collections.ObjectModel.ObservableCollection<ServiceProjectSummary>()
            {
                new ServiceProjectSummary { ProjectId = oldProjectId }
            };

            var navigationService = Substitute.For<IRegionNavigationService>();
            var navContext = new NavigationContext(navigationService, new Uri("StartPlanningPoker", UriKind.Relative));

            vm.OnNavigatedTo(navContext);

            projectServiceGateway.Received().GetProjectNames(true, false, false);

            vm.Projects.Single().ProjectId.Should().Be(projectId);
            vm.Projects.Single().ProjectName.Should().Be(projectName);
            vm.Projects.Select(p => p.ProjectId).Should().NotContain(oldProjectId);
        }

        #endregion 

        #region SetProjectId Tests 

        [Test]
        public void SetProjectId()
        {
            var sprints = new List<SprintModels.ServiceSprintSummary>()
            {
                new SprintModels.ServiceSprintSummary { Description = "Test", Name = "Test", SprintId = Guid.NewGuid()}
            };

            var projectId = Guid.NewGuid();
            var oldSprintId = Guid.NewGuid();

            sprintServiceGateway.GetSprintNames(projectId, Arg.Any<bool>(), Arg.Any<bool>()).Returns(sprints);

            vm.Sprints = new System.Collections.ObjectModel.ObservableCollection<SprintModels.ServiceSprintSummary>()
            {
                new SprintModels.ServiceSprintSummary() { SprintId = oldSprintId}
            }; 

            vm.ProjectId = projectId;
            sprintServiceGateway.Received().GetSprintNames(projectId, true, false);
            vm.Sprints.Count.Should().Be(1);
            vm.Sprints.Select(s => s.SprintId).Should().NotContain(oldSprintId);
            vm.Sprints.Single().SprintId.Should().Be(sprints.Single().SprintId);
            vm.Sprints.Single().Name.Should().Be(sprints.Single().Name);
            vm.Sprints.Single().Description.Should().Be(sprints.Single().Description);
        }

        #endregion 

        #region CanStartSession() Tests 

        [Test]
        public void CallCanStartSessionWithSprintIdSet()
        {
            vm.SprintId = Guid.NewGuid();
            var res = vm.Call<bool>("CanStartSession");

            res.Should().BeTrue();
        }

        [Test]
        public void CallCanStartSessionWithNoSprintIdSet()
        {
            vm.SprintId = null;
            var res = vm.Call<bool>("CanStartSession");

            res.Should().BeFalse();
        }

        #endregion 

        #region EstablishPokerSession() Tests

        [Test]
        public void CallEstablishPokerSession()
        {
            vm.MainWindow = Substitute.For<WindowProvider>();
            vm.SprintId = Guid.NewGuid();

            vm.Call("EstablishPokerSession");

            serviceGateway.Received().StartNewPokerSession(vm.SprintId.Value);
            vm.MainWindow.Received().ShowMessageAsync("Sesion started", "The planning poker session has been started");
        }

        #endregion 
    }

    #endregion 
}
