using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Outlines the IDateTime service
    /// </summary>
    [ServiceContract]
    public interface IDateTimeService
    {
        /// <summary>
        /// Returns the current datetime. 
        /// </summary>
        /// <returns>The current time.</returns>
        [OperationContract]
        DateTime GetCurrentDate();
    }
}
