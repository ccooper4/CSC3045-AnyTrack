using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Outlines the data that is sent to a pending client when a session change is observed. 
    /// </summary>
    public class ServiceSessionChangeInfo
    {
        /// <summary>
        /// Gets or sets the sprint id.
        /// </summary>
        public Guid SprintId { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether or not a session is available. 
        /// </summary>
        public bool SessionAvailable { get; set; }

        /// <summary>
        /// Gets or sets the session id if one is available.
        /// </summary>
        public Guid? SessionId { get; set; }

        /// <summary>
        /// Gets or sets the sprint name.
        /// </summary>
        public string SprintName { get; set; }

        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        public string ProjectName { get; set; }
    }
}
