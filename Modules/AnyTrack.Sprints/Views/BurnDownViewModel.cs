using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.ServiceGateways;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using Prism.Commands;
using Prism.Regions;
using DateTimeAxis = OxyPlot.Axes.DateTimeAxis;
using SprintModels = AnyTrack.Infrastructure.BackendSprintService;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// The burn down view model.
    /// </summary>
    public class BurnDownViewModel : ValidatedBindableBase, IRegionMemberLifetime, INavigationAware
    {
        #region Fields 

        /// <summary>
        /// The sprint service gateway
        /// </summary>
        private readonly ISprintServiceGateway sprintServiceGateway;

        /// <summary>
        /// The project service gateway.
        /// </summary>
        private readonly IProjectServiceGateway projectServiceGateway;

        /// <summary>
        /// The selected project id.
        /// </summary>
        private Guid? projectId;

        /// <summary>
        /// The selected sprint id.
        /// </summary>
        private Guid? sprintID;

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
        /// The plot model.
        /// </summary>
        private PlotModel model; 

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

            this.projectServiceGateway = projectServiceGateway;
            this.sprintServiceGateway = sprintServiceGateway;
            this.Sprints = new ObservableCollection<SprintModels.ServiceSprintSummary>();
            this.Points = new ObservableCollection<DataPoint>();
            this.PlotModel = new PlotModel();

            EmailFlyoutCommand = new DelegateCommand(EmailFlyout);
        }
        #endregion

        #region Properties 

        /// <summary>
        /// Gets or sets PlotGraph
        /// </summary>
        public Plot PlotGraph { get; set; }

        /// <summary>
        /// Gets or sets the command for flyout email
        /// </summary>
        public DelegateCommand EmailFlyoutCommand { get; set; }

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
        public ObservableCollection<SprintModels.ServiceSprintSummary> Sprints { get; set; }

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
        public ObservableCollection<ServiceProjectSummary> Projects { get; set; }

        /// <summary>
        /// Gets or sets the projectID
        /// </summary>
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
        public Guid? SprintID
        {
            get
            {
                return sprintID;
            }

            set
            {
                SetProperty(ref sprintID, value);
            }
        }
        
        /// <summary>
        /// Gets or sets the plot model.
        /// </summary>
        public PlotModel PlotModel
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        #endregion Properties

        #region Methods
        /// <summary>
        /// Handles the On Navigated To event.
        /// </summary>
        /// <param name="navigationContext">The navigation context</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("sprintId"))
            {
                sprintID = (Guid)navigationContext.Parameters["sprintId"];
                List<SprintModels.ServiceTask> listOfAllTasks = sprintServiceGateway.GetAllTasksForSprint(sprintID.Value);
                foreach (var item in listOfAllTasks)
                {
                    foreach (var taskHour in item.TaskHourEstimates)
                    {
                        this.Points.Add(new DataPoint(DateTimeAxis.ToDouble(taskHour.Created), taskHour.Estimate));
                    }
                }

                var x = new OxyPlot.Series.LineSeries()
                {
                    ItemsSource = this.Points
                }; 
                PlotModel.Series.Add(x);
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
        /// Method to open the email flyout
        /// </summary>
        private void EmailFlyout()
        {
            ////Burndown chart will be added as a nav param!
            ////var navParams = new NavigationParameters();
            this.ShowMetroFlyout("BurnDownEmailOptions");
        }
        #endregion Methods
    }
}