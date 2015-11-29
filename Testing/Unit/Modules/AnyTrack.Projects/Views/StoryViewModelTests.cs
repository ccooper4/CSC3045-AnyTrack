using AnyTrack.Infrastructure.ServiceGateways;
using AnyTrack.Projects.Views;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using System.Collections.ObjectModel;
using Prism.Regions;
using AnyTrack.Infrastructure.Providers;
using MahApps.Metro.Controls.Dialogs;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;

namespace Unit.Modules.AnyTrack.Projects.Views.StoryViewModelTests
{
    #region Context

    public class Context
    {
        public static IProjectServiceGateway serviceGateway;
        public static StoryViewModel vm;

        [SetUp]
        public void SetUp()
        {
            serviceGateway = Substitute.For<IProjectServiceGateway>();
            vm = new StoryViewModel(serviceGateway);
        }
    }

    #endregion 

    #region Tests 

    public class StoryViewModelTests: Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoService()
        {
            vm = new StoryViewModel(null);
        }

        [Test]
        public void ConstructViewModel()
        {
            vm = new StoryViewModel(serviceGateway);
            vm.Projects.Should().NotBeNull();
            vm.SaveUpdateStoryCommand.Should().NotBeNull();
            vm.Header.Should().Be("Story");
        }

        #endregion 

        #region OnNavigatedTo(NavigationContext navigationContext) Tests 

        [Test]
        public void CallOnNavigatedTo()
        {
            var navService = Substitute.For<IRegionNavigationService>();

            var context = new NavigationContext(navService, new Uri("Story", UriKind.Relative));

            var projectDetails = new List<ServiceProjectSummary>()
            {
                new ServiceProjectSummary { ProjectId = Guid.NewGuid(), ProjectName = "Test"}
            };

            serviceGateway.GetProjectNames(false, true, false).Returns(projectDetails);

            vm.Projects = new ObservableCollection<ServiceProjectSummary>()
            {
                new ServiceProjectSummary() { ProjectId = Guid.NewGuid(), ProjectName = "Old"}
            };

            vm.OnNavigatedTo(context);

            serviceGateway.Received().GetProjectNames(false, true, false);
            vm.Projects.Select(p => p.ProjectName).Should().NotContain("Old");
            vm.Projects.Select(p => p.ProjectName).Should().Contain("Test");
        }

        [Test]
        public void CallOnNavigatedToWithProjectId()
        {
            var projectId = Guid.NewGuid();

            var navService = Substitute.For<IRegionNavigationService>();

            var context = new NavigationContext(navService, new Uri("Story", UriKind.Relative));
            context.Parameters.Add("projectId", projectId);

            var projectDetails = new List<ServiceProjectSummary>()
            {
                new ServiceProjectSummary { ProjectId = Guid.NewGuid(), ProjectName = "Test"}
            };

            serviceGateway.GetProjectNames(false, true, false).Returns(projectDetails);

            vm.Projects = new ObservableCollection<ServiceProjectSummary>()
            {
                new ServiceProjectSummary() { ProjectId = Guid.NewGuid(), ProjectName = "Old"}
            };

            vm.OnNavigatedTo(context);

            serviceGateway.Received().GetProjectNames(false, true, false);
            vm.Projects.Select(p => p.ProjectName).Should().NotContain("Old");
            vm.Projects.Select(p => p.ProjectName).Should().Contain("Test");
            vm.ProjectId.Should().Be(projectId);
        }

