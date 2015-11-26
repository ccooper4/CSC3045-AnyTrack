using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Data.Model
{
    /// <summary>
    /// Updated hours entity
    /// </summary>
    public class UpdatedHours : BaseEntity
    {
        /// <summary>
        /// Gets or sets the new estimate
        /// </summary>
        public virtual double NewEstimate { get; set; }

        /// <summary>
        /// Gets or sets the log estimate
        /// </summary>
        public virtual double LogEstimate { get; set; }
    }
}
