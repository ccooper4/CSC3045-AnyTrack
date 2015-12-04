using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendSprintService;
using AnyTrack.Infrastructure.ServiceGateways;
using Prism.Commands;
using Prism.Regions;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// The view model for the Update Task Hours ViewModel
    /// </summary>
    public class UpdateTaskHoursViewModel : ValidatedBindableBase
    {
        #region Fields

        /// <summary>
        /// The srpint service gateway
        /// </summary>
        private readonly ISprintServiceGateway serviceGateway;

        /// <summary>
        /// The sprint Id
        /// </summary>
        private Guid sprintId;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Update Task Hours View Model
        /// </summary>
        /// <param name="serviceGateway">The project service gateway</param>
        public UpdateTaskHoursViewModel(ISprintServiceGateway serviceGateway)
        {
            if (serviceGateway == null)
            {
                throw new ArgumentNullException("serviceGateway");
            }

            this.serviceGateway = serviceGateway;
         
            UpdateTaskHoursCommand = new DelegateCommand(SaveTaskHours);
            CancelCommand = new DelegateCommand(GoToSprintBoard);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Tasks
        /// </summary>
        public ObservableCollection<ServiceTask> Tasks { get; set; }

        /// <summary>
        /// Gets or sets the sprint id
        /// </summary>
        public Guid SprintId
        {
            get
            {
                return sprintId;
            }

            set
            {
                sprintId = value;
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the update hours command
        /// </summary>
        public DelegateCommand UpdateTaskHoursCommand { get; set; }

        /// <summary>
        /// Gets or sets the cancel hours command
        /// </summary>
        public DelegateCommand CancelCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the navigated to.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("sprintId"))
            {
                sprintId = (Guid)navigationContext.Parameters["sprintId"];
                this.Tasks = new ObservableCollection<ServiceTask>();
                Tasks.AddRange(serviceGateway.GetAllTasksForSprintCurrentUser(SprintId));
            }
        }

        /// <summary>
        /// Gets all the tasks for the current user
        /// </summary>
        /// <param name="sprintId">The id of the sprint</param>
        /// <returns>A list of taks</returns>
        private ObservableCollection<ServiceTask> GetTasksForUser(Guid sprintId)
        {
            return new ObservableCollection<ServiceTask>(serviceGateway.GetAllTasksForSprint(sprintId));
        }

        /// <summary>
        /// This is the method to save a task hours
        /// </summary>
        private void SaveTaskHours()
        {
            serviceGateway.SaveUpdatedTaskHours(this.Tasks.ToList());
        }

        /// <summary>
        /// Navigates to sprint board
        /// </summary>
        private void GoToSprintBoard()
        {
            var navParams = new NavigationParameters();
            navParams.Add("sprintId", sprintId);
            NavigateToItem("SprintBoard", navParams);
        }

        #endregion
    }
}
