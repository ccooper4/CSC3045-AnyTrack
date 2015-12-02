using System;
using System.Collections.Generic;
using AnyTrack.Infrastructure.BackendSprintService;

namespace AnyTrack.Infrastructure.ServiceGateways
{
    /// <summary>
    /// Outlines the Sprint Gateway.
    /// </summary>
    public interface ISprintServiceGateway
    {
        /// <summary>
        /// Creates a sprint and adds it to the project.
        /// </summary>
        /// <param name="projectId">Id of the project to add the sprint to</param>
        /// <param name="sprint">ServiceSprint entity</param>
        void AddSprint(Guid projectId, ServiceSprint sprint);

        /// <summary>
        /// Edits an exiting sprint.
        /// </summary>
        /// <param name="sprintId">Id of the sprint to be edited</param>
        /// <param name="updatedSprint">ServiceSprint entity containing changes</param>
        void EditSprint(Guid sprintId, ServiceSprint updatedSprint);

        /// <summary>
        /// Retrieves a specified sprint.
        /// </summary>
        /// <param name="sprintId">Id of the sprint</param>
        /// <returns>The sprint</returns>
        ServiceSprint GetSprint(Guid sprintId);

        /// <summary>
        /// Gets all tasks for sprint
        /// </summary>
        /// <param name="sprintId">The sprint id</param>
        /// <returns>A list of tasks</returns>
        List<ServiceTask> GetAllTasksForSprint(Guid sprintId);

        /// <summary>
        /// Saves the updated task hours
        /// </summary>
        /// <param name="tasks">The list of tasks</param>
        void SaveUpdatedTaskHours(List<ServiceTask> tasks);

        /// <summary>
        /// Gets a summary of this user's sprints. 
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <param name="scrumMaster">A flag indicating if sprints where this user is a scrummaster should be returned.</param>
        /// <param name="developer">A flag indicating if sprints where this user is a developer should be returned.</param>
        /// <returns>A list of sprints for this user.</returns>
        List<ServiceSprintSummary> GetSprintNames(Guid? projectId, bool scrumMaster, bool developer);
    }
}
