﻿using System;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.Service;
using AnyTrack.Infrastructure.Service.Model;
using AnyTrack.Projects.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace AnyTrack.Projects
{
    /// <summary>
    /// Configures the Accounting Module. 
    /// </summary>
    public class ProjectModule : IModule
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
        /// Constructs the overall Project Module. 
        /// </summary>
        /// <param name="container">The Unity container</param>
        /// <param name="regionManager">The Prism region manager</param>
        /// <param name="menuService">The menu service.</param>
        public ProjectModule(IUnityContainer container, IRegionManager regionManager, IMenuService menuService)
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
            container.RegisterType<object, ProductBacklog>("ProductBacklog");
            container.RegisterType<object, Story>("Story");
            container.RegisterType<object, CreateProject>("Project");
            container.RegisterType<object, MyProjects>("MyProjects");
            container.RegisterType<object, ProjectOptions>("ProjectOptions");

            menuService.AddMenuItem(new MenuItem { Color = "Gray", Title = "Projects", NavigationViewName = "MyProjects", Icon = NavigationIcons.Projects });
            regionManager.RequestNavigate(RegionNames.MainRegion, "ProductBacklog");
        }

        #endregion 
    }
}
