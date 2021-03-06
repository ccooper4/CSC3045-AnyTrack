﻿using System;
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
        /// Adds a Project to the database.
        /// </summary>
        /// <param name="project">Project to be added</param>
        [OperationContract]
        void AddProject(ServiceProject project);

        /// <summary>
        /// Update project in the database.
        /// </summary>
        /// <param name="updatedProject">Project to be updated</param>
        [OperationContract]
        void UpdateProject(ServiceProject updatedProject);

        /// <summary>
        /// Delete a project in the database.
        /// </summary>
        /// <param name="projectId">ID of the project to be deleted</param>
        [OperationContract]
        void DeleteProject(Guid projectId);

        /// <summary>
        /// Deleting a story from the project.
        /// </summary>
        /// <param name="projectId">the projectid to be unlinked and removed</param>
        /// <param name="storyId">the storyid to be unlinked and removed</param>
        [OperationContract]
        void DeleteStoryFromProject(Guid projectId, Guid storyId);

        /// <summary>
        /// Gets a specified project from the database.
        /// </summary>
        /// <param name="projectId">ID of the project to be retrieved from the database</param>
        /// <returns>Specified Project</returns>
        [OperationContract]
        ServiceProject GetProject(Guid projectId);

        /// <summary>
        /// Gets all existing projects from the database.
        /// </summary>
        /// <returns>List of all Projects in the database</returns>
        [OperationContract]
        List<ServiceProject> GetProjects();

        /// <summary>
        /// Searches for users who can be added to a project.
        /// </summary>
        /// <param name="filter">The user filter.</param>
        /// <returns>A list of user information objects.</returns>
        [OperationContract]
        List<ServiceUserSearchInfo> SearchUsers(ServiceUserSearchFilter filter);

        /// <summary>
        /// Returns the list of project names that this user can see.
        /// </summary>
        /// <param name="scrumMaster">The Scrum master flag.</param>
        /// <param name="productOwner">The PO flag.</param>
        /// <param name="developer">The developer flag.</param>
        /// <returns>A list of project detail models.</returns>
        [OperationContract]
        List<ServiceProjectSummary> GetProjectNames(bool scrumMaster, bool productOwner, bool developer);

        /// <summary>
        /// Get all stories based on project from the database.
        /// </summary>
        /// <param name="projectId">Project id to be checked</param>
        /// <returns>A list of stories for a project</returns>
        [OperationContract]
        List<ServiceStorySummary> GetProjectStoryDetails(Guid projectId);

        /// <summary>
        /// Adds a story to a project.
        /// </summary>
        /// <param name="projectGuid">takes in the project id</param>
        /// <param name="story">takes in the story to add</param>
        [OperationContract]
        void AddStoryToProject(Guid projectGuid, ServiceStory story);

        /// <summary>
        /// Retrieves the logged in users projects that they are a member of.
        /// </summary>
        /// <param name="currentUserEmailAddress">The email of the currently logged in user</param>
        /// <returns>List of the users project role details</returns>
        [OperationContract]
        List<ServiceProjectRoleSummary> GetUserProjectRoleSummaries(string currentUserEmailAddress);

        /// <summary>
        /// Gets a specified project from the database.
        /// </summary>
        /// <param name="projectId">ID of the project to be retrieved from the database</param>
        /// <param name = "storyId" > ID of the story to be retrieved from the database</param>
        /// <returns>Specified Project</returns>
        [OperationContract]
        ServiceStory GetProjectStory(Guid projectId, Guid storyId);

        /// <summary>
        /// Update story in the database.
        /// </summary>
        /// <param name="story">story to be updated</param>
        [OperationContract]
        void EditStory(ServiceStory story);

        /// <summary>
        /// Update story in the database.
        /// </summary>        
        /// <param name="projectId">ID of the project to be retrieved from the database</param>
        /// <param name = "storyId" > ID of the story to be retrieved from the database</param>
        /// <param name="story">story to be updated</param>
        [OperationContract]
        void SaveUpdateStory(Guid projectId, Guid storyId, ServiceStory story);
    }
}
