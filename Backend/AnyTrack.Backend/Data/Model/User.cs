using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Data.Model
{
    /// <summary>
    /// Represents a user entity.
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public virtual string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scrum mater flag has been set.
        /// </summary>
        public virtual bool ScrumMaster { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product owner flag has been set.
        /// </summary>
        public virtual bool ProductOwner { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the developer flag has been set.
        /// </summary>
        public virtual bool Developer { get; set; }
    }
}
