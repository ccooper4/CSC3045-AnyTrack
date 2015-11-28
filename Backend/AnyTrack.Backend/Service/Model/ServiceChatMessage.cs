using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// The class representing a chat message.
    /// </summary>
    public class ServiceChatMessage
    {
        /// <summary>
        /// Gets or sets the session id. 
        /// </summary>
        public Guid SessionID { get; set; }

        /// <summary>
        /// Gets or sets the name. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the message. 
        /// </summary>
        public string Message { get; set; }
    }
}
