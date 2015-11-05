using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Windows;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Projects.BackendProjectService;
using AnyTrack.Projects.ServiceGateways;
using AnyTrack.SharedUtilities.Extensions;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Unity;
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
        /// The search email address for the product owner.
        /// </summary>
        private string productOwnerSearchEmailAddress; 

        /// <summary>
        /// The email address of the selected product owner.
        /// </summary>
        private string selectProductOwnerEmailAddress;

        /// <summary>
        /// Retrieves a flag indicating if the PO search grid is enabled.
        /// </summary>
        private bool enablePoSearchGrid;

        /// <summary>
        /// Retrieves a flag indicating if the PO has been confirmed.
        /// </summary>
        private bool productOwnerConfirmed;

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

            saveProjectCommand = new DelegateCommand(SaveProject, CanSave);
            cancelProjectCommand = new DelegateCommand(CancelProject);
            SearchPOUserCommand = new DelegateCommand(SearchProjectOwners);
            SetProductOwnerCommand = new DelegateCommand<string>(SetProductOwner);

            startedOn = DateTime.Now;

            POSearchUserResults = new ObservableCollection<UserSearchInfo>();
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

        /// <summary>
        /// Gets or sets the PO search email address.
        /// </summary>
        public string ProductOwnerSearchEmailAddress
        {
            get { return productOwnerSearchEmailAddress; }
            set { SetProperty(ref productOwnerSearchEmailAddress, value); }
        }

        /// <summary>
        /// Gets or sets the selected product owner's email address.
        /// </summary>
        public string SelectProductOwnerEmailAddress
        {
            get { return selectProductOwnerEmailAddress; }
            set { SetProperty(ref selectProductOwnerEmailAddress, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the PO search grid is enabled.
        /// </summary>
        public bool EnablePoSearchGrid
        {
            get { return enablePoSearchGrid; }
            set { SetProperty(ref enablePoSearchGrid, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the PO has been confirmed.
        /// </summary>
        public bool POConfirmed
        {
            get { return productOwnerConfirmed; }
            set { SetProperty(ref productOwnerConfirmed, value); }
        }

        /// <summary>
        /// Gets or sets the observable collection that stores the PO search results.
        /// </summary>
        public ObservableCollection<UserSearchInfo> POSearchUserResults { get; set; }

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

        /// <summary>
        /// Gets or sets the command that can be used to search for a PO.
        /// </summary>
        public DelegateCommand SearchPOUserCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that can be used to set a product owner on a new project.
        /// </summary>
        public DelegateCommand<string> SetProductOwnerCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This is a method to cancel the project wizard
        /// </summary>
        public void CancelProject()
        {
        }

        /// <summary>
        /// This is the method to save a project from the view
        /// </summary>
        private void SaveProject()
        {
            ServiceProject project = new ServiceProject
            {
                Name = this.ProjectName,
                Description = this.Description,
                VersionControl = this.VersionControl,
                StartedOn = this.StartedOn,
                ProductOwnerEmailAddress = selectProductOwnerEmailAddress,
                ProjectManagerEmailAddress = this.LoggedInUserPrincipal.Identity.Name,
            };

            serviceGateway.CreateProject(project);
            ShowMetroDialog("Project created", "The {0} project has successfully been created".Substitute(this.ProjectName), MessageDialogStyle.Affirmative);
        }

        /// <summary>
        /// This is a method that checks for validation errors and retruns the result of
        /// whether a save can be made
        /// </summary>
        /// <returns>bool of whether a save can be made</returns>
        private bool CanSave()
        {
            return !base.HasErrors;
        }

        /// <summary>
        /// Searches for a project owner using the details entered by the user.
        /// </summary>
        private void SearchProjectOwners()
        {
            var filter = new UserSearchFilter { EmailAddress = productOwnerSearchEmailAddress, ProductOwner = true };
            var results = serviceGateway.SearchUsers(filter);

            POSearchUserResults.Clear();
            POSearchUserResults.AddRange(results);
            EnablePoSearchGrid = true;
            POConfirmed = false;
        }

        /// <summary>
        /// Sets the product owner of this project to the user with the specified email address.
        /// </summary>
        /// <param name="emailAddress">The email address of the user to be set as the Product Owner.</param>
        private void SetProductOwner(string emailAddress)
        {
            SelectProductOwnerEmailAddress = emailAddress;
            POSearchUserResults.Clear();
            EnablePoSearchGrid = false;
            POConfirmed = true;
            ProductOwnerSearchEmailAddress = string.Empty;
            ShowMetroDialog("Product Owner confirmed", "The product owner has been successfully set to user - {0}".Substitute(emailAddress), MessageDialogStyle.Affirmative);
        }

        #endregion
    }
}
