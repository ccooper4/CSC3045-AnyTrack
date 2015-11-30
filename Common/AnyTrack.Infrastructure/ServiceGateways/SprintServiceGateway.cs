using System;
using System.Collections.Generic;
using AnyTrack.Infrastructure.BackendSprintService;

namespace AnyTrack.Infrastructure.ServiceGateways
{
    /// <summary>
    /// Provides a gateway to the Sprint Service
    /// </summary>
    public class SprintServiceGateway : ISprintServiceGateway
    {
        #region Fields
        /// <summary>
        /// The web client
        /// </summary>
        private readonly ISprintService client;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new project service gateway
        /// </summary>
        /// <param name="client">The web client</param>
        public SprintServiceGateway(ISprintService client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            this.client = client;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a sprint and adds it to the project.
        /// </summary>
        /// <param name="projectId">Id of the project to add the sprint to</param>
        /// <param name="sprint">ServiceSprint entity</param>
        public void AddSprint(Guid projectId, ServiceSprint sprint)
        {
            client.AddSprint(projectId, sprint);
        }

        /// <summary>
        /// Edits an exiting sprint.
        /// </summary>
        /// <param name="sprintId">Id of the sprint to be edited</param>
        /// <param name="updatedSprint">ServiceSprint entity containing changes</param>
        public void EditSprint(Guid sprintId, ServiceSprint updatedSprint)
        {
            client.EditSprint(sprintId, updatedSprint);
        }

        /// <summary>
        /// Get all tasks for sprint
        /// </summary>
        /// <param name="sprintId">The sprint id</param>
        /// <returns>Returns a list of tasks</returns>
        public List<ServiceTask> GetAllTasksForSprint(Guid sprintId)
        {
            return new List<ServiceTask>(client.GetAllTasksForSprint(sprintId));
        }

        /// <summary>
        /// Gets a summary of this user's sprints. 
        /// </summary>
        /// <param name="projectId">The project id to scope the query to.</param>
        /// <param name="scrumMaster">A flag indicating if sprints where this user is a scrum master should be returned.</param>
        /// <param name="developer">A flag indicating if sprints where this user is a developer should be returned.</param>
        /// <returns>A list of sprints for this user.</returns>
        public List<ServiceSprintSummary> GetSprintNames(Guid? projectId, bool scrumMaster, bool developer)
        {
            return client.GetSprintNames(projectId, scrumMaster, developer);
        }

        #endregion
    }
}
