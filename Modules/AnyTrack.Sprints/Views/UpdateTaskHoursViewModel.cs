using System;
using System.Collections.ObjectModel;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendSprintService;
using AnyTrack.Infrastructure.ServiceGateways;

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
            Tasks = GetTasksForUser(this.sprintId);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Tasks
        /// </summary>
        public ObservableCollection<ServiceTask> Tasks { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all the tasks for the current user
        /// </summary>
        /// <param name="sprintId">The id of the sprint</param>
        /// <returns>A list of taks</returns>
        private ObservableCollection<ServiceTask> GetTasksForUser(Guid sprintId)
        {
            return new ObservableCollection<ServiceTask>(serviceGateway.GetAllTasksForSprint(sprintId));
        }

        #endregion
    }
}
