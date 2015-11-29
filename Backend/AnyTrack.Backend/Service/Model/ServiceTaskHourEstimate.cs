using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Updated hours class
    /// </summary>
    public class ServiceTaskHourEstimate
    {
        #region Constructor

        /// <summary>
        /// Creates a new Service updated hours
        /// </summary>
        public ServiceTaskHourEstimate()
        {
            ServiceTaskHourEstimateId = new Guid();
        }

        #endregion

        /// <summary>
        /// Gets or sets Updated Hours ID
        /// </summary>
        public Guid ServiceTaskHourEstimateId { get; set; }

        /// <summary>
        /// Gets or sets the new estimate
        /// </summary>
        public double Estimate { get; set; }

        /// <summary>
        /// Gets or sets the taskId
        /// </summary>
        public Guid TaskId { get; set; }
    }
}
