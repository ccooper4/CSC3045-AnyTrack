using System;
using System.Collections.Generic;
using System.Net.Mail;
using AnyTrack.Infrastructure.BackendSprintService;
using MemoryStream = System.IO.MemoryStream;

namespace AnyTrack.Infrastructure.ServiceGateways
{
    /// <summary>
    /// Outlines the Sprint Gateway.
    /// </summary>
    public interface ISprintServiceGateway
    {
        /// <summary>
        /// Delete a task. 
        /// </summary>
        /// <param name="serviceTaskId">the task to delete</param>
        void DeleteTask(Guid serviceTaskId);

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
        /// Gets all tasks for sprint current user
        /// </summary>
        /// <param name="sprintId">The sprint id</param>
        /// <returns>A list of tasks</returns>
        List<ServiceTask> GetAllTasksForSprintCurrentUser(Guid sprintId);

        /// <summary>
        /// Get all the tasks for a given sprint story.
        /// </summary>
        /// <param name="sprintStoryId">the id of the sprint story</param>
        /// <returns>the list of tasks</returns>
        List<ServiceTask> GetAllTasksForSprintStory(Guid sprintStoryId);

        /// <summary>
        /// Gets the end date of the sprint
        /// </summary>
        /// <param name="sprintId">the sprint id</param>
        /// <returns>the end date of sprint</returns>
        DateTime? GetDateSprintEnds(Guid sprintId);

        /// <summary>
        /// Gets the start date of the sprint
        /// </summary>
        /// <param name="sprintId">the sprint id</param>
        /// <returns>the start date of sprint</returns>
        DateTime? GetDateSprintStarted(Guid sprintId);

        /// <summary>
        /// Gets the max estimate of the sprint
        /// </summary>
        /// <param name="sprintId">the sprint id</param>
        /// <returns>the maximum estimate of the sprint</returns>
        double GetSprintMaxEstimate(Guid sprintId);

        /// <summary>
        /// Saves the updated task hours
        /// </summary>
        /// <param name="tasks">The list of tasks</param>
        void SaveUpdatedTaskHours(List<ServiceTask> tasks);

        /// <summary>
        /// Add the initial task hour estimate to a task. 
        /// </summary>
        /// <param name="taskId"> the id of the task</param>
        /// <param name="serviceTaskHourEstimate"> the task hour estimate </param>
        void AddTaskHourEstimateToTask(Guid taskId, ServiceTaskHourEstimate serviceTaskHourEstimate);

        /// <summary>
        /// Add a new task to a sprint story.
        /// </summary>
        /// <param name="sprintStoryId">The story to add the task to.</param>
        /// <param name="serviceTask">The task to add.</param>
        void AddTaskToSprintStory(Guid sprintStoryId, ServiceTask serviceTask);

        /// <summary>
        /// Gets a summary of this user's sprints. 
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <param name="scrumMaster">A flag indicating if sprints where this user is a scrummaster should be returned.</param>
        /// <param name="developer">A flag indicating if sprints where this user is a developer should be returned.</param>
        /// <returns>A list of sprints for this user.</returns>
        List<ServiceSprintSummary> GetSprintNames(Guid? projectId, bool scrumMaster, bool developer);

        /// <summary>
        /// Rertrieves the sprint stories with their estimates fotr this sprint
        /// </summary>
        /// <param name="sprintId">the sprint id</param>
        /// <returns>List of stories</returns>
         List<ServiceSprintStory> GetSprintStoryEstimates(Guid sprintId);

        /// <summary>
        /// Returns the list of sprint stories with specified id
        /// </summary>
        /// <param name="sprintId">The id of the sprint</param>
        /// <returns>The list of sprint stories</returns>
        List<ServiceSprintStory> GetSprintStories(Guid sprintId);

        /// <summary>
        /// Manages the sprint backlog
        /// </summary>
        /// <param name="projectId">the id of the project</param>
        /// <param name="sprintId">the id of the sprint</param>
        /// <param name="updatedSprintBacklog">the updated sprint backlog</param>
        void ManageSprintBacklog(Guid projectId, Guid sprintId, List<ServiceSprintStory> updatedSprintBacklog);

        /// <summary>
        /// Acquires the total story point estimate for a sprint
        /// </summary>
        /// <param name="sprintId">the id of the sprint</param>
        /// <returns>the total story point estimate</returns>
        double GetTotalStoryPointEstimate(Guid sprintId);        

        /// <summary>
        /// Sends an email of a burndown chart
        /// </summary>
        /// <param name="senderEmailAddress">The email adddress to send the email to</param>
        /// <param name="recipientEmailAddress">The email adddress where the email is sent from</param>
        /// <param name="emailMessage">The email address of the </param>
        /// <param name="emailAttachment">The email attachment of the </param>
        void SendEmailRequest(string senderEmailAddress, string recipientEmailAddress, string emailMessage, MemoryStream emailAttachment);
        
        /// <summary>
        /// Acquires the maxmium story estimate for a given sprint
        /// </summary>
        /// <param name="sprintId">the sprint id</param>
        /// <returns>the maximum story estimate</returns>
        //// double GetSprintMaxStoryEstimate(Guid sprintId);

        ///// <summary>
        ///// Sends an email of a burndown chart
        ///// </summary>
        ///// <param name="senderEmailAddress">The email adddress to send the email to</param>
        ///// <param name="recipientEmailAddress">The email adddress where the email is sent from</param>
        ///// <param name="emailMessage">The email address of the </param>
        ///// <param name="emailAttachment">Attachment for the email</param>
        //// void SendEmailRequest(string senderEmailAddress, string recipientEmailAddress, string emailMessage, System.Net.Mail.Attachment emailAttachment);
    }
}
