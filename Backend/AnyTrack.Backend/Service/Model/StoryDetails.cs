using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Represents the fields required to obtain a story from given projects.
    /// </summary>
    public class StoryDetails
    {
        /// <summary>
        /// Gets or sets the story id.
        /// </summary>
        public Guid StoryId { get; set; }

        /// <summary>
        /// Gets or sets the story name.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public Guid ProjectId { get; set; }
    }
}
