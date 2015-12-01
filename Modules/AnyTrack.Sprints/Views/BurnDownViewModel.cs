using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendSprintService;
using AnyTrack.Infrastructure.ServiceGateways;
using OxyPlot;
using OxyPlot.Axes;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// The burn down view model.
    /// </summary>
    public class BurnDownViewModel : ValidatedBindableBase
    {
        #region Fields 

        /// <summary>
        /// The sprint service gateway
        /// </summary>
        private readonly ISprintServiceGateway serviceGateway;

        /// <summary>
        /// The sprintID.
        /// </summary>
        private Guid sprintID;

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

        #endregion 

        #region Constructor

        /// <summary>
        /// Burn down view model
        /// </summary>
        /// <param name="serviceGateway"> the service gateway</param>
        public BurnDownViewModel(ISprintServiceGateway serviceGateway)
        {
            if (serviceGateway == null)
            {
                throw new ArgumentNullException("serviceGateway");
            }

            this.serviceGateway = serviceGateway;
            this.Title = "Sprint Burndown Chart";
            this.points = new ObservableCollection<DataPoint>();

            List<ServiceTask> litsOfAllTasks = serviceGateway.GetAllTasksForSprint(sprintID);

            foreach (var item in litsOfAllTasks)
            {
                foreach (var taskHour in item.TaskHourEstimates)
                {
                    this.Points.Add(new DataPoint(DateTimeAxis.ToDouble(taskHour.Created), taskHour.Estimate));
                }
            }
        }
        #endregion

        #region Properties 

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
        /// Gets or sets the sprintID
        /// </summary>
        public Guid SprintID
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
        #endregion
    }
}