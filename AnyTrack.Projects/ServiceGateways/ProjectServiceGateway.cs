﻿using System;
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
        /// Retrieves any users who match the specified filter.
        /// </summary>
        /// <param name="filter">The search filter.</param>
        /// <returns>Returns a list of users who match the search filter.</returns>
        public List<UserSearchInfo> SearchUsers(UserSearchFilter filter)
        {
            return client.SearchUsers(filter);
        }

        #endregion
    }
}