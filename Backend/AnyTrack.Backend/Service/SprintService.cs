using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Backend.Security;
using AnyTrack.Backend.Service.Model;
using Task = AnyTrack.Backend.Data.Model.Task;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Provides the methods of the SprintService
    /// </summary>
    [CreatePrincipal]
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

            var currentUserEmail = Thread.CurrentPrincipal.Identity.Name;

            Sprint dataSprint = new Sprint()
            {
                Name = sprint.Name,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                Description = sprint.Description,
            };

            dataSprint.ScrumMaster = AssignScrumMaster(projectId, dataSprint.Id, currentUserEmail);

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

            var dataProject =
                unitOfWork.ProjectRepository.Items.SingleOrDefault(p => p.Sprints.Any(s => s.Id == sprintId));

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
        /// Retrieves a specified sprint.
        /// </summary>
        /// <param name="sprintId">Id of the sprint</param>
        /// <returns>The sprint</returns>
        public ServiceSprint GetSprint(Guid sprintId)
        {
            if (sprintId == Guid.Empty)
            {
                throw new ArgumentNullException("sprintId");
            }

            var dataSprint = unitOfWork.SprintRepository.Items.SingleOrDefault(s => s.Id == sprintId);

            ServiceSprint sprint = new ServiceSprint
            {
                SprintId = sprintId,
                ProjectId = dataSprint.Project.Id,
                Name = dataSprint.Name,
                StartDate = dataSprint.StartDate,
                EndDate = dataSprint.EndDate,
                Description = dataSprint.Description,
            };

            if (dataSprint.Team != null)
            {
                foreach (var developer in dataSprint.Team)
                {
                    sprint.TeamEmailAddresses.Add(developer.EmailAddress);
                }
            }

            if (dataSprint.Backlog != null)
            {
                foreach (var sprintStory in dataSprint.Backlog)
                {
                    sprint.Backlog.Add(new ServiceSprintStory
                    {
                        SprintStoryId = sprintStory.Id,
                        SprintId = dataSprint.Id,
                        Story = new ServiceStory
                        {
                            StoryId = sprintStory.Story.Id,
                            Summary = sprintStory.Story.Summary,
                            ProjectId = dataSprint.Project.Id,
                            ConditionsOfSatisfaction = sprintStory.Story.ConditionsOfSatisfaction,
                            AsA = sprintStory.Story.AsA,
                            IWant = sprintStory.Story.IWant,
                            SoThat = sprintStory.Story.SoThat
                        }
                    });
                }
            }

            return sprint;
        }
    
    /// <summary>
        /// Gets all tasks for a sprint
        /// </summary>
        /// <param name="sprintId">The sprint id</param>
        /// <returns>A list of tasks</returns>
        public List<ServiceTask> GetAllTasksForSprintCurrentUser(Guid sprintId)
        {
            var userEmail = Thread.CurrentPrincipal.Identity.Name;
            var user = MapEmailAddressToUser(userEmail);

            if (user == null)
            {
                throw new ArgumentException("User does not exist");
            }

            var tasks = unitOfWork.TaskRepository.Items.Where(t => t.SprintStory.Sprint.Id == sprintId).Where(u => u.Assignee == user).ToList();

            List<ServiceTask> serviceTasks = new List<ServiceTask>();
            foreach (var dataTask in tasks)
            {
                var remainingTaskHours =
                    unitOfWork.TaskHourEstimateRepository.Items.Where(t => t.Id == dataTask.Id).ToList();

                List<ServiceTaskHourEstimate> serviceRemainingTaskHours = new List<ServiceTaskHourEstimate>();
                foreach (var dataRemainingTaskHours in remainingTaskHours)
                {
                    ServiceTaskHourEstimate serviceTaskHourEstimate = new ServiceTaskHourEstimate()
                    {
                        Estimate = dataRemainingTaskHours.Estimate,
                        TaskId = dataTask.Id,
                        Created = dataRemainingTaskHours.Created
                    };

                    serviceRemainingTaskHours.Add(serviceTaskHourEstimate);
                }
                
                ServiceTask task = new ServiceTask
                {
                    Blocked = dataTask.Blocked,
                    ConditionsOfSatisfaction = dataTask.ConditionsOfSatisfaction,
                    Description = dataTask.Description,
                    TaskHourEstimates = serviceRemainingTaskHours,
                    SprintStoryId = dataTask.SprintStory.Id,
                    Summary = dataTask.Summary,
                    TaskId = dataTask.Id
                };

                serviceTasks.Add(task);
            }

            return serviceTasks;
        }

        /// <summary>
        /// Delete a task. 
        /// </summary>
        /// <param name="serviceTaskId">the task to delete</param>
        public void DeleteTask(Guid serviceTaskId)
        {
            var dataTask = unitOfWork.TaskRepository.Items.SingleOrDefault(t => t.Id == serviceTaskId);

            if (dataTask != null)
            {
                var taskHourEstimates = unitOfWork.TaskHourEstimateRepository.Items.Where(t => t.Task.Id == dataTask.Id).ToList();

                if (taskHourEstimates.Count > 0)
                {
                    foreach (TaskHourEstimate estimate in taskHourEstimates)
                    {
                        unitOfWork.TaskHourEstimateRepository.Delete(estimate);
                    }
                }

                unitOfWork.TaskRepository.Delete(dataTask);
            }

            unitOfWork.Commit();
        }
        
        /// <summary>
        /// Get all the tasks for a given sprint story.
        /// </summary>
        /// <param name="sprintStoryId">the id of the sprint story</param>
        /// <returns>the list of tasks</returns>
        public List<ServiceTask> GetAllTasksForSprintStory(Guid sprintStoryId)
        {
            var tasks = unitOfWork.TaskRepository.Items.Where(t => t.SprintStory.Id == sprintStoryId).ToList();

            List<ServiceTask> serviceTasks = new List<ServiceTask>();
            foreach (var dataTask in tasks)
            {
                var remainingTaskHours =
                    unitOfWork.TaskHourEstimateRepository.Items.Where(t => t.Task.Id == dataTask.Id).ToList();

                List<ServiceTaskHourEstimate> serviceRemainingTaskHours = new List<ServiceTaskHourEstimate>();
                foreach (var dataRemainingTaskHours in remainingTaskHours)
                {
                    ServiceTaskHourEstimate serviceTaskHourEstimate = new ServiceTaskHourEstimate()
                    {
                        Estimate = dataRemainingTaskHours.Estimate,
                        TaskId = dataTask.Id,
                        Created = dataRemainingTaskHours.Created
                    };
                    serviceRemainingTaskHours.Add(serviceTaskHourEstimate);
                }

                ServiceTask task = new ServiceTask
                {
                    Blocked = dataTask.Blocked,
                    ConditionsOfSatisfaction = dataTask.ConditionsOfSatisfaction,
                    Description = dataTask.Description,
                    TaskHourEstimates = serviceRemainingTaskHours,
                    SprintStoryId = dataTask.SprintStory.Id,
                    Summary = dataTask.Summary,
                    TaskId = dataTask.Id
                };

                serviceTasks.Add(task);
            }

            return serviceTasks;
        } 

        /// <summary>
        /// Gets all tasks for a sprint for a burndown
        /// </summary>
        /// <param name="sprintId">The sprint id</param>
        /// <returns>A list of tasks</returns>
        public List<ServiceTask> GetAllTasksForSprint(Guid sprintId)
        {
            var tasks = unitOfWork.TaskRepository.Items.Where(t => t.SprintStory.Sprint.Id == sprintId).ToList();

            List<ServiceTask> serviceTasks = new List<ServiceTask>();
            foreach (var dataTask in tasks)
            {
                var remainingTaskHours =
                    unitOfWork.TaskHourEstimateRepository.Items.Where(t => t.Task.Id == dataTask.Id).ToList();

                List<ServiceTaskHourEstimate> serviceRemainingTaskHours = new List<ServiceTaskHourEstimate>();
                foreach (var dataRemainingTaskHours in remainingTaskHours)
                {
                    ServiceTaskHourEstimate serviceTaskHourEstimate = new ServiceTaskHourEstimate()
                    {
                        Estimate = dataRemainingTaskHours.Estimate,
                        TaskId = dataTask.Id,
                        Created = dataRemainingTaskHours.Created
                    };
                    serviceRemainingTaskHours.Add(serviceTaskHourEstimate);
                }

                ServiceTask task = new ServiceTask
                {
                    Blocked = dataTask.Blocked,
                    ConditionsOfSatisfaction = dataTask.ConditionsOfSatisfaction,
                    Description = dataTask.Description,
                    TaskHourEstimates = serviceRemainingTaskHours,
                    SprintStoryId = dataTask.SprintStory.Id,
                    Summary = dataTask.Summary,
                    TaskId = dataTask.Id
                };

                serviceTasks.Add(task);
            }

            return serviceTasks;
        }

        /// <summary>
        /// Gets a startdate for a given sprint
        /// </summary>
        /// <param name="sprintId">The sprint id</param>
        /// <returns>the startDate</returns>
        public DateTime? GetStartDateOfSprint(Guid sprintId)
        {
            var startDate = unitOfWork.SprintRepository.Items.Min(s => s.StartDate);
            return startDate;
        }

        /// <summary>
        /// Gets a story estimates for a given sprintId
        /// </summary>
        /// <param name="sprintId">The sprint id</param>
        /// <returns>the list of estimates</returns>
        public List<ServiceSprintStory> GetSprintStoryEstimates(Guid sprintId)
        {
            List<ServiceSprintStory> sprintStoryList = new List<ServiceSprintStory>();
            var sprintStoryEstimates = unitOfWork.SprintStoryRepository.Items.Where(ss => ss.Sprint.Id == sprintId).ToList();

            foreach (var sprintEst in sprintStoryEstimates)
            {
                ServiceSprintStory sprintStory = new ServiceSprintStory
                {
                    SprintId = sprintEst.Id,
                    StoryEstimate = sprintEst.StoryEstimate,
                    DateCompleted = sprintEst.DateCompleted
                };                  
            }

            return sprintStoryList;
        }

        /// <summary>
        /// Gets an enddate for a given sprint.
        /// </summary>
        /// <param name="sprintId">the sprintid</param>
        /// <returns>the enddate of sprint</returns>
        public DateTime? GetEndDateOfSprint(Guid sprintId)
        {
            var endDate = unitOfWork.SprintRepository.Items.Max(s => s.EndDate);
            return endDate;
        }

        /// <summary>
        /// Gets the maximum estimate of a sprint
        /// </summary>
        /// <param name="sprintId">the sprintId</param>
        /// <returns>retrieve the max estimate of the sprint</returns>
        public double GetMaxEstimateOfSprint(Guid sprintId)
        {
            var maxEst = unitOfWork.TaskHourEstimateRepository.Items.Max(t => t.Estimate);
            return maxEst;
        }

        /// <summary>
        /// Gets the total story estimate of a sprint
        /// </summary>
        /// <param name="sprintId">the sprintId</param>
        /// <returns>retrieve total story estimate of the sprint</returns>
        public double GetTotalStoryPointEstimate(Guid sprintId)
        {
            var totalEstimateStoryPoints = unitOfWork.SprintStoryRepository.Items.Sum(ss => ss.StoryEstimate);
            return totalEstimateStoryPoints;
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
                ServiceTaskHourEstimate updatedTaskHour = t.TaskHourEstimates.LastOrDefault();

                if (updatedTaskHour != null)
                {
                    TaskHourEstimate estimateToAdd = new TaskHourEstimate
                    {
                        Estimate = updatedTaskHour.NewEstimate,
                        Task = task
                    };

                    task.TaskHourEstimate.Add(estimateToAdd);
                    unitOfWork.Commit();
                }
            }
        }

        /// <summary>
        /// Add the initial task hour estimate to a task. 
        /// </summary>
        /// <param name="taskId"> the id of the task</param>
        /// <param name="serviceTaskHourEstimate"> the task hour estimate </param>
        public void AddTaskHourEstimateToTask(Guid taskId, ServiceTaskHourEstimate serviceTaskHourEstimate)
        {
            if (taskId == null)
            {
                throw new ArgumentNullException("taskId");
            }

            if (serviceTaskHourEstimate == null)
            {
                throw new ArgumentNullException("serviceTaskHourEstimate");
            }

            TaskHourEstimate dataTaskHourEstimate = new TaskHourEstimate()
            {
                Created = serviceTaskHourEstimate.Created,
                Estimate = serviceTaskHourEstimate.Estimate
            };

            var task = unitOfWork.TaskRepository.Items.SingleOrDefault(t => t.Id == taskId);

            if (task.TaskHourEstimate == null)
            {
                List<TaskHourEstimate> taskHourEstimates = new List<TaskHourEstimate>();
                taskHourEstimates.Add(dataTaskHourEstimate);
                task.TaskHourEstimate = taskHourEstimates;
            }
            else
            {
                task.TaskHourEstimate.Add(dataTaskHourEstimate);
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

            if (serviceTask == null)
            {
                throw new ArgumentNullException("serviceTask");
            }

            User assignee = null;
            if (serviceTask.Assignee != null)
            {
                assignee = unitOfWork.UserRepository.Items.SingleOrDefault(
                u => u.EmailAddress == serviceTask.Assignee.EmailAddress);
            }

            SprintStory dataSprintStory = unitOfWork.SprintStoryRepository.Items.SingleOrDefault(
                s => s.Id == sprintStoryId);

            var dataTask = dataSprintStory.Tasks.SingleOrDefault(t => t.Id == serviceTask.TaskId);

            if (dataTask == null)
            {
                dataTask = new Task(); 
                dataSprintStory.Tasks.Add(dataTask);
            }

            dataTask.Assignee = assignee;
            dataTask.Blocked = serviceTask.Blocked;
            dataTask.ConditionsOfSatisfaction = serviceTask.ConditionsOfSatisfaction;
            dataTask.Description = serviceTask.Description;
            dataTask.Summary = serviceTask.Summary;
            dataTask.SprintStory = dataSprintStory;

            unitOfWork.Commit();

            ServiceTaskHourEstimate taskHourEstimate = serviceTask.TaskHourEstimates.LastOrDefault();
            Guid taskId = dataTask.Id;
            AddTaskHourEstimateToTask(taskId, taskHourEstimate);
        }

        /// <summary>
        /// Get a sprint story given it's id.
        /// </summary>
        /// <param name="sprintStoryId">the sprint story id</param>
        /// <returns>the sprint story</returns>
        public ServiceSprintStory GetSprintStory(Guid sprintStoryId)
        {
            SprintStory dataSprintStory = unitOfWork.SprintStoryRepository.Items.SingleOrDefault(s => s.Id == sprintStoryId);
            Story dataStory = unitOfWork.StoryRepository.Items.SingleOrDefault(s => s.Id == dataSprintStory.Story.Id);

            ServiceStory serviceStory = new ServiceStory()
            {
                StoryId = dataStory.Id,
                ProjectId = dataStory.Project.Id,
                Summary = dataStory.Summary,
                ConditionsOfSatisfaction = dataStory.ConditionsOfSatisfaction,
                SoThat = dataStory.SoThat,
                AsA = dataStory.AsA,
                IWant = dataStory.IWant,
                InSprint = dataStory.InSprint
            };

            ServiceSprintStory serviceSprintStory = new ServiceSprintStory()
            {
                SprintId = dataSprintStory.Sprint.Id,
                Status = dataSprintStory.Status,
                SprintStoryId = dataSprintStory.Id,
                Story = serviceStory
            };

            return serviceSprintStory;
        }

        /// <summary>
        /// Save a sprint story
        /// </summary>
        /// <param name="sprintStory">the spritn story id</param>
        public void SaveSprintStory(ServiceSprintStory sprintStory)
        {
            var sprint = unitOfWork.SprintRepository.Items.SingleOrDefault(s => s.Id == sprintStory.SprintId);

            if (sprint != null)
            {
                SprintStory dataSprintStory = new SprintStory()
                {
                    Id = sprintStory.SprintStoryId,
                    Status = sprintStory.Status,
                    StoryEstimate = sprintStory.StoryEstimate,
                };

                sprint.Backlog.Add(dataSprintStory);
            }

            unitOfWork.Commit();
        }

        /// <summary>
        /// Gets all sprints for the current user
        /// </summary>
        /// <param name="projectId">The project id.</param>         
        /// <param name="scrumMaster">A flag indicating if sprints where the user is the SM should be included.</param>
        /// <param name="developer">A flag indicating if sprints where the user is a Deveoper should be included.</param>
        /// <returns>A summary list of this user's sprints.</returns>
        public List<ServiceSprintSummary> GetSprintNames(Guid? projectId, bool scrumMaster, bool developer)
        {
            var userEmail = Thread.CurrentPrincipal.Identity.Name;
            var user = unitOfWork.UserRepository.Items.SingleOrDefault(u => u.EmailAddress == userEmail);

            if (user == null)
            {
                throw new ArgumentException("User does not exist");
            }

            var sprintIds = new List<Guid>();

            var userRoles = user.Roles;

            if (projectId != null)
            {
                userRoles = userRoles.Where(r => r.ProjectId == projectId).ToList();
            }

            if (scrumMaster)
            {
                var smSprintIds = userRoles.Where(r => r.RoleName == "Scrum Master" && r.SprintId.HasValue).Select(r => r.SprintId.Value).ToList();
                sprintIds = sprintIds.Union(smSprintIds).ToList();
            }

            if (developer)
            {
                var devSprintIds = userRoles.Where(r => r.RoleName == "Developer" && r.SprintId.HasValue).Select(r => r.SprintId.Value).ToList();
                sprintIds = sprintIds.Union(devSprintIds).ToList();
            }

            var sprintSummary = unitOfWork.SprintRepository.Items.Where(s => sprintIds.Contains(s.Id)).Select(s => new ServiceSprintSummary
            {
                Description = s.Description,
                Name = s.Name,
                SprintId = s.Id
            }).ToList();

            return sprintSummary;
        }

        /// <summary>
        /// Method to return all stories in the sprint
        /// </summary>
        /// <param name="sprintId">The id of the sprint</param>
        /// <returns>The list of stories from the sprint</returns>
        public List<ServiceSprintStory> GetSprintStories(Guid sprintId)
        {
            if (sprintId == null)
            {
                throw new ArgumentException("SprintId cannot be null");
            }

            var dataSprint = unitOfWork.SprintRepository.Items.SingleOrDefault(s => s.Id == sprintId);

            if (dataSprint == null)
            {
                throw new NullReferenceException(string.Format(
                    "A sprint with the id {0} does not exist in the database", sprintId));
            }

            if (dataSprint.Backlog == null)
            {
                return new List<ServiceSprintStory>();
            }

            List<ServiceSprintStory> sprintStories = new List<ServiceSprintStory>();

            foreach (var sprintStory in dataSprint.Backlog)
            {
                if (sprintStory != null)
                {
                    sprintStories.Add(new ServiceSprintStory
                    {
                        SprintStoryId = sprintStory.Id,
                        Story = new ServiceStory
                        {
                            StoryId = sprintStory.Story.Id,
                            AsA = sprintStory.Story.AsA,
                            IWant = sprintStory.Story.IWant,
                            SoThat = sprintStory.Story.SoThat,
                            Summary = sprintStory.Story.Summary,
                            InSprint = sprintStory.Story.InSprint,
                            ConditionsOfSatisfaction = sprintStory.Story.ConditionsOfSatisfaction
                        },
                        Status = sprintStory.Status
                    });
                }
                else
                {
                    {
                        return new List<ServiceSprintStory>();
                    }
                }
           }

           return sprintStories;
        }

        /// <summary>
        /// All stories in the backlog right now
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="sprintId">The sprint id</param>
        /// <param name="sprintStories">The Sprint stories</param>
        public void ManageSprintBacklog(Guid projectId, Guid sprintId, List<ServiceSprintStory> sprintStories)
        {
            if (sprintId == null)
            {
                throw new ArgumentNullException("sprintId");
            }

            if (sprintStories == null)
            {
                throw new ArgumentNullException("sprintStories");
            }

            var dataSprint = unitOfWork.SprintRepository.Items.SingleOrDefault(s => s.Id == sprintId);

            if (dataSprint == null)
            {
                throw new NullReferenceException(string.Format(
                    "A sprint with the id {0} does not exist in the database", sprintId));
            }

            List<Guid> currentSprintIds = new List<Guid>();
            List<Guid> newSprintIds = new List<Guid>();

            foreach (var sprintStory in dataSprint.Backlog)
            {
                currentSprintIds.Add(sprintStory.Story.Id);
            }

            foreach (var sprintStory in sprintStories)
            {
                newSprintIds.Add(sprintStory.Story.StoryId);
            }

            foreach (var story in sprintStories)
            {
                ////If the current sprint backlog does not contain the story passed in
                ////Then we must add this to the repo
                if (!currentSprintIds.Contains(story.Story.StoryId))
                {
                    var productBacklogStory = unitOfWork.StoryRepository.Items.Single(s => s.Id == story.Story.StoryId);
                    productBacklogStory.InSprint = true;
                    dataSprint.Backlog.Add(new SprintStory
                    {
                        Story = productBacklogStory,
                        Status = story.Status
                    });
                }
            }

            List<SprintStory> removeStories = new List<SprintStory>();

            foreach (var story in dataSprint.Backlog)
            {
                ////If there is a story on the current sprint backlog, which is not passed in
                ////Then we need to remove this from the repo
                if (!newSprintIds.Contains(story.Story.Id))
                {
                    removeStories.Add(story);
                }
            }

            foreach (var story in removeStories)
            {
                var project = unitOfWork.ProjectRepository.Items.SingleOrDefault(p => p.Id == projectId);

                if (project == null)
                {
                    throw new NullReferenceException("project");
                }

                var dataStory = project.Stories.SingleOrDefault(s => s.Id == story.Story.Id);
                if (dataStory == null)
                {
                    throw new NullReferenceException("dataStory");
                }

                dataStory.InSprint = false;
                unitOfWork.Commit();
                                
                var sprintStory = unitOfWork.SprintStoryRepository.Items.Single(s => s.Story.Id == story.Story.Id);
                unitOfWork.SprintStoryRepository.Delete(sprintStory);
            }

            unitOfWork.Commit();
        }

        /// <summary>
        /// Sends an email request
        /// </summary>
        /// <param name="senderEmailAddress">The sender email address</param>
        /// <param name="recipientEmailAddress">The recipient email message</param>
        /// <param name="emailMessage">The email address</param>
        /// <param name="emailAttachment">The email attachment of the </param>
        public void SendEmailRequest(string senderEmailAddress, string recipientEmailAddress, string emailMessage, MemoryStream emailAttachment)
        {
            var fromAddress = new MailAddress(senderEmailAddress, "From Name");
            var toAddress = new MailAddress(recipientEmailAddress, "To Name");
            const string ConstFromPassword = "password";
            const string ConstSubject = "Subject";
            const string ConstBody = "Body";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, ConstFromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = ConstSubject,
                Body = ConstBody,
            })
            {
                smtp.Send(message);
            }
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

                var roles = user.Roles.Where(r => r.ProjectId == role.ProjectId && r.RoleName == role.RoleName && r.SprintId == role.SprintId).ToList();

                if (roles.Count == 0)
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
        /// Retrieves user from database with email address and assigns them as the scrum on the sprint.
        /// </summary>
        /// <param name="projectId">Id of project that has the sprint</param>
        /// <param name="sprintId">Id of the sprint</param>
        /// <param name="emailAddress">Email address of the user</param>
        /// <returns>The assigned user</returns>
        private User AssignScrumMaster(Guid projectId, Guid sprintId, string emailAddress)
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
                    RoleName = "Scrum Master",
                    SprintId = sprintId,
                    User = user
                };

                // Add role to user
                if (user.Roles == null)
                {
                    user.Roles = new List<Role>();
                }

                var roles = user.Roles.Where(r => r.ProjectId == role.ProjectId && r.RoleName == role.RoleName && r.SprintId == role.SprintId).ToList();

                if (roles.Count == 0)
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

            Role role = unitOfWork.RoleRepository.Items.SingleOrDefault(r => r.User.EmailAddress == user.EmailAddress && (r.SprintId == sprintId) && r.RoleName == "Developer");

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