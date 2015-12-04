using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure.ServiceGateways;
using AnyTrack.Projects.Views;
using AnyTrack.Sprints.Views;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Prism.Regions;

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
    }
}
