using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.CustomValidationAttributes;
using AnyTrack.SharedUtilities.Extensions;
using AnyTrack.Sprints.BackendSprintService;
using AnyTrack.Sprints.ServiceGateways;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    ///  The view model for the Create Sprint View.
    /// </summary>
    public class CreateSprintViewModel : ValidatedBindableBase
    {
        #region Fields

        /// <summary>
        /// The client.
        /// </summary>
        private readonly ISprintServiceGateway serviceGateway;

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
        /// Description of the sprint.
        /// </summary>
        private string description;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Create Sprint View Model.
        /// </summary>
        /// <param name="serviceGateway">The sprint service gateway</param>
        public CreateSprintViewModel(ISprintServiceGateway serviceGateway)
        {
            if (serviceGateway == null)
           {
                throw new ArgumentNullException("serviceGateway");
           }

            this.serviceGateway = serviceGateway;

            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(14);

            SaveSprintCommand = new DelegateCommand(SaveSprint);
            CancelSprintCommand = new DelegateCommand(CancelSprintCreation);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the project id the sprint will be added to.
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the name of the sprint.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A Project Name is Required")]
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
        [DateTimeCompare("<=", "endDate", "Start Date must be before the End Date")]
        public DateTime StartDate
        {
            get { return startDate; }
            set { SetProperty(ref startDate, value); }
        }

        /// <summary>
        /// Gets or sets the end date of the sprint.
        /// </summary>
        [Required(ErrorMessage = "End Date is Required")]
        [DateTimeCompare(">=", "startDate", "End Date must be after the Start Date")]
        public DateTime EndDate
        {
            get { return endDate; }
            set { SetProperty(ref endDate, value); }
        }

        /// <summary>
        /// Gets or sets the description of the sprint.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }
        
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

        #endregion

        #region Methods

        /// <summary>
        /// Saves a sprint to the database.
        /// </summary>
        private void SaveSprint()
        {
            if (HasErrors)
            {
                ShowMetroDialog("Sprint was not Saved", "There are errors in the form. Please correct them and try again.");
                return;
            }

            ServiceSprint sprint = new ServiceSprint
            {
                Name = SprintName,
                Description = this.Description,
                StartDate = this.StartDate,
                EndDate = this.EndDate
            };

            //// add team members here
            serviceGateway.AddSprint(ProjectId, sprint);
            ShowMetroDialog("Save Successful", "Your sprint {0} has been saved successfully.".Substitute(sprintName));
            NavigateToItem("MySprints");
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
                    NavigateToItem("MyProjects");
                }
            });

            ShowMetroDialog("Sprint Creation Cancellation", "Are you sure that you want to cancel?", MessageDialogStyle.AffirmativeAndNegative, callbackAction); 
        }

        #endregion
    }
}
