using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

            if (projectExists == null)
            {
                Project dataProject = new Project
                {
                    Description = project.Description,
                    Name = project.Name,
                    ProductOwner = new Data.Model.User
                    {
                        EmailAddress = project.ProductOwner.EmailAddress,
                        Password = project.ProductOwner.Password,
                        FirstName = project.ProductOwner.FirstName,
                        LastName = project.ProductOwner.LastName

                        // skillset
                    },
                    ProjectManager = new Data.Model.User
                    {
                        EmailAddress = project.ProjectManager.EmailAddress,
                        Password = project.ProjectManager.Password,
                        FirstName = project.ProjectManager.FirstName,
                        LastName = project.ProjectManager.LastName
                    },
                    StartedOn = project.StartedOn,
                    VersionControl = project.VersionControl
                };

                dataProject.ScrumMasters = new List<User>();
                dataProject.Id = project.ProjectId;

                foreach (var scrumMaster in project.ScrumMasters)
                {
                    dataProject.ScrumMasters.Add(new Data.Model.User
                    {
                        EmailAddress = scrumMaster.EmailAddress,
                        Password = scrumMaster.Password,
                        FirstName = scrumMaster.FirstName,
                        LastName = scrumMaster.LastName
                    });
                }

                unitOfWork.ProjectRepository.Insert(dataProject);
                unitOfWork.Commit();
            }
            else
            {
                throw new ArgumentException("Project already exists in database");
            }
        }

        /// <summary>
        /// Update project in the database
        /// </summary>
        /// <param name="project">Project to be updated</param>
        public void UpdateProject(ServiceProject project)
        {
            var updatedProject = unitOfWork.ProjectRepository.Items.SingleOrDefault(p => p.Id == project.ProjectId);

            if (updatedProject != null)
            {
                updatedProject.Name = project.Name;
                updatedProject.Description = project.Description;
                updatedProject.StartedOn = updatedProject.StartedOn;
                updatedProject.VersionControl = project.VersionControl;

                updatedProject.ProductOwner = new Data.Model.User
                {
                    EmailAddress = project.ProductOwner.EmailAddress,
                    Password = project.ProductOwner.Password,
                    FirstName = project.ProductOwner.FirstName,
                    LastName = project.ProductOwner.LastName,
                };

                updatedProject.ProjectManager = new Data.Model.User
                {
                    EmailAddress = project.ProjectManager.EmailAddress,
                    Password = project.ProjectManager.Password,
                    FirstName = project.ProjectManager.FirstName,
                    LastName = project.ProjectManager.LastName,
                };

                updatedProject.ScrumMasters.Clear();
                foreach (var scrumMaster in project.ScrumMasters)
                {
                    updatedProject.ScrumMasters.Add(new Data.Model.User
                    {
                        EmailAddress = scrumMaster.EmailAddress,
                        Password = scrumMaster.Password,
                        FirstName = scrumMaster.FirstName,
                        LastName = scrumMaster.LastName,
                    });
                }

                unitOfWork.Commit();
            }
            else
            {
                throw new ArgumentException("Project does not exist in database");
            }
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
            var query = unitOfWork.ProjectRepository.Items.SingleOrDefault(p => p.Id == projectId);

            if (query == null)
            {
                throw new ArgumentException("Project does not exist");
            }

            ServiceProject project = new ServiceProject
            {
                Description = query.Description,
                ProjectId = query.Id,
                Name = query.Name,
                ProductOwner = new NewUser
                {
                    EmailAddress = query.ProductOwner.EmailAddress,
                    Password = query.ProductOwner.Password,
                    FirstName = query.ProductOwner.FirstName,
                    LastName = query.ProductOwner.LastName
                },
                ProjectManager = new NewUser
                {
                    EmailAddress = query.ProjectManager.EmailAddress,
                    Password = query.ProjectManager.Password,
                    FirstName = query.ProjectManager.FirstName,
                    LastName = query.ProjectManager.LastName
                    
                    // include skillset
                },
                StartedOn = query.StartedOn,
                VersionControl = query.VersionControl
            };

            foreach (var scrumMaster in query.ScrumMasters)
            {
                project.ScrumMasters.Add(new NewUser
                {
                    EmailAddress = scrumMaster.EmailAddress,
                    Password = scrumMaster.Password,
                    FirstName = scrumMaster.FirstName,
                    LastName = scrumMaster.LastName
                    
                    // include skillset
                });
            }

            return project;
        }

        /// <summary>
        /// Gets all existing projects from the database
        /// </summary>
        /// <returns>List of all Projects in the database</returns>
        public List<ServiceProject> GetProjects()
        {
            var query = unitOfWork.ProjectRepository.Items.Select(p => new ServiceProject
            {
                ProjectId = p.Id,
                Description = p.Description,
                Name = p.Name,
                VersionControl = p.VersionControl,
                StartedOn = p.StartedOn,
                ProductOwner = new NewUser
                {
                    EmailAddress = p.ProductOwner.EmailAddress,
                    Password = p.ProductOwner.Password,
                    FirstName = p.ProductOwner.FirstName,
                    LastName = p.ProductOwner.LastName
                },
                ProjectManager = new NewUser
                {
                    EmailAddress = p.ProjectManager.EmailAddress,
                    Password = p.ProjectManager.Password,
                    FirstName = p.ProjectManager.FirstName,
                    LastName = p.ProjectManager.LastName
                },
            }).ToList();

            foreach (var project in query)
            {
                var query2 = unitOfWork.ProjectRepository.Items.SingleOrDefault(p => p.Id == project.ProjectId);
                foreach (var scrumMaster in query2.ScrumMasters)
                {
                    project.ScrumMasters.Add(new NewUser
                    {
                        EmailAddress = scrumMaster.EmailAddress,
                        Password = scrumMaster.Password,
                        FirstName = scrumMaster.FirstName,
                        LastName = scrumMaster.LastName
                    });
                }
            }

            return query;
        }
    }
}
