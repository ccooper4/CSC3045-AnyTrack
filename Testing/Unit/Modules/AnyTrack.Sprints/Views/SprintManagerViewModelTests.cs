using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.Security;
using AnyTrack.Infrastructure.ServiceGateways;
using AnyTrack.Projects.Views;
using AnyTrack.Sprints.Views;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using Prism.Regions;

namespace Unit.Modules.AnyTrack.Sprints.Views.SprintManagerViewModelTests
{
    public class Context
    {
        public static IRegionManager regionManager;
        public static IProjectService gateway;
        public static SprintManagerViewModel vm;
        public static List<ServiceProjectRoleSummary> listOfProjects;
            
        [SetUp]
        public static void SetUp()
        {
            regionManager = Substitute.For<IRegionManager>();
            gateway = Substitute.For<IProjectService>();

            listOfProjects = new List<ServiceProjectRoleSummary>()
            {
                new ServiceProjectRoleSummary()
                {
                    ProjectId = new Guid(),
                    Sprints = new List<ServiceSprintSummary>()
                    {
                        new ServiceSprintSummary()
                    }
                }
            };

            UserDetailsStore.LoggedInUserPrincipal = new ServiceUserPrincipal(new ServiceLoginResult { EmailAddress = "test@agile.local" }, "");
            gateway.GetUserProjectRoleSummaries("test@agile.local").Returns(listOfProjects); 

            vm = new SprintManagerViewModel(gateway);
            vm.RegionManager = regionManager;

                    
        }
    }

    public class SprintManagerViewModelTests : Context
    {
        #region Constructor Tests

        [Test]
        public void ConstructSprintManager()
        {
            UserDetailsStore.LoggedInUserPrincipal = new ServiceUserPrincipal(new ServiceLoginResult { EmailAddress = "test@agile.local" }, "");
            gateway.GetUserProjectRoleSummaries("test@agile.local").Returns(listOfProjects);

            vm = new SprintManagerViewModel(gateway);
            vm.RegionManager = regionManager;

            vm = new SprintManagerViewModel(gateway);
            vm.Projects.Should().Contain(listOfProjects);
            vm.AddSprintCommand.Should().NotBeNull();
            vm.UpdateProjectDisplayedCommand.Should().NotBeNull();
            vm.OpenSprintOptionsCommand.Should().NotBeNull();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructSprintManagerNullGateway()
        {
            gateway = null;
            vm = new SprintManagerViewModel(gateway);
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
        public void CallOnNav()
        {
            var projectId = new Guid();

            var context = new NavigationContext(Substitute.For<IRegionNavigationService>(), new Uri("SprintManager", UriKind.Relative));
            context.Parameters.Add("ProjectId", projectId);

            vm.OnNavigatedTo(context);

            vm.SelectedProject.Should().Be(listOfProjects[0]);
            vm.CurrentlyShowingProject.Should().Be(listOfProjects[0]);
        }

        #endregion 
    }
}
