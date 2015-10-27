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
        /// Gets or sets the Scrum Master Flag.
        /// </summary>
        public virtual bool ScrumMaster { get; set; }

        /// <summary>
        /// Gets or sets the Product Owner Flag.
        /// </summary>
        public virtual bool ProductOwner { get; set; }

        /// <summary>
        /// Gets or sets the Developer Flag.
        /// </summary>
        public virtual bool Developer { get; set; }
    }
}
