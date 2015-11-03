using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Represents a story entity.
    /// </summary>
    public class Story
    {
        /// <summary>
        /// Creates a new Story Entity
        /// </summary>
        public Story()
        {
            StoryId = new Guid();
        }

        /// <summary>
        /// Gets or sets Story ID
        /// </summary>
        public Guid StoryId { get; set; }

        /// <summary>
        /// Gets or sets the story summary
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the projectId
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the conditions of satisfaction
        /// </summary>
        public string ConditionsOfSatisfaction { get; set; }

        /// <summary>
        /// Gets or sets the project the story is part of
        /// </summary>
        public ServiceProject Project { get; set; }
    }
}
