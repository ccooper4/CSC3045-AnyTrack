using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Projects.BackendProjectService;

namespace AnyTrack.Projects.ServiceGateways
{
    /// <summary>
    /// Outlines the Project Service Gateway
    /// </summary>
    public interface IProjectServiceGateway
    {
        /// <summary>
        /// Creates a new Project and adds it to the database
        /// </summary>
        /// <param name="project">Project to be inserted</param>
        void CreateProject(ServiceProject project);

        /// <summary>
        /// Updates an existing project in the database
        /// </summary>
        /// <param name="project">Project to be updated</param>
        void UpdateProject(ServiceProject project);

        /// <summary>
        /// Deletes a specidied project in the database
        /// </summary>
        /// <param name="id">Id of the project needing deleted</param>
        void DeleteProject(Guid id);

        /// <summary>
        /// Retrieves a specified Project from the database
        /// </summary>
        /// <param name="id">Id of the project needing retrieved</param>
        /// <returns>Returns the project which had its ID passed in</returns>
        ServiceProject GetProject(Guid id);

        /// <summary>
        /// Retrieves all the projects
        /// </summary>
        /// <returns>Returns a list of all the projects in the system</returns>
        List<ServiceProject> GetProjects();

        /// <summary>
        /// Retrieves all project names in the database
        /// </summary>
        /// <returns>Returns a list of all the project names in the system</returns>
        List<ProjectDetails> GetProjectNames();

        /// <summary>
        /// Retrieves all project stories in the database
        /// </summary>
        /// <param name="projectId">the project id to be checked</param>
        /// <returns>Returns a list of all the stories in the system</returns>
        List<StoryDetails> GetProjectStories(Guid projectId);

        /// <summary>
        /// Retrieves a list containing a summary of the projects the logged in user
        /// is a member of along with the roles they have in them
        /// </summary>
        /// <param name="currentUserEmailAddress">The logged in users email address</param>
        /// <returns>List containg project role summaries</returns>
        List<ProjectRoleSummary> GetLoggedInUserProjectRoleSummaries(string currentUserEmailAddress);

            /// <summary>
        /// Retrieves any users who match the specified filter.
        /// </summary>
        /// <param name="filter">The search filter.</param>
        /// <returns>Returns a list of users who match the search filter.</returns>
        List<UserSearchInfo> SearchUsers(UserSearchFilter filter);

        /// <summary>
        /// Adds a story to a project
        /// </summary>
        /// <param name="projectGuid">Project id</param>
        /// <param name="story">The story to add</param>
        void AddStory(Guid projectGuid, ServiceStory story);
    }
}
