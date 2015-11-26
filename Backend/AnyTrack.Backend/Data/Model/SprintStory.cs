using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Data.Model
{
    /// <summary>
    /// Represents a Sprint Story.
    /// </summary>
    public class SprintStory : BaseEntity
    {
        /// <summary>
        /// Gets or sets the story that the sprint references to.
        /// </summary>
        public virtual Story Story { get; set; }

        /// <summary>
        /// Gets or sets the sprint id
        /// </summary>
        public virtual Sprint Sprint { get; set; }
    }
}
