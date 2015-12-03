using AnyTrack.Infrastructure.ServiceGateways;
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
using AnyTrack.Infrastructure.Providers;
using System.Collections.ObjectModel;
using AnyTrack.Infrastructure.Security;
using MahApps.Metro.Controls.Dialogs;
using AnyTrack.SharedUtilities.Extensions;
using System.Security.Principal;
using System.Threading;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Infrastructure.BackendProjectService;

namespace Unit.Modules.AnyTrack.Projects.Views.CreateProjectViewModelTests
{
    #region Context 
    public class Context
    {
        public static IRegionManager regionManager;
        public static IProjectServiceGateway gateway;
        public static CreateProjectViewModel vm; 

        [SetUp]
        public static void SetUp()
        {
            regionManager = Substitute.For<IRegionManager>();
            gateway = Substitute.For<IProjectServiceGateway>();
            vm = new CreateProjectViewModel(gateway);
            vm.RegionManager = regionManager;
        }
    }

    #endregion 

    #region Tests 

    public class CreateProjectViewModelTests: Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructNoGateway()
        {
            vm = new CreateProjectViewModel(null);
        }

        [Test]
        public void ConstructVm()
        {
            vm = new CreateProjectViewModel(gateway);
            vm.SaveProjectCommand.Should().NotBeNull();
            vm.CancelProjectCommand.Should().NotBeNull();
            vm.SetProductOwnerCommand.Should().NotBeNull();
            vm.ProductOwnerSearchUserResults.Should().NotBeNull();
            vm.StartedOn.ToString("d").Should().Be(DateTime.Now.ToString("d"));
            vm.SearchScrumMasterCommand.Should().NotBeNull();
            vm.SelectScrumMasterCommand.Should().NotBeNull();
            vm.ScrumMasterSearchUserResults.Should().NotBeNull();
            vm.SelectedScrumMasters.Should().NotBeNull();
        }

        #endregion 

        #region SearchProjectOwners() Tests 

        [Test]
        public void SearchProjectOwners()
        {
            vm.ProductOwnerSearchEmailAddress = "test@agile.com";
            ServiceUserSearchFilter sentFilter = null;

            var gatewayResponse = new List<ServiceUserSearchInfo>()
            {
                new ServiceUserSearchInfo()
            };

            gateway.SearchUsers(Arg.Do<ServiceUserSearchFilter>(f => sentFilter = f)).Returns(gatewayResponse);

            vm.Call("SearchProjectOwners");
            sentFilter.Should().NotBeNull();
            sentFilter.EmailAddress.Should().Be(vm.ProductOwnerSearchEmailAddress);
            sentFilter.ProductOwner.Should().BeTrue();
            sentFilter.ScrumMaster.HasValue.Should().BeFalse();
            gateway.Received().SearchUsers(sentFilter);

            vm.ProductOwnerSearchUserResults.Should().Contain(gatewayResponse);
            vm.EnableProductOwnerSearchGrid.Should().BeTrue();
            vm.ProductOwnerConfirmed.Should().BeFalse();
        }

        [Test]
        public void SearchProjectOwnersNoResults()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;
            vm.ProductOwnerSearchEmailAddress = "test@agile.com";
            ServiceUserSearchFilter sentFilter = null;

            var gatewayResponse = new List<ServiceUserSearchInfo>();

            gateway.SearchUsers(Arg.Do<ServiceUserSearchFilter>(f => sentFilter = f)).Returns(gatewayResponse);

            vm.Call("SearchProjectOwners");
            sentFilter.Should().NotBeNull();
            sentFilter.EmailAddress.Should().Be(vm.ProductOwnerSearchEmailAddress);
            sentFilter.ProductOwner.Should().BeTrue();
            sentFilter.ScrumMaster.HasValue.Should().BeFalse();
            gateway.Received().SearchUsers(sentFilter);
            windowProvider.Received().ShowMessageAsync("No Results Found", "No Product Owners with the email address {0} have been found.".Substitute(vm.ProductOwnerSearchEmailAddress).Substitute(vm.ProjectName), MessageDialogStyle.Affirmative);
            vm.ProductOwnerSearchUserResults.Count.Should().Be(0);
            vm.EnableProductOwnerSearchGrid.Should().BeTrue();

