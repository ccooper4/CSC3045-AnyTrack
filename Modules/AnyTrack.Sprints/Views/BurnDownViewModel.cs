using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.BackendSprintService;
using AnyTrack.Infrastructure.ServiceGateways;
using OxyPlot;
using OxyPlot.Axes;
using Prism.Commands;
using Prism.Regions;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// The burn down view model.
    /// </summary>
    public class BurnDownViewModel : ValidatedBindableBase, IRegionMemberLifetime, INavigationAware
    {
        #region Fields 

        /// <summary>
        /// The project service gateway.
        /// </summary>
        private readonly IProjectServiceGateway projectServiceGateway;

        /// <summary>
        /// The sprint service gateway
        /// </summary>
        private readonly ISprintServiceGateway sprintServiceGateway;

        /// <summary>
        /// The selected project id.
        /// </summary>
        private Guid? projectId;

        /// <summary>
        /// The selected sprint id.
        /// </summary>
        private Guid? sprintId;

        /// <summary>
        /// The list of projects.
        /// </summary>
        private ObservableCollection<ServiceProjectSummary> projects;

        /// <summary>
        /// The list of sprints.
        /// </summary>
        private ObservableCollection<Infrastructure.BackendSprintService.ServiceSprintSummary> sprints;
       
        /// <summary>
        /// The title.
        /// </summary>
        private string title;

        /// <summary>
        /// The graph points.
        /// </summary>
        private ObservableCollection<DataPoint> points;
        
        /// <summary>
        /// The trend points.
        /// </summary>
        private ObservableCollection<DataPoint> trend;

        /// <summary>
        /// The plot model for the burndowns
        /// </summary>
        private PlotModel plotModel;

        /// <summary>
        /// The story burn down to 0 option.
        /// </summary>
        private bool storyBurnDownOption = false;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Gets a value indicating whether or not the service gateway exists
        /// </summary>
        /// <param name="projectServiceGateway"> the service gateway for projects</param>
        /// <param name="sprintServiceGateway"> the service gateway for sprint</param>
        public BurnDownViewModel(IProjectServiceGateway projectServiceGateway, ISprintServiceGateway sprintServiceGateway)
        {
            if (projectServiceGateway == null)
            {
                throw new ArgumentNullException("projectServiceGateway");
            }

            if (sprintServiceGateway == null)
            {
                throw new ArgumentNullException("sprintServiceGateway");
            }

            this.GetStoryPointBD = new DelegateCommand(GetStoryPointBurnDown);
            this.GetChartForProjectAndSprint = new DelegateCommand(GetBurndownChartForProjectAndSprint);
            this.projectServiceGateway = projectServiceGateway;
            this.sprintServiceGateway = sprintServiceGateway;
            this.Projects = new ObservableCollection<ServiceProjectSummary>(projectServiceGateway.GetProjectNames(true, true, true));
            this.Sprints = new ObservableCollection<Infrastructure.BackendSprintService.ServiceSprintSummary>(sprintServiceGateway.GetSprintNames(projectId, true, true));
            this.Points = new ObservableCollection<DataPoint>();
            this.Trend = new ObservableCollection<DataPoint>();
            this.PlotModel = new PlotModel();
        }
        #endregion

        #region Properties 

        /// <summary>
        /// Gets a value indicating whether the view can be reused
        /// </summary>
        public bool KeepAlive
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                SetProperty(ref title, value);
            }
        }

        /// <summary>
        /// Gets or sets the plotmodel
        /// </summary>
        public PlotModel PlotModel
        {
            get
            {
                return plotModel;
            }

            set
            {
                SetProperty(ref plotModel, value);
            }
        }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        public ObservableCollection<DataPoint> Points
        {
            get
            {
                return points;
            }

            set
            {
                SetProperty(ref points, value);
            }
        }

        /// <summary>
        /// Gets or sets a list of sprints
        /// </summary>
        public ObservableCollection<Infrastructure.BackendSprintService.ServiceSprintSummary> Sprints
        {
            get
            {
                return sprints;
            }

            set
            {
                SetProperty(ref sprints, value);
            }
        }

        /// <summary>
        /// Gets or sets the trend line.
        /// </summary>
        public ObservableCollection<DataPoint> Trend
        {
            get
            {
                return trend;
            }

            set
            {
                SetProperty(ref trend, value);
            }
        }

        /// <summary>
        /// Gets or sets the list of projects.
        /// </summary>
        public ObservableCollection<ServiceProjectSummary> Projects
        {
            get
            {
                return projects;
            }

            set
            {
                SetProperty(ref projects, value);
            }
        }

        /// <summary>
        /// Gets or sets the projectID
        /// </summary>
        [Required]
        public Guid? ProjectId
        {
            get
            {
                return projectId;
            }

            set
            {
                SetProperty(ref projectId, value);
                this.Sprints.Clear();
                var sprints = sprintServiceGateway.GetSprintNames(value, true, false);
                Sprints.AddRange(sprints);
            }
        }

        /// <summary>
        /// Gets or sets the sprintID
        /// </summary>
        [Required]
        public Guid? SprintId
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
        /// Gets or sets a command to get a chart based upon selected sprint and project names.
        /// </summary>
        public DelegateCommand GetChartForProjectAndSprint { get; set; }

        /// <summary>
        /// Gets or sets a command to get a chart based upon story points and done tasks for a given sprint.
        /// </summary>
        public DelegateCommand GetStoryPointBD { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the On Navigated To event.
        /// </summary>
        /// <param name="navigationContext">The navigation context</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {        
            if (navigationContext.Parameters.ContainsKey("sprintId") 
                && navigationContext.Parameters.ContainsKey("projectId"))
            {
                ProjectId = (Guid)navigationContext.Parameters["projectId"];
                SprintId = (Guid)navigationContext.Parameters["sprintId"];
                if (navigationContext.Parameters.ContainsKey("storyBurnDownOption"))
                {
                    this.storyBurnDownOption = (bool)navigationContext.Parameters["storyBurnDownOption"];
                }

                List<ServiceTask> listOfAllTasks = sprintServiceGateway.GetAllTasksForSprint(sprintId.Value);
                List<Infrastructure.BackendSprintService.ServiceSprintStory> listOfAllEstimates = sprintServiceGateway.GetSprintStoryEstimates(sprintId.Value); 
                if (listOfAllTasks.Count != 0)
                {
                    if (!this.storyBurnDownOption)
                    {
                        DateTime? start = sprintServiceGateway.GetDateSprintStarted(sprintId.Value);
                        DateTime? end = sprintServiceGateway.GetDateSprintEnds(sprintId.Value);
                        double highestEstimate = sprintServiceGateway.GetSprintMaxEstimate(sprintId.Value);
                        this.Trend.Add(new DataPoint(DateTimeAxis.ToDouble(start), highestEstimate));
                        this.Trend.Add(new DataPoint(DateTimeAxis.ToDouble(end), 0));
                        foreach (var item in listOfAllTasks)
                        {
                            foreach (var taskHour in item.TaskHourEstimates)
                            {
                                this.Points.Add(new DataPoint(DateTimeAxis.ToDouble(taskHour.Created), taskHour.Estimate));
                            }
                        }
                    }
                    else
                    {
                        DateTime? start = sprintServiceGateway.GetDateSprintStarted(sprintId.Value);
                        DateTime? end = sprintServiceGateway.GetDateSprintEnds(sprintId.Value);
                        double totalStoryPoints = sprintServiceGateway.GetTotalStoryPointEstimate(sprintId.Value);
                        this.Trend.Add(new DataPoint(DateTimeAxis.ToDouble(start), totalStoryPoints));
                        this.Trend.Add(new DataPoint(DateTimeAxis.ToDouble(end), 0));
                        foreach (var sprintStory in listOfAllEstimates)
                        {
                            this.points.Add(new DataPoint(DateTimeAxis.ToDouble(sprintStory.DateCompleted), sprintStory.StoryEstimate));
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Handles the Is Navigation Target event
        /// </summary>
        /// <param name="navigationContext">The navigation context</param>
        /// <returns>Returns false as this is not a target</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        /// <summary>
        /// Handles the On Navigated From event
        /// </summary>
        /// <param name="navigationContext">The navigation context</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// Navigates to the display burndown charts.
        /// </summary>
        private void GetBurndownChartForProjectAndSprint()
        {
            this.storyBurnDownOption = false;
            this.Trend.Clear();
            this.Points.Clear();
            ValidateViewModelNow();
            if (!this.HasErrors)
            {
                var navParams = new NavigationParameters();
                navParams.Add("projectId", projectId);
                navParams.Add("sprintId", sprintId);
                NavigateToItem("BurnDown", navParams);
            }
            else
            {
                if (projectId == null)
                {
                    this.ShowMetroDialog("Invalid Operation!", "You have not provided a project.");
                }

                if (sprintId == null)
                {
                    this.ShowMetroDialog("Invalid Operation!", "You have not provided a sprint.");
                }
            }
        }

        /// <summary>
        /// Navigates to the display burndown charts for the story burn down to 0.
        /// </summary>
        private void GetStoryPointBurnDown()
        {
            this.storyBurnDownOption = true;
            this.Trend.Clear();
            ValidateViewModelNow();
            if (!this.HasErrors)
            {
                var navParams = new NavigationParameters();
                navParams.Add("projectId", projectId);
                navParams.Add("sprintId", sprintId);
                navParams.Add("storyBurnDownOption", storyBurnDownOption);
                NavigateToItem("BurnDown", navParams);
            }
            else
            {
                if (projectId == null)
                {
                    this.ShowMetroDialog("Invalid Operation!", "You have not provided a project.");
                }

                if (sprintId == null)
                {
                    this.ShowMetroDialog("Invalid Operation!", "You have not provided a sprint.");
                }
            }
        }

        #endregion Methods
    }
}