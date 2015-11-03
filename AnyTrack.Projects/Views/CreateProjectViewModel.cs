using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using AnyTrack.Infrastructure;
using AnyTrack.Projects.BackendProjectService;
using AnyTrack.Projects.ServiceGateways;
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
    public class CreateProjectViewModel : ValidatedBindableBase
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
        /// Project Name
        /// </summary>
        private string projectName;

        /// <summary>
        /// Project Description
        /// </summary>
        private string description;

        /// <summary>
        /// Version Control
        /// </summary>
        private string versionControl;

        /// <summary>
        /// Time Project Started
        /// </summary>
        private DateTime startedOn;

        /// <summary>
        /// Project Manager
        /// </summary>
        private string projectManagerEmailAddress;

        /// <summary>
        /// Command to save a project
        /// </summary>
        private DelegateCommand saveProjectCommand;

        /// <summary>
        /// Command to cancel a project
        /// </summary>
        private DelegateCommand cancelProjectCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Create Project View Model
        /// </summary>
        /// <param name="regionManager">The region manager</param>
        /// <param name="serviceGateway">The project service gateway</param>
        public CreateProjectViewModel(IRegionManager regionManager, IProjectServiceGateway serviceGateway)
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

            saveProjectCommand = new DelegateCommand(SaveProject);
            cancelProjectCommand = new DelegateCommand(CancelProject);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the project name
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A Project Name is Required")]
        [MinLength(3, ErrorMessage = "Project name must be atleast 3 characters")]
        public string ProjectName
        {
            get { return projectName; }
            set { SetProperty(ref projectName, value); }
        }

        /// <summary>
        /// Gets or sets the description for the project
        /// </summary>
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        /// <summary>
        /// Gets or sets the version control for the project
        /// </summary>
        public string VersionControl
        {
            get { return versionControl; }
            set { SetProperty(ref versionControl, value); }
        }

        /// <summary>
        /// Gets or sets the start date for the project
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Project must have start date")]
        public DateTime StartedOn
        {
            get { return startedOn; }
            set { SetProperty(ref startedOn, value); }
        }

        /// <summary>
        /// Gets or sets the project manager of the project
        /// </summary>
        public string ProjectManagerEmailAddress
        {
            get { return projectManagerEmailAddress; }
            set { SetProperty(ref projectManagerEmailAddress, value); }
        }
        #endregion

        #region Commands

        /// <summary>
        /// Gets the project save command
        /// </summary>
        public DelegateCommand SaveProjectCommand
        {
            get { return saveProjectCommand; }
        }

        /// <summary>
        /// Gets the project cancel command
        /// </summary>
        public DelegateCommand CancelProjectCommand
        {
            get { return cancelProjectCommand; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// This is the method to save a project from the view
        /// </summary>
        public void SaveProject()
        {
            ServiceProject project = new ServiceProject
            {
                Name = this.ProjectName,
                Description = this.Description,
                VersionControl = this.VersionControl,
                StartedOn = this.StartedOn
            };

            serviceGateway.CreateProject(project);     
        }

        /// <summary>
        /// This is a method to cancel the project wizard
        /// </summary>
        public void CancelProject()
        {
        }
        #endregion
    }
}
