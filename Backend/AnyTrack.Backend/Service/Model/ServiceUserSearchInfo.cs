using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Represents the information provided about a user in a search.
    /// </summary>
    public class ServiceUserSearchInfo
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the availability the user has on the sprint.
        /// </summary>
        public int? Availability { get; set; }

        /// <summary>
        /// Gets or sets the skills the user has as a string.
        /// </summary>
        public string Skills { get; set; }
    }
}
