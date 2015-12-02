using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Outlines the details concerning a single user in a planning poker session. 
    /// </summary>
    public class ServicePlanningPokerUser
    {
        /// <summary>
        /// Gets or sets the user id. 
        /// </summary>
        public Guid UserID { get; set; }

        /// <summary>
        /// Gets or sets the user's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the user's estimate
        /// </summary>
        public ServicePlanningPokerEstimate Estimate { get; set; }

        /// <summary>
        /// Gets or sets the list of user roles for this user.
        /// </summary>
        public List<string> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the channel that can be used for duplex communication with this client.
        /// </summary>
        [XmlIgnore]
        public IPlanningPokerClientService ClientChannel { get; set; }
    }
}
