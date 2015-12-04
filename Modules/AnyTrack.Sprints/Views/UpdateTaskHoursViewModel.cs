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
    public class UpdateTaskHoursViewModel : ValidatedBindableBase, INavigationAware, IRegionMemberLifetime
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

            Tasks = new ObservableCollection<ServiceTask>();
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

        /// <summary>
        /// Gets a value indicating whether or not this view/view model should be reused.
        /// </summary>
        public bool KeepAlive
        {
            get
            {
                return false;
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
        /// Handles the Is Navigation target event. 
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns>A true or false value indicating if this viewmodel can handle the navigation request.</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        /// <summary>
        /// Handles the navigated to.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("sprintId"))
            {
                sprintId = (Guid)navigationContext.Parameters["sprintId"];
                var tasks = serviceGateway.GetAllTasksForSprintCurrentUser(SprintId);

                foreach (var task in Tasks)
                {
                    task.TaskHourEstimates = new List<ServiceTaskHourEstimate>
                    {
                        task.TaskHourEstimates.LastOrDefault()
                    };
                }

                Tasks.Clear();
                Tasks.AddRange(tasks);
            }
        }

        /// <summary>
        /// Handles the on navigated from event. 
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
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
