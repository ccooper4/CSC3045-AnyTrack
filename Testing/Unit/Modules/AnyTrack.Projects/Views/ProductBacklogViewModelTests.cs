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

            vm = new ProductBacklogViewModel(regionManager, gateway);
        }
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
        }

        #endregion 
    }

    #endregion 
}
