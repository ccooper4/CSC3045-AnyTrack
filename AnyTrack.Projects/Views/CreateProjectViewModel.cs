using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Windows;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.ServiceGateways;
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
    public class CreateProjectViewModel : BaseViewModel, INavigationAware
    {
        #region Fields

        /// <summary>
        /// Id of project (Edit Mode)
        /// </summary>
        private Guid projectId;

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
        /// Indicates if the project is being edited.
        /// </summary>
        private bool editMode;

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
            if (iProjectServiceGateway == null)
            {
                throw new ArgumentNullException("iProjectServiceGateway");
            }

            SaveProjectCommand = new DelegateCommand(SaveProject);
            CancelProjectCommand = new DelegateCommand(CancelProject);
            SearchProductOwnerUserCommand = new DelegateCommand(SearchProjectOwners);
            SetProductOwnerCommand = new DelegateCommand<string>(SetProductOwner);
            SearchScrumMasterCommand = new DelegateCommand(SearchScrumMasters);
            SelectScrumMasterCommand = new DelegateCommand<ServiceUserSearchInfo>(AddScrumMaster, CanAddScrumMaster);
            RemoveScrumMasterCommand = new DelegateCommand<ServiceUserSearchInfo>(RemoveScrumMaster);

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

        /// <summary>
        /// Gets or sets the command that can be used to remove a scrum master.
        /// </summary>
        public DelegateCommand<ServiceUserSearchInfo> RemoveScrumMasterCommand { get; set; }

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
                projectId = (Guid)navigationContext.Parameters["ProjectId"];

                if (navigationContext.Parameters.ContainsKey("EditMode"))
                {
                    bool edit;
                    bool.TryParse(navigationContext.Parameters["EditMode"].ToString(), out edit);
                    editMode = edit;

                    if (editMode)
                    {
                        var project = ServiceGateway.GetProject(projectId);
                        ProjectName = project.Name;
                        Description = project.Description;
                        VersionControl = project.VersionControl;
                        StartedOn = project.StartedOn;
                        SelectProductOwnerEmailAddress = project.ProductOwnerEmailAddress;

                        if (project.ScrumMasterEmailAddresses != null && project.ScrumMasterEmailAddresses.Count > 0)
                        {
                            foreach (var scrumMasterEmail in project.ScrumMasterEmailAddresses)
                            {
                                var filter = new ServiceUserSearchFilter { EmailAddress = scrumMasterEmail, ScrumMaster = true };
                                var result = ServiceGateway.SearchUsers(filter);
                                SelectedScrumMasters.Add(result.First());
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This is a method to cancel the project wizard
        /// </summary>
        private void CancelProject()
        {
            var callbackAction = new Action<MessageDialogResult>(mdr =>
            {
                if (mdr == MessageDialogResult.Affirmative)
                {
                    NavigateToItem("MyProjects");
                }
            });

            if (editMode)
            {
                ShowMetroDialog("Project Edit Cancellation", "Are you sure you want to cancel? All changes will be lost.", MessageDialogStyle.AffirmativeAndNegative, callbackAction);
            }
            else
            {
                ShowMetroDialog("Project Creation Cancellation", "Are you sure you want to cancel? All data will be lost.", MessageDialogStyle.AffirmativeAndNegative, callbackAction);
            }                
        }

        /// <summary>
        /// This is the method to save a project from the view
        /// </summary>
        private void SaveProject()
        {
            this.ValidateViewModelNow();
            if (!this.HasErrors)
            {
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

                if (editMode)
                {
                   project.ProjectId = projectId;
                   ServiceGateway.UpdateProject(project);
                   ShowMetroDialog("Project Updated", "The {0} project has been updated successfully.".Substitute(ProjectName), MessageDialogStyle.Affirmative);
                }
                else
                {
                    ServiceGateway.CreateProject(project);
                    ShowMetroDialog("Project created", "The {0} project has been created sucessfully.".Substitute(ProjectName), MessageDialogStyle.Affirmative);
                }

                NavigateToItem("MyProjects");
            }
            else
            {
                ShowMetroDialog("Project was not Saved", "The project could not be saved. There are errors on the page. Please fix them and try again.", MessageDialogStyle.Affirmative);
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

            if (ProductOwnerSearchUserResults.Count == 0)
            {
                ShowMetroDialog("No Results Found", "No Product Owners with the email address {0} have been found.".Substitute(productOwnerSearchEmailAddress));
            }

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

            if (ScrumMasterSearchUserResults.Count == 0)
            {
                ShowMetroDialog("No Results Found", "No Scrum masters with the email address {0} have been found.".Substitute(ScrumMasterSearchEmailAddress));
            }

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
            ShowMetroDialog("Product Owner Assigned", "{0} has been assigned the role Product Owner of the project.".Substitute(emailAddress), MessageDialogStyle.Affirmative);
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
            ShowMetroDialog("Scrum Master Assigned", "{0} has been assigned the role of Scrum Master on the project.".Substitute(selectedScrumMaster.FullName));
        }

        /// <summary>
        /// Removes the selected scrumMaster from this project.
        /// </summary>
        /// <param name="selectedScrumMaster">The selected scrum master</param>
        private void RemoveScrumMaster(ServiceUserSearchInfo selectedScrumMaster)
        {
            var callbackAction = new Action<MessageDialogResult>(mdr =>
            {
                if (mdr == MessageDialogResult.Affirmative)
                {
                    SelectedScrumMasters.Remove(selectedScrumMaster);
                }
            });

            ShowMetroDialog("Scrum Master Removal", "Are you sure you want to remove {0} as a scrum master on the project?".Substitute(selectedScrumMaster.FullName), MessageDialogStyle.AffirmativeAndNegative, callbackAction);          
        }
        #endregion    
    }
}
