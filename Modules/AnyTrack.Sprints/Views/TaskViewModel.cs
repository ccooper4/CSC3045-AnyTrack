using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendSprintService;
using AnyTrack.Infrastructure.ServiceGateways;
using MahApps.Metro.Controls;
using OxyPlot;
using Prism.Commands;
using Prism.Regions;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// The sprint story view model.
    /// </summary>
    public class TaskViewModel : ValidatedBindableBase, INavigationAware, IRegionMemberLifetime, IFlyoutCompatibleViewModel
    {
        #region Fields

        /// <summary>
        /// Temporary store for the passed sprint story
        /// </summary>
        private ServiceSprintStory serviceSprintStory;

        /// <summary>
        /// sprint story id field.
        /// </summary>
        private Guid sprintStoryId;

        /// <summary>
        /// task id field.
        /// </summary>
        private Guid taskId;

        /// <summary>
        /// Summary field.
        /// </summary>
        private string summary;

        /// <summary>
        /// Description field.
        /// </summary>
        private string description;

        /// <summary>
        /// COS field.
        /// </summary>
        private string conditionsOfSatisfaction;

        /// <summary>
        /// The available assignees
        /// </summary>
        private List<string> availableAssignees; 

        /// <summary>
        /// Assignee field.
        /// </summary>
        private string assignee;

        /// <summary>
        /// Tester field.
        /// </summary>
        private string tester;

        /// <summary>
        /// Hours remaining field.
        /// </summary>
        private string hoursRemaining;

        /// <summary>
        /// blocked field.
        /// </summary>
        private bool blocked;

        /// <summary>
        /// The header field.
        /// </summary>
        private string header;

        /// <summary>
        /// The position field.
        /// </summary>
        private Position position;

        /// <summary>
        /// The is model field. 
        /// </summary>
        private bool isModel;

        /// <summary>
        /// The is model field. 
        /// </summary>
        private bool isOpen;

        /// <summary>
        /// The flyout theme field.
        /// </summary>
        private FlyoutTheme theme;

        /// <summary>
        /// The close button visibility field.
        /// </summary>
        private Visibility closeButtonVisibility;

        /// <summary>
        /// The close button visibility field.
        /// </summary>
        private Visibility titleVisibility;

        /// <summary>
        /// The Sprint Service Gateway.
        /// </summary>
        private ISprintServiceGateway sprintServiceGateway;

        #endregion

        /// <summary>
        /// Creates a new Sprint Story View Model
        /// </summary>
        /// <param name="sprintServiceGateway">The sprint Service Gateway</param>
        public TaskViewModel(ISprintServiceGateway sprintServiceGateway)
        {
            // Null checks
            if (sprintServiceGateway == null)
            {
                throw new ArgumentNullException("sprintServiceGateway");
            }

            // Set service
            this.sprintServiceGateway = sprintServiceGateway;

            // Set flyout properties
            this.Header = null;
            this.Theme = FlyoutTheme.Accent;
            this.Position = Position.Left;
            this.IsModal = true;
            this.CloseButtonVisibility = Visibility.Collapsed;

            // Commands
            SaveTaskCommand = new DelegateCommand(this.Save);
        }

        /// <summary>
        /// Gets or sets the command used to cancel 
        /// </summary>
        public DelegateCommand SaveTaskCommand { get; set; }

        /// <summary>
        /// Gets or sets the sprint story id
        /// </summary>
        public Guid SprintStoryId
        {
            get
            {
                return sprintStoryId;
            }

            set
            {
                SetProperty(ref sprintStoryId, value);
            }
        }

        /// <summary>
        /// Gets or sets the task id
        /// </summary>
        public Guid TaskId
        {
            get
            {
                return taskId;
            }

            set
            {
                SetProperty(ref taskId, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating summary
        /// </summary>
        public string Summary
        {
            get
            {
                return summary;
            }

            set
            {
                SetProperty(ref summary, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating description
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                SetProperty(ref description, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating COS
        /// </summary>
        public string ConditionsOfSatisfaction
        {
            get
            {
                return conditionsOfSatisfaction;
            }

            set
            {
                SetProperty(ref conditionsOfSatisfaction, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating COS
        /// </summary>
        public string HoursRemaining
        {
            get
            {
                return hoursRemaining;
            }

            set
            {
                SetProperty(ref hoursRemaining, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the task is blocked
        /// </summary>
        public bool Blocked
        {
            get
            {
                return blocked;
            }

            set
            {
                SetProperty(ref blocked, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating assignee
        /// </summary>
        public string Assignee
        {
            get
            {
                return conditionsOfSatisfaction;
            }

            set
            {
                SetProperty(ref conditionsOfSatisfaction, value);
            }
        }

        #region Flyouts

        /// <summary>
        /// Gets a value indicating whether it should refresh everytime
        /// </summary>
        public bool KeepAlive
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this flyout is open.
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return isOpen;
            }

            set
            {
                SetProperty(ref isOpen, value);
            }
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public string Header
        {
            get
            {
                return header;
            }

            set
            {
                SetProperty(ref header, value);
            }
        }

        /// <summary>
        /// Gets or sets position.
        /// </summary>
        public Position Position
        {
            get
            {
                return position;
            }

            set
            {
                SetProperty(ref position, value);
            }
        }

        /// <summary>
        /// Gets or sets the flyout theme.
        /// </summary>
        public FlyoutTheme Theme
        {
            get
            {
                return theme;
            }

            set
            {
                SetProperty(ref theme, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this flyout is a model.
        /// </summary>
        public bool IsModal
        {
            get
            {
                return isModel;
            }

            set
            {
                SetProperty(ref isModel, value);
            }
        }

        /// <summary>
        /// Gets or sets the close button visibility
        /// </summary>
        public Visibility CloseButtonVisibility
        {
            get
            {
                return closeButtonVisibility;
            }

            set
            {
                SetProperty(ref closeButtonVisibility, value);
            }
        }

        /// <summary>
        /// Gets or sets the title visibility
        /// </summary>
        public Visibility TitleVisibility
        {
            get
            {
                return titleVisibility;
            }

            set
            {
                SetProperty(ref titleVisibility, value);
            }
        }

        #endregion

        /// <summary>
        /// On navigated to.
        /// </summary>
        /// <param name="navigationContext"> The navigation context </param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        { 
            if (navigationContext.Parameters.ContainsKey("sprintStoryId")) 
            {
                this.SprintStoryId = (Guid)navigationContext.Parameters["sprintStoryId"];
                this.TaskId = Guid.NewGuid();
            }
            else if (navigationContext.Parameters.ContainsKey("task"))
            {
                ServiceTask task = (ServiceTask)navigationContext.Parameters["task"];
                this.TaskId = task.TaskId;
                this.Summary = task.Summary;
                this.Description = task.Description;
                this.ConditionsOfSatisfaction = task.ConditionsOfSatisfaction;
                this.Blocked = task.Blocked;

                ServiceTaskHourEstimate serviceTaskHourEstimate = task.TaskHourEstimates.LastOrDefault();
                if (serviceTaskHourEstimate != null)
                {
                    this.HoursRemaining = serviceTaskHourEstimate.Estimate.ToString();
                }
            }

            if (navigationContext.Parameters.ContainsKey("sprintStory"))
            {
                this.serviceSprintStory = (ServiceSprintStory)navigationContext.Parameters["sprintStory"];
            }
        }

        /// <summary>
        /// Is navigation target.
        /// </summary>
        /// <param name="navigationContext"> The navigation context isNavigationTarget</param>
        /// <returns> The navigation context </returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        /// <summary>
        /// On navigated from.
        /// </summary>
        /// <param name="navigationContext"> The navigation context onNavigatedFrom</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// Save the task.
        /// </summary>
        private void Save()
        {
            ServiceTaskHourEstimate serviceTaskHourEstimate = new ServiceTaskHourEstimate()
            {
                NewEstimate = double.Parse(this.HoursRemaining, System.Globalization.CultureInfo.InvariantCulture),
                Estimate = double.Parse(this.HoursRemaining, System.Globalization.CultureInfo.InvariantCulture),
                Created = DateTime.Now,
                TaskId = this.taskId
            };

            ServiceTask serviceTask = new ServiceTask()
            {
                TaskId = this.TaskId,
                SprintStoryId = this.sprintStoryId,
                Summary = this.Summary,
                Description = this.Description,
                ConditionsOfSatisfaction = this.ConditionsOfSatisfaction,
                Blocked = this.Blocked
            };

            //// Save task
            sprintServiceGateway.AddTaskToSprintStory(this.SprintStoryId, serviceTask);

            //// Save update hours
            sprintServiceGateway.AddTaskHourEstimateToTask(this.TaskId, serviceTaskHourEstimate);

            NavigationParameters navParams = new NavigationParameters();
            navParams.Add("sprintStory", this.serviceSprintStory);
            this.ShowMetroFlyout("SprintStory", navParams);
            this.IsOpen = false;
        }
    }
}
