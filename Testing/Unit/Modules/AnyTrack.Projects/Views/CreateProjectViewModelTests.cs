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
    }

    #endregion 
}
