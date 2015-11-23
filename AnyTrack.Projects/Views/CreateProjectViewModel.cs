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
    public class CreateProjectViewModel : BaseViewModel
    {
        #region Fields

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
        private bool enableProductOwnerSearchGrid;

        /// <summary>
        /// Retrieves a flag indicating if the PO has been confirmed.
        /// </summary>
        private bool productOwnerConfirmed;

        /// <summary>
        /// The scrum master email address. being searched for. 
        /// </summary>
        private string scrumMasterSearchEmailAddress;

        /// <summary>
        /// A flag indicating if the scrum master search grid is enabled.
        /// </summary>
        private bool enableScrumMasterSearchGrid; 

        #endregion
        
        #region Constructor

        /// <summary>
        /// Creates a new Create Project View Model.
        /// </summary>
        /// <param name="iProjectServiceGateway">The project service gateway</param>
        public CreateProjectViewModel(IProjectServiceGateway iProjectServiceGateway)
            : base(iProjectServiceGateway)
        {
            SaveProjectCommand = new DelegateCommand(SaveProject);
            CancelProjectCommand = new DelegateCommand(CancelProject);
            SearchProductOwnerUserCommand = new DelegateCommand(SearchProjectOwners);
            SetProductOwnerCommand = new DelegateCommand<string>(SetProductOwner);
            SearchScrumMasterCommand = new DelegateCommand(SearchScrumMasters);
            SelectScrumMasterCommand = new DelegateCommand<ServiceUserSearchInfo>(AddScrumMaster, CanAddScrumMaster);

            startedOn = DateTime.Now;

            ProductOwnerSearchUserResults = new ObservableCollection<ServiceUserSearchInfo>();
            ScrumMasterSearchUserResults = new ObservableCollection<ServiceUserSearchInfo>();
            SelectedScrumMasters = new ObservableCollection<ServiceUserSearchInfo>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A Project Name is Required")]
        [MinLength(3, ErrorMessage = "Project name must be atleast 3 characters")]
        public string ProjectName
        {
            get { return projectName; }
            set { SetProperty(ref projectName, value); }
        }

        /// <summary>
        /// Gets or sets the description for the project.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        /// <summary>
        /// Gets or sets the version control for the project.
        /// </summary>
        public string VersionControl
        {
            get { return versionControl; }
            set { SetProperty(ref versionControl, value); }
        }

        /// <summary>
        /// Gets or sets the start date for the project.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Project must have start date")]
        public DateTime StartedOn
        {
            get { return startedOn; }
            set { SetProperty(ref startedOn, value); }
        }

        /// <summary>
        /// Gets or sets the Product Owner search email address.
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
        public bool EnableProductOwnerSearchGrid
        {
            get { return enableProductOwnerSearchGrid; }
            set { SetProperty(ref enableProductOwnerSearchGrid, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the PO has been confirmed.
        /// </summary>
        public bool ProductOwnerConfirmed
        {
            get { return productOwnerConfirmed; }
            set { SetProperty(ref productOwnerConfirmed, value); }
        }

        /// <summary>
        /// Gets or sets the observable collection that stores the Product Owner search results.
        /// </summary>
        public ObservableCollection<ServiceUserSearchInfo> ProductOwnerSearchUserResults { get; set; }

        /// <summary>
        /// Gets or sets the scrum master email address being searched.
        /// </summary>
        public string ScrumMasterSearchEmailAddress
        {
            get
            {
                return scrumMasterSearchEmailAddress;
            }

            set
            {
                SetProperty(ref scrumMasterSearchEmailAddress, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the scrum master search grid is enabled.
        /// </summary>
        public bool EnableScrumMasterSearchGrid
        {
            get
            {
                return enableScrumMasterSearchGrid;
            }

            set
            {
                SetProperty(ref enableScrumMasterSearchGrid, value);
            }
        }

        /// <summary>
        /// Gets or sets the collection containing scrum master search results.
        /// </summary>
        public ObservableCollection<ServiceUserSearchInfo> ScrumMasterSearchUserResults { get; set; }

        /// <summary>
        /// Gets or sets the collection of selected scrum masters.
        /// </summary>
        public ObservableCollection<ServiceUserSearchInfo> SelectedScrumMasters { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the project save command
        /// </summary>
        public DelegateCommand SaveProjectCommand { get; set; }

        /// <summary>
        /// Gets or sets the project cancel command
        /// </summary>
        public DelegateCommand CancelProjectCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that can be used to search for a PO.
        /// </summary>
        public DelegateCommand SearchProductOwnerUserCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that can be used to set a product owner on a new project.
        /// </summary>
        public DelegateCommand<string> SetProductOwnerCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that can be used to search for scrum masters.
        /// </summary>
        public DelegateCommand SearchScrumMasterCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that can be used to select a scrum master.
        /// </summary>
        public DelegateCommand<ServiceUserSearchInfo> SelectScrumMasterCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This is a method to cancel the project wizard
        /// </summary>
        public void CancelProject()
        {
            NavigateToItem("MyProjects");
        }

        /// <summary>
        /// This is the method to save a project from the view
        /// </summary>
        private void SaveProject()
        {
            this.ValidateViewModelNow();
            if (!this.HasErrors)
            {
                var hasScrumMasters = this.SelectedScrumMasters.Count > 0;
                var hasProductOwner = this.ProductOwnerConfirmed;

                var hasBoth = hasScrumMasters && hasProductOwner;

                if (!hasBoth)
                {
                    this.ShowMetroDialog("The project cannot be saved!", "This project cannot be saved because both a product owner and at least one scrum master is required.");
                    return;
                }

                ServiceProject project = new ServiceProject
                {
                    Name = this.ProjectName,
                    Description = this.Description,
                    VersionControl = this.VersionControl,
                    StartedOn = this.StartedOn,
                    ProductOwnerEmailAddress = selectProductOwnerEmailAddress,
                    ProjectManagerEmailAddress = UserDetailsStore.LoggedInUserPrincipal.Identity.Name,
                    ScrumMasterEmailAddresses = this.SelectedScrumMasters.Select(sm => sm.EmailAddress).ToList()
                };

                ServiceGateway.CreateProject(project);
                ShowMetroDialog("Project created", "The {0} project has successfully been created".Substitute(this.ProjectName), MessageDialogStyle.Affirmative);
                NavigateToItem("MyProjects");
            }
        }

        /// <summary>
        /// Searches for a project owner using the details entered by the user.
        /// </summary>
        private void SearchProjectOwners()
        {
            var filter = new ServiceUserSearchFilter { EmailAddress = productOwnerSearchEmailAddress, ProductOwner = true };
            var results = ServiceGateway.SearchUsers(filter);
            
            ProductOwnerSearchUserResults.Clear();
            ProductOwnerSearchUserResults.AddRange(results);
            EnableProductOwnerSearchGrid = true;
        }

        /// <summary>
        /// Searches for scrum masters using the details entered by the user.
        /// </summary>
        private void SearchScrumMasters()
        {
            var filter = new ServiceUserSearchFilter { EmailAddress = scrumMasterSearchEmailAddress, ScrumMaster = true };
            var results = ServiceGateway.SearchUsers(filter);

            ScrumMasterSearchUserResults.Clear();
            ScrumMasterSearchUserResults.AddRange(results);
            EnableScrumMasterSearchGrid = true; 
        }

        /// <summary>
        /// Sets the product owner of this project to the user with the specified email address.
        /// </summary>
        /// <param name="emailAddress">The email address of the user to be set as the Product Owner.</param>
        private void SetProductOwner(string emailAddress)
        {
            SelectProductOwnerEmailAddress = emailAddress;
            ProductOwnerSearchUserResults.Clear();
            EnableProductOwnerSearchGrid = false;
            ProductOwnerConfirmed = true;
            ProductOwnerSearchEmailAddress = string.Empty;
            ShowMetroDialog("Product Owner confirmed", "The product owner has been successfully set to user - {0}".Substitute(emailAddress), MessageDialogStyle.Affirmative);
        }

        /// <summary>
        /// Verifies that the selected scrum master can be added to this project.
        /// </summary>
        /// <param name="selectedScrumMaster">The selected scrum master.</param>
        /// <returns>A true or false flag indicating if this scrum master can be added to this project.</returns>
        private bool CanAddScrumMaster(ServiceUserSearchInfo selectedScrumMaster)
        {
            if (selectedScrumMaster != null)
            {
                var result = SelectedScrumMasters.Any(u => u.EmailAddress == selectedScrumMaster.EmailAddress);
                if (result)
                {
                    this.ShowMetroDialog("Unable to add this scrum master to the selected scrum masters!", "The selected scrum master has already been added as a scrum master for this project.");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Adds the specified scrum master to this project.
        /// </summary>
        /// <param name="selectedScrumMaster">The selected scrum master.</param>
        private void AddScrumMaster(ServiceUserSearchInfo selectedScrumMaster)
        {
            SelectedScrumMasters.Add(selectedScrumMaster);
            ScrumMasterSearchUserResults.Clear();
            EnableScrumMasterSearchGrid = false;
        }

        #endregion
    }
}
