using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Security;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Backend.Service.Model;
using AnyTrack.SharedUtilities.Extensions;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Provides the methods of the ProjectService
    /// </summary>
    public class ProjectService : IProjectService
    {
        #region Fields
        /// <summary>
        /// IUnit of Work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new ProjectService
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public ProjectService(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            this.unitOfWork = unitOfWork;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Adds a Project to the database
        /// </summary>
        /// <param name="project">Project to be added</param>
        public void AddProject(ServiceProject project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            var projectExists = unitOfWork.ProjectRepository.Items.SingleOrDefault(p => p.Id == project.ProjectId);

            if (projectExists != null)
            {
                throw new ArgumentException("Project already exists in database");
            }

            // Assign Project initial values
            Project dataProject = new Project
            {
                Description = project.Description,
                Name = project.Name,
                StartedOn = project.StartedOn,
                VersionControl = project.VersionControl
            };

            // Assign Project Manager
            dataProject.ProjectManager =
                AssignUserRole(dataProject.Id, Thread.CurrentPrincipal.Identity.Name, "Project Manager");

            // Assign Product Owner
            dataProject.ProductOwner = project.ProductOwnerEmailAddress != null
                ? AssignUserRole(dataProject.Id, project.ProductOwnerEmailAddress, "Product Owner")
                : null;

            // Assign Scrum Masters
            dataProject.ScrumMasters = new List<User>();
            foreach (var scrumMasterEmailAddress in project.ScrumMasterEmailAddresses)
            {
                dataProject.ScrumMasters.Add(AssignUserRole(dataProject.Id, scrumMasterEmailAddress, "Scrum Master"));
            }

            unitOfWork.ProjectRepository.Insert(dataProject);

            unitOfWork.Commit();
        }

        /// <summary>
        /// Update project in the database
        /// </summary>
        /// <param name="updatedProject">Project to be updated</param>
        public void UpdateProject(ServiceProject updatedProject)
        {
            if (updatedProject == null)
            {
                throw new ArgumentNullException("updatedProject");
            }

            var project = unitOfWork.ProjectRepository.Items.SingleOrDefault(p => p.Id == updatedProject.ProjectId);

            if (project == null)
            {
                throw new ArgumentException("Project does not exist in database");
            }

            project.Name = updatedProject.Name;
            project.Description = updatedProject.Description;
            project.StartedOn = project.StartedOn;
            project.VersionControl = updatedProject.VersionControl;

            // Assign Scrum Master
            if (project.ScrumMasters != null)
            {
                foreach (var scrumMaster in project.ScrumMasters)
                {
                    if (!updatedProject.ScrumMasterEmailAddresses.Contains(scrumMaster.EmailAddress))
                    {
                        project.ScrumMasters.Remove(UnassignUserRole(project.Id, scrumMaster.EmailAddress, "Scrum Master"));
                    }
                }

                foreach (var updatedScrumMasterEmailAddress in updatedProject.ScrumMasterEmailAddresses)
                {
                    if (!project.ScrumMasters.Contains(MapEmailAddressToUser(updatedScrumMasterEmailAddress)))
                    {
                        project.ScrumMasters.Add(AssignUserRole(project.Id, updatedScrumMasterEmailAddress, "Scrum Master"));
                    }
                }
            }
            else
            {
               project.ScrumMasters = new List<User>();

               foreach (var updatedScrumMasterEmailAddress in updatedProject.ScrumMasterEmailAddresses)
                {
                    project.ScrumMasters.Add(AssignUserRole(project.Id, updatedScrumMasterEmailAddress, "Scrum Master"));
                }
            }

            unitOfWork.Commit();
        }

        /// <summary>
        /// Delete a project in the database
        /// </summary>
        /// <param name="projectId">ID of the project to be deleted</param>
        public void DeleteProject(Guid projectId)
        {
            var project = unitOfWork.ProjectRepository.Items.SingleOrDefault(p => p.Id == projectId);

            if (project == null)
            {
                throw new ArgumentException("Project does not exist");
            }

            UnassignUserRole(projectId, project.ProjectManager.EmailAddress, "Project Manager");

            if (project.ProductOwner != null)
            {
                UnassignUserRole(projectId, project.ProductOwner.EmailAddress, "Product Owner");
            }

            if (project.ScrumMasters != null)
            {
                foreach (var scrumMaster in project.ScrumMasters)
                {
                    UnassignUserRole(projectId, scrumMaster.EmailAddress, "Scrum Master");
                }
            }

            unitOfWork.ProjectRepository.Delete(project);
            unitOfWork.Commit();
        }

        /// <summary>
        /// Gets a specified project from the database
        /// </summary>
        /// <param name="projectId">ID of the project to be retrieved from the database</param>
        /// <returns>Specified Project</returns>
        public ServiceProject GetProject(Guid projectId)
        {
            var dataProject = unitOfWork.ProjectRepository.Items.SingleOrDefault(p => p.Id == projectId);

            if (dataProject == null)
            {
                throw new ArgumentException("Project does not exist");
            }

            ServiceProject project = new ServiceProject
            {
                Description = dataProject.Description,
                ProjectId = dataProject.Id,
                Name = dataProject.Name,          
                StartedOn = dataProject.StartedOn,
                VersionControl = dataProject.VersionControl
            };

            project.ProjectManagerEmailAddress = dataProject.ProjectManager.EmailAddress;
            project.ProductOwnerEmailAddress = dataProject.ProductOwner != null ? dataProject.ProductOwner.EmailAddress : null;

            if (dataProject.ScrumMasters != null)
            {
                foreach (var scrumMaster in dataProject.ScrumMasters)
                {
                    project.ScrumMasterEmailAddresses.Add(scrumMaster.EmailAddress);
                }
            }

            if (dataProject.Stories != null)
            {
                foreach (var story in dataProject.Stories)
                {
                    project.Stories.Add(new Service.Model.ServiceStory
                    {
                        StoryId = story.Id,
                        Summary = story.Summary,
                        ConditionsOfSatisfaction = story.ConditionsOfSatisfaction,
                        AsA = story.AsA,
                        IWant = story.IWant,
                        SoThat = story.IWant
                    });
                }
            }

            return project;
        }

        /// <summary>
        /// Obtains a list of project names with respective id
        /// </summary>
        /// <returns>a list of projects</returns>
        public List<ProjectDetails> GetProjectNames()
        {
            var projects = unitOfWork.ProjectRepository.Items.Select(p => new ProjectDetails
            {
                ProjectId = p.Id,
                ProjectName = p.Name
            }).ToList();
            return projects;
        }

        /// <summary>
        /// Method to get the project stories
        /// </summary>
        /// <param name="projectId">projectId to be checked</param>
        /// <returns>a list of stories</returns>
        public List<StoryDetails> GetProjectStoryDetails(Guid projectId)
        {
            var stories = unitOfWork.StoryRepository.Items.Where(s => s.Project.Id == projectId).Select(s => new StoryDetails
            {
                StoryId = s.Id,
                Summary = s.Summary
            });
            return stories.ToList();
        }

        /// <summary>
        /// Gets all existing projects from the database
        /// </summary>
        /// <returns>List of all Projects in the database</returns>
        public List<ServiceProject> GetProjects()
        {
            var projects = unitOfWork.ProjectRepository.Items.Select(p => new ServiceProject
            {
                ProjectId = p.Id,
                Description = p.Description,
                Name = p.Name,
                VersionControl = p.VersionControl,
                StartedOn = p.StartedOn,
            }).ToList();

            foreach (ServiceProject project in projects)
            {
                var dataProject = unitOfWork.ProjectRepository.Items.Single(p => p.Id == project.ProjectId);
                project.Stories = new List<Service.Model.ServiceStory>();
                project.ProjectManagerEmailAddress = dataProject.ProjectManager.EmailAddress;
                project.ProductOwnerEmailAddress = dataProject.ProductOwner != null ? dataProject.ProductOwner.EmailAddress : null;

                if (dataProject.ScrumMasters != null)
                {
                    foreach (var scrumMaster in dataProject.ScrumMasters)
                    {
                        project.ScrumMasterEmailAddresses.Add(scrumMaster.EmailAddress);
                    }
                }

                if (dataProject.Stories != null)
                {
                    foreach (var story in dataProject.Stories)
                    {
                    project.Stories.Add(new Service.Model.ServiceStory
                        {
                            Summary = story.Summary,
                            ConditionsOfSatisfaction = story.ConditionsOfSatisfaction,
                        });
                    }
                }
            }

            return projects;
        }

        /// <summary>
        /// Searches for users who can be added to a project.
        /// </summary>
        /// <param name="filter">The user filter.</param>
        /// <returns>A list of user information objects.</returns>
        public List<UserSearchInfo> SearchUsers(UserSearchFilter filter)
        {
            var users = unitOfWork.UserRepository.Items;

            if (filter.EmailAddress.IsNotEmpty())
            {
                users = users.Where(u => u.EmailAddress == filter.EmailAddress);
            }

            if (filter.ProductOwner.HasValue)
            {
                users = users.Where(u => u.ProductOwner == filter.ProductOwner);
            }

            if (filter.ScrumMaster.HasValue)
            {
                users = users.Where(u => u.ScrumMaster == filter.ScrumMaster);
            }

            var userInfos = users.Select(u => new UserSearchInfo
            {
                EmailAddress = u.EmailAddress,
                FullName = u.FirstName + " " + u.LastName,
                UserID = u.Id
            }).OrderBy(u => u.FullName);

            return userInfos.ToList();
        }

        /// <summary>
        /// Adds a story to the database and associates it with the specified project.
        /// </summary>
        /// <param name="projectGuid">The Guid of the project to add stories to</param>
        /// <param name="story">The story to add to the project</param>
        public void AddStoryToProject(Guid projectGuid, ServiceStory story)
        {
            var project = unitOfWork.ProjectRepository.Items.SingleOrDefault(p => p.Id == projectGuid);

            if (project == null)
            {
                throw new ArgumentException("Project not found for Guid" + projectGuid.ToString());
            }

            var storyExists = unitOfWork.StoryRepository.Items.SingleOrDefault(s => s.Id == story.StoryId);
            if (storyExists != null)
            {
                throw new ArgumentException("Story already exists in database");
            }

            Story newStory = new Story()
            {
                Summary = story.Summary,
                ConditionsOfSatisfaction = story.ConditionsOfSatisfaction,
                AsA = story.AsA,
                IWant = story.IWant,
                SoThat = story.SoThat
            };
            
            project.Stories.Add(newStory);
            unitOfWork.Commit();
        }

        /// <summary>
        /// Deleting a story from the product backlog
        /// </summary>
        /// <param name="projectId">the projectid to be unlinked and removed</param>
        /// <param name="storyId">the storyid to be unlinked and removed</param>
        public void DeleteStoryFromProductBacklog(Guid projectId, Guid storyId)
        {
            var storyEntity = unitOfWork.StoryRepository.Items.Single(s => s.Id == storyId);
            var projectEntity = unitOfWork.ProjectRepository.Items.Single(p => p.Id == projectId);
            projectEntity.Stories.Remove(storyEntity);
            unitOfWork.StoryRepository.Delete(storyEntity);
            unitOfWork.Commit();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Retrieves user from database with email address and assigns them a role on the project
        /// </summary>
        /// <param name="projectId">Id of project to be added to</param>
        /// <param name="emailAddress">users email address</param>
        /// <param name="roleName">Name of role in project</param>
        /// <returns>user with added role</returns>
        private User AssignUserRole(Guid projectId, string emailAddress, string roleName)
        {
            User user =
                        unitOfWork.UserRepository.Items.SingleOrDefault(
                            u => u.EmailAddress == emailAddress);

            if (user != null)
            {
                // Create role
                Role role = new Role
                {
                    ProjectID = projectId,
                    RoleName = roleName,
                    User = user
                };

                // Add role to user
                if (user.Roles == null)
                {
                    user.Roles = new List<Role>();
                }

                if (!user.Roles.Contains(role))
                {
                    user.Roles.Add(role);
                }

                return user;
            }
            else
            {
                throw new Exception("User does not exist");
            }
        }

        /// <summary>
        /// Retrieves user from database with email address and unassigns them from role on the project
        /// </summary>
        /// <param name="projectId">Id of project to be added to</param>
        /// <param name="emailAddress">users email address</param>
        /// <param name="roleName">Name of role in project</param>
        /// <returns>user with added role</returns>
        private User UnassignUserRole(Guid projectId, string emailAddress, string roleName)
        {
            User user =
                        unitOfWork.UserRepository.Items.SingleOrDefault(
                            u => u.EmailAddress == emailAddress);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }

            Role role =
                        unitOfWork.RoleRepository.Items.SingleOrDefault(r => r.RoleName == roleName && r.User == user && r.ProjectID == projectId);

            if (role == null)
            {
                throw new Exception("Role does not exist for user in project");
            }

            user.Roles.Remove(role);

            return user;
        }

        /// <summary>
        /// Maps a New User to User
        /// </summary>
        /// <param name="emailAddress">Users email address</param>
        /// <returns>Returns a converted User</returns>
        private User MapEmailAddressToUser(string emailAddress)
        {
            User user =
                        unitOfWork.UserRepository.Items.SingleOrDefault(
                            u => u.EmailAddress == emailAddress);

            if (user == null)
            {
                throw new NullReferenceException(string.Format("User with the email address {0} does not exist in the User Repository", emailAddress));
            }

            return user;
        }

        #endregion
    }
}
