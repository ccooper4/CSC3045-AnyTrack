using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Represents the fields required to create a sprint story.
    /// </summary>
    public class ServiceSprintStory
    {
        /// <summary>
        /// Gets or sets the sprintstory id.
        /// </summary>
        public Guid SprintStoryId { get; set; }

        /// <summary>
        /// Gets or sets the sprint id of the sprint this story belongs to
        /// </summary>
        public Guid SprintId { get; set; }

        /// <summary>
        /// Gets or sets the story that the sprint references to.
        /// </summary>
        public ServiceStory Story { get; set; }

        /// <summary>
        /// Gets or sets the sprint story status.
        /// </summary>
        public string Status { get; set; }
    }
}
