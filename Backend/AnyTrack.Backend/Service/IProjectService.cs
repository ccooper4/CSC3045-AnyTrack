using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Service.Model;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Outlines the Project Service
    /// </summary>
    [ServiceContract]
    public interface IProjectService
    {
        /// <summary>
        /// Adds a Project to the database
        /// </summary>
        /// <param name="project">Project to be added</param>
        [OperationContract]
        void AddProject(ServiceProject project);

        /// <summary>
        /// Update project in the database
        /// </summary>
        /// <param name="project">Project to be updated</param>
        [OperationContract]
        void UpdateProject(ServiceProject project);

        /// <summary>
        /// Delete a project in the database
        /// </summary>
        /// <param name="projectId">ID of the project to be deleted</param>
        [OperationContract]
        void DeleteProject(Guid projectId);

        /// <summary>
        /// Gets a specified project from the database
        /// </summary>
        /// <param name="projectId">ID of the project to be retrieved from the database</param>
        /// <returns>Specified Project</returns>
        [OperationContract]
        ServiceProject GetProject(Guid projectId);

        /// <summary>
        /// Gets all existing projects from the database
        /// </summary>
        /// <returns>List of all Projects in the database</returns>
        [OperationContract]
        List<ServiceProject> GetProjects();
    }
}
