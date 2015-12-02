using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Class for planning poker estimates
    /// </summary>
    public class ServicePlanningPokerEstimate
    {
        /// <summary>
        /// Gets or sets the session id. 
        /// </summary>
        public Guid SessionID { get; set; }

        /// <summary>
        /// Gets or sets the user's estimate
        /// </summary>
        public double Estimate { get; set; }
    }
}
