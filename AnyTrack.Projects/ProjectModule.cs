using System;
using AnyTrack.Infrastructure;
using AnyTrack.Projects.BackendProjectService;
using AnyTrack.Projects.ServiceGateways;
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

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs the overall Project Module. 
        /// </summary>
        /// <param name="container">The Unity container</param>
        /// <param name="regionManager">The Prism region manager</param>
        public ProjectModule(IUnityContainer container, IRegionManager regionManager)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }

            this.container = container;
            this.regionManager = regionManager;
        }

        #endregion

        #region Methods 

        /// <summary>
        /// Performs any neccessary logic to initalize the module. 
        /// </summary>
        public void Initialize()
        {
            // regionManager.RequestNavigate(RegionNames.AppContainer, )
            container.RegisterType<IProjectService, ProjectServiceClient>(new InjectionConstructor());

            container.RegisterType<IProjectServiceGateway, ProjectServiceGateway>();

            container.RegisterType<object, CreateProject>("Project");

            regionManager.RequestNavigate(RegionNames.AppContainer, "Project");
        }

        #endregion 
    }
}
