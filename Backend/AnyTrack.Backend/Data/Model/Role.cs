using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Data.Model
{
    /// <summary>
    /// Represents a user's role assignment. 
    /// </summary>
    public class Role : BaseEntity
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the role name.
        /// </summary>
        public virtual string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public virtual Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the sprint id.
        /// </summary>
        public virtual Guid? SprintId { get; set; }
    }
}
