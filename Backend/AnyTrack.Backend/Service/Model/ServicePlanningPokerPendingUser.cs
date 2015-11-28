using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Represents a user who is connected but not yet in a session. 
    /// </summary>
    public class ServicePlanningPokerPendingUser
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public Guid UserID { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the user's full name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets this user's roles.
        /// </summary>
        public List<string> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the channel used for communication with this user.
        /// </summary>
        [XmlIgnore]
        public IPlanningPokerClientService ClientChannel { get; set; }
    }
}
