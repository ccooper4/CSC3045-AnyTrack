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
        /// Gets or sets the name of the story
        /// </summary>
        public string StoryName { get; set; }

        /// <summary>
        /// Gets or sets the story summary
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the story description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the assignee
        /// </summary>
        public string Assignee { get; set; }

        /// <summary>
        /// Gets or sets the tester
        /// </summary>
        public string Tester { get; set; }

        /// <summary>
        /// Gets or sets the conditions of satisfaction
        /// </summary>
        public string ConditionsOfSatisfaction { get; set; }

        /// <summary>
        /// Gets or sets the project the story is part of
        /// </summary>
        public Project Project { get; set; }
    }
}
