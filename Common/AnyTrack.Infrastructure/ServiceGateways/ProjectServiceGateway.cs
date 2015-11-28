using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure.BackendProjectService;

namespace AnyTrack.Infrastructure.ServiceGateways
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
        /// Retrieves the project by its id
        /// </summary>
        /// <param name="projectId">id of the project to be retrieved</param>
        /// <param name="storyId">id of the story to be retrieved</param>
        /// <returns>Specified project</returns>
        public ServiceStory GetProjectStory(Guid projectId, Guid storyId)
        {
            return client.GetProjectStory(projectId, storyId);
        }

        /// <summary>
        /// Update specified story in the database
        /// </summary>
        /// <param name="story">The story to be updated</param>
        public void EditStory(ServiceStory story)
        {
            client.EditStory(story);
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
        public List<ServiceProjectRoleSummary> GetLoggedInUserProjectRoleSummaries(string currentUserEmailAddress)
        {
            return client.GetUserProjectRoleSummaries(currentUserEmailAddress);
        }

        /// <summary>
        /// Retrieves any users who match the specified filter.
        /// </summary>
        /// <param name="filter">The search filter.</param>
        /// <returns>Returns a list of users who match the search filter.</returns>
        public List<ServiceUserSearchInfo> SearchUsers(ServiceUserSearchFilter filter)
        {
            return client.SearchUsers(filter);
        }

        /// <summary>
        /// Gets the project names for the current user.
        /// </summary>
        /// <param name="scrumMaster">The scrum master flag.</param>
        /// <param name="productOwner">The PO flag.</param>
        /// <param name="developer">The developer flag.</param>
        /// <returns>The list of project details.</returns>
        public List<ServiceProjectSummary> GetProjectNames(bool scrumMaster, bool productOwner, bool developer)
        {
            return client.GetProjectNames(scrumMaster, productOwner, developer);
        }

        /// <summary>
        /// Retrieves all project stories in the database
        /// </summary>
        /// <param name="projectId">id of the project to be retrieved</param>
        /// <returns>A list of project stories</returns>
        public List<ServiceStorySummary> GetProjectStories(Guid projectId)
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

        /// <summary>
        /// Adds a story to a project
        /// </summary>
        /// <param name="projectGuid">id of project to add tos</param>
        /// <param name="storyGuid">storyid to add to projects</param>
        /// <param name="story">story to add to projects</param>
        public void SaveUpdateStory(Guid projectGuid, Guid storyGuid, ServiceStory story)
        {
            client.SaveUpdateStory(projectGuid, storyGuid, story);
        }

        /// <summary>
        /// Deleting a story from the product backlog
        /// </summary>
        /// <param name="projectId">the projectid to be unlinked and removed</param>
        /// <param name="storyId">the storyid to be unlinked and removed</param>
        public void DeleteStoryFromProductBacklog(Guid projectId, Guid storyId)
        {
            client.DeleteStoryFromProject(projectId, storyId);
        }

        #endregion
    }
}
