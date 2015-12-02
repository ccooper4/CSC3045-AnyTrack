using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendSprintService;
using AnyTrack.Infrastructure.Service;
using AnyTrack.Infrastructure.Service.Model;
using AnyTrack.Sprints.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using MenuItem = AnyTrack.Infrastructure.Service.Model.MenuItem;

namespace AnyTrack.Sprints
{
    /// <summary>
    /// Provides init logic for the sprint module. 
    /// </summary>
    public class SprintModule : IModule
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
        /// Constructs the overall Sprint Module. 
        /// </summary>
        /// <param name="container">The Unity container</param>
        /// <param name="regionManager">The Prism region manager</param>
        /// <param name="menuService">The menu service.</param>
        public SprintModule(IUnityContainer container, IRegionManager regionManager, IMenuService menuService)
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
        /// Performs any neccessary logic to initalize the module. 
        /// </summary>
        public void Initialize()
        {          
            container.RegisterType<object, CreateSprint>("CreateSprint");
            container.RegisterType<object, SprintManager>("SprintManager");
            container.RegisterType<object, SprintOptions>("SprintOptions");
            container.RegisterType<object, UpdateTaskHours>("UpdateTaskHours");
            container.RegisterType<object, SprintBoard>("SprintBoard");
            container.RegisterType<object, SprintStory>("SprintStory");
            container.RegisterType<object, Views.Task>("Task");
            container.RegisterType<object, ManageSprintBacklog>("ManageSprintBacklog");
            container.RegisterType<object, BurnDown>("BurnDown");

            menuService.AddMenuItem(new MenuItem { Color = "Gray", Title = "Sprints", NavigationViewName = "SprintManager", Icon = NavigationIcons.Sprints });
            menuService.AddMenuItem(new MenuItem { Color = "Gray", Title = "Sprint Board", NavigationViewName = "SprintBoard", Icon = NavigationIcons.SprintBoard });
            menuService.AddMenuItem(new MenuItem { Color = "Gray", Title = "Burndown", NavigationViewName = "BurnDown", Icon = NavigationIcons.Burndown });
        }

        #endregion 
    }
}
