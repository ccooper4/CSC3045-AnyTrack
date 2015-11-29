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
        /// Gets all tasks for sprint
        /// </summary>
        /// <param name="sprintId">The sprint id</param>
        /// <returns>A list of tasks</returns>
        List<ServiceTask> GetAllTasksForSprint(Guid sprintId);
    }
}
