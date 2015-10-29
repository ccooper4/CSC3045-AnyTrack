using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// The time service implementation.
    /// </summary>
    public class TimeService : ITimeService
    {
        #region Methods 

        /// <summary>
        /// Returns the current date;
        /// </summary>
        /// <returns>The current date.</returns>
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public DateTime GetDate()
        {
            return DateTime.Now;
        }

        #endregion 
    }
}
