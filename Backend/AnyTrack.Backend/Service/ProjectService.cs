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
            dataProject.ProductOwner = project.ProductOwner != null
                ? AssignUserRole(dataProject.Id, project.ProductOwner.EmailAddress, "Product Owner")
                : null;

            // Assign Scrum Masters
            dataProject.ScrumMasters = new List<User>();
            foreach (var scrumMaster in project.ScrumMasters)
            {
                dataProject.ScrumMasters.Add(AssignUserRole(dataProject.Id, scrumMaster.EmailAddress, "Scrum Master"));
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

            if (project.ProductOwner.EmailAddress != updatedProject.ProductOwner.EmailAddress)
            {
                if (project.ProductOwner.EmailAddress != null)
                {
                    UnassignUserRole(project.Id, project.ProductOwner.EmailAddress, "Product Owner");
                }

                project.ProductOwner = AssignUserRole(project.Id, updatedProject.ProductOwner.EmailAddress, "Product Owner");
            }

            // Assign Scrum Master
            foreach (var scrumMaster in project.ScrumMasters)
            {
                if (!updatedProject.ScrumMasters.Contains(MapUserToNewUser(scrumMaster)))
                {
                    project.ScrumMasters.Remove(UnassignUserRole(project.Id, scrumMaster.EmailAddress, "Scrum Master"));
                }
            }

            foreach (var updatedScrumMaster in updatedProject.ScrumMasters)
            {
                if (!project.ScrumMasters.Contains(MapNewUserToUser(updatedScrumMaster)))
                {
                    project.ScrumMasters.Add(AssignUserRole(project.Id, updatedScrumMaster.EmailAddress, "Scrum Master"));
                }
            }

            // Assign Scrum Master
            foreach (var scrumMaster in updatedProject.ScrumMasters)
            {
                project.ScrumMasters.Add(new User
                {
                    EmailAddress = scrumMaster.EmailAddress,
                    Password = scrumMaster.Password,
                    FirstName = scrumMaster.FirstName,
                    LastName = scrumMaster.LastName,
                });
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

            UnassignUserRole(projectId, project.ProductOwner.EmailAddress, "Product Owner");
            UnassignUserRole(projectId, project.ProjectManager.EmailAddress, "Project Manager");

            foreach (var scrumMaster in project.ScrumMasters)
            {
                UnassignUserRole(projectId, scrumMaster.EmailAddress, "Scrum Master");
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
                ProjectManager = MapUserToNewUser(dataProject.ProjectManager),
                StartedOn = dataProject.StartedOn,
                VersionControl = dataProject.VersionControl
            };

            project.ProductOwner = dataProject.ProductOwner != null ? MapUserToNewUser(dataProject.ProductOwner) : null;

            if (dataProject.ScrumMasters != null)
            {
                foreach (var scrumMaster in dataProject.ScrumMasters)
                {
                    project.ScrumMasters.Add(MapUserToNewUser(scrumMaster));
                }
            }

            return project;
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
                ProjectManager = MapUserToNewUser(p.ProjectManager),
            }).ToList();

            foreach (ServiceProject project in projects)
            {
                var dataProject = unitOfWork.ProjectRepository.Items.Single(p => p.Id == project.ProjectId);

                project.ProductOwner = dataProject.ProductOwner != null ? MapUserToNewUser(dataProject.ProductOwner) : null;

                if (dataProject.ScrumMasters != null)
                {
                    foreach (var scrumMaster in dataProject.ScrumMasters)
                    {
                        project.ScrumMasters.Add(MapUserToNewUser(scrumMaster));
                    }
                }
            }

            return projects;
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
        /// Converts a User to a NewUser datatype
        /// </summary>
        /// <param name="user">User to be converted</param>
        /// <returns>Converted NewUser</returns>
        private NewUser MapUserToNewUser(User user)
        {
            return new NewUser
            {
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Developer = user.Developer,
                ProductOwner = user.ProductOwner,
                ScrumMaster = user.ScrumMaster,
                Skills = user.Skills,
                SecretQuestion = user.SecretQuestion,
                SecretAnswer = user.SecretAnswer
            };
        }

        /// <summary>
        /// Maps a New User to User
        /// </summary>
        /// <param name="user">NewUser to be converted</param>
        /// <returns>Returns a converted User</returns>
        private User MapNewUserToUser(NewUser user)
        {
            return new User
            {
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Developer = user.Developer,
                ProductOwner = user.ProductOwner,
                ScrumMaster = user.ScrumMaster,
                Skills = user.Skills,
                SecretQuestion = user.SecretQuestion,
                SecretAnswer = user.SecretAnswer
            };
        }

        #endregion
    }
}
