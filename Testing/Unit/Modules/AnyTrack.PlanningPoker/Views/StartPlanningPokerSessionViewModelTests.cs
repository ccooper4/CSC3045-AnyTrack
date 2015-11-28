using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using NSubstitute;
using AnyTrack.PlanningPoker.Views;
using AnyTrack.PlanningPoker.ServiceGateways;
using Prism.Regions;
using AnyTrack.Infrastructure.ServiceGateways;

namespace Unit.Modules.AnyTrack.PlanningPoker.Views.StartPlanningPokerSessionViewModelTests
{
    #region Setup

    public class Context
    {
        public static IPlanningPokerManagerServiceGateway serviceGateway;
        public static IProjectServiceGateway projectServiceGateway; 
        public static StartPlanningPokerSessionViewModel vm;

        [SetUp]
        public void SetUp()
        {
            serviceGateway = Substitute.For<IPlanningPokerManagerServiceGateway>();
            projectServiceGateway = Substitute.For<IProjectServiceGateway>();
            vm = new StartPlanningPokerSessionViewModel(serviceGateway, projectServiceGateway);
        }
    }

    #endregion 

    #region Tests 

    public class StartPlanningPokerSessionViewModelTests : Context
    {
        #region Constructor Tests 

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoService()
        {
            vm = new StartPlanningPokerSessionViewModel(null, projectServiceGateway);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoProjectService()
        {
            vm = new StartPlanningPokerSessionViewModel(serviceGateway, null);
        }

        [Test]
        public void ConstructViewModel()
        {
            vm = new StartPlanningPokerSessionViewModel(serviceGateway, projectServiceGateway);
            projectServiceGateway.Received().GetProjectNames(true, false, false);
        }

        #endregion 

        #region IsNavigationTarget(NavigationContext navigationContext) Tests 

        [Test]
        public void CallIsNavigationTarget()
        {
            var navService = Substitute.For<IRegionNavigationService>();
            var navContext = new NavigationContext(navService, new Uri("Test", UriKind.Relative));

            var res = vm.IsNavigationTarget(navContext);

            res.Should().BeFalse();
        }

        #endregion 
    }

    #endregion 
}
