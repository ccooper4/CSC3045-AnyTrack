using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Data.Model
{
    /// <summary>
    /// Represents a Sprint Entity.
    /// </summary>
    public class Sprint : BaseEntity
    {
        /// <summary>
        /// Gets or sets the sprint's name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the sprint'a start date.
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the sprint'a end date. 
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the sprint's descriptions.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets a collection of the sprint's team members.
        /// </summary>
        public virtual ICollection<User> Team { get; set; }

        /// <summary>
        /// Gets or sets a collection of the sprint's backlog of stories.
        /// </summary>
        public virtual ICollection<SprintStory> Backlog { get; set; }
    }
}
