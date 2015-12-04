using System;
using System.Collections.ObjectModel;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.BackendSprintService;
using AnyTrack.Infrastructure.ServiceGateways;
using Prism.Commands;
using Prism.Regions;
using ServiceProjectSummary = AnyTrack.Infrastructure.BackendProjectService.ServiceProjectSummary;
using ServiceSprintStory = AnyTrack.Infrastructure.BackendSprintService.ServiceSprintStory;
using ServiceSprintSummary = AnyTrack.Infrastructure.BackendSprintService.ServiceSprintSummary;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// The sprint board view model.
    /// </summary>
    public class SprintBoardViewModel : ValidatedBindableBase, INavigationAware
    {
        /// <summary>
        /// The Sprint Service Gateway.
        /// </summary>
        private ISprintServiceGateway sprintServiceGateway;

        /// <summary>
        /// The Sprint Service Gateway.
        /// </summary>
        private IProjectServiceGateway projectServiceGateway;

        /// <summary>
        /// The project list.
        /// </summary>
        private ObservableCollection<ServiceProjectSummary> projects;

        /// <summary>
        /// The sprint list.
        /// </summary>
        private ObservableCollection<ServiceSprintSummary> sprints;

        /// <summary>
        /// The sprint id
        /// </summary>
        private Guid sprintId;

        /// <summary>
        /// The sprint id
        /// </summary>
        private Guid projectId;

        /// <summary>
        /// The total list of sprint stories. 
        /// </summary>
        private ObservableCollection<ServiceSprintStory> allSprintStories;

        /// <summary>
        /// Not started stories 
        /// </summary>
        private ObservableCollection<ServiceSprintStory> notStartedSprintStories;

        /// <summary>
        /// Not started stories 
        /// </summary>
        private ObservableCollection<ServiceSprintStory> inProgressSprintStories;

        /// <summary>
        /// Not started stories 
        /// </summary>
        private ObservableCollection<ServiceSprintStory> awaitingTestSprintStories;

        /// <summary>
        /// Not started stories 
        /// </summary>
        private ObservableCollection<ServiceSprintStory> inTestSprintStories;

        /// <summary>
        /// Not started stories 
        /// </summary>
        private ObservableCollection<ServiceSprintStory> doneSprintStories;

        #region Constructor

        /// <summary>
        /// Creates a new SprintBoardViewModel
        /// </summary>
        /// <param name="sprintServiceGateway">The sprint service gateway</param>
        /// <param name="projectServiceGateway">The project service gateway</param>
        public SprintBoardViewModel(ISprintServiceGateway sprintServiceGateway, IProjectServiceGateway projectServiceGateway)
        {
            // Null checks
            if (sprintServiceGateway == null)
            {
                throw new ArgumentNullException("sprintServiceGateway");
            }

            if (projectServiceGateway == null)
            {
                throw new ArgumentNullException("projectServiceGateway");
            }

            // Set services 
            this.sprintServiceGateway = sprintServiceGateway;
            this.projectServiceGateway = projectServiceGateway;

            // Set commands
            EditSprintStoryCommand = new DelegateCommand<ServiceSprintStory>(this.EditSprintStory);
            EditTaskHoursCommand = new DelegateCommand(this.GoToUpdateTaskHours);

            // Make service calls
            Projects = new ObservableCollection<ServiceProjectSummary>(projectServiceGateway.GetProjectNames(true, true, true));

            // Initialise others
            Sprints = new ObservableCollection<ServiceSprintSummary>();
            NotStartedSprintStories = new ObservableCollection<ServiceSprintStory>();
            InProgressSprintStories = new ObservableCollection<ServiceSprintStory>();
            AwaitingTestSprintStories = new ObservableCollection<ServiceSprintStory>();
            InTestSprintStories = new ObservableCollection<ServiceSprintStory>();
            DoneSprintStories = new ObservableCollection<ServiceSprintStory>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Projects.
        /// </summary>
        public ObservableCollection<ServiceProjectSummary> Projects
        {
            get { return projects; }
            set { SetProperty(ref projects, value); }
        }

        /// <summary>
        /// Gets a value indicating whether or not this view/view model should be reused.
        /// </summary>
        public bool KeepAlive
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the Sprints.
        /// </summary>
        public ObservableCollection<ServiceSprintSummary> Sprints
        {
            get { return sprints; }
            set { SetProperty(ref sprints, value); }
        }

        /// <summary>
        /// Gets or sets the project name
        /// </summary>
        public Guid ProjectId
        {
            get
            {
                return projectId;
            }

            set
            {
                var result = SetProperty(ref projectId, value);
                if (result)
                {
                    Sprints.Clear();
                    Sprints.AddRange(sprintServiceGateway.GetSprintNames(projectId, true, true));
                }
            }
        }

        /// <summary>
        /// Gets or sets the seleted sprint id
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
                ObservableCollection<ServiceSprintStory> sprintStories = new ObservableCollection<ServiceSprintStory>(sprintServiceGateway.GetSprintStories(sprintId));
                AllSprintStories = sprintStories;
            }
        }

        /// <summary>
        /// Gets or sets all sprint stories
        /// </summary>
        public ObservableCollection<ServiceSprintStory> AllSprintStories
        {
            get
            {
                return allSprintStories;
            }

            set
            {
                SetProperty(ref allSprintStories, value); 
                SortAllSprintStories(allSprintStories);
            }
        }

        /// <summary>
        /// Gets or sets the seleted sprint id
        /// </summary>
        public ObservableCollection<ServiceSprintStory> NotStartedSprintStories
        {
            get
            {
                return notStartedSprintStories;
            }

            set
            {
                SetProperty(ref notStartedSprintStories, value);
            }
        }

        /// <summary>
        /// Gets or sets the seleted sprint id
        /// </summary>
        public ObservableCollection<ServiceSprintStory> InProgressSprintStories
        {
            get
            {
                return inProgressSprintStories;
            }

            set
            {
                SetProperty(ref inProgressSprintStories, value);
            }
        }

        /// <summary>
        /// Gets or sets awaiting test sprint stories
        /// </summary>
        public ObservableCollection<ServiceSprintStory> AwaitingTestSprintStories
        {
            get
            {
                return awaitingTestSprintStories;
            }

            set
            {
                SetProperty(ref awaitingTestSprintStories, value);
            }
        }

        /// <summary>
        /// Gets or sets in test sprint stories
        /// </summary>
        public ObservableCollection<ServiceSprintStory> InTestSprintStories
        {
            get
            {
                return inTestSprintStories;
            }

            set
            {
                SetProperty(ref inTestSprintStories, value);
            }
        }

        /// <summary>
        /// Gets or sets Done sprint stories
        /// </summary>
        public ObservableCollection<ServiceSprintStory> DoneSprintStories
        {
            get
            {
                return doneSprintStories;
            }

            set
            {
                SetProperty(ref doneSprintStories, value);
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command used to open a sprint story view. 
        /// </summary>
        public DelegateCommand<ServiceSprintStory> EditSprintStoryCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that can be used to add a sprint.
        /// </summary>
        public DelegateCommand EditTaskHoursCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Is Navigation target event. 
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns>A true or false value indicating if this viewmodel can handle the navigation request.</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        /// <summary>
        /// Handles the on navigated from event. 
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// Handles the navigated to.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("sprintId"))
            {
                SprintId = (Guid)navigationContext.Parameters["sprintId"];
            }

            if (navigationContext.Parameters.ContainsKey("projectId"))
            {
                ProjectId = (Guid)navigationContext.Parameters["projectId"];
            }
        }

        /// <summary>
        /// Sort the supplied list of stories into their relevant status property lists.
        /// </summary>
        /// <param name="allSprintStories"> the list of all stories </param>
        private void SortAllSprintStories(ObservableCollection<ServiceSprintStory> allSprintStories)
        {
            // Clear previous stories
            NotStartedSprintStories.Clear();
            InProgressSprintStories.Clear();
            AwaitingTestSprintStories.Clear();
            InTestSprintStories.Clear();
            DoneSprintStories.Clear();

            // Populate for new stories
            foreach (var story in allSprintStories)
            {
                if (story.Status.Equals(ServiceSprintStoryStatus.NotStarted))
                {
                    NotStartedSprintStories.Add(story);
                }
                else if (story.Status.Equals(ServiceSprintStoryStatus.InProgress))
                {
                    InProgressSprintStories.Add(story);
                }
                else if (story.Status.Equals(ServiceSprintStoryStatus.AwaitingTest))
                {
                    AwaitingTestSprintStories.Add(story);
                }
                else if (story.Status.Equals(ServiceSprintStoryStatus.InTest))
                {
                    InTestSprintStories.Add(story);
                }
                else if (story.Status.Equals(ServiceSprintStoryStatus.Done))
                {
                    DoneSprintStories.Add(story);
                }
            }
        }

        /// <summary>
        /// Navigates to update task hours
        /// </summary>
        private void GoToUpdateTaskHours()
        {
            var navParams = new NavigationParameters();
            navParams.Add("sprintId", sprintId);
            navParams.Add("projectId", projectId);
            NavigateToItem("UpdateTaskHours", navParams);
        }

        /// <summary>
        /// Open story view.
        /// </summary>
        /// <param name="story">story object</param>
        private void EditSprintStory(ServiceSprintStory story)
        {
            var navParams = new NavigationParameters();
            navParams.Add("sprintStory", story);
            navParams.Add("sprintId", sprintId);
            navParams.Add("projectId", projectId);
            this.ShowMetroFlyout("SprintStory", navParams);
        }

        #endregion
    }
}
