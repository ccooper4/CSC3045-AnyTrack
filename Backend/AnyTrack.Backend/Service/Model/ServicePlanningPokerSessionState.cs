using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Contains the various states of a planning poker session.
    /// </summary>
    public enum ServicePlanningPokerSessionState
    {
        /// <summary>
        /// The session is pending. 
        /// </summary>
        Pending = 0,

        /// <summary>
        /// The session has been started. 
        /// </summary>
        Started = 1,

        /// <summary>
        /// Getting story estimates started 
        /// </summary>
        GettingEstimates = 2,

        /// <summary>
        /// Showing story estimates
        /// </summary>
        ShowingEstimates = 3
    }
}
