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
    public class ServiceUpdatedHours
    {
        #region Constructor

        /// <summary>
        /// Creates a new Service updated hours
        /// </summary>
        public ServiceUpdatedHours()
        {
            UpdatedHoursId = new Guid();
        }

        #endregion

        /// <summary>
        /// Gets or sets Updated Hours ID
        /// </summary>
        public Guid UpdatedHoursId { get; set; }

        /// <summary>
        /// Gets or sets the new estimate
        /// </summary>
        public double NewEstimate { get; set; }

        /// <summary>
        /// Gets or sets the log estimate
        /// </summary>
        public double LogEstimate { get; set; }
    }
}
