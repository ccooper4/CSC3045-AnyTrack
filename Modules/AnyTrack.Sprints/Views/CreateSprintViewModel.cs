﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.CustomValidationAttributes;
using AnyTrack.Infrastructure.ServiceGateways;
using AnyTrack.SharedUtilities.Extensions;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Regions;
using ServiceSprint = AnyTrack.Infrastructure.BackendSprintService.ServiceSprint;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    ///  The view model for the Create Sprint View.
    /// </summary>
    public class CreateSprintViewModel : ValidatedBindableBase, INavigationAware
    {
        #region Fields

        /// <summary>
        /// The sprint service client.
        /// </summary>
        private readonly ISprintServiceGateway sprintServiceGateway;

        /// <summary>
        /// The project service client.
        /// </summary>
        private readonly IProjectServiceGateway projectServiceGateway;

        /// <summary>
        /// Name of the sprint.
        /// </summary>
        private string sprintName;

        /// <summary>
        /// Date the sprint starts on.
        /// </summary>
        private DateTime startDate;

        /// <summary>
        /// Date the sprint finishes on.
        /// </summary>
        private DateTime endDate;

        /// <summary>
        /// Sprint length in days.
        /// </summary>
        private int sprintLength;

        /// <summary>
        /// Description of the sprint.
        /// </summary>
        private string description;

        /// <summary>
        /// List of email addresses of each member of the team (developers)
        /// </summary>
        private List<string> teamMemberEmailAddresses;

        /// <summary>
        /// The id of the project the sprint will be added to.
        /// </summary>
        private Guid projectId;

        /// <summary>
        /// The skillset searched for.
        /// </summary>
        private string skillSetSearch;

        /// <summary>
        /// Indicates whether the developer search grid is enabled.
        /// </summary>
        private bool enableDeveloperSearchGrid;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Create Sprint View Model.
        /// </summary>
        /// <param name="sprintServiceGateway">The sprint service gateway</param>
        /// <param name="projectServiceGateway">The project service gateway</param>
        public CreateSprintViewModel(ISprintServiceGateway sprintServiceGateway, IProjectServiceGateway projectServiceGateway)
        {
            if (sprintServiceGateway == null)
           {
                throw new ArgumentNullException("sprintServiceGateway");
           }

            if (projectServiceGateway == null)
            {
                throw new ArgumentNullException("projectServiceGateway");
            }

            this.sprintServiceGateway = sprintServiceGateway;
            this.projectServiceGateway = projectServiceGateway;

            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(14);

            DeveloperSearchUserResults = new ObservableCollection<ServiceUserSearchInfo>();
            SelectedDevelopers = new ObservableCollection<ServiceUserSearchInfo>();

            SaveSprintCommand = new DelegateCommand(SaveSprint);
            CancelSprintCommand = new DelegateCommand(CancelSprintCreation);
            SearchDeveloperCommand = new DelegateCommand(SearchDevelopers);
            SelectDeveloperCommand = new DelegateCommand<ServiceUserSearchInfo>(AddDeveloper, CanAddDeveloper);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the sprint.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A Sprint Name is Required")]
        [MinLength(3, ErrorMessage = "Sprint name must be atleast 3 characters")]
        public string SprintName
        {
            get { return sprintName; }
            set { SetProperty(ref sprintName, value); }
        }

        /// <summary>
        /// Gets or sets the start date of the sprint.
        /// </summary> 
        [Required(ErrorMessage = "Start Date is Required")]
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }

            set
            {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
                SetProperty(ref startDate, value);
                EndDate = endDate.AddDays(1);
                EndDate = endDate.Subtract(new TimeSpan(1, 0, 0, 0));
                SprintLength = CalculateSprintLength();
            }
        }

        /// <summary>
        /// Gets or sets the end date of the sprint.
        /// </summary>
        [Required(ErrorMessage = "End Date is Required")]
        [DateTimeCompare(">=", "StartDate", "End Date must be after the Start Date")]
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                SetProperty(ref endDate, value);
                SprintLength = CalculateSprintLength();
            }
        }

        /// <summary>
        /// Gets or sets the sprint length in days.
        /// </summary>
        public int SprintLength
        {
            get { return sprintLength; } 
            set { SetProperty(ref sprintLength, value); }
        }

        /// <summary>
        /// Gets or sets the description of the sprint.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        /// <summary>
        /// Gets or sets the skillset being searched for.
        /// </summary>
        public string SkillSetSearch 
        {
            get
            {
                return skillSetSearch;
            }

            set
            {
                SetProperty(ref skillSetSearch, value); 
                SearchDeveloperCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the developer search grid is enabled.
        /// </summary>
        public bool EnableDeveloperSearchGrid
        {
            get { return enableDeveloperSearchGrid; }
            set { SetProperty(ref enableDeveloperSearchGrid, value); }
        }

        /// <summary>
        /// Gets or sets the collection containing developer search results.
        /// </summary>
        public ObservableCollection<ServiceUserSearchInfo> DeveloperSearchUserResults { get; set; }

        /// <summary>
        /// Gets or sets the collection of selected developers.
        /// </summary>
        public ObservableCollection<ServiceUserSearchInfo> SelectedDevelopers { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that can be used to save a sprint.
        /// </summary>
        public DelegateCommand SaveSprintCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that can be used to cancel the creation of a sprint.
        /// </summary>
        public DelegateCommand CancelSprintCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that can be used to search for a developer.
        /// </summary>
        public DelegateCommand SearchDeveloperCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that can be used to select a developer.
        /// </summary>
        public DelegateCommand<ServiceUserSearchInfo> SelectDeveloperCommand { get; set; }

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
            if (navigationContext.Parameters.ContainsKey("projectId"))
            {
                projectId = (Guid)navigationContext.Parameters["projectId"];
            }
        }

        /// <summary>
        /// Saves a sprint to the database.
        /// </summary>
        private void SaveSprint()
        {
            if (SprintName == null)
            {
                SprintName = string.Empty;
            }

            if (HasErrors)
            {
                ShowMetroDialog("Sprint was not Saved", "There are errors in the form. Please correct them and try again.");
                return;
            }

            ServiceSprint sprint = new ServiceSprint()
            {
                Name = SprintName,
                Description = this.Description,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                TeamEmailAddresses = new List<string>()
            };

            if (SelectedDevelopers.Count > 0)
            {
                sprint.TeamEmailAddresses.AddRange(SelectedDevelopers.Select(d => d.EmailAddress).ToList());
            }

            sprintServiceGateway.AddSprint(projectId, sprint);
            ShowMetroDialog("Save Successful", "Your sprint {0} has been saved successfully.".Substitute(sprintName));
            NavigateToItem("SprintManager");
        }

        /// <summary>
        /// Cancels the creation of a sprint.
        /// </summary>
        private void CancelSprintCreation()
        {
            var callbackAction = new Action<MessageDialogResult>(mdr =>
            {
                if (mdr == MessageDialogResult.Affirmative)
                {
                    NavigateToItem("SprintManager");
                }
            });

            ShowMetroDialog("Sprint Creation Cancellation", "Are you sure you want to cancel? All entered data will be lost.", MessageDialogStyle.AffirmativeAndNegative, callbackAction); 
        }

        /// <summary>
        /// Searches for developers using the details entered by the user.
        /// </summary>
        private void SearchDevelopers()
        {
            var skillsList = skillSetSearch.Split(',').Select(p => p.Trim()).ToList();
            var filter = new ServiceUserSearchFilter { Developer = true, SprintStartingDate = startDate, SprintEndingDate = endDate, Skillset = skillsList };
            var results = projectServiceGateway.SearchUsers(filter);

            DeveloperSearchUserResults.Clear();
            DeveloperSearchUserResults.AddRange(results);
            EnableDeveloperSearchGrid = true;
        }

        /// <summary>
        /// Indicates if a selected developer can be added.
        /// </summary>
        /// <param name="selectedDeveloper">The developer to try and add</param>
        /// <returns>Whether developer can be added</returns>
        private bool CanAddDeveloper(ServiceUserSearchInfo selectedDeveloper)
        {
            return !SelectedDevelopers.Contains(selectedDeveloper);
        }

        /// <summary>
        /// Adds a selected developer to the sprint.
        /// </summary>
        /// <param name="selectedDeveloper">The developer to add</param>
        private void AddDeveloper(ServiceUserSearchInfo selectedDeveloper)
        {
            if (SelectedDevelopers.Contains(selectedDeveloper))
            {
                ShowMetroDialog("Developer Already Added", "{0} is already assigned as a developer for the sprint".Substitute(selectedDeveloper.FullName));
                return;
            }

            SelectedDevelopers.Add(selectedDeveloper);
            ShowMetroDialog("Developer Added", "{0} has been successfully added as a developer for the sprint".Substitute(selectedDeveloper.FullName));
            SelectDeveloperCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Calculates the length of the sprint in days.
        /// </summary>
        /// <returns>Sprint length in days</returns>
        private int CalculateSprintLength()
        {
            return (EndDate - StartDate).Days + 1;
        }

        #endregion
    }
}
