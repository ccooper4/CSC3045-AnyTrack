using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Runtime.Remoting;
using System.Security.RightsManagement;
using AnyTrack.Infrastructure.BackendSprintService;
using MemoryStream = System.IO.MemoryStream;

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
        /// Delete a task. 
        /// </summary>
        /// <param name="serviceTaskId">the task to delete</param>
        public void DeleteTask(Guid serviceTaskId)
        {
            client.DeleteTask(serviceTaskId);
        }

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
        /// Save a sprint story
        /// </summary>
        /// <param name="sprintStory">the spritn story id</param>
        public void SaveSprintStory(ServiceSprintStory sprintStory)
        {
            client.SaveSprintStory(sprintStory);
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
        /// Get all tasks for sprint
        /// </summary>
        /// <param name="sprintId">The sprint id</param>
        /// <returns>Returns a list of tasks</returns>
        public List<ServiceTask> GetAllTasksForSprintCurrentUser(Guid sprintId)
        {
            return new List<ServiceTask>(client.GetAllTasksForSprint(sprintId));
        }

        /// <summary>
        /// Get all the tasks for a given sprint story.
        /// </summary>
        /// <param name="sprintStoryId">the id of the sprint story</param>
        /// <returns>the list of tasks</returns>
        public List<ServiceTask> GetAllTasksForSprintStory(Guid sprintStoryId)
        {
            return client.GetAllTasksForSprintStory(sprintStoryId);
        }

        /// <summary>
        /// Gets the maximum estimate of the sprint
        /// </summary>
        /// <param name="sprintId">the sprint id</param>
        /// <returns>the max value of estimate in the sprint</returns>
        public double GetSprintMaxEstimate(Guid sprintId)
        {
            return client.GetMaxEstimateOfSprint(sprintId);
        }

        /// <summary>
        /// Gets the start date of the current sprint
        /// </summary>
        /// <param name="sprintId">the sprintId</param>
        /// <returns>the start date of the current sprint</returns>
        public DateTime? GetDateSprintStarted(Guid sprintId)
        {
            return client.GetStartDateOfSprint(sprintId);
        }

        /// <summary>
        /// Gets the end date of the current sprint
        /// </summary>
        /// <param name="sprintId">the sprintId</param>
        /// <returns>the enddate of the sprint</returns>
        public DateTime? GetDateSprintEnds(Guid sprintId)
        {
            return client.GetEndDateOfSprint(sprintId);
        }

        /// <summary>
        /// Retrieves a specified sprint.
        /// </summary>
        /// <param name="sprintId">Id of the sprint</param>
        /// <returns>The sprint</returns>
        public ServiceSprint GetSprint(Guid sprintId)
         {
             return client.GetSprint(sprintId);
         }

        /// <summary>
        /// Saves the updated task hours
        /// </summary>
        /// <param name="tasks">The list of tasks</param>
        public void SaveUpdatedTaskHours(List<ServiceTask> tasks)
        {
            client.SaveUpdatedTaskHours(tasks);
        }

        /// <summary>
        /// Add the initial task hour estimate to a task. 
        /// </summary>
        /// <param name="taskId"> the id of the task</param>
        /// <param name="serviceTaskHourEstimate"> the task hour estimate </param>
        public void AddTaskHourEstimateToTask(Guid taskId, ServiceTaskHourEstimate serviceTaskHourEstimate)
        {
            client.AddTaskHourEstimateToTask(taskId, serviceTaskHourEstimate);
        }

        /// <summary>
        /// Add a new task to a sprint story.
        /// </summary>
        /// <param name="sprintStoryId">The story to add the task to.</param>
        /// <param name="serviceTask">The task to add.</param>
        public void AddTaskToSprintStory(Guid sprintStoryId, ServiceTask serviceTask)
        {
            client.AddTaskToSprintStory(sprintStoryId, serviceTask);
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

        /// <summary>
        /// Method to get the stories for a sprint
        /// </summary>
        /// <param name="sprintId">The id of the sprint</param>
        /// <returns>A list of sprint stories</returns>
        public List<ServiceSprintStory> GetSprintStories(Guid sprintId)
        {
            return new List<ServiceSprintStory>(client.GetSprintStories(sprintId));
        }

        /// <summary>
        /// Manages the backlog of sprints
        /// </summary>
        /// <param name="projectId">the id of the project</param>
        /// <param name="sprintId">The id of the sprint</param>
        /// <param name="updatedSprintBacklog">The updated backlog</param>
        public void ManageSprintBacklog(Guid projectId, Guid sprintId, List<ServiceSprintStory> updatedSprintBacklog)
        {
            client.ManageSprintBacklog(projectId, sprintId, updatedSprintBacklog);
        }

        //// <summary>
        //// Sending an email request via burndown
        //// </summary>
        //// <param name="senderEmailAddress">The email adddress to send the email to</param>
        //// <param name="recipientEmailAddress">The email adddress where the email is sent from</param>
        ////<param name="emailMessage">The email address of the </param>
        //// <param name="emailAttachment">Attachment for the email</param>
        //// public void SendEmailRequest(string senderEmailAddress, string recipientEmailAddress, string emailMessage, System.Net.Mail.Attachment emailAttachment)
       //// {
           //// client.SendEmailRequest(senderEmailAddress, recipientEmailAddress, emailMessage, emailAttachment);
       //// }

        /// <summary>
        /// Gets Sprint stories for a given sprint with estimates
        /// </summary>
        /// <param name="sprintId">The sprint id</param>
        /// <returns>sprint stories</returns>
        public List<ServiceSprintStory> GetSprintStoryEstimates(Guid sprintId)
        {
            return client.GetSprintStoryEstimates(sprintId);
        }
        
        /// <summary>
        /// Sending an email request via burndown
        /// </summary>
        /// <param name="senderEmailAddress">The email adddress to send the email to</param>
        /// <param name="recipientEmailAddress">The email adddress where the email is sent from</param>
        /// <param name="emailMessage">The email address of the </param>
        /// <param name="emailAttachment">The email attachment of the </param>
        public void SendEmailRequest(string senderEmailAddress, string recipientEmailAddress, string emailMessage, MemoryStream emailAttachment)
        {
            client.SendEmailRequest(senderEmailAddress, recipientEmailAddress, emailMessage, emailAttachment);
        }

        /// <summary>
        /// Gets Sprint stories total story point estimate within sprint
        /// </summary>
        /// <param name="sprintId">the sprint id</param>
        /// <returns>the total story point estimates for a given sprint</returns>
        public double GetTotalStoryPointEstimate(Guid sprintId)
        {
            return client.GetTotalStoryPointEstimate(sprintId);
        }

        #endregion
    }
}
