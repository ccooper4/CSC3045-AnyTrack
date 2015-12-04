using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Backend.Service.Model;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Outlines the sprint service.
    /// </summary>
    [ServiceContract]
    public interface ISprintService
    {
        /// <summary>
        /// Creates a sprint and adds it to the project.
        /// </summary>
        /// <param name="projectId">Id of the project to add the sprint to</param>
        /// <param name="sprint">ServiceSprint entity</param>
        [OperationContract]
        void AddSprint(Guid projectId, ServiceSprint sprint);

        /// <summary>
        /// Edits an existing sprint.
        /// </summary>
        /// <param name="sprintId">Id of the sprint to be edited</param>
        /// <param name="updatedSprint">ServiceSprint entity containing changes</param>
        [OperationContract]
        void EditSprint(Guid sprintId, ServiceSprint updatedSprint);

        /// <summary>
        /// Retrieves a specified sprint.
        /// </summary>
        /// <param name="sprintId">Id of the sprint</param>
        /// <returns>The sprint</returns>
        [OperationContract]
        ServiceSprint GetSprint(Guid sprintId);

        /// <summary>
        /// Gets all task for a sprint for the current user
        /// </summary>
        /// <param name="sprintId">The sprint id</param>
        /// <returns>Returns a list of tasks</returns>
        [OperationContract]
        List<ServiceTask> GetAllTasksForSprintCurrentUser(Guid sprintId);

        /// <summary>
        /// Gets all tasks for the burndown
        /// </summary>
        /// <param name="sprintId">The sprint id</param>
        /// <returns>Returns a list of tasks</returns>
        [OperationContract]
        List<ServiceTask> GetAllTasksForSprint(Guid sprintId);

        /// <summary>
        /// Method to save the update hours for tasks
        /// </summary>
        /// <param name="tasks">List of tasks to save</param>
        [OperationContract]
        void SaveUpdatedTaskHours(List<ServiceTask> tasks);

        /// <summary>
        /// Add the initial task hour estimate to a task. 
        /// </summary>
        /// <param name="taskId"> the id of the task</param>
        /// <param name="serviceTaskHourEstimate"> the task hour estimate </param>
        [OperationContract]
        void AddTaskHourEstimateToTask(Guid taskId, ServiceTaskHourEstimate serviceTaskHourEstimate);

        /// <summary>
        /// Add a new task to a sprint story.
        /// </summary>
        /// <param name="sprintStoryId">The story to add the task to.</param>
        /// <param name="serviceTask">The task to add.</param>
        [OperationContract]
        void AddTaskToSprintStory(Guid sprintStoryId, ServiceTask serviceTask);

        /// <summary>
        /// Gets all sprints for the current user
        /// </summary>
        /// <param name="projectId">The id of the project.</param>
        /// <param name="scrumMaster">A flag indicating if sprints where this user is a scrummaster should be returned.</param>
        /// <param name="developer">A flag indicating if sprints where this user is a developer should be returned.</param>
        /// <returns>A summary list of this user's sprints.</returns>
        [OperationContract]
        List<ServiceSprintSummary> GetSprintNames(Guid? projectId, bool scrumMaster, bool developer);

        /// <summary>
        /// Gets all the stories associated with a sprintISprintService
        /// </summary>
        /// <param name="sprintId">Sprint id to retrieve sprints from</param>
        /// <returns>List of sprint stories</returns>
        [OperationContract]
        List<ServiceSprintStory> GetSprintStories(Guid sprintId);

        /// <summary>
        /// Gets the start date associated with a sprint.
        /// </summary>
        /// <param name="sprintId">the sprint id</param>
        /// <returns>the start date of the sprint</returns>
        [OperationContract]
        DateTime? GetStartDateOfSprint(Guid sprintId);

        /// <summary>
        /// Gets the end date associated with a sprint.
        /// </summary>
        /// <param name="sprintId">the sprint id</param>
        /// <returns>the end date of the sprint</returns>
        [OperationContract]
        DateTime? GetEndDateOfSprint(Guid sprintId);

        /// <summary>
        /// Gets the maximum estimate of the sprint tasks in a given sprint
        /// </summary>
        /// <param name="sprintId">the sprint id</param>
        /// <returns>the maximum value of the estimate in a given sprint</returns>
        [OperationContract]
        double GetMaxEstimateOfSprint(Guid sprintId);

        /// <summary>
        /// Gets the total story point estimate of a sprint
        /// </summary>
        /// <param name="sprintId">the sprint id</param>
        /// <returns>the total value of the story estimates summed together</returns>
        [OperationContract]
        double GetTotalStoryPointEstimate(Guid sprintId);
       
        /// <summary>
        /// Retrieves Sprint stories for this sprint with estimates
        /// </summary>
        /// <param name="sprintId">Id of the sprint</param>
        /// <returns>List of Service sprint stories containing</returns>
        [OperationContract]
        List<ServiceSprintStory> GetSprintStoryEstimates(Guid sprintId);

        /// <summary>
        /// Manages the sprint backlog
        /// </summary>aram>
        /// <param name="projectId">Project id of the project</param>
        /// <param name="sprintId">Sprint id of sprint</param>
        /// <param name="sprintStories">List of sprint stories</param>
        [OperationContract]
        void ManageSprintBacklog(Guid projectId, Guid sprintId, List<ServiceSprintStory> sprintStories);

        /// <summary>
        /// Sending an email request
        /// </summary>
        /// <param name="senderEmailAddress">The email adddress to send the email to</param>
        /// <param name="recipientEmailAddress">The email adddress where the email is sent from</param>
        /// <param name="emailMessage">The email address of the </param>
        /// <param name="emailAttachment">The email attachment of the </param>
        [OperationContract]
        void SendEmailRequest(string senderEmailAddress, string recipientEmailAddress, string emailMessage, MemoryStream emailAttachment);
    }
}
