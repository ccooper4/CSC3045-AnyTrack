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
using AnyTrack.Infrastructure.Security;
using AnyTrack.Infrastructure.BackendAccountService;
using MahApps.Metro.Controls.Dialogs;
using AnyTrack.SharedUtilities.Extensions;
using System.Security.Principal;

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

            vm = new CreateProjectViewModel(regionManager, gateway);
        }
    }

    #endregion 

    #region Tests 

    public class CreateProjectViewModelTests: Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructNoRegionManager()
        {
            vm = new CreateProjectViewModel(null, gateway); 
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructNoGateway()
        {
            vm = new CreateProjectViewModel(regionManager, null);
        }

        [Test]
        public void ConstructVm()
        {
            vm = new CreateProjectViewModel(regionManager, gateway);
            vm.SaveProjectCommand.Should().NotBeNull();
            vm.CancelProjectCommand.Should().NotBeNull();
            vm.SetProductOwnerCommand.Should().NotBeNull();
            vm.POSearchUserResults.Should().NotBeNull();
            vm.StartedOn.ToString("d").Should().Be(DateTime.Now.ToString("d"));
        }

        #endregion 

        #region SearchProjectOwners() Tests 

        [Test]
        public void SearchProjectOwners()
        {
            vm.ProductOwnerSearchEmailAddress = "test@agile.com";
            UserSearchFilter sentFilter = null;

            var gatewayResponse = new List<UserSearchInfo>()
            {
                new UserSearchInfo()
            };

            gateway.SearchUsers(Arg.Do<UserSearchFilter>(f => sentFilter = f)).Returns(gatewayResponse);

            vm.Call("SearchProjectOwners");
            sentFilter.Should().NotBeNull();
            sentFilter.EmailAddress.Should().Be(vm.ProductOwnerSearchEmailAddress);
            sentFilter.ProductOwner.Should().BeTrue();
            sentFilter.ScrumMaster.HasValue.Should().BeFalse();
            gateway.Received().SearchUsers(sentFilter);

            vm.POSearchUserResults.Should().Contain(gatewayResponse);
            vm.EnablePoSearchGrid.Should().BeTrue();
            vm.POConfirmed.Should().BeFalse();

        }

        #endregion 

        #region .SetProductOwner(string emailAddress) Tests 

        [Test]
        public void SetProductOwnerTests()
        {
            var windowProvider = Substitute.For<WindowProvider>();
            var emailAddress = "test@agile.local"; 
            vm.MainWindow = windowProvider;

            vm.POSearchUserResults = new ObservableCollection<UserSearchInfo>()
            {
                new UserSearchInfo()
            };

            vm.Call("SetProductOwner", emailAddress);
            vm.SelectProductOwnerEmailAddress.Should().Be(emailAddress);
            vm.POSearchUserResults.Count.Should().Be(0);
            vm.EnablePoSearchGrid.Should().BeFalse();
            vm.POConfirmed.Should().BeTrue();
            vm.ProductOwnerSearchEmailAddress.Should().Be(string.Empty);
            windowProvider.Received().ShowMessageAsync("Product Owner confirmed", "The product owner has been successfully set to user - " + emailAddress, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative, null);

        }

        #endregion 

        #region CreateAProject Test
        [Test]
        public void SaveProject()
        {
            var windowProvider = Substitute.For<WindowProvider>();

            var currentPrincipal = new ServiceUserPrincipal(new LoginResult { EmailAddress = "testmanager@agile.local" }, "");

            vm.LoggedInUserPrincipal = currentPrincipal;
            vm.MainWindow = windowProvider;

            vm.ProjectName = "Test Project";
            vm.Description = "This is a description";
            vm.VersionControl = "V4";
            vm.StartedOn = new DateTime(30, 09, 15);
            vm.SelectProductOwnerEmailAddress = "test@agile.local";

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

        }
        #endregion

        #region CanSave Test 

        [Test]
        public void CallCanSaveWithNoErrors()
        {
            var result = vm.Call<bool>("CanSave");
            result.Should().BeTrue();
        }

        [Test]
        public void CallCanSaveWithErrors()
        {
            vm.ProjectName = "";
            var result = vm.Call<bool>("CanSave");
            result.Should().BeFalse();
        }

        #endregion 
    }

    #endregion 
}