        [Test]
        public void CallOnNavigatedToWithProjectIdAndStoryId()
        {
            var projectId = Guid.NewGuid();
            var storyId = Guid.NewGuid();

            var navService = Substitute.For<IRegionNavigationService>();

            var context = new NavigationContext(navService, new Uri("Story", UriKind.Relative));
            context.Parameters.Add("projectId", projectId);
            context.Parameters.Add("storyId", storyId);

            var projectDetails = new List<ServiceProjectSummary>()
            {
                new ServiceProjectSummary { ProjectId = Guid.NewGuid(), ProjectName = "Test"}
            };

            serviceGateway.GetProjectNames(false, true, false).Returns(projectDetails);

            vm.Projects = new ObservableCollection<ServiceProjectSummary>()
            {
                new ServiceProjectSummary() { ProjectId = Guid.NewGuid(), ProjectName = "Old"}
            };

            var story = new ServiceStory
            {
                StoryId = storyId,
                ProjectId = projectId,
                AsA = "AsA",
                ConditionsOfSatisfaction = "Conditions",
                IWant = "IWant",
                SoThat = "SoThat",
                Summary = "Summary"
            };

            serviceGateway.GetProjectStory(projectId, storyId).Returns(story);

            vm.OnNavigatedTo(context);

            serviceGateway.Received().GetProjectNames(false, true, false);
            vm.Projects.Select(p => p.ProjectName).Should().NotContain("Old");
            vm.Projects.Select(p => p.ProjectName).Should().Contain("Test");
            vm.ProjectId.Should().Be(projectId);
            serviceGateway.Received().GetProjectStory(projectId, storyId);
            vm.Summary.Should().Be(story.Summary);
            vm.AsA.Should().Be(story.AsA);
            vm.ConditionsOfSatisfaction.Should().Be(story.ConditionsOfSatisfaction);
            vm.IWant.Should().Be(story.IWant);
            vm.SoThat.Should().Be(story.SoThat);
            vm.Summary.Should().Be(story.Summary);
        }

        #endregion 

        #region IsNavigationTarget(NavigationContext navigationContext) Tests 

        [Test]
        public void CallIsNavigationTarget()
        {
            var service = Substitute.For<IRegionNavigationService>();
            var context = new NavigationContext(service, new Uri("Story", UriKind.Relative));

            var result = vm.IsNavigationTarget(context);
            result.Should().BeFalse();
        }

        #endregion 

        #region SaveUpdateStory() Tests 

        [Test]
        public void CallSaveUpdateStoryWithAValidationError()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;

            var regionManager = Substitute.For<IRegionManager>();
            vm.RegionManager = regionManager;
            
            vm.IsOpen = true; 

            vm.Call("SaveUpdateStory");

            serviceGateway.DidNotReceive().SaveUpdateStory(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<ServiceStory>());
            windowProvider.DidNotReceive().ShowMessageAsync("Story has been saved!", "Success", MessageDialogStyle.Affirmative);
            vm.IsOpen.Should().BeTrue();
            regionManager.DidNotReceive().RequestNavigate(RegionNames.MainRegion, "ProductBacklog", Arg.Any<NavigationParameters>());
            
        }

        [Test]
        public void CallSaveUpdateStoryWithNoErrors()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;

            NavigationParameters sentParams = null; 
            var regionManager = Substitute.For<IRegionManager>();
            vm.RegionManager = regionManager;
            regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(n => sentParams = n));

            ServiceStory sentStory = null;
            serviceGateway.SaveUpdateStory(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Do<ServiceStory>(s => sentStory = s));

            vm.IsOpen = true;

            vm.Summary = "Summary";
            vm.AsA = "AsA";
            vm.IWant = "IWant";
            vm.SoThat = "SoThat";
            vm.ConditionsOfSatisfaction = "Conditions";
            vm.ProjectId = Guid.NewGuid();

            vm.Call("SaveUpdateStory");

            sentStory.Should().NotBeNull();
            serviceGateway.Received().SaveUpdateStory(vm.ProjectId, Arg.Any<Guid>(), sentStory);
            windowProvider.Received().ShowMessageAsync("Story has been saved!", "Success", MessageDialogStyle.Affirmative);
            vm.IsOpen.Should().BeFalse();
            sentParams.Should().NotBeNull();
            sentParams.ContainsKey("projectId").Should().BeTrue();
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "ProductBacklog", sentParams);

        }

        #endregion 

    }

    #endregion 
}
