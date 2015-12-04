using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
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
using ServiceSprint = AnyTrack.Infrastructure.BackendSprintService.ServiceSprint;

namespace Unit.Modules.AnyTrack.Sprints.Views.CreateSprintViewModelTests
{
    public class Context
    {
        public static IRegionManager regionManager;
        public static IProjectServiceGateway projectGateway;
        public static ISprintServiceGateway sprintGateway;
        public static CreateSprintViewModel vm;

        [SetUp]
        public static void SetUp()
        {
            regionManager = Substitute.For<IRegionManager>();
            projectGateway = Substitute.For<IProjectServiceGateway>();
            sprintGateway = Substitute.For<ISprintServiceGateway>();
            vm = new CreateSprintViewModel(sprintGateway, projectGateway);
            vm.RegionManager = regionManager;
        }
    }

    public class CreateSprintViewModelTests : Context
    {
        #region Constructor Tests

        [Test]
        public void ConstructViewModel()
        {
            vm = new CreateSprintViewModel(sprintGateway, projectGateway);
            vm.StartDate.Should().Be(DateTime.Today);
            vm.EndDate.Should().Be(DateTime.Today.AddDays(14));
            vm.DeveloperSearchUserResults.Should().NotBeNull();
            vm.SelectedDevelopers.Count.Should().Be(0);

            vm.SaveSprintCommand.Should().NotBeNull();
            vm.CancelSprintCommand.Should().NotBeNull();
            vm.SearchDeveloperCommand.Should().NotBeNull();
            vm.SelectDeveloperCommand.Should().NotBeNull();
            vm.RemoveDeveloperCommand.Should().NotBeNull();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructViewModelNullSprintGateway()
        {
            sprintGateway = null;

            vm = new CreateSprintViewModel(sprintGateway, projectGateway);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructViewModelNullProjectGateway()
        {
            projectGateway = null;

            vm = new CreateSprintViewModel(sprintGateway, projectGateway);
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
        public void CallOnNavEditModeNoDevelopers()
        {
            var projectId = Guid.NewGuid();
            var sprintId = Guid.NewGuid();
            var editmode = "true";

            ServiceSprint sprint = new ServiceSprint()
            {
                SprintId = sprintId,
                Name = "name",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(14),
                Description = "description"
            };

            var context = new NavigationContext(Substitute.For<IRegionNavigationService>(), new Uri("SprintOptions", UriKind.Relative));
            context.Parameters.Add("ProjectId", projectId);
            context.Parameters.Add("SprintId", sprintId);
            context.Parameters.Add("EditMode", editmode);

            sprintGateway.GetSprint(sprintId).Returns(sprint);

            vm.OnNavigatedTo(context);

            vm.SprintName.Should().Be(sprint.Name);
            vm.StartDate.Should().Be(sprint.StartDate);
            vm.EndDate.Should().Be(sprint.EndDate);
            vm.Description.Should().Be(sprint.Description);
        }

        [Test]
        public void CallOnNavEditModeHasDevelopers()
        {
            var projectId = Guid.NewGuid();
            var sprintId = Guid.NewGuid();
            var editmode = "true";

            ServiceSprint sprint = new ServiceSprint()
            {
                SprintId = sprintId,
                Name = "name",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(14),
                Description = "description",
                TeamEmailAddresses = new List<string>(){ "test@agile.com"}
            };

            var context = new NavigationContext(Substitute.For<IRegionNavigationService>(), new Uri("SprintOptions", UriKind.Relative));
            context.Parameters.Add("ProjectId", projectId);
            context.Parameters.Add("SprintId", sprintId);
            context.Parameters.Add("EditMode", editmode);

            sprintGateway.GetSprint(sprintId).Returns(sprint);

            ServiceUserSearchFilter sentFilter = null;

            var gatewayResponse = new List<ServiceUserSearchInfo>()
            {
                new ServiceUserSearchInfo()
            };

            projectGateway.SearchUsers(Arg.Do<ServiceUserSearchFilter>(f => sentFilter = f)).Returns(gatewayResponse);

            vm.OnNavigatedTo(context);

            vm.SprintName.Should().Be(sprint.Name);
            vm.StartDate.Should().Be(sprint.StartDate);
            vm.EndDate.Should().Be(sprint.EndDate);
            vm.Description.Should().Be(sprint.Description);
            projectGateway.Received().SearchUsers(sentFilter);
            vm.SelectedDevelopers.Count.Should().Be(1);
        }

        [Test]
        public void CallOnNavCreateMode()
        {
            var projectId = Guid.NewGuid();

            var context = new NavigationContext(Substitute.For<IRegionNavigationService>(), new Uri("SprintOptions", UriKind.Relative));
            context.Parameters.Add("ProjectId", projectId);

            vm.OnNavigatedTo(context);

            vm.SprintName.Should().Be(null);
            vm.StartDate.Should().Be(DateTime.Today);
            vm.EndDate.Should().Be(DateTime.Today.AddDays(14));
            vm.Description.Should().Be(null);
        }

        #endregion 

        #region SaveSprint() Tests

        [Test]
        public void SaveSprintHasErrors()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;

            vm.SprintName = "s";
            vm.Call("SaveSprint");
            windowProvider.Received().ShowMessageAsync("Sprint was not Saved", "There are errors in the form. Please correct them and try again.", MessageDialogStyle.Affirmative);
        }

        [Test]
        public void SaveSprintEditMode()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;

            vm.EditMode = true;
            vm.SprintName = "sprint";

            vm.Call("SaveSprint");
            windowProvider.Received().ShowMessageAsync("Save Successful", "Your sprint {0} has been saved successfully.".Substitute(vm.SprintName), MessageDialogStyle.Affirmative);
        }

        [Test]
        public void SaveSprintCreateMode()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;

            vm.EditMode = false;
            vm.SprintName = "sprint";

            vm.Call("SaveSprint");
            windowProvider.Received().ShowMessageAsync("Save Successful", "Your sprint {0} has been saved successfully.".Substitute(vm.SprintName), MessageDialogStyle.Affirmative);
        }

