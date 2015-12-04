using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using NSubstitute;
using AnyTrack.Sprints.Views;
using Prism.Regions;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.ServiceGateways;
using AnyTrack.Infrastructure.Providers;
using OxyPlot;
using System.Collections.ObjectModel;
using System.Threading;
using AnyTrack.Infrastructure;
using MahApps.Metro.Controls.Dialogs;
using ServiceModel = AnyTrack.Infrastructure.BackendSprintService;

namespace Unit.Modules.AnyTrack.Sprints.Views.BurnDownViewModelTests
{
    #region Setup

    public class Context
    {
        public static IRegionManager regionManager;
        public static Guid projectId = Guid.NewGuid();
        public static Guid sprintId = Guid.NewGuid();
        public static string projectName = "Test";
        public static string sprintName = "SprintNameTest";
        public static IProjectServiceGateway projectServiceGateway;
        public static ISprintServiceGateway sprintServiceGateway;
        public static BurnDownViewModel vm;

        [SetUp]
        public void SetUp()
        {
            projectServiceGateway = Substitute.For<IProjectServiceGateway>();
            sprintServiceGateway = Substitute.For<ISprintServiceGateway>();

            projectServiceGateway.GetProjectNames(Arg.Any<bool>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(new List<ServiceProjectSummary>());

            sprintServiceGateway.GetSprintNames(Arg.Any<Nullable<Guid>>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(new List<ServiceModel.ServiceSprintSummary>());

            vm = new BurnDownViewModel(projectServiceGateway, sprintServiceGateway);
            vm.RegionManager = Substitute.For<IRegionManager>();

        }
        #endregion Setup
    }
    public class BurnDownViewModelTests : Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructNoServices()
        {
            vm = new BurnDownViewModel(null, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructNoProjectServices()
        {
            vm = new BurnDownViewModel(null, sprintServiceGateway);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructNoSprintServices()
        {
            vm = new BurnDownViewModel(projectServiceGateway, null);
        }

        [Test]
        public void ConstructVm()
        {
            vm = new BurnDownViewModel(projectServiceGateway, sprintServiceGateway);
            vm.GetChartForProjectAndSprint.Should().NotBeNull();
            vm.GetStoryPointBD.Should().NotBeNull();
            vm.Sprints.Should().NotBeNull();
            vm.Projects.Should().NotBeNull();
        }

        #endregion Constructor Tests

        #region GetBurndownChartForProjectAndSprint() Tests 

        [Test]
        public void GetBurndownChartForProjectAndSprint()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;
            NavigationParameters sentParams = null;
            var regionManager = Substitute.For<IRegionManager>();
            regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.RegionManager = regionManager;

            vm.Trend.Add(new DataPoint(40, 0));
            vm.Points.Add(new DataPoint(30, 10));
            vm.Points.Add(new DataPoint(20, 5));

            sprintServiceGateway.GetSprintNames(Arg.Any<Guid>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(new List<ServiceModel.ServiceSprintSummary>());
            vm.Sprints = new ObservableCollection<ServiceModel.ServiceSprintSummary>();
            vm.ProjectId = Guid.NewGuid();
            vm.SprintId = Guid.NewGuid();

            vm.Call("GetBurndownChartForProjectAndSprint");
            vm.Trend.Should().BeEmpty();
            vm.Points.Should().BeEmpty();
            vm.HasErrors.Should().BeFalse();
            sentParams.Should().NotBeNull();
            sentParams["projectId"].Should().Be(vm.ProjectId);
            sentParams["sprintId"].Should().Be(vm.SprintId);
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "BurnDown", sentParams);
        }

        #endregion GetBurndownChartForProjectAndSprint() Tests

        #region GetStoryPointBurnDown() Tests 

        [Test]
        public void GetStoryPointBurnDown()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;
            NavigationParameters sentParams = null;
            var regionManager = Substitute.For<IRegionManager>();
            regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.RegionManager = regionManager;

            vm.Trend.Add(new DataPoint(40, 0));
            vm.Points.Add(new DataPoint(30, 10));
            vm.Points.Add(new DataPoint(20, 5));

            sprintServiceGateway.GetSprintNames(Arg.Any<Guid>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(new List<ServiceModel.ServiceSprintSummary>());
            vm.Sprints = new ObservableCollection<ServiceModel.ServiceSprintSummary>();
            vm.ProjectId = Guid.NewGuid();
            vm.SprintId = Guid.NewGuid();

            vm.Call("GetStoryPointBurnDown");
            vm.Trend.Should().BeEmpty();
            vm.Points.Should().BeEmpty();
            vm.HasErrors.Should().BeFalse();
            sentParams.Should().NotBeNull();
            sentParams["projectId"].Should().Be(vm.ProjectId);
            sentParams["sprintId"].Should().Be(vm.SprintId);
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "BurnDown", sentParams);

        }

        #endregion GetStoryPointBurnDown() Tests

        #region OnNavigatedTo Tests()

        [Test]
        public void OnNavigatedTo()
        {
            var sprintId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            var navigationService = Substitute.For<IRegionNavigationService>();
            var navContext = new NavigationContext(navigationService, new Uri("BurnDown", UriKind.Relative));
            navContext.Parameters.Add("projectId", projectId);
            navContext.Parameters.Add("sprintId", sprintId);

            sprintServiceGateway.GetAllTasksForSprint(sprintId).Returns(new List<ServiceModel.ServiceTask>()
            {
                new ServiceModel.ServiceTask
                {
                    TaskHourEstimates = new List<ServiceModel.ServiceTaskHourEstimate>()
                    {
                        new ServiceModel.ServiceTaskHourEstimate
                        {
                            Estimate = 10,
                            Created = DateTime.Now
                        }
                    }

                }
            });

            sprintServiceGateway.GetSprintStoryEstimates(sprintId).Returns(new List<ServiceModel.ServiceSprintStory>());

            vm.OnNavigatedTo(navContext);

            vm.SprintId.Should().Be(sprintId);
            vm.ProjectId.Should().Be(projectId);
            sprintServiceGateway.Received().GetAllTasksForSprint(sprintId);
            sprintServiceGateway.Received().GetSprintStoryEstimates(sprintId);
        }
        #endregion OnNavigatedTo Tests()
    }
}
               