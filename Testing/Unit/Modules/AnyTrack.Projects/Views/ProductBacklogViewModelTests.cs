using AnyTrack.Projects.ServiceGateways;
using AnyTrack.Projects.Views;
using NSubstitute;
using NUnit.Framework;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using AnyTrack.Projects.BackendProjectService;
using AnyTrack.Infrastructure.Providers;
using System.Collections.ObjectModel;
using MahApps.Metro.Controls.Dialogs;
using System.Threading;
using AnyTrack.Infrastructure;
using Microsoft.Practices.Unity;
using AnyTrack.Infrastructure.Service;

namespace Unit.Modules.AnyTrack.Projects.Views.ProductBacklogViewModelTests
{
    #region Context 

    public class Context
    {
        public static IRegionManager regionManager;
        public static IProjectServiceGateway gateway;
        public static ProductBacklogViewModel vm;

        [SetUp]
        public static void SetUp()
        {
            regionManager = Substitute.For<IRegionManager>();
            gateway = Substitute.For<IProjectServiceGateway>();
            gateway.GetProjectNames(Arg.Any<bool>(),Arg.Any<bool>(),Arg.Any<bool>()).Returns(new List<ServiceProjectSummary>());
            vm = new ProductBacklogViewModel(gateway);

            vm.RegionManager = regionManager;
        }
    }

    #endregion

    #region Tests

    public class ProductBacklogViewModelTests : Context
    {
        #region Constructor Tests

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructNoGateway()
        {
            vm = new ProductBacklogViewModel(null);
        }

        [Test]
        public void ConstructVm()
        {
            vm = new ProductBacklogViewModel(gateway);
            vm.DeleteStoryCommand.Should().NotBeNull();
            vm.OpenStoryViewCommand.Should().NotBeNull();
        }

        #endregion

        #region DeleteStory() Tests

        [Test]
        public void DeleteStoryTests()
        {
            gateway.GetProjectStories(Arg.Any<Guid>()).Returns(new List<ServiceStorySummary>());

            var waitObject = new ManualResetEvent(false);

            vm.MainWindow = Substitute.For<WindowProvider>();
            vm.MainWindow.ShowMessageAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<MessageDialogStyle>()).Returns(MessageDialogResult.Affirmative);
            vm.MainWindow.InvokeAction(Arg.Do<Action>(a => { a(); waitObject.Set(); }));
            var story = new ServiceStorySummary
            {
                ProjectId = new Guid("11111111-1111-1111-1111-111111111111"),
                StoryId = new Guid("21111111-1111-1111-1111-111111111111")
            };
            vm.ProjectId = new Guid("11111111-1111-1111-1111-111111111111");
            vm.Stories.Add(story);

            vm.DeleteStory(story.StoryId.ToString());

            waitObject.WaitOne();

            vm.MainWindow.Received().ShowMessageAsync("Delete story - confirmation", "Are you sure that you want to delete this story from the backlog?", MessageDialogStyle.AffirmativeAndNegative);

            gateway.Received().DeleteStoryFromProductBacklog(story.ProjectId, story.StoryId);
            gateway.Received().GetProjectStories(story.ProjectId);
            vm.Stories.Should().BeEmpty();
        }

        #endregion DeleteStory() Tests

        #region OpenStoryView() Tests

        [Test]
        public void OpenStoryViewTests()
        {
            NavigationParameters navParams = null;
            var flyoutService = Substitute.For<IFlyoutService>();
            flyoutService.ShowMetroFlyout(Arg.Any<string>(), Arg.Do<NavigationParameters>(np => navParams = np));

            vm.FlyoutService = flyoutService;

            gateway.GetProjectStories(Arg.Any<Guid>()).Returns(new List<ServiceStorySummary>());

            vm.ProjectId = Guid.NewGuid();
            vm.Call("OpenStoryView");
            navParams.Should().NotBeNull();
            navParams.ContainsKey("projectId").Should().BeTrue();
            flyoutService.Received().ShowMetroFlyout("Story", navParams);
        }

        #endregion OpenStoryView() Tests

        #region EditStory() Tests

        [Test]
        public void EditStoryTests()
        {
            NavigationParameters navParams = null;

            var windowProvider = Substitute.For<WindowProvider>();
            var flyoutService = Substitute.For<IFlyoutService>();
            flyoutService.ShowMetroFlyout(Arg.Any<string>(), Arg.Do<NavigationParameters>(np => navParams = np));

            vm.FlyoutService = flyoutService;

            gateway.GetProjectStories(Arg.Any<Guid>()).Returns(new List<StoryDetails>());

            var story = new StoryDetails { StoryId = Guid.NewGuid() };

            vm.ProjectId = Guid.NewGuid();

            vm.Call("EditStory", story);
            navParams.Should().NotBeNull();
            navParams.ContainsKey("projectId").Should().BeTrue();
            navParams.ContainsKey("storyId").Should().BeTrue();
            flyoutService.Received().ShowMetroFlyout("Story", navParams);
        }

        #endregion EditStory() Tests
    }

    #endregion Tests
}