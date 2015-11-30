using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Data.Model
{
    /// <summary>
    /// Remaining task hours entity.
    /// </summary>
    public class TaskHourEstimate : BaseEntity
    {
        /// <summary>
        /// Gets or sets the new estimate.
        /// </summary>
        public virtual double Estimate { get; set; }

        /// <summary>
        /// Gets or sets the task this new estimate is linked to.
        /// </summary>
        public virtual Task Task { get; set; }
    }
}
