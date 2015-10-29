using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Outlines the time servie. 
    /// </summary>
    public interface ITimeService
    {
        /// <summary>
        /// Returns the current date;
        /// </summary>
        /// <returns>The current date.</returns>
        DateTime GetDate();
    }
}
