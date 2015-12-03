using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.Service;
using AnyTrack.Infrastructure.Service.Model;
using AnyTrack.PlanningPoker.BackendPlanningPokerManagerService;
using AnyTrack.PlanningPoker.ServiceGateways;
using AnyTrack.PlanningPoker.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace AnyTrack.PlanningPoker
{
    /// <summary>
    /// The module initalization logic for the planning poker module.
    /// </summary>
    public class PlanningPokerModule : IModule
    {
        #region Fields 

        /// <summary>
        /// The Unity Container. 
        /// </summary>
        private readonly IUnityContainer container;

        /// <summary>
        /// The region Manager.
        /// </summary>
        private readonly IRegionManager regionManager;

        /// <summary>
        /// The menu service.
        /// </summary>
        private readonly IMenuService menuService; 

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs the overall Planning Poker Module. 
        /// </summary>
        /// <param name="container">The Unity container</param>
        /// <param name="regionManager">The Prism region manager</param>
        /// <param name="menuService">The menu service.</param>
        public PlanningPokerModule(IUnityContainer container, IRegionManager regionManager, IMenuService menuService)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }

            if (menuService == null)
            {
                throw new ArgumentNullException("menuService");
            }

            this.container = container;
            this.regionManager = regionManager;
            this.menuService = menuService;
        }

        #endregion

        #region Methods 

        /// <summary>
        /// Runs any setup logic for the planning poker module.
        /// </summary>
        public void Initialize()
        {
            // Service client. 
            container.RegisterType<IPlanningPokerManagerService, PlanningPokerManagerServiceClient>(new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(InstanceContext)));

            // Service gateways.
            container.RegisterType<IPlanningPokerManagerServiceGateway, PlanningPokerManagerServiceGateway>(new ContainerControlledLifetimeManager());

            // Views.
            container.RegisterType<object, StartPlanningPokerSession>("StartPlanningPokerSession");
            container.RegisterType<object, SearchForPlanningPokerSession>("SearchForPlanningPokerSession");
            container.RegisterType<object, PokerLobby>("PokerLobby");
        }

        #endregion 
    }
}
