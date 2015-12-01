﻿using System;
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

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// Sprint Options view model.
    /// </summary>
    public class SprintOptionsViewModel : ValidatedBindableBase, IFlyoutCompatibleViewModel, INavigationAware
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
        /// The Project name field.
        /// </summary>
        private string projectName;

        /// <summary>
        /// The sprint name field.
        /// </summary>
        private string sprintName;

        /// <summary>
        /// The sprint Description.
        /// </summary>
        private string sprintDescription;

        /// <summary>
        /// The project id field.
        /// </summary>
        private Guid projectId;

        /// <summary>
        /// The sprint id field.
        /// </summary>
        private Guid sprintId;

        /// <summary>
        /// Indicates whether the user is a scrum master in the project.
        /// </summary>
        private bool isScrumMaster;

        #endregion 

        #region Constructor 

        /// <summary>
        /// Constructs a new instance of the project options view model.
        /// </summary>
        public SprintOptionsViewModel()
        {
            this.Header = "Sprint Options";
            this.IsModal = true;
            this.Position = Position.Right;
            this.Theme = FlyoutTheme.Accent;
            this.IsOpen = true;
            this.CloseButtonVisibility = Visibility.Hidden;
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
        /// Gets or sets the sprint name.
        /// </summary>
        public string SprintDescription
        {
            get
            {
                return sprintDescription;
            }

            set
            {
                SetProperty(ref sprintDescription, value);
            }
        }

        /// <summary>
        /// Gets or sets the sprint name.
        /// </summary>
        public string SprintName
        {
            get
            {
                return sprintName;
            }

            set
            {
                SetProperty(ref sprintName, value);
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
        /// Gets or sets the sprint id.
        /// </summary>
        public Guid SprintId
        {
            get
            {
                return sprintId;
            }

            set
            {
                SetProperty(ref sprintId, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user is a scrum master of the project.
        /// </summary>
        public bool IsScrumMaster
        {
            get { return isScrumMaster; }
            set { SetProperty(ref isScrumMaster, value); }
        }

        #endregion 

        #region Commands

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
            if (navigationContext.Parameters.ContainsKey("projectRoleInfo"))
            {
                var projectInfo = navigationContext.Parameters["projectRoleInfo"] as ServiceProjectRoleSummary;
                this.ProjectId = projectInfo.ProjectId;
                this.ProjectName = projectInfo.Name;
                this.IsScrumMaster = projectInfo.ScrumMaster;
            }

            if (navigationContext.Parameters.ContainsKey("sprintSummary"))
            {
                var sprintInfo = navigationContext.Parameters["sprintSummary"] as ServiceSprintSummary;
                this.SprintId = sprintInfo.SprintId;
                this.SprintName = sprintInfo.Name;
                this.SprintDescription = sprintInfo.Description;
            }
        }

        #endregion
    }
}
