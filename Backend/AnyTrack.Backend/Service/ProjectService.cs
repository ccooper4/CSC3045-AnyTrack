using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Security;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Backend.Providers;
using AnyTrack.Backend.Security;
using AnyTrack.Backend.Service.Model;
using AnyTrack.SharedUtilities.Extensions;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Provides the methods of the ProjectService
    /// </summary>
    [CreatePrincipal]
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
        /// Creates a new ProjectService.
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
                AssignUserRole(dataProject.Id, project.ProjectManagerEmailAddress, "Project Manager");

            // Assign Product Owner
            dataProject.ProductOwner = project.ProductOwnerEmailAddress != null
                ? AssignUserRole(dataProject.Id, project.ProductOwnerEmailAddress, "Product Owner")
                : null;

            // Assign Scrum Masters
            dataProject.ScrumMasters = new List<User>();
            if (project.ScrumMasterEmailAddresses != null)
            {
                foreach (var scrumMasterEmailAddress in project.ScrumMasterEmailAddresses)
                {
                    dataProject.ScrumMasters.Add(AssignUserRole(dataProject.Id, scrumMasterEmailAddress, "Scrum Master"));
                }
            }

            unitOfWork.ProjectRepository.Insert(dataProject);

            unitOfWork.Commit();
        }

        /// <summary>
        /// Update project in the database.
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
            project.StartedOn = updatedProject.StartedOn;
            project.VersionControl = updatedProject.VersionControl;

            // Assign Scrum Master
            if (project.ScrumMasters != null)
            {
                // Remove no longer present scrum masters
                foreach (var scrumMaster in project.ScrumMasters)
                {
                    if (!updatedProject.ScrumMasterEmailAddresses.Contains(scrumMaster.EmailAddress))
                    {
                        project.ScrumMasters.Remove(UnassignUserRole(project.Id, scrumMaster.EmailAddress, "Scrum Master"));
                    }
                }

                // Add new scrum masters
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
        /// Delete a project in the database.
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
        /// Gets a specified project from the database.
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
                    project.Stories.Add(new ServiceStory
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

            if (dataProject.Sprints != null)
            {
                foreach (var sprint in dataProject.Sprints)
                {
                    var serviceSprint = new ServiceSprint
                    {
                        SprintId = sprint.Id,
                        Name = sprint.Name,
                        Description = sprint.Description,
                        StartDate = sprint.StartDate,
                        EndDate = sprint.EndDate
                    };

                    if (sprint.Backlog != null)
                    {
                        foreach (var sprintStory in sprint.Backlog)
                        {
                            serviceSprint.Backlog.Add(new ServiceSprintStory
                            {
                                SprintStoryId = sprintStory.Id,
                                Story = new ServiceStory()
                                {
                                    StoryId = sprintStory.Story.Id,
                                    Summary = sprintStory.Story.Summary,
                                    ConditionsOfSatisfaction = sprintStory.Story.ConditionsOfSatisfaction,
                                    AsA = sprintStory.Story.AsA,
                                    IWant = sprintStory.Story.IWant,
                                    SoThat = sprintStory.Story.IWant
                                }                             
                            });
                        }
                    }

                    if (sprint.Team != null)
                    {
                        foreach (var teamMember in sprint.Team)
                        {
                            serviceSprint.TeamEmailAddresses.Add(teamMember.EmailAddress);
                        }
                    }

                    project.Sprints.Add(serviceSprint);
                }    
            }

            return project;
        }

        /// <summary>
        /// Gets a specified project from the database.
        /// </summary>
        /// <param name="projectId">ID of the project to be retrieved from the database</param>
        /// <param name = "storyId" > ID of the story to be retrieved from the database</param>
        /// <returns>Specified Project</returns>
        public ServiceStory GetProjectStory(Guid projectId, Guid storyId)
        {
            var dataProject = unitOfWork.ProjectRepository.Items.SingleOrDefault(p => p.Id == projectId);

            if (dataProject == null)
            {
                throw new ArgumentException("Project does not exist");
            }

            var dataStory = dataProject.Stories.SingleOrDefault(p => p.Id == storyId);

            if (dataStory == null)
            {
                throw new ArgumentException("Story does not exist");
            }

            ServiceStory story = new ServiceStory
            {
                StoryId = dataStory.Id,
                Summary = dataStory.Summary,
                ConditionsOfSatisfaction = dataStory.ConditionsOfSatisfaction,
                AsA = dataStory.AsA,
                IWant = dataStory.IWant,
                SoThat = dataStory.SoThat
            };           

            return story;
        }

        /// <summary>
        /// Returns the list of project names that this user can see.
        /// </summary>
        /// <param name="scrumMaster">The Scrum master flag.</param>
        /// <param name="productOwner">The PO flag.</param>
        /// <param name="developer">The developer flag.</param>
        /// <returns>A list of project detail models.</returns>
        public List<ServiceProjectSummary> GetProjectNames(bool scrumMaster, bool productOwner, bool developer)
        {
            var userEmail = Thread.CurrentPrincipal.Identity.Name;
            var user = unitOfWork.UserRepository.Items.SingleOrDefault(u => u.EmailAddress == userEmail);

            if (user == null)
            {
                throw new ArgumentException("User does not exist");
            }

            var projectIds = new List<Guid>();

            if (scrumMaster)
            {
                var smProjects = user.Roles.Where(r => r.RoleName == "Scrum Master").Select(r => r.ProjectId);
                projectIds = projectIds.Union(smProjects).ToList();
            }

            if (productOwner)
            {
                var poProjects = user.Roles.Where(r => r.RoleName == "Product Owner").Select(r => r.ProjectId);
                projectIds = projectIds.Union(poProjects).ToList();                
            }

            if (developer)
            {
                var devProjects = user.Roles.Where(r => r.RoleName == "Developer").Select(r => r.ProjectId);
                projectIds = projectIds.Union(devProjects).ToList();
            }

            var projects = unitOfWork.ProjectRepository.Items.Where(project => projectIds.Contains(project.Id)).Select(p => new ServiceProjectSummary
            {
                ProjectId = p.Id,
                ProjectName = p.Name
            }).ToList();

            return projects;
        }

        /// <summary>
        /// Method to get the project stories.
        /// </summary>
        /// <param name="projectId">projectId to be checked</param>
        /// <returns>a list of stories</returns>
        public List<ServiceStorySummary> GetProjectStoryDetails(Guid projectId)
        {
            var stories = unitOfWork.StoryRepository.Items.Where(s => s.Project.Id == projectId).Select(s => new ServiceStorySummary
            {
                StoryId = s.Id,
                Summary = s.Summary,
                InSprint = s.InSprint
            });
            return stories.ToList();
        }

        /// <summary>
        /// Gets all existing projects from the database.
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
                        project.Stories.Add(new ServiceStory
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

                if (dataProject.Sprints != null)
                {
                    foreach (var sprint in dataProject.Sprints)
                    {
                        var serviceSprint = new ServiceSprint
                        {
                            SprintId = sprint.Id,
                            Name = sprint.Name,
                            Description = sprint.Description,
                            StartDate = sprint.StartDate,
                            EndDate = sprint.EndDate
                        };

                        if (sprint.Backlog != null)
                        {
                            foreach (var sprintStory in sprint.Backlog)
                            {
                                serviceSprint.Backlog.Add(new ServiceSprintStory
                                {
                                    SprintStoryId = sprintStory.Id,
                                    Story = new ServiceStory()
                                    {
                                        StoryId = sprintStory.Story.Id,
                                        Summary = sprintStory.Story.Summary,
                                        ConditionsOfSatisfaction = sprintStory.Story.ConditionsOfSatisfaction,
                                        AsA = sprintStory.Story.AsA,
                                        IWant = sprintStory.Story.IWant,
                                        SoThat = sprintStory.Story.IWant
                                    }
                                });
                            }
                        }

                        if (sprint.Team != null)
                        {
                            foreach (var teamMember in sprint.Team)
                            {
                                serviceSprint.TeamEmailAddresses.Add(teamMember.EmailAddress);
                            }
                        }

                        project.Sprints.Add(serviceSprint);
                    }
                }
            }

            return projects;
        }

        /// <summary>
        /// Retrieves the a list summarising the logged in users projects and roles
        /// </summary>
        /// <param name="currentUserEmailAddress">The email of the currently logged in user</param>
        /// <returns>A list containing Project role summaries</returns>
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public List<ServiceProjectRoleSummary> GetUserProjectRoleSummaries(string currentUserEmailAddress)
        {          
            User loggedInUser = unitOfWork.UserRepository.Items.Single(u => u.EmailAddress == currentUserEmailAddress);

            var projectIds = unitOfWork.RoleRepository.Items.Where(r => r.User.EmailAddress == loggedInUser.EmailAddress).Select(r => r.ProjectId).Distinct().ToList();

            if (projectIds.Count == 0)
            {
                return new List<ServiceProjectRoleSummary>();
            }

            List<ServiceProjectRoleSummary> projectRoleDetails = new List<ServiceProjectRoleSummary>();

            foreach (var projectId in projectIds)
            {
                var project =
                    unitOfWork.ProjectRepository.Items.Single(p => p.Id == projectId);

                var tempRoles = loggedInUser.Roles.Where(r => r.ProjectId == projectId).Select(r => r.RoleName).ToList();

                ServiceProjectRoleSummary summary = new ServiceProjectRoleSummary
                {
                    ProjectId = projectId,
                    Name = project.Name,
                    Description = project.Description,
                    ProjectManager = tempRoles.Contains("Project Manager"),
                    ProductOwner = tempRoles.Contains("Product Owner"),
                    ScrumMaster = tempRoles.Contains("Scrum Master"),
                    Developer = tempRoles.Contains("Developer")
                };

                if (summary.Developer && !summary.ProjectManager && !summary.ProductOwner && !summary.ScrumMaster)
                {
                    var sprintIds =
                        loggedInUser.Roles.Where(r => r.ProjectId == projectId && r.RoleName == "Developer")
                            .Select(r => r.SprintId)
                            .ToList();

                    foreach (var sprintId in sprintIds)
                    {
                        var sprint = unitOfWork.SprintRepository.Items.Single(s => s.Id == sprintId);

                        summary.Sprints.Add(new ServiceSprintSummary()
                        {
                            SprintId = (Guid)sprintId,
                            Name = sprint.Name,
                            Description = sprint.Description
                        });
                    }
                }
                else
                {
                    foreach (var sprint in project.Sprints)
                    {
                        summary.Sprints.Add(new ServiceSprintSummary()
                        {
                            SprintId = sprint.Id,
                            Name = sprint.Name,
                            Description = sprint.Description
                        });
                    }
                }

                projectRoleDetails.Add(summary);
            }

            return projectRoleDetails;
        }

        /// <summary>
        /// Searches for users who can be added to a project.
        /// </summary>
        /// <param name="filter">The user filter.</param>
        /// <returns>A list of user information objects.</returns>
        public List<ServiceUserSearchInfo> SearchUsers(ServiceUserSearchFilter filter)
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

            var userInfos = users.Select(u => new ServiceUserSearchInfo
            {
                EmailAddress = u.EmailAddress,
                FullName = u.FirstName + " " + u.LastName,
                UserId = u.Id
            }).OrderBy(u => u.FullName);

            return userInfos.ToList();
        }

        /// <summary>
        /// Adds or updates a story to/in the database and associates it with the specified project.
        /// </summary>
        /// <param name="projectId">The Guid of the project to add stories to</param>
        /// <param name="storyId">The Guid of the story to add stories to</param>
        /// <param name="story">The story to add/update</param>
        public void SaveUpdateStory(Guid projectId, Guid storyId, ServiceStory story)
        {
            var project = unitOfWork.ProjectRepository.Items.SingleOrDefault(p => p.Id == projectId);

            if (project == null)
            {
                throw new ArgumentException("Project does not exist in the database");
            }

            var dataStory = project.Stories.SingleOrDefault(s => s.Id == storyId);

            if (dataStory == null)
            {
                dataStory = new Story();
                project.Stories.Add(dataStory);
            }

            dataStory.Summary = story.Summary;
            dataStory.ConditionsOfSatisfaction = story.ConditionsOfSatisfaction;
            dataStory.AsA = story.AsA;
            dataStory.IWant = story.IWant;
            dataStory.SoThat = story.SoThat;

            unitOfWork.Commit();
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

            var dataStory = unitOfWork.StoryRepository.Items.SingleOrDefault(s => s.Id == story.StoryId);
            if (dataStory != null)
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
        /// Update story in the database.
        /// </summary>
        /// <param name="story">story to be updated</param>
        public void EditStory(ServiceStory story)
        {
            if (story == null)
            {
                throw new ArgumentNullException("story");
            }

            var project = unitOfWork.ProjectRepository.Items.SingleOrDefault(p => p.Id == story.ProjectId);

            if (project == null)
            {
                throw new ArgumentException("Project does not exist in database");
            }

            var dataStory = project.Stories.SingleOrDefault(p => p.Id == story.StoryId);

            if (dataStory == null)
            {
                throw new ArgumentException("story does not exist in database");
            }
            
            dataStory.Summary = story.Summary;
            dataStory.ConditionsOfSatisfaction = story.ConditionsOfSatisfaction;
            dataStory.AsA = story.AsA;
            dataStory.IWant = story.IWant;
            dataStory.SoThat = story.SoThat;           
            
            unitOfWork.Commit();
        }

        /// <summary>
        /// Deleting a story from the product backlog
        /// </summary>
        /// <param name="projectId">the projectid to be unlinked and removed</param>
        /// <param name="storyId">the storyid to be unlinked and removed</param>
        public void DeleteStoryFromProject(Guid projectId, Guid storyId)
        {
            var story = unitOfWork.StoryRepository.Items.Single(s => s.Id == storyId);
            var project = unitOfWork.ProjectRepository.Items.Single(p => p.Id == projectId);
            project.Stories.Remove(story);
            unitOfWork.StoryRepository.Delete(story);
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
                    ProjectId = projectId,
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
                        unitOfWork.RoleRepository.Items.SingleOrDefault(r => r.RoleName == roleName && r.User == user && r.ProjectId == projectId);

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
