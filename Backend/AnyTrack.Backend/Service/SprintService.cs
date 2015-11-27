using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Backend.Service.Model;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Provides the methods of the SprintService
    /// </summary>
    public class SprintService : ISprintService
    {
        #region Fields

        /// <summary>
        /// The application's unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new SprintService.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public SprintService(IUnitOfWork unitOfWork)
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
        /// Creates a sprint and adds it to the project.
        /// </summary>
        /// <param name="projectId">Id of the project to add the sprint to</param>
        /// <param name="sprint">ServiceSprint entity</param>
        public void AddSprint(Guid projectId, ServiceSprint sprint)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException("projectId");
            }

            if (sprint == null)
            {
                throw new ArgumentNullException("sprint");
            }

            var project = unitOfWork.ProjectRepository.Items.SingleOrDefault(p => p.Id == projectId);

            if (project == null)
            {
                throw new NullReferenceException(string.Format("A project with the Guid {0} does not exist", projectId));
            }

            Sprint dataSprint = new Sprint()
            {
                Name = sprint.Name,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                Description = sprint.Description
            };

            dataSprint.Team = new List<User>();

            foreach (var teamMember in sprint.TeamEmailAddresses)
            {
                dataSprint.Team.Add(AssignDeveloper(projectId, dataSprint.Id, teamMember));
            }

            if (project.Sprints == null)
            {
                project.Sprints = new List<Sprint>();
            }

            project.Sprints.Add(dataSprint);

            unitOfWork.Commit();
        }

        #endregion

        /// <summary>
        /// Edits an exiting sprint.
        /// </summary>
        /// <param name="sprintId">Id of the sprint to be edited</param>
        /// <param name="updatedSprint">ServiceSprint entity containing changes</param>
        public void EditSprint(Guid sprintId, ServiceSprint updatedSprint)
        {
            if (sprintId == null)
            {
                throw new ArgumentNullException("sprintId");
            }

            if (updatedSprint == null)
            {
                throw new ArgumentNullException("updatedSprint");
            }

            var dataSprint = unitOfWork.SprintRepository.Items.SingleOrDefault(s => s.Id == sprintId);

            if (dataSprint == null)
            {
                throw new NullReferenceException(string.Format(
                    "A sprint with the id {0} does not exist in the database", sprintId));
            }

            dataSprint.Name = updatedSprint.Name;
            dataSprint.Description = updatedSprint.Description;
            dataSprint.StartDate = updatedSprint.StartDate;
            dataSprint.EndDate = updatedSprint.EndDate;

            var dataProject = unitOfWork.ProjectRepository.Items.Where(p => p.Sprints.Any(s => s.Id == sprintId)).SingleOrDefault();

            if (dataProject == null)
            {
                throw new Exception("Project doesn't exist with this sprint");
            }

            List<User> teamMembersToRemove = new List<User>();
            foreach (var teamMember in dataSprint.Team)
            {
                if (!updatedSprint.TeamEmailAddresses.Contains(teamMember.EmailAddress))
                {
                    teamMembersToRemove.Add(UnassignDeveloper(dataProject, dataSprint.Id, teamMember.EmailAddress));
                }
            }            

            foreach (var teamMemberEmailAddress in updatedSprint.TeamEmailAddresses)
            {
                if (!dataSprint.Team.Contains(MapEmailAddressToUser(teamMemberEmailAddress)))
                {
                    dataSprint.Team.Add(AssignDeveloper(dataProject.Id, sprintId, teamMemberEmailAddress));
                }
            }

            foreach (var user in teamMembersToRemove)
            {
                dataProject.Sprints.SingleOrDefault(s => s.Id == sprintId).Team.Remove(user);
            }
           
            unitOfWork.Commit();
        }

        /// <summary>
        /// Gets all tasks for a sprint
        /// </summary>
        /// <param name="sprintId">the sprint id</param>
        /// <param name="assignee">the assignee</param>
        /// <returns>A list of tasks</returns>
        public List<ServiceTask> GetAllTasksForSprint(Guid sprintId, User assignee)
        {
            var tasks = unitOfWork.TaskRepository.Items.Where(s => s.SprintStory.Sprint.Id == sprintId).Where(u => u.Assignee == assignee).ToList();

            List<ServiceTask> serviceTasks = new List<ServiceTask>();
            foreach (var t in tasks)
            {
                ServiceTask task = new ServiceTask
                {
                    Blocked = t.Blocked,
                    ConditionsOfSatisfaction = t.ConditionsOfSatisfaction,
                    Description = t.Description,
                    HoursRemaining = t.HoursRemaining,
                    SprintStory = t.SprintStory,
                    Summary = t.Summary,
                    TaskId = t.Id
                };

                foreach (var u in t.UpdatedHours)
                {
                    ServiceUpdatedHours updatedHours = new ServiceUpdatedHours
                    {
                        LogEstimate = u.LogEstimate,
                        UpdatedHoursId = u.Id,
                        NewEstimate = u.NewEstimate
                    };
                    task.UpdatedHours.Add(updatedHours);
                }

                serviceTasks.Add(task);
            }

            return serviceTasks;
        }
    
    #region Helper Methods

        /// <summary>
        /// Retrieves user from database with email address and assigns them as a developer on the sprint.
        /// </summary>
        /// <param name="projectId">Id of project that has the sprint</param>
        /// <param name="sprintId">Id of the sprint</param>
        /// <param name="emailAddress">Email address of the user</param>
        /// <returns>The assigned user</returns>
        private User AssignDeveloper(Guid projectId, Guid sprintId, string emailAddress)
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
                    RoleName = "Developer",
                    SprintId = sprintId,
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
        /// Retrieves user from database with email address and unassigns them as a developer on the sprint.
        /// </summary>
        /// <param name="project">Project containing the sprint</param>
        /// <param name="sprintId">Id of the sprint</param>
        /// <param name="emailAddress">Email address of the user</param>
        /// <returns>The unassigned user</returns>
        private User UnassignDeveloper(Project project, Guid sprintId, string emailAddress)
        {
            User user =
                unitOfWork.UserRepository.Items.SingleOrDefault(
                    u => u.EmailAddress == emailAddress);

            if (user == null)
            {
                throw new Exception("User does not exist");
            }

            Role role = unitOfWork.RoleRepository.Items.SingleOrDefault(r => r.User == user && r.SprintId == sprintId);

            if (role == null)
            {
                throw new Exception("Role does not exist");
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
