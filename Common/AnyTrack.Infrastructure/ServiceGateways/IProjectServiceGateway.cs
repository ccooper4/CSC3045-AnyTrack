using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure.BackendProjectService;

namespace AnyTrack.Infrastructure.ServiceGateways
{
    /// <summary>
    /// Outlines the Project Service Gateway
    /// </summary>
    public interface IProjectServiceGateway
    {
        /// <summary>
        /// Creates a new Project and adds it to the database.
        /// </summary>
        /// <param name="project">Project to be inserted</param>
        void CreateProject(ServiceProject project);

        /// <summary>
        /// Updates an existing project in the database.
        /// </summary>
        /// <param name="project">Project to be updated</param>
        void UpdateProject(ServiceProject project);

        /// <summary>
        /// Deletes a specidied project in the database.
        /// </summary>
        /// <param name="id">Id of the project needing deleted</param>
        void DeleteProject(Guid id);

        /// <summary>
        /// Retrieves a specified Project from the database.
        /// </summary>
        /// <param name="id">Id of the project needing retrieved</param>
        /// <returns>Returns the project which had its ID passed in</returns>
        ServiceProject GetProject(Guid id);

        /// <summary>
        /// Retrieves all the projects.
        /// </summary>
        /// <returns>Returns a list of all the projects in the system</returns>
        List<ServiceProject> GetProjects();

        /// <summary>
        /// Gets the project names for the current user.
        /// </summary>
        /// <param name="scrumMaster">The scrum master flag.</param>
        /// <param name="productOwner">The PO flag.</param>
        /// <param name="developer">The developer flag.</param>
        /// <returns>The list of project details.</returns>
        List<ServiceProjectSummary> GetProjectNames(bool scrumMaster, bool productOwner, bool developer);

        /// <summary>
        /// Retrieves all project stories in the database.
        /// </summary>
        /// <param name="projectId">the project id to be checked</param>
        /// <returns>Returns a list of all the stories in the system</returns>
        List<ServiceStorySummary> GetProjectStories(Guid projectId);

        /// <summary>
        /// Retrieves a list containing a summary of the projects the logged in user
        /// is a member of along with the roles they have in them.
        /// </summary>
        /// <param name="currentUserEmailAddress">The logged in users email address</param>
        /// <returns>List containg project role summaries</returns>
        List<ServiceProjectRoleSummary> GetLoggedInUserProjectRoleSummaries(string currentUserEmailAddress);

            /// <summary>
        /// Retrieves any users who match the specified filter.
        /// </summary>
        /// <param name="filter">The search filter.</param>
        /// <returns>Returns a list of users who match the search filter.</returns>
        List<ServiceUserSearchInfo> SearchUsers(ServiceUserSearchFilter filter);

        /// <summary>
        /// Updates an existing story in the database.
        /// </summary>
        /// <param name="story">Story to be updated</param>
        void EditStory(ServiceStory story);

        /// <summary>
        /// Adds a story to a project.
        /// </summary>
        /// <param name="projectGuid">Project id</param>
        /// <param name="story">The story to add</param>
        void AddStory(Guid projectGuid, ServiceStory story);

        /// <summary>
        /// Retrieves the project by its id.
        /// </summary>
        /// <param name="projectId">id of the project to be retrieved</param>
        /// <param name="storyId">id of the story to be retrieved</param>
        /// <returns>Specified project</returns>
        ServiceStory GetProjectStory(Guid projectId, Guid storyId);

        /// <summary>
        /// Adds or updates a story to/in the database and associates it with the specified project.
        /// </summary>
        /// <param name="projectId">The Guid of the project to add stories to</param>
        /// <param name="storyId">The Guid of the story to add stories to</param>
        /// <param name="story">The story to add/update</param>
        void SaveUpdateStory(Guid projectId, Guid storyId, ServiceStory story);

        /// <summary>
        /// Deleting a story from the product backlog.
        /// </summary>
        /// <param name="projectId">the projectid to be unlinked and removed</param>
        /// <param name="storyId">the storyid to be unlinked and removed</param>
        void DeleteStoryFromProductBacklog(Guid projectId, Guid storyId);
    }
}
