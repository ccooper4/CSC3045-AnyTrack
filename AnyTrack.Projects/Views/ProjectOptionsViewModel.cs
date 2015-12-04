using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.SharedUtilities.Extensions;
using MahApps.Metro.Controls;
using Prism.Commands;
using Prism.Regions;

namespace AnyTrack.Projects.Views
{
    /// <summary>
    /// The project options view model.
    /// </summary>
    public class ProjectOptionsViewModel : ValidatedBindableBase, IFlyoutCompatibleViewModel, INavigationAware
    {
        #region Fields

        /// <summary>
        /// The is open field. 
        /// </summary>
        private bool isOpen;

        /// <summary>
        /// The header field.
        /// </summary>
        private string header;

        /// <summary>
        /// The position of the flyout. 
        /// </summary>
        private Position position;

        /// <summary>
        /// The flyout theme.
        /// </summary>
        private FlyoutTheme theme;

        /// <summary>
        /// The is modal field.
        /// </summary>
        private bool isModal;

        /// <summary>
        /// The close button visibility field.
        /// </summary>
        private Visibility closeButtonVisibility;

        /// <summary>
        /// The close button visibility field.
        /// </summary>
        private Visibility titleVisibility;

        /// <summary>
        /// The project name field.
        /// </summary>
        private string projectName;

        /// <summary>
        /// The project description field.
        /// </summary>
        private string projectDescription;

        /// <summary>
        /// The project id field.
        /// </summary>
        private Guid projectId;

        /// <summary>
        /// Indicates whether user is peoject manager of the project.
        /// </summary>
        private bool projectManager;

        #endregion 

        #region Constructor 

        /// <summary>
        /// Constructs a new instance of the project options view model.
        /// </summary>
        public ProjectOptionsViewModel()
        {
            this.Header = "Project Options";
            this.IsModal = true;
            this.Position = Position.Right;
            this.Theme = FlyoutTheme.Accent;

            ViewBacklog = new DelegateCommand(GoToBacklog);
            ViewEditProject = new DelegateCommand(GoToEditProject);
        }

        #endregion 

        #region Properties

        /// <summary>
        /// Gets or sets close button visibility
        /// </summary>
        public Visibility CloseButtonVisibility
        {
            get
            {
                return closeButtonVisibility;
            }

            set
            {
                SetProperty(ref closeButtonVisibility, value);
            }
        }

        /// <summary>
        /// Gets or sets the title visibility
        /// </summary>
        public Visibility TitleVisibility
        {
            get
            {
                return titleVisibility;
            }

            set
            {
                SetProperty(ref titleVisibility, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this flyout is open.
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return isOpen;
            }

            set
            {
                SetProperty(ref isOpen, value);
            }
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public string Header
        {
            get
            {
                return header;
            }

            set
            {
                SetProperty(ref header, value);
            }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Position Position
        {
            get
            {
                return position;
            }

            set
            {
                SetProperty(ref position, value);
            }
        }

        /// <summary>
        /// Gets or sets the flyout theme.
        /// </summary>
        public FlyoutTheme Theme
        {
            get
            {
                return theme;
            }

            set
            {
                SetProperty(ref theme, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this flyout is a modal.
        /// </summary>
        public bool IsModal
        {
            get
            {
                return isModal;
            }

            set
            {
                SetProperty(ref isModal, value);
            }
        }

        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        public string ProjectName
        {
            get
            {
                return projectName;
            }

            set
            {
                SetProperty(ref projectName, value);
            }
        }

        /// <summary>
        /// Gets or sets the project description.
        /// </summary>
        public string ProjectDescription
        {
            get
            {
                return projectDescription;
            }

            set
            {
                SetProperty(ref projectDescription, value);
            }
        }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public Guid ProjectId
        {
            get
            {
                return projectId;
            }

            set
            {
                SetProperty(ref projectId, value);
            }
        }

        /// <summary>
        /// Gets or sets the command used to view the product backlog.
        /// </summary>
        public DelegateCommand ViewBacklog { get; set; }

        /// <summary>
        /// Gets or sets the command used to view the Edit Project screen.
        /// </summary>
        public DelegateCommand ViewEditProject { get; set; }

        #endregion 

        #region Methods

        /// <summary>
        /// Allows this flyout to return a value indicating if it can be re-used.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns>A true or false value indicating if this flyout can be re-used.</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false; 
        }

        /// <summary>
        /// Allows this flyout to handle any on navigated from logic. 
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// Allows this flyout to run any navigate to logic.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("projectInfo"))
            {
                var projectInfo = navigationContext.Parameters["projectInfo"] as ServiceProjectRoleSummary;
                this.ProjectId = (Guid)projectInfo.ProjectId;
                this.Header = "Project Options - {0}".Substitute(projectInfo.Name);
                this.ProjectDescription = projectInfo.Description;
                this.ProjectName = projectInfo.Name;
                this.projectManager = projectInfo.ProjectManager;
            }
            else 
            {
                this.IsOpen = false;
            }
        }

        /// <summary>
        /// Navigates to the backlog view. 
        /// </summary>
        private void GoToBacklog()
        {
            this.IsOpen = false;
            var navParams = new NavigationParameters();
            navParams.Add("projectId", (Guid)projectId);
            this.NavigateToItem("ProductBacklog", navParams);
        }

        /// <summary>
        /// Naviagtes to Edit projects and sends required parameteres.
        /// </summary>
        private void GoToEditProject()
        {
            this.IsOpen = false;
            var navParams = new NavigationParameters();
            navParams.Add("ProjectId", (Guid)projectId);
            navParams.Add("EditMode", "true");
            this.NavigateToItem("Project", navParams);
            this.IsOpen = false;
        }

        #endregion
    }
}
