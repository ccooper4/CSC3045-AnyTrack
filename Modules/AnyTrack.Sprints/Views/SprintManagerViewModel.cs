using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
using Prism.Commands;
using Prism.Regions;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// The view model for the Sprint Manager.
    /// </summary>
    public class SprintManagerViewModel : ValidatedBindableBase, INavigationAware
    {
        #region Fields

        /// <summary>
        /// The Project Service Gateway.
        /// </summary>
        private IProjectService serviceGateway;

        /// <summary>
        /// The Project of id of the current project being viewed.
        /// </summary>
        private Guid projectId;

        /// <summary>
        /// The id of the Sprint that is selected.
        /// </summary>
        private Guid sprintId;

        /// <summary>
        /// The currently selected project in the dropdown.
        /// </summary>
        private ServiceProjectRoleSummary selectedProject;

        /// <summary>
        /// The project which sprints are currently showing.
        /// </summary>
        private ServiceProjectRoleSummary currentlyShowingProject;

        /// <summary>
        /// The collection of projects the user is in.
        /// </summary>
        private ObservableCollection<ServiceProjectRoleSummary> projects;

        /// <summary>
        /// The list of sprints displayed.
        /// </summary>
        private ObservableCollection<ServiceSprintSummary> sprints; 

        /// <summary>
        /// Indicates whether the add button show be displayed or not.
        /// </summary>
        private bool showAddButton;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Sprint Manager.
        /// </summary>
        /// <param name="serviceGateway">The Service Gateway</param>
        public SprintManagerViewModel(IProjectService serviceGateway)
        {
            if (serviceGateway == null)
            {
                throw new ArgumentNullException("serviceGateway");
            }

            this.serviceGateway = serviceGateway;

            string userEmail = UserDetailsStore.LoggedInUserPrincipal.Identity.Name;
            Projects = new ObservableCollection<ServiceProjectRoleSummary>(serviceGateway.GetUserProjectRoleSummaries(userEmail));

            AddSprintCommand = new DelegateCommand(GoToCreateSprint);
            UpdateProjectDisplayedCommand = new DelegateCommand(UpdateProjectDisplayed, CanUpdateProject);
            OpenSprintOptions = new DelegateCommand<ServiceSprintSummary>(ShowSprintOptions);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Project Id of the currently viewed Project.
        /// </summary>
        public Guid ProjectId
        {
            get { return projectId; }
            set { SetProperty(ref projectId, value); }
        }

        /// <summary>
        /// Gets or sets the id of the selected sprint.
        /// </summary>
        public Guid SprintId
        {
            get { return sprintId;  }
            set { SetProperty(ref sprintId, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the add sprint button show be displayed.
        /// </summary>
        public bool ShowAddButton
        {
            get { return showAddButton; }
            set { SetProperty(ref showAddButton, value); }
        }

        /// <summary>
        /// Gets or sets the selected project in the dropdown.
        /// </summary>
        public ServiceProjectRoleSummary SelectedProject
        {
            get
            {
                return selectedProject;
            }

            set
            {
                if (selectedProject == null)
                {
                    SetProperty(ref selectedProject, value);
                    UpdateProjectDisplayedCommand.RaiseCanExecuteChanged();
                }

                SetProperty(ref selectedProject, value);
            }
        }

        /// <summary>
        /// Gets or sets the project whos sprints are being shown.
        /// </summary>
        public ServiceProjectRoleSummary CurrentlyShowingProject
        {
            get { return currentlyShowingProject; }
            set { SetProperty(ref currentlyShowingProject, value); }
        }

        /// <summary>
        /// Gets or sets the collection of projects the user is in.
        /// </summary>
        public ObservableCollection<ServiceProjectRoleSummary> Projects
        {
            get { return projects; }
            set { SetProperty(ref projects, value); }
        }

        /// <summary>
        /// Gets or sets the list of sprints displayed.
        /// </summary>
        public ObservableCollection<ServiceSprintSummary> Sprints
        {
            get { return sprints; }
            set { SetProperty(ref sprints, value); }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that can be used to add a sprint.
        /// </summary>
        public DelegateCommand AddSprintCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that can be used to update the displayed project.
        /// </summary>
        public DelegateCommand UpdateProjectDisplayedCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that can be used to display the Sprint Options panel.
        /// </summary>
        public DelegateCommand<ServiceSprintSummary> OpenSprintOptions { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Handles the Is Navigation target event. 
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns>A true or false value indicating if this viewmodel can handle the navigation request.</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        /// <summary>
        /// Handles the on navigated from event. 
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// Handles the navigated to.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("ProjectId"))
            {
                Guid projectId = (Guid)navigationContext.Parameters["ProjectId"];
                ServiceProjectRoleSummary project = Projects.SingleOrDefault(p => p.ProjectId == projectId);
                SelectedProject = project;
                CurrentlyShowingProject = project;
                this.projectId = projectId;
                UpdateProjectDisplayed();
            }
        }

        /// <summary>
        /// Navigates to create a Sprint screen.
        /// </summary>
        private void GoToCreateSprint()
        {
            var navParams = new NavigationParameters();
            navParams.Add("ProjectId", ProjectId);
            NavigateToItem("CreateSprint", navParams);
        }

        /// <summary>
        /// Indicates if the selected project can be updated.
        /// </summary>
        /// <returns>Whether selected roject can be updated</returns>
        private bool CanUpdateProject()
        {
            return SelectedProject != null;
        }

        /// <summary>
        /// Update the sprints displayed on the screen.
        /// </summary>
        private void UpdateProjectDisplayed()
        {
            CurrentlyShowingProject = SelectedProject;
            ProjectId = currentlyShowingProject.ProjectId;
            ShowAddButton = currentlyShowingProject.ScrumMaster;
            Sprints = new ObservableCollection<ServiceSprintSummary>(currentlyShowingProject.Sprints);
        }

          /// <summary>
          /// Shows the Sprint Options Flyout.
          /// </summary>
          /// <param name="sprintSummary">Summary of the selected sprint</param>
        private void ShowSprintOptions(ServiceSprintSummary sprintSummary)
        {
            var navParams = new NavigationParameters();
            navParams.Add("projectRoleInfo", currentlyShowingProject);
            navParams.Add("sprintSummary", sprintSummary);
            this.ShowMetroFlyout("SprintOptions", navParams);
        }

        #endregion
    }
}
