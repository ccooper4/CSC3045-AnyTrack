using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Backend.Service.Model;
using Task = AnyTrack.Backend.Data.Model.Task;

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

            var dataProject = unitOfWork.ProjectRepository.Items.SingleOrDefault(p => p.Sprints.Any(s => s.Id == sprintId));

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
        /// <param name="sprintId">The sprint id</param>
        /// <returns>A list of tasks</returns>
        public List<ServiceTask> GetAllTasksForSprint(Guid sprintId)
        {
            var userEmail = Thread.CurrentPrincipal.Identity.Name;
            var user = MapEmailAddressToUser(userEmail);
            var tasks = unitOfWork.TaskRepository.Items.Where(t => t.SprintStory.Sprint.Id == sprintId).Where(u => u.Assignee == user).ToList();

            List<ServiceTask> serviceTasks = new List<ServiceTask>();
            foreach (var dataTask in tasks)
            {
                ServiceStory serviceStory = new ServiceStory()
                {
                    Summary = dataTask.SprintStory.Story.Summary,
                    ConditionsOfSatisfaction = dataTask.SprintStory.Story.ConditionsOfSatisfaction,
                    SoThat = dataTask.SprintStory.Story.SoThat,
                    AsA = dataTask.SprintStory.Story.AsA,
                    IWant = dataTask.SprintStory.Story.IWant,
                    ProjectId = dataTask.SprintStory.Story.Project.Id,
                    StoryId = dataTask.SprintStory.Story.Id,
                };

                ServiceSprintStory serviceSprintStory = new ServiceSprintStory()
                {
                    Story = serviceStory,
                    SprintId = sprintId
                };

                var remainingTaskHours =
                    unitOfWork.TaskHourEstimateRepository.Items.Where(t => t.Id == dataTask.Id).ToList();

                List<ServiceTaskHourEstimate> serviceRemainingTaskHours = new List<ServiceTaskHourEstimate>();
                foreach (var dataRemainingTaskHours in remainingTaskHours)
                {
                    ServiceTaskHourEstimate serviceTaskHourEstimate = new ServiceTaskHourEstimate()
                    {
                        Estimate = dataRemainingTaskHours.Estimate,
                        TaskId = dataTask.Id
                    };

                    serviceRemainingTaskHours.Add(serviceTaskHourEstimate);
                }
                
                ServiceTask task = new ServiceTask
                {
                    Blocked = dataTask.Blocked,
                    ConditionsOfSatisfaction = dataTask.ConditionsOfSatisfaction,
                    Description = dataTask.Description,
                    TaskHourEstimates = serviceRemainingTaskHours,
                    SprintStory = serviceSprintStory,
                    Summary = dataTask.Summary,
                    TaskId = dataTask.Id
                };

                serviceTasks.Add(task);
            }

            return serviceTasks;
        }

        /// <summary>
        /// Method to save the update hours for tasks
        /// </summary>
        /// <param name="tasks">List of tasks to save</param>
        public void SaveUpdatedTaskHours(List<ServiceTask> tasks)
        {
            foreach (var t in tasks)
            {
                var task = unitOfWork.TaskRepository.Items.Single(x => x.Id == t.TaskId);
                var serviceUpdatedHours = t.TaskHourEstimates.LastOrDefault();

                if (serviceUpdatedHours != null)
                {
                    task.TaskHourEstimates.Add(new TaskHourEstimate
                    {
                        Estimate = serviceUpdatedHours.Estimate
                    });
                }
            }

            unitOfWork.Commit();
        }

        /// <summary>
        /// Add a new task to a sprint story.
        /// </summary>
        /// <param name="sprintStoryId">The story to add the task to.</param>
        /// <param name="serviceTask">The task to add.</param>
        public void AddTaskToSprintStory(Guid sprintStoryId, ServiceTask serviceTask)
        {
            if (sprintStoryId == null)
            {
                throw new ArgumentNullException("sprintStoryId");
            }

            if (sprintStoryId == null)
            {
                throw new ArgumentNullException("serviceTask");
            }

            User assignee = unitOfWork.UserRepository.Items.SingleOrDefault(
                u => u.EmailAddress == serviceTask.Assignee.EmailAddress);

            User tester = unitOfWork.UserRepository.Items.SingleOrDefault(
                u => u.EmailAddress == serviceTask.Tester.EmailAddress);

            DateTime now = DateTime.Now;

            Task task = new Task()
            {
                Assignee = assignee,
                Tester = tester,
                Blocked = serviceTask.Blocked,
                ConditionsOfSatisfaction = serviceTask.ConditionsOfSatisfaction,
                Description = serviceTask.Description,
                Summary = serviceTask.Summary,
                Updated = now,
            };
        }

        #endregion

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
