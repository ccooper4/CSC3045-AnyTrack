using System.Collections.ObjectModel;
using AnyTrack.Infrastructure;
using OxyPlot;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// The burn down view model.
    /// </summary>
    public class BurnDownViewModel : ValidatedBindableBase
    {
        #region Fields 

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
        /// Constructs the burn down view model.
        /// </summary>
        public BurnDownViewModel()
        {
            this.Title = "Sprint Burndown Chart";
            this.Points = new ObservableCollection<DataPoint>
                            {
                                new DataPoint(0, 16),
                                new DataPoint(10, 20),
                                new DataPoint(20, 34),
                                new DataPoint(30, 10),
                                new DataPoint(40, 12),
                                new DataPoint(50, 0),
            };
            this.Trend = new ObservableCollection<DataPoint>
                            {
                ////replace this with the trend points suitable to the sitch.
                                new DataPoint(0, 16),
                                new DataPoint(10, 14),
                                new DataPoint(20, 12),
                                new DataPoint(30, 8),
                                new DataPoint(40, 6),
                                new DataPoint(50, 0),
            };
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

        #endregion
    }
}