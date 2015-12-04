using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.BackendSprintService;
using AnyTrack.Infrastructure.Providers;
using AnyTrack.Infrastructure.ServiceGateways;
using AnyTrack.Projects.Views;
using AnyTrack.SharedUtilities.Extensions;
using AnyTrack.Sprints.Views;
using FluentAssertions;
using MahApps.Metro.Controls.Dialogs;
using NSubstitute;
using NUnit.Framework;
using Prism.Regions;
using ServiceSprintStory = AnyTrack.Infrastructure.BackendSprintService.ServiceSprintStory;
using ServiceSprintSummary = AnyTrack.Infrastructure.BackendSprintService.ServiceSprintSummary;
using ServiceStory = AnyTrack.Infrastructure.BackendSprintService.ServiceStory;

namespace Unit.Modules.AnyTrack.Sprints.Views
{
    #region Context
    public class Context
    {
        public static IRegionManager regionManager;
        public static IProjectServiceGateway projectGateway;
        public static ISprintServiceGateway sprintGateway;
        public static ManageSprintBacklogViewModel vm;

        [SetUp]
        public static void SetUp()
        {
            regionManager = Substitute.For<IRegionManager>();
            projectGateway = Substitute.For<IProjectServiceGateway>();
            sprintGateway = Substitute.For<ISprintServiceGateway>();
            vm = new ManageSprintBacklogViewModel(sprintGateway, projectGateway);
            vm.RegionManager = regionManager;
        }
    }

    #endregion 

    #region Tests
    class ManageSprintBacklogViewModelTests : Context
    {
        #region Constructor Tests

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructNoGateways()
        {
            vm = new ManageSprintBacklogViewModel(null, null);
        }

        [Test]
        public void ConstructVm()
        {
            vm = new ManageSprintBacklogViewModel(sprintGateway,projectGateway);
            vm.AddAllToSprintCommand.Should().NotBeNull();
            vm.AddToSprintCommand.Should().NotBeNull();
            vm.RemoveAllFromSprintCommand.Should().NotBeNull();
            vm.RemoveFromSprintCommand.Should().NotBeNull();
            vm.SaveCommand.Should().NotBeNull();
        }

        #endregion 

        #region Method Tests

        #region IsNavigationTarget(NavigationContext navigationContext) Tests

        [Test]
        public void CallIsNavTarget()
        {
            var context = new NavigationContext(Substitute.For<IRegionNavigationService>(), new Uri("SprintOptions", UriKind.Relative));
            vm.IsNavigationTarget(context).Should().BeFalse();
        }

        #endregion 

        [Test]
        public void AddToSprintBacklog()
        {
            vm.SprintActive = false;
            projectGateway.GetProjectStories(Arg.Any<Guid>()).Returns(new List<ServiceStorySummary>());
            sprintGateway.GetSprintStories(Arg.Any<Guid>()).Returns(new List<ServiceSprintStory>());
        }

        #endregion

        #region AddToSprintBacklog

        [Test]
        public void AddToSprint()
        {
            vm.SprintActive = false;
            vm.SelectedProductStory = new ServiceStorySummary()
            {
                InSprint=false
            };
            vm.SprintBacklog = new ObservableCollection<ServiceSprintStory>();
            vm.ProductBacklog = new ObservableCollection<ServiceStorySummary>()
            {
                vm.SelectedProductStory,
                new ServiceStorySummary()
            };
            
            vm.Call("AddToSprint");
            vm.SprintBacklog.Count().Should().Be(1);
            vm.ProductBacklog.Count().Should().Be(1);
        }

        [Test]
        public void AddToSprintActive()
        {
            vm.SprintActive = true;

            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;
            
            vm.Call("AddToSprint");
            windowProvider.Received().ShowMessageAsync("Sprint is Active", "You cannot move stories during an active sprint");
        }

        #endregion

        #region RemoveFromSprintBacklog

        [Test]
        public void RemoveFromSprint()
        {
            vm.SprintActive = false;
            vm.SelectedSprint = new ServiceSprintStory()
            {
                Story = new ServiceStory()
            };

            vm.SprintBacklog = new ObservableCollection<ServiceSprintStory>()
            {
                vm.SelectedSprint
            };

            vm.ProductBacklog = new ObservableCollection<ServiceStorySummary>();

            vm.SprintBacklog.Count().Should().Be(1);
            vm.Call("RemoveFromSprint");
            vm.SprintBacklog.Count().Should().Be(0);
            vm.ProductBacklog.Count().Should().Be(1);
        }

        [Test]
        public void RemoveFromSprintActive()
        {
            vm.SprintActive = true;

            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;

            vm.Call("RemoveFromSprint");
            windowProvider.Received().ShowMessageAsync("Sprint is Active", "You cannot move stories during an active sprint");
        }

        #endregion

        #region PreventDuplicateStories

        [Test]
        public void PreventDuplicateStories()
        {
            vm.SprintBacklog = new ObservableCollection<ServiceSprintStory>()
            {
                new ServiceSprintStory()
                {
                    Story = new ServiceStory()
                    {
                        StoryId = new Guid("5e3f3a41-0c1c-4ae5-89eb-d2f37a3b5948"),
                        InSprint = true
                    },
                }
            };

            vm.ProductBacklog = new ObservableCollection<ServiceStorySummary>()
            {
                new ServiceStorySummary()
                {
                    StoryId = new Guid("5e3f3a41-0c1c-4ae5-89eb-d2f37a3b5948")
                }
            };

            vm.SprintBacklog.Count().Should().Be(1);
            vm.ProductBacklog.Count().Should().Be(1);

            vm.Call("PreventDuplicateStories");
            vm.ProductBacklog.Count().Should().Be(0);
        }
        #endregion

        #region MapSprintStoryToProductStory

        [Test]
        public void MapSprintStoryToProductStory()
        {
            vm.SprintBacklog = new ObservableCollection<ServiceSprintStory>()
            {
                new ServiceSprintStory()
                {
                    Story = new ServiceStory()
                    {
                        StoryId = new Guid("5e3f3a41-0c1c-4ae5-89eb-d2f37a3b5948"),
                        InSprint = true
                    },
                }
            };

            vm.Call("MapSprintStoryToProductStory", vm.SprintBacklog.First());
        }

        [Test]
        public void MapProductStoryToSprintStory()
        {
            vm.ProductBacklog = new ObservableCollection<ServiceStorySummary>()
            {
                new ServiceStorySummary()
                {
                    StoryId = new Guid("5e3f3a41-0c1c-4ae5-89eb-d2f37a3b5948"),
                    InSprint=false
                }
            };

            vm.Call("MapProductStoryToSprintStory", vm.ProductBacklog.First());
        }

        #endregion
    }
    #endregion
}