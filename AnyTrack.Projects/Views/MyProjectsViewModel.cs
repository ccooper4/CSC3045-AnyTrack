using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Projects.BackendProjectService;
using AnyTrack.Projects.ServiceGateways;
using AnyTrack.SharedUtilities.Extensions;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace AnyTrack.Projects.Views
{
    /// <summary>
    /// The view model for the project
    /// </summary>
    public class MyProjectsViewModel : ValidatedBindableBase
    {
        #region Fields

        /// <summary>
        /// The region manager
        /// </summary>
        private readonly IRegionManager regionManager;

        /// <summary>
        /// The project service gateway
        /// </summary>
        private readonly IProjectServiceGateway serviceGateway;

        /// <summary>
        /// The list of project tiles
        /// </summary>
        private List<ProjectRoleSummary> tiles;

        /// <summary>
        /// Value to store whether projects there are no projects for active user
        /// </summary>
        private bool emptyProject;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new Create Project View Model
        /// </summary>
        /// <param name="regionManager">The region manager</param>
        /// <param name="serviceGateway">The project service gateway</param>
        public MyProjectsViewModel(IRegionManager regionManager, IProjectServiceGateway serviceGateway)
        {
            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }

            if (serviceGateway == null)
            {
                throw new ArgumentNullException("serviceGateway");
            }

            this.regionManager = regionManager;
            this.serviceGateway = serviceGateway;

            this.Tiles = serviceGateway.GetLoggedInUserProjectRoleSummaries();
            
            if (this.Tiles.Count == 0)
            {
                emptyProject = true;
            }
            else
            {
                emptyProject = false;
            }

            CreateProjectCommand = new DelegateCommand(AddProjectView);
            ManageProjectCommand = new DelegateCommand(ManageProjectView);
            ManageBacklogCommand = new DelegateCommand<ProjectRoleSummary>(ManageBacklogView);
            ManageSprintsCommand = new DelegateCommand(ManageSprintsView);
            BurndownChartsCommand = new DelegateCommand(BurndownChartsView);
            OfflineModeCommand = new DelegateCommand(OfflineModeView);
        }
        #endregion

        #region Mutators

        /// <summary>
        /// Gets or sets the Project Tiles for the active user
        /// </summary>
        public List<ProjectRoleSummary> Tiles
        {
            get { return tiles; }
            set { SetProperty(ref tiles, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether projects is empty or not
        /// </summary>
        public bool EmptyProject
        {
            get { return emptyProject; }
            set { SetProperty(ref emptyProject, value); }
        }

        /// <summary>
        /// Gets or sets create project command
        /// </summary>
        public DelegateCommand CreateProjectCommand { get; set; }

        /// <summary>
        /// Gets or sets manage project command
        /// </summary>
        public DelegateCommand ManageProjectCommand { get; set; }

        /// <summary>
        /// Gets or sets manage backlog command
        /// </summary>
        public DelegateCommand<ProjectRoleSummary> ManageBacklogCommand { get; set; }

        /// <summary>
        /// Gets or sets manage sprint command
        /// </summary>
        public DelegateCommand ManageSprintsCommand { get; set; }

        /// <summary>
        /// Gets or sets burndown charts command
        /// </summary>
        public DelegateCommand BurndownChartsCommand { get; set; }

        /// <summary>
        /// Gets or sets project settings command
        /// </summary>
        public DelegateCommand ProjectSettingsCommand { get; set; }

        /// <summary>
        /// Gets or sets offline mode command
        /// </summary>
        public DelegateCommand OfflineModeCommand { get; set; }

#endregion

        #region Methods
        /// <summary>
        /// Add A Project
        /// </summary>
        private void AddProjectView()
        {
            regionManager.RequestNavigate(RegionNames.AppContainer, "Project");
        }

        /// <summary>
        /// Manage A Project
        /// </summary>
        private void ManageProjectView()
        {
            ShowMetroDialog("Not Implemented", "This would lead you to the Manage Project View", MessageDialogStyle.Affirmative);
        }

        /// <summary>
        /// Manage Backlog View
        /// </summary>
        /// <param name="summary">The project summary.</param>
        private void ManageBacklogView(ProjectRoleSummary summary)
        {
            var navParams = new NavigationParameters();
            navParams.Add("projectId", summary.ProjectId);
            regionManager.RequestNavigate(RegionNames.MainRegion, "ProductBacklog", navParams);
        }

        /// <summary>
        /// Manage Sprints View
        /// </summary>
        private void ManageSprintsView()
        {
            ShowMetroDialog("Not Implemented", "This would lead you to the ManageSprints View", MessageDialogStyle.Affirmative);
        }

        /// <summary>
        /// Burndown Charts View
        /// </summary>
        private void BurndownChartsView()
        {
            ShowMetroDialog("Not Implemented", "This would lead you to the BurndownCharts View", MessageDialogStyle.Affirmative);
        }

        /// <summary>
        /// Offline Mode View
        /// </summary>
        private void OfflineModeView()
        {
            ShowMetroDialog("Not Implemented", "This would lead you to the Offline Mode View", MessageDialogStyle.Affirmative);
        }
        #endregion
    }
}