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
            gateway.GetProjectNames().Returns(new List<ProjectDetails>());
            vm = new ProductBacklogViewModel(regionManager, gateway);
        }

        #endregion

        #region Tests 

        public class ProductBacklogViewModelTests : Context
        {
            #region Constructor Tests 

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ConstructNoRegionManager()
            {
                vm = new ProductBacklogViewModel(null, gateway);
            }

            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ConstructNoGateway()
            {
                vm = new ProductBacklogViewModel(regionManager, null);
            }

            [Test]
            public void ConstructVm()
            {
                vm = new ProductBacklogViewModel(regionManager, gateway);
                vm.DeleteStoryCommand.Should().NotBeNull();
                vm.OpenStoryViewCommand.Should().NotBeNull();
            }
            #endregion

            #region DeleteStory() Tests 

            [Test]
            public void DeleteStoryTests()
            {
                gateway.GetProjectStories(Arg.Any<Guid>()).Returns(new List<StoryDetails>());

                var waitObject = new ManualResetEvent(false);

                vm.MainWindow = Substitute.For<WindowProvider>();
                vm.MainWindow.ShowMessageAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<MessageDialogStyle>()).Returns(MessageDialogResult.Affirmative);
                vm.MainWindow.InvokeAction(Arg.Do<Action>(a => { a(); waitObject.Set(); }));
                var story = new StoryDetails
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
                regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(np => navParams = np));

                gateway.GetProjectStories(Arg.Any<Guid>()).Returns(new List<StoryDetails>());

                vm.ProjectId = Guid.NewGuid();
                vm.Call("OpenStoryView");
                navParams.Should().NotBeNull();
                navParams.ContainsKey("projectId").Should().BeTrue();
                regionManager.Received().RequestNavigate(RegionNames.MainRegion, "Story", navParams);
            }

            #endregion OpenStoryView() Tests
            #endregion Tests
        }
    }
}