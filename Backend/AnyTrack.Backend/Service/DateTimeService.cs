using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Provides a service for getting the time.
    /// </summary>
    public class DateTimeService : IDateTimeService
    {
        #region Methods 

        /// <summary>
        /// Gets the current date. 
        /// </summary>
        /// <returns>The current date.</returns>
        [PrincipalPermission(SecurityAction.Demand, Name = "andrew.fletcher16@gmail.com")]
        public DateTime GetCurrentDate()
        {
            return DateTime.Now;
        }

        #endregion 
    }
}
