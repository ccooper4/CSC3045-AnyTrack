using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Represents the fields required to create a user account.
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
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
        /// Gets or sets a value indicating whether the product owner flag has been set.
        /// </summary>
        public bool ProductOwner { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scrum master flag has been set.
        /// </summary>
        public bool ScrumMaster { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the developer flag has been set.
        /// </summary>
        public bool Developer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the success flag has been set.
        /// </summary>
        public bool Success { get; set; }
    }
}
