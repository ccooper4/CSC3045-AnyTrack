using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.BackendSprintService;
using AnyTrack.Infrastructure.ServiceGateways;
using MahApps.Metro.Controls;
using Prism.Commands;
using Prism.Regions;
using ServiceSprintStory = AnyTrack.Infrastructure.BackendSprintService.ServiceSprintStory;
using ServiceStory = AnyTrack.Infrastructure.BackendSprintService.ServiceStory;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// The sprint story view model.
    /// </summary>
    public class SprintStoryViewModel : ValidatedBindableBase, INavigationAware, IRegionMemberLifetime, IFlyoutCompatibleViewModel
    {
        #region Fields

        /// <summary>
        /// The sprint story passed to this view model. 
        /// </summary>
        private ServiceSprintStory sprintStory;

        /// <summary>
        /// Project Name
        /// </summary>
        private string projectName;

        /// <summary>
        /// The story belonging to this sprint story.
        /// </summary>
        private ServiceStory story;

        /// <summary>
        /// The project Id
        /// </summary>
        private Guid sprintId;

        /// <summary>
        /// The project Id
        /// </summary>
        private Guid sprintStoryId;

        /// <summary>
        /// summary field
        /// </summary>
        private string summary;

        /// <summary>
        /// as a
        /// </summary>
        private string asA;

        /// <summary>
        /// I want
        /// </summary>
        private string iWant;

        /// <summary>
        /// so that
        /// </summary>
        private string soThat;

        /// <summary>
        /// conditionsOfSatisfaction var
        /// </summary>
        private string conditionsOfSatisfaction;

        /// <summary>
        /// all status member
        /// </summary>
        private ObservableCollection<string> allStatus; 

        /// <summary>
        /// status member
        /// </summary>
        private string status;

        /// <summary>
        /// status member
        /// </summary>
        private int storyPoints;

        /// <summary>
        /// The is open field.
        /// </summary>
        private bool isOpen;

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

        /// <summary>
        /// The list of tasks for this sprint story.
        /// </summary>
        private ObservableCollection<ServiceTask> tasks; 

        #endregion

        /// <summary>
        /// Creates a new Sprint Story View Model
        /// </summary>
        /// <param name="sprintServiceGateway"> the sprint service gateway </param>
        public SprintStoryViewModel(ISprintServiceGateway sprintServiceGateway)
        {
            // Null checks
            if (sprintServiceGateway == null)
            {
                throw new ArgumentNullException("sprintServiceGateway");
            }

            // Set service
            this.sprintServiceGateway = sprintServiceGateway;

            // flyout settings
            this.Header = null;
            this.Theme = FlyoutTheme.Accent;
            this.Position = Position.Right;
            this.IsModal = true;
            this.CloseButtonVisibility = Visibility.Collapsed;

            //// Set status combobox values
            this.AllStatus = new ObservableCollection<string>();
            AllStatus.Add(ServiceSprintStoryStatus.NotStarted);
            AllStatus.Add(ServiceSprintStoryStatus.InProgress);
            AllStatus.Add(ServiceSprintStoryStatus.AwaitingTest);
            AllStatus.Add(ServiceSprintStoryStatus.InTest);
            AllStatus.Add(ServiceSprintStoryStatus.Done);

            OpenTaskViewCommand = new DelegateCommand(this.OpenTaskView);
            SaveSprintStoryCommand = new DelegateCommand(this.SaveSprintStory);
            DeleteTaskCommand = new DelegateCommand<ServiceTask>(this.DeleteTask);
            EditTaskCommand = new DelegateCommand<ServiceTask>(this.EditTask);
        }

        /// <summary>
        /// Gets the command used to open a sprint story view. 
        /// </summary>
        public DelegateCommand OpenTaskViewCommand { get; private set; }

        /// <summary>
        /// Gets the command used to open a sprint story view. 
        /// </summary>
        public DelegateCommand SaveSprintStoryCommand { get; private set; }

        /// <summary>
        /// Gets the command used to open a sprint story view. 
        /// </summary>
        public DelegateCommand<ServiceTask> DeleteTaskCommand { get; private set; }

        /// <summary>
        /// Gets the comman used to edit a task.
        /// </summary>
        public DelegateCommand<ServiceTask> EditTaskCommand { get; private set; }

        /// <summary>
        /// Gets or sets all the status'
        /// </summary>
        public ObservableCollection<ServiceTask> Tasks
        {
            get
            {
                return tasks;
            }

            set
            {
                SetProperty(ref tasks, value);
            }
        }

        /// <summary>
        /// Gets or sets all the status'
        /// </summary>
        public ObservableCollection<string> AllStatus
        {
            get
            {
                return allStatus;
            }

            set
            {
                SetProperty(ref allStatus, value);
            }
        }

        /// <summary>
        /// Gets or sets the sprint story Id
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
        /// Gets or sets the sprint Id
        /// </summary>
        public Guid SprintId
        {
            get
            {
                return sprintId;
            }

            set
            {
                SetProperty(ref sprintId, value);
            }
        }

        /// <summary>
        /// Gets or sets the project Id
        /// </summary>
        public ServiceStory Story
        {
            get
            {
                return story;
            }

            set
            {
                SetProperty(ref story, value);
            }
        }

        /// <summary>
        /// Gets or sets the summary
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
        /// Gets or sets the as a
        /// </summary>
        public string AsA
        {
            get
            {
                return asA;
            }

            set
            {
                SetProperty(ref asA, value);
            }
        }

        /// <summary>
        /// Gets or sets the i want
        /// </summary>
        public string IWant
        {
            get
            {
                return iWant;
            }

            set
            {
                SetProperty(ref iWant, value);
            }
        }

        /// <summary>
        /// Gets or sets the so that
        /// </summary>
        public string SoThat
        {
            get
            {
                return soThat;
            }

            set
            {
                SetProperty(ref soThat, value);
            }
        }

        /// <summary>
        /// Gets or sets the conditions of satisfaction
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
        /// Gets or sets the status
        /// </summary>
        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                this.sprintStory.Status = value;
                SetProperty(ref status, value);
            }
        }

        /// <summary>
        /// Gets or sets the story points
        /// </summary>
        public int StoryPoints
        {
            get
            {
                return storyPoints;
            }

            set
            {
                SetProperty(ref storyPoints, value);
            }
        }

        #region Flyouts

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

        #endregion

        /// <summary>
        /// On navigated to.
        /// </summary>
        /// <param name="navigationContext"> The navigation context </param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("sprintStory"))
            {
                var sprintStory = (ServiceSprintStory)navigationContext.Parameters["sprintStory"];
                this.sprintStory = sprintStory;

                //// IDs
                this.SprintStoryId = sprintStory.SprintStoryId;
                this.SprintId = sprintStory.SprintId;

                //// Story attributes
                this.Summary = sprintStory.Story.Summary;
                this.AsA = sprintStory.Story.AsA;
                this.IWant = sprintStory.Story.IWant;
                this.SoThat = sprintStory.Story.SoThat;
                this.ConditionsOfSatisfaction = sprintStory.Story.ConditionsOfSatisfaction;

                //// Sprint story attributes
                this.Status = sprintStory.Status;
                //// TODO - story points, created, updated.

                var tasks = sprintServiceGateway.GetAllTasksForSprintStory(sprintStory.SprintStoryId);
                if (tasks != null)
                {
                    this.Tasks = new ObservableCollection<ServiceTask>(tasks);
                }
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
        /// Save the story.
        /// </summary>
        private void SaveSprintStory()
        {
            ServiceSprintStory sprintStory = new ServiceSprintStory()
            {
                SprintId = this.SprintId,
                Story = this.Story,
                Status = this.Status,
                SprintStoryId = this.SprintStoryId,
            };

            IsOpen = false;
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="task">the task to delete</param>
        private void DeleteTask(ServiceTask task)
        {
            sprintServiceGateway.DeleteTask(task.TaskId);
            Tasks.Remove(task);
        }

        /// <summary>
        /// Open story view.
        /// </summary>
        private void OpenTaskView()
        {
            NavigationParameters navParams = new NavigationParameters();
            navParams.Add("sprintStoryId", SprintStoryId);
            Navigate(navParams);
        }

        /// <summary>
        /// Edit a task
        /// </summary>
        /// <param name="task">the task to delete</param>
        private void EditTask(ServiceTask task)
        {
            NavigationParameters navParams = new NavigationParameters();
            navParams.Add("task", task);
            Navigate(navParams);
        }

        /// <summary>
        /// Base navigation method.
        /// </summary>
        /// <param name="navParams">the nav params</param>
        private void Navigate(NavigationParameters navParams)
        {
            navParams.Add("sprintStory", this.sprintStory);
            this.ShowMetroFlyout("Task", navParams);
            IsOpen = false;
        }
    }
}
