using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Accounting.ServiceGateways.Models
{
    /// <summary>
    /// Contains the fields required to register a user account. 
    /// </summary>
    public class NewUserRegistration
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user want to be a Product Owner.
        /// </summary>
        public bool ProductOwner { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user want to be a Scrum Master.
        /// </summary>
        public bool ScrumMaster { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user want to be a Developer.
        /// </summary>
        public bool Developer { get; set; }
    }
}
