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
            windowProvider.Received().ShowMessageAsync("Product Owner confirmed", "The product owner has been successfully set to user - " + emailAddress, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative, null);

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
            windowProvider.Received().ShowMessageAsync("Project created", "The {0} project has successfully been created".Substitute(vm.ProjectName), MessageDialogStyle.Affirmative);
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "MyProjects");

        }

        [Test]
        public void SaveProjectWithErrors()
        {
            var currentPrincipal = new ServiceUserPrincipal(new ServiceLoginResult { EmailAddress = "testmanager@agile.local" }, "");

            UserDetailsStore.LoggedInUserPrincipal = currentPrincipal;

            vm.ProjectName = "Mi";
            vm.Description = "This is a description";
            vm.VersionControl = "V4";
            vm.StartedOn = new DateTime(30, 09, 15);
            vm.SelectProductOwnerEmailAddress = "test@agile.local";

            vm.Call("SaveProject");

            gateway.DidNotReceive().CreateProject(Arg.Any<ServiceProject>());

        }

        [Test]
        public void SaveProjectNoScrumMasterOrPO()
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

            gateway.DidNotReceive().CreateProject(Arg.Any<ServiceProject>());

            windowProvider.Received().ShowMessageAsync("The project cannot be saved!", "This project cannot be saved because both a product owner and at least one scrum master is required.");

        }

        #endregion

        #region SearchScrumMastters() Tests 

        [Test]
        public void TestSearchScrumMasters()
        {
            var serviceResult = new List<ServiceUserSearchInfo>()
            {
                new ServiceUserSearchInfo()
            };
            ServiceUserSearchFilter filter = null;

            gateway.SearchUsers(Arg.Do<ServiceUserSearchFilter>(f => filter = f)).Returns(serviceResult);

            vm.ScrumMasterSearchEmailAddress = "test@agile.local";

            vm.Call("SearchScrumMasters");

            filter.Should().NotBeNull();
            filter.ScrumMaster.Should().BeTrue();
            filter.EmailAddress.Should().Be(vm.ScrumMasterSearchEmailAddress);
            gateway.Received().SearchUsers(filter);
            vm.ScrumMasterSearchUserResults.Should().Contain(serviceResult);
            vm.EnableScrumMasterSearchGrid.Should().BeTrue();
        }

        #endregion 

        #region CanAddScrumMaster(ServiceUserSearchInfo selectedScrumMaster) 

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
            var addScrumMaster = new ServiceUserSearchInfo { EmailAddress = "test@agile.local" };

            vm.ScrumMasterSearchUserResults = new ObservableCollection<ServiceUserSearchInfo>()
            {
                new ServiceUserSearchInfo()
            };

            vm.Call("AddScrumMaster", addScrumMaster);

            vm.SelectedScrumMasters.Should().Contain(addScrumMaster);
            vm.ScrumMasterSearchUserResults.Should().BeEmpty();
            vm.EnableScrumMasterSearchGrid.Should().BeFalse();
        }

        #endregion 

    }

    #endregion 
}