            vm.ProductOwnerConfirmed.Should().BeFalse();

        }

        #endregion 

        #region .SetProductOwner(string emailAddress) Tests 

        [Test]
        public void SetProductOwnerTests()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            var emailAddress = "test@agile.local"; 
            vm.MainWindow = windowProvider;

            vm.ProductOwnerSearchUserResults = new ObservableCollection<ServiceUserSearchInfo>()
            {
                new ServiceUserSearchInfo()
            };

            vm.Call("SetProductOwner", emailAddress);
            vm.SelectProductOwnerEmailAddress.Should().Be(emailAddress);
            vm.ProductOwnerSearchUserResults.Count.Should().Be(0);
            vm.EnableProductOwnerSearchGrid.Should().BeFalse();
            vm.ProductOwnerConfirmed.Should().BeTrue();
            vm.ProductOwnerSearchEmailAddress.Should().Be(string.Empty);
            windowProvider.Received().ShowMessageAsync("Product Owner Assigned", "{0} has been assigned the role Product Owner of the project.".Substitute(emailAddress));

        }

        #endregion 

        #region SaveProject Test

        [Test]
        public void SaveProject()
        {
            var windowProvider = Substitute.For<WindowProvider>();

            var currentPrincipal = new ServiceUserPrincipal(new ServiceLoginResult { EmailAddress = "testmanager@agile.local" }, "");

            UserDetailsStore.LoggedInUserPrincipal = currentPrincipal;
            vm.MainWindow = windowProvider;

            vm.ProjectName = "Test Project";
            vm.Description = "This is a description";
            vm.VersionControl = "V4";
            vm.StartedOn = new DateTime(30, 09, 15);
            vm.SelectProductOwnerEmailAddress = "test@agile.local";
            vm.ProductOwnerConfirmed = true;
            vm.SelectedScrumMasters = new ObservableCollection<ServiceUserSearchInfo>()
            {
                new ServiceUserSearchInfo { EmailAddress = "test@agile.local"}
            };

            ServiceProject sentProject = null;

            gateway.CreateProject(Arg.Do<ServiceProject>(n => sentProject = n));

            vm.Call("SaveProject");

            sentProject.Should().NotBeNull();
            sentProject.Description.Should().Be(vm.Description);
            sentProject.Name.Should().Be(vm.ProjectName);
            sentProject.VersionControl.Should().Be(vm.VersionControl);
            sentProject.StartedOn.Should().Be(vm.StartedOn);
            sentProject.ProductOwnerEmailAddress.Should().Be(vm.SelectProductOwnerEmailAddress);
            sentProject.ProjectManagerEmailAddress.Should().Be("testmanager@agile.local");
            gateway.Received().CreateProject(sentProject);
            windowProvider.Received().ShowMessageAsync("Project created", "The {0} project has been created sucessfully.".Substitute(vm.ProjectName), MessageDialogStyle.Affirmative);
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "MyProjects");
        }

        [Test]
        public void SaveProjectEditMode()
        {
            var windowProvider = Substitute.For<WindowProvider>();

            var currentPrincipal = new ServiceUserPrincipal(new ServiceLoginResult { EmailAddress = "testmanager@agile.local" }, "");

            UserDetailsStore.LoggedInUserPrincipal = currentPrincipal;
            vm.MainWindow = windowProvider;

            #region Test Data

            ServiceProject serviceProject = new ServiceProject
            {
                Name = "Project",
                Description = "This is a new project",
                VersionControl = "queens.git",
                ProjectManagerEmailAddress = "tester@agile.local",
                ProductOwnerEmailAddress = "tester@agile.local",
                StartedOn = DateTime.Today
            };

            serviceProject.ScrumMasterEmailAddresses = new List<string>();
            serviceProject.ScrumMasterEmailAddresses.Add("S1@test.com");
            serviceProject.ScrumMasterEmailAddresses.Add("S2@test.com");

            var serviceResult = new List<ServiceUserSearchInfo>()
            {
                new ServiceUserSearchInfo()
            };
            ServiceUserSearchFilter filter = null;
            gateway.SearchUsers(Arg.Do<ServiceUserSearchFilter>(f => filter = f)).Returns(serviceResult);

            #endregion

            var context = new NavigationContext(Substitute.For<IRegionNavigationService>(), new Uri("MyProjects", UriKind.Relative));
            context.Parameters.Add("ProjectId", new Guid("11111111-1111-1111-1111-111111111111"));
            context.Parameters.Add("EditMode", "true");

            gateway.GetProject(new Guid("11111111-1111-1111-1111-111111111111")).Returns(serviceProject);

            vm.OnNavigatedTo(context);

            vm.ProjectName.Should().Be(serviceProject.Name);
            vm.Description.Should().Be(serviceProject.Description);
            vm.VersionControl.Should().Be(serviceProject.VersionControl);
            vm.StartedOn.Should().Be(serviceProject.StartedOn);
            vm.SelectProductOwnerEmailAddress.Should().Be(serviceProject.ProductOwnerEmailAddress);
            vm.SelectedScrumMasters.Count.Should().Be(2);

            ServiceProject sentProject = null;

            gateway.UpdateProject(Arg.Do<ServiceProject>(n => sentProject = n));

            vm.Call("SaveProject");

            sentProject.Should().NotBeNull();
            sentProject.Description.Should().Be(vm.Description);
            sentProject.Name.Should().Be(vm.ProjectName);
            sentProject.VersionControl.Should().Be(vm.VersionControl);
            sentProject.StartedOn.Should().Be(vm.StartedOn);
            sentProject.ProductOwnerEmailAddress.Should().Be(vm.SelectProductOwnerEmailAddress);
            sentProject.ProjectManagerEmailAddress.Should().Be("testmanager@agile.local");
            gateway.Received().UpdateProject(sentProject);
            windowProvider.Received().ShowMessageAsync("Project Updated", "The {0} project has been updated successfully.".Substitute(vm.ProjectName), MessageDialogStyle.Affirmative);
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "MyProjects");
        }

        [Test]
        public void SaveProjectWithErrors()
        {
            var currentPrincipal = new ServiceUserPrincipal(new ServiceLoginResult { EmailAddress = "testmanager@agile.local" }, "");

            UserDetailsStore.LoggedInUserPrincipal = currentPrincipal;
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;

            vm.ProjectName = "Mi";
            vm.Description = "This is a description";
            vm.VersionControl = "V4";
            vm.StartedOn = new DateTime(30, 09, 15);
            vm.SelectProductOwnerEmailAddress = "test@agile.local";

            vm.Call("SaveProject");

            gateway.DidNotReceive().CreateProject(Arg.Any<ServiceProject>());
            windowProvider.Received().ShowMessageAsync("Project was not Saved", "The project could not be saved. There are errors on the page. Please fix them and try again.", MessageDialogStyle.Affirmative);
        }

        [Test]
        public void SaveProjectNoScrumMasterOrPo()
        {
            var windowProvider = Substitute.For<WindowProvider>();

            var currentPrincipal = new ServiceUserPrincipal(new ServiceLoginResult { EmailAddress = "testmanager@agile.local" }, "");

            UserDetailsStore.LoggedInUserPrincipal = currentPrincipal;
            vm.MainWindow = windowProvider;

            vm.ProjectName = "Project name";
            vm.Description = "This is a description";
            vm.VersionControl = "V4";
            vm.StartedOn = new DateTime(30, 09, 15);
            vm.SelectProductOwnerEmailAddress = "test@agile.local";

            vm.Call("SaveProject");

            gateway.Received().CreateProject(Arg.Any<ServiceProject>());
             windowProvider.Received().ShowMessageAsync("Project created", "The {0} project has been created sucessfully.".Substitute(vm.ProjectName), MessageDialogStyle.Affirmative);
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "MyProjects");
        }

        #endregion

        #region SearchScrumMasters() Tests 

        [Test]
        public void TestSearchScrumMasters()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            var serviceResult = new List<ServiceUserSearchInfo>()
            {
                new ServiceUserSearchInfo()
            };

            vm.MainWindow = windowProvider;
            ServiceUserSearchFilter filter = null;

            gateway.SearchUsers(Arg.Do<ServiceUserSearchFilter>(f => filter = f)).Returns(serviceResult);

            vm.ScrumMasterSearchEmailAddress = "test@agile.local";

            vm.Call("SearchScrumMasters");

            filter.Should().NotBeNull();
            filter.ScrumMaster.Should().BeTrue();
            filter.EmailAddress.Should().Be(vm.ScrumMasterSearchEmailAddress);
            gateway.Received().SearchUsers(filter);
            vm.ScrumMasterSearchUserResults.Count.Should().Be(1);
            vm.EnableScrumMasterSearchGrid.Should().BeTrue();
        }

        [Test]
        public void TestSearchScrumMastersNoResults()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            var serviceResult = new List<ServiceUserSearchInfo>();
            vm.MainWindow = windowProvider;
            ServiceUserSearchFilter filter = null;

            gateway.SearchUsers(Arg.Do<ServiceUserSearchFilter>(f => filter = f)).Returns(serviceResult);

            vm.ScrumMasterSearchEmailAddress = "test@agile.local";

            vm.Call("SearchScrumMasters");

            filter.Should().NotBeNull();
            filter.ScrumMaster.Should().BeTrue();
            filter.EmailAddress.Should().Be(vm.ScrumMasterSearchEmailAddress);
            gateway.Received().SearchUsers(filter);
            vm.ScrumMasterSearchUserResults.Count.Should().Be(0);
            windowProvider.Received().ShowMessageAsync("No Results Found", "No Scrum masters with the email address {0} have been found.".Substitute(vm.ScrumMasterSearchEmailAddress).Substitute(vm.ProjectName), MessageDialogStyle.Affirmative);
            vm.EnableScrumMasterSearchGrid.Should().BeTrue();
        }

        #endregion 

        #region CanAddScrumMaster(ServiceUserSearchInfo selectedScrumMaster) Tests

        [Test]
        public void CanAddScrumMasterWithScrumMasterNotInResults()
        {
            var currentResults = new List<ServiceUserSearchInfo>();
            vm.SelectedScrumMasters = new ObservableCollection<ServiceUserSearchInfo>(currentResults);

            var newScrumMaster = new ServiceUserSearchInfo { EmailAddress = "test@agile.local" };

            var result = vm.Call<bool>("CanAddScrumMaster", newScrumMaster);

           result.Should().BeTrue();

        }

        [Test]
        public void CanAddScrumMasterWithScrumMasterInResults()
        {
            var currentResults = new List<ServiceUserSearchInfo>()
            {
                new ServiceUserSearchInfo() { EmailAddress = "test@agile.local"}
            };

            vm.SelectedScrumMasters = new ObservableCollection<ServiceUserSearchInfo>(currentResults);

            var newScrumMaster = new ServiceUserSearchInfo { EmailAddress = "test@agile.local" };

            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;

            var result = vm.Call<bool>("CanAddScrumMaster", newScrumMaster);

            result.Should().BeFalse();
            windowProvider.Received().ShowMessageAsync("Unable to add this scrum master to the selected scrum masters!", "The selected scrum master has already been added as a scrum master for this project.");

        }

        #endregion 

        #region AddScrumMaster(ServiceUserSearchInfo selectedScrumMaster) Tests 

        [Test]
        public void CallAddScrumMaster()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;

            var addScrumMaster = new ServiceUserSearchInfo { EmailAddress = "test@agile.local", FullName = "John"};

            vm.ScrumMasterSearchUserResults = new ObservableCollection<ServiceUserSearchInfo>()
            {
                new ServiceUserSearchInfo()
            };

            vm.Call("AddScrumMaster", addScrumMaster);

            vm.SelectedScrumMasters.Should().Contain(addScrumMaster);
            vm.ScrumMasterSearchUserResults.Should().BeEmpty();
            vm.EnableScrumMasterSearchGrid.Should().BeFalse();
            vm.MainWindow.Received().ShowMessageAsync("Scrum Master Assigned", "{0} has been assigned the role of Scrum Master on the project.".Substitute(addScrumMaster.FullName));

        }

        #endregion 

        #region CancelProject() Tests

        [Test]
        public void CancelProject()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;
            NavigationParameters sentParams = null;
            var regionManager = Substitute.For<IRegionManager>();
            regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.RegionManager = regionManager;

            var waitObject = new ManualResetEvent(false);

            vm.MainWindow = Substitute.For<WindowProvider>();
            vm.MainWindow.ShowMessageAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<MessageDialogStyle>()).Returns(MessageDialogResult.Affirmative);
            vm.MainWindow.InvokeAction(Arg.Do<Action>(a => { a(); waitObject.Set(); }));

            vm.Call("CancelProject");

            waitObject.WaitOne();

            vm.MainWindow.Received().ShowMessageAsync("Project Creation Cancellation", "Are you sure you want to cancel? All data will be lost.", MessageDialogStyle.AffirmativeAndNegative);
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "MyProjects");
        }

        [Test]
        public void CancelledCancelProject()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;
            NavigationParameters sentParams = null;
            var regionManager = Substitute.For<IRegionManager>();
            regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.RegionManager = regionManager;

            var waitObject = new ManualResetEvent(false);

            vm.MainWindow = Substitute.For<WindowProvider>();
            vm.MainWindow.ShowMessageAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<MessageDialogStyle>()).Returns(MessageDialogResult.Negative);
            vm.MainWindow.InvokeAction(Arg.Do<Action>(a => { a(); waitObject.Set(); }));

            vm.Call("CancelProject");

            waitObject.WaitOne();

            vm.MainWindow.Received().ShowMessageAsync("Project Creation Cancellation", "Are you sure you want to cancel? All data will be lost.", MessageDialogStyle.AffirmativeAndNegative);
            regionManager.DidNotReceive().RequestNavigate(RegionNames.MainRegion, "MyProjects");
        }

        [Test]
        public void CancelProjectEditMode()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            vm.MainWindow = windowProvider;
            NavigationParameters sentParams = null;
            var regionManager = Substitute.For<IRegionManager>();
            regionManager.RequestNavigate(Arg.Any<string>(), Arg.Any<string>(), Arg.Do<NavigationParameters>(np => sentParams = np));
            vm.RegionManager = regionManager;

            #region Test Data

            ServiceProject serviceProject = new ServiceProject
            {
                Name = "Project",
                Description = "This is a new project",
                VersionControl = "queens.git",
                ProjectManagerEmailAddress = "tester@agile.local",
                ProductOwnerEmailAddress = "tester@agile.local",
                StartedOn = DateTime.Today
            };

            serviceProject.ScrumMasterEmailAddresses = new List<string>();
            serviceProject.ScrumMasterEmailAddresses.Add("S1@test.com");
            serviceProject.ScrumMasterEmailAddresses.Add("S2@test.com");

            var serviceResult = new List<ServiceUserSearchInfo>()
            {
                new ServiceUserSearchInfo()
            };
            ServiceUserSearchFilter filter = null;
            gateway.SearchUsers(Arg.Do<ServiceUserSearchFilter>(f => filter = f)).Returns(serviceResult);

            #endregion

            var context = new NavigationContext(Substitute.For<IRegionNavigationService>(), new Uri("MyProjects", UriKind.Relative));
            context.Parameters.Add("ProjectId", new Guid("11111111-1111-1111-1111-111111111111"));
            context.Parameters.Add("EditMode", "true");

            gateway.GetProject(new Guid("11111111-1111-1111-1111-111111111111")).Returns(serviceProject);

            vm.OnNavigatedTo(context);

            var waitObject = new ManualResetEvent(false);

            vm.MainWindow = Substitute.For<WindowProvider>();
            vm.MainWindow.ShowMessageAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<MessageDialogStyle>()).Returns(MessageDialogResult.Affirmative);
            vm.MainWindow.InvokeAction(Arg.Do<Action>(a => { a(); waitObject.Set(); }));

            vm.Call("CancelProject");

            waitObject.WaitOne();

            vm.MainWindow.Received().ShowMessageAsync("Project Edit Cancellation", "Are you sure you want to cancel? All changes will be lost.", MessageDialogStyle.AffirmativeAndNegative);
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "MyProjects");
        }

        #endregion

        #region RemoveScrumMaster() Tests

        [Test]
        public void RemoveScrumMaster()
        {
            vm.SelectedScrumMasters = new ObservableCollection<ServiceUserSearchInfo>();
            vm.SelectedScrumMasters.Add(new ServiceUserSearchInfo(){FullName = "paul"});
            var sm = new ServiceUserSearchInfo {FullName = "phill"};
            vm.SelectedScrumMasters.Add(sm);

            var waitObject = new ManualResetEvent(false);
            vm.MainWindow = Substitute.For<WindowProvider>();
            vm.MainWindow.ShowMessageAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<MessageDialogStyle>()).Returns(MessageDialogResult.Affirmative);
            vm.MainWindow.InvokeAction(Arg.Do<Action>(a => { a(); waitObject.Set(); }));

            vm.Call("RemoveScrumMaster", sm);

            waitObject.WaitOne();
            vm.MainWindow.Received().ShowMessageAsync("Scrum Master Removal", "Are you sure you want to remove {0} as a scrum master on the project?".Substitute(sm.FullName), MessageDialogStyle.AffirmativeAndNegative);
            vm.SelectedScrumMasters.Count().Should().Be(1);
        }

        [Test]
        public void RemoveScrumMasterCancel()
        {
            vm.SelectedScrumMasters = new ObservableCollection<ServiceUserSearchInfo>();
            vm.SelectedScrumMasters.Add(new ServiceUserSearchInfo() { FullName = "paul" });
            var sm = new ServiceUserSearchInfo { FullName = "phill" };
            vm.SelectedScrumMasters.Add(sm);

            var waitObject = new ManualResetEvent(false);
            vm.MainWindow = Substitute.For<WindowProvider>();
            vm.MainWindow.ShowMessageAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<MessageDialogStyle>()).Returns(MessageDialogResult.Negative);
            vm.MainWindow.InvokeAction(Arg.Do<Action>(a => { a(); waitObject.Set(); }));

            vm.Call("RemoveScrumMaster", sm);

            waitObject.WaitOne();

            vm.MainWindow.Received().ShowMessageAsync("Scrum Master Removal", "Are you sure you want to remove {0} as a scrum master on the project?".Substitute(sm.FullName), MessageDialogStyle.AffirmativeAndNegative);
            vm.SelectedScrumMasters.Count().Should().Be(2);
        }

        #endregion
    }

    #endregion 
}
