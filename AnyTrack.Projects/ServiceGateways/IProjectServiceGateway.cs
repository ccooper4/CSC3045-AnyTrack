using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Projects.Service_References.BackendProjectService;

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
        void CreateProject(Project project);

        /// <summary>
        /// Updates an existing project in the database
        /// </summary>
        /// <param name="project">Project to be updated</param>
        void UpdateProject(Project project);

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
        Project GetProject(Guid id);

        /// <summary>
        /// Retrieves all the projects
        /// </summary>
        /// <returns>Returns a list of all the projects in the system</returns>
        List<Project> GetProjects();
    }
}
