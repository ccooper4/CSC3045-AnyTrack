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
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.ServiceGateways;
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
    public class MyProjectsViewModel : BaseViewModel, INavigationAware
    {
        #region Fields

        /// <summary>
        /// The list of project tiles
        /// </summary>
        private List<ServiceProjectRoleSummary> tiles;

        /// <summary>
        /// Value to store whether projects there are no projects for active user
        /// </summary>
        private bool emptyProject;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Create Project View Model
        /// </summary>
        /// <param name="iProjectServiceGateway">The project service gateway</param>
        public MyProjectsViewModel(IProjectServiceGateway iProjectServiceGateway)
            : base(iProjectServiceGateway)
        {
            this.Tiles = ServiceGateway.GetLoggedInUserProjectRoleSummaries(UserDetailsStore.LoggedInUserPrincipal.Identity.Name);

            emptyProject = this.Tiles.Count == 0;

            CreateProjectCommand = new DelegateCommand(AddProjectView);
            ViewProjectOptions = new DelegateCommand<ServiceProjectRoleSummary>(ShowProjectOptions);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Project Tiles for the active user
        /// </summary>
        public List<ServiceProjectRoleSummary> Tiles
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

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets create project command
        /// </summary>
        public DelegateCommand CreateProjectCommand { get; set; }

        /// <summary>
        /// Gets or sets manage backlog command
        /// </summary>
        public DelegateCommand<ServiceProjectRoleSummary> ViewProjectOptions { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the event where this view is the navigation target. 
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns>A true or false value indicating if this is the navigation target.</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        /// <summary>
        /// Handles on navigated from.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// Handles this view's on navigated to.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("openProjectOptions"))
            {
                bool openProjectOptions;
                bool.TryParse(navigationContext.Parameters["openProjectOptions"].ToString(), out openProjectOptions);
                if (openProjectOptions)
                {
                    if (navigationContext.Parameters.ContainsKey("projectInfo"))
                    {
                        ShowProjectOptions(navigationContext.Parameters["projectInfo"] as ServiceProjectRoleSummary);
                    }
                }
            }
        }

        /// <summary>
        /// Add A Project
        /// </summary>
        private void AddProjectView()
        {
            NavigateToItem("Project");
        }

        /// <summary>
        /// Shows the project options.
        /// </summary>
        /// <param name="summary">The project summary.</param>
        private void ShowProjectOptions(ServiceProjectRoleSummary summary)
        {
            var navParams = new NavigationParameters();
            navParams.Add("projectInfo", summary);
            this.ShowMetroFlyout("ProjectOptions", navParams);
        }

        #endregion
    }
}