        #endregion

        #region CancelSprintCreation() Tests

        [Test]
        public void CancelSprintCreationTrue()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;
            NavigationParameters sentParams = null;
            var regionManager = Substitute.For<IRegionManager>();
            regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.RegionManager = regionManager;
            vm.EditMode = false;

            var waitObject = new ManualResetEvent(false);
            vm.MainWindow.ShowMessageAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<MessageDialogStyle>()).Returns(MessageDialogResult.Affirmative);
            vm.MainWindow.InvokeAction(Arg.Do<Action>(a => { a(); waitObject.Set(); }));

            vm.Call("CancelSprintCreation");

            waitObject.WaitOne();

            vm.MainWindow.Received().ShowMessageAsync("Sprint Creation Cancellation", "Are you sure you want to cancel? All data will be lost.", MessageDialogStyle.AffirmativeAndNegative);
            sentParams.Should().ContainKey("ProjectId");
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "SprintManager", sentParams);
        }

        [Test]
        public void CancelSprintEditModeTrue()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;
            NavigationParameters sentParams = null;
            var regionManager = Substitute.For<IRegionManager>();
            regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.RegionManager = regionManager;
            vm.EditMode = true;

            var waitObject = new ManualResetEvent(false);
            vm.MainWindow.ShowMessageAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<MessageDialogStyle>()).Returns(MessageDialogResult.Affirmative);
            vm.MainWindow.InvokeAction(Arg.Do<Action>(a => { a(); waitObject.Set(); }));

            vm.Call("CancelSprintCreation");

            waitObject.WaitOne();

            vm.MainWindow.Received().ShowMessageAsync("Sprint Edit Cancellation", "Are you sure you want to cancel? All changes will be lost.", MessageDialogStyle.AffirmativeAndNegative);
            sentParams.Should().ContainKey("ProjectId");
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "SprintManager", sentParams);
        }

        [Test]
        public void CancelSprintFalse()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;
            NavigationParameters sentParams = null;
            var regionManager = Substitute.For<IRegionManager>();
            regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.RegionManager = regionManager;
            vm.EditMode = true;

            var waitObject = new ManualResetEvent(false);
            vm.MainWindow.ShowMessageAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<MessageDialogStyle>()).Returns(MessageDialogResult.Negative);
            vm.MainWindow.InvokeAction(Arg.Do<Action>(a => { a(); waitObject.Set(); }));

            vm.Call("CancelSprintCreation");

            waitObject.WaitOne();

            vm.MainWindow.Received().ShowMessageAsync("Sprint Edit Cancellation", "Are you sure you want to cancel? All changes will be lost.", MessageDialogStyle.AffirmativeAndNegative);
        }

        #endregion

        #region SearchDevelopers

        [Test]
        public void SearchDevelopersNoResults()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;
            vm.SkillSetSearch = "C#, WPF";

            ServiceUserSearchFilter filter = null;
            var serviceResult = new List<ServiceUserSearchInfo>()
            {
            };

            projectGateway.SearchUsers(Arg.Do<ServiceUserSearchFilter>(f => filter = f)).Returns(serviceResult);
            vm.Call("SearchDevelopers");
            vm.DeveloperSearchUserResults.Count.Should().Be(0);
            vm.EnableDeveloperSearchGrid.Should().BeTrue();
            vm.MainWindow.Received().ShowMessageAsync("No Results Found", "There are no developers available that have the required skills. Please try another search.", MessageDialogStyle.Affirmative);
        }

        [Test]
        public void SearchDevelopersHasResults()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;
            vm.SkillSetSearch = "C#, WPF";

            ServiceUserSearchFilter filter = null;
            var serviceResult = new List<ServiceUserSearchInfo>()
            {
                new ServiceUserSearchInfo()
            };

            projectGateway.SearchUsers(Arg.Do<ServiceUserSearchFilter>(f => filter = f)).Returns(serviceResult);
            vm.Call("SearchDevelopers");
            vm.DeveloperSearchUserResults.Count.Should().Be(1);
            vm.EnableDeveloperSearchGrid.Should().BeTrue();
        }
        #endregion 

        #region CanAddDeveloper() Tests

        [Test]
        public void DeveloperCanNotBeAdded()
        {
            vm.SelectedDevelopers = new ObservableCollection<ServiceUserSearchInfo>()
            {
                new ServiceUserSearchInfo() {EmailAddress = "email@hotmail.com"},
                new ServiceUserSearchInfo()
            };

            var result = vm.Call<bool>("CanAddDeveloper", vm.SelectedDevelopers[0]);
            result.Should().BeFalse();
        }

        [Test]
        public void DeveloperCanBeAdded()
        {
            vm.SelectedDevelopers = new ObservableCollection<ServiceUserSearchInfo>()
            {
                new ServiceUserSearchInfo() {EmailAddress = "email@hotmail.com"},
            };

            var result = vm.Call<bool>("CanAddDeveloper", new ServiceUserSearchInfo());
            result.Should().BeTrue();
        }

        #endregion

        #region AddDeveloper Tests

        [Test]
        public void AddDeveloper()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;
            vm.SelectedDevelopers = new ObservableCollection<ServiceUserSearchInfo>();

            ServiceUserSearchInfo developer = new ServiceUserSearchInfo() {FullName = "John"};

            vm.Call("AddDeveloper", developer);
            vm.SelectedDevelopers.Count.Should().Be(1);
            vm.MainWindow.Received().ShowMessageAsync("Developer Added", "{0} has been successfully added as a developer for the sprint".Substitute(developer.FullName), MessageDialogStyle.Affirmative);
        }

        #endregion
    }
}
