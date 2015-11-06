using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Projects.BackendProjectService;

namespace AnyTrack.Projects.ServiceGateways
{
    /// <summary>
    /// Provides a gateway to the Project Service
    /// </summary>
    public class ProjectServiceGateway : IProjectServiceGateway
    {
        #region Fields
        /// <summary>
        /// The web client
        /// </summary>
        private readonly IProjectService client;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new project service gateway
        /// </summary>
        /// <param name="client">The web client</param>
        public ProjectServiceGateway(IProjectService client)
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
        /// Adds the provided project to the database
        /// </summary>
        /// <param name="project">project to be added to the database</param>
        public void CreateProject(ServiceProject project)
        {
            client.AddProject(project);
        }

        /// <summary>
        /// Update specified project in the database
        /// </summary>
        /// <param name="project">The project to be updated</param>
        public void UpdateProject(ServiceProject project)
        {
            client.UpdateProject(project);
        }

        /// <summary>
        /// Deletes the specified project from the database
        /// </summary>
        /// <param name="id">id of the project to be deleted</param>
        public void DeleteProject(Guid id)
        {
            client.DeleteProject(id);
        }

        /// <summary>
        /// Retrieves the project by its id
        /// </summary>
        /// <param name="id">id of the project to be retrieved</param>
        /// <returns>Specified project</returns>
        public ServiceProject GetProject(Guid id)
        {
            return client.GetProject(id);
        }

        /// <summary>
        /// Retrieves all projects in the database
        /// </summary>
        /// <returns>A list of all protects</returns>
        public List<ServiceProject> GetProjects()
        {
            return client.GetProjects();
        }

        /// <summary>
        /// Retrieves a list containing a summary of the projects the logged in user
        /// is a member of along with the roles they have in them
        /// </summary>
        /// <param name="currentUserEmailAddress">The logged in users email address</param>
        /// <returns>List containg project role summaries</returns>
        public List<ProjectRoleSummary> GetLoggedInUserProjectRoleSummaries(string currentUserEmailAddress)
        {
            return client.GetUserProjectRoleSummaries(currentUserEmailAddress);
        }

        /// <summary>
        /// Retrieves any users who match the specified filter.
        /// </summary>
        /// <param name="filter">The search filter.</param>
        /// <returns>Returns a list of users who match the search filter.</returns>
        public List<UserSearchInfo> SearchUsers(UserSearchFilter filter)
        {
            return client.SearchUsers(filter);
        }

        /// <summary>
        /// Retrieves all project names in the database
        /// </summary>
        /// <returns>A list of project names </returns>
        public List<ProjectDetails> GetProjectNames()
        {
            return client.GetProjectNames();
        }

        /// <summary>
        /// Retrieves all project stories in the database
        /// </summary>
        /// <param name="projectId">id of the project to be retrieved</param>
        /// <returns>A list of project stories</returns>
        public List<StoryDetails> GetProjectStories(Guid projectId)
        {
            return client.GetProjectStoryDetails(projectId);
        }

        /// <summary>
        /// Adds a story to a project
        /// </summary>
        /// <param name="projectGuid">id of project to add to</param>
        /// <param name="story">story to add to project</param>
        public void AddStory(Guid projectGuid, ServiceStory story)
        {
            client.AddStoryToProject(projectGuid, story);
        }

        #endregion
    }
}
