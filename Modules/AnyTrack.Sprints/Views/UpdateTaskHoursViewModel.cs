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
            this.Tasks = new ObservableCollection<ServiceTask>();
            ////Tasks = GetTasksForUser(this.sprintId);
            UpdateTaskHoursCommand = new DelegateCommand(SaveTaskHours);

            #region mockdata
            List<ServiceTask> tasksList = new List<ServiceTask>();
            ServiceTask t = new ServiceTask
            {
                Assignee = new ServiceUser(),
                ConditionsOfSatisfaction = "asdsad",
                Description = "asd",
                TaskHourEstimates = new List<ServiceTaskHourEstimate>(),
                SprintStoryId = new Guid("cfc247ce-f830-4a4d-bd39-74999c66ef3e")
            };

            t.TaskHourEstimates.Add(new ServiceTaskHourEstimate
            {
                Estimate = 2
            });
            tasksList.Add(t);

            #endregion

            this.Tasks = new ObservableCollection<ServiceTask>(tasksList);
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
        /// Gets or sets a given story to delete from the backlog
        /// </summary>
        public DelegateCommand UpdateTaskHoursCommand { get; set; }

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

        #endregion
    }
}
