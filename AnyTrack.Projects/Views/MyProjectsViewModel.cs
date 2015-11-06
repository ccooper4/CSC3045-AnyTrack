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
    public class MyProjectsViewModel : ValidatedBindableBase, INavigationAware
    {
        #region Fields

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
        /// <param name="serviceGateway">The project service gateway</param>
        public MyProjectsViewModel(IProjectServiceGateway serviceGateway)
        {
            if (serviceGateway == null)
            {
                throw new ArgumentNullException("serviceGateway");
            }

            this.serviceGateway = serviceGateway;

            this.Tiles = serviceGateway.GetLoggedInUserProjectRoleSummaries(UserDetailsStore.LoggedInUserPrincipal.Identity.Name);

            emptyProject = this.Tiles.Count == 0;

            CreateProjectCommand = new DelegateCommand(AddProjectView);
            ManageBacklogCommand = new DelegateCommand<ProjectRoleSummary>(ManageBacklogView);
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
        /// Gets or sets manage backlog command
        /// </summary>
        public DelegateCommand<ProjectRoleSummary> ManageBacklogCommand { get; set; }

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
        }

        /// <summary>
        /// Add A Project
        /// </summary>
        private void AddProjectView()
        {
            NavigateToItem("Project");
        }

        /// <summary>
        /// Manage Backlog View
        /// </summary>
        /// <param name="summary">The project summary.</param>
        private void ManageBacklogView(ProjectRoleSummary summary)
        {
            var navParams = new NavigationParameters();
            navParams.Add("projectId", summary.ProjectId);
            NavigateToItem("ProductBacklog", navParams);
        }

        #endregion
    }
}