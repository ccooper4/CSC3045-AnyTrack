using AnyTrack.Projects.Views;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MahApps.Metro.Controls;
using Prism.Regions;
using NSubstitute;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;

namespace Unit.Modules.AnyTrack.Projects.Views.ProjectOptionsViewModelTests
{
    #region Context 

    public class Context
    {
        public static ProjectOptionsViewModel vm;

        [SetUp]
        public void Setup()
        {
            vm = new ProjectOptionsViewModel();
        }
    }

    #endregion 

    #region Tests 

    public class ProjectOptionsViewModelTests : Context
    {
        #region Constructor Tests 

        [Test]
        public void ConstructVm()
        {
            vm = new ProjectOptionsViewModel();
            vm.Header.Should().Be("Project Options");
            vm.IsModal.Should().BeTrue();
            vm.Position.Should().Be(Position.Right);
            vm.Theme.Should().Be(FlyoutTheme.Accent);

            vm.ViewBacklog.Should().NotBeNull();
        }

        #endregion 

        #region IsNavigationTarget(NavigationContext navigationContext) Tests 

        [Test]
        public void CallIsNavTarget()
        {
            var context = new NavigationContext(Substitute.For<IRegionNavigationService>(), new Uri("ProductBacklog", UriKind.Relative));
            vm.IsNavigationTarget(context).Should().BeFalse();
        }

        #endregion 

        #region OnNavigatedTo(NavigationContext navigationContext) Tests 

        [Test]
        public void CallOnNavToWithNoSummary()
        {
            var context = new NavigationContext(Substitute.For<IRegionNavigationService>(), new Uri("ProductBacklog", UriKind.Relative));
            vm.IsOpen = true;

            vm.OnNavigatedTo(context);

            vm.IsOpen.Should().BeFalse();
        }

        [Test]
        public void CallOnNavToWithSummary()
        {
            var summary = new ServiceProjectRoleSummary();
            summary.ProjectId = Guid.NewGuid();
            summary.Name = "Test";
            summary.Description = "Test";

            var context = new NavigationContext(Substitute.For<IRegionNavigationService>(), new Uri("ProductBacklog", UriKind.Relative));
            context.Parameters.Add("projectInfo", summary);

            vm.IsOpen = true;

            vm.OnNavigatedTo(context);

            vm.IsOpen.Should().BeTrue();
            vm.Header.Should().Be("Project Options - " + summary.Name);
            vm.ProjectName.Should().Be(summary.Name);
            vm.ProjectDescription.Should().Be(summary.Description);
            vm.ProjectId.Should().Be(summary.ProjectId.ToString());
        }

        #endregion 

        #region GoToBacklog(string projectId) Tests 

        [Test]
        public void CallGoToBacklog()
        {
            NavigationParameters sentParams = null;
            string projectId = Guid.NewGuid().ToString();
            vm.IsOpen = true;

            var regionManager = Substitute.For<IRegionManager>();
            regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.RegionManager = regionManager;

            vm.Call("GoToBacklog", projectId);
            sentParams.Should().NotBeNull();
            sentParams.ContainsKey("projectId").Should().BeTrue();
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "ProductBacklog", sentParams);
            vm.IsOpen.Should().BeFalse();
        }

        #endregion 
    }

    #endregion 
}
