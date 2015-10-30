using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Data.Model
{
    /// <summary>
    /// Represents a story entity.
    /// </summary>
    public class Story : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the story
        /// </summary>
        public virtual string StoryName { get; set; }

        /// <summary>
        /// Gets or sets the description of the story
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the story summary
        /// </summary>
        public virtual string Summary { get; set; }

        /// <summary>
        /// Gets or sets the assignee
        /// </summary>
        public virtual string Assignee { get; set; }

        /// <summary>
        /// Gets or sets the tester
        /// </summary>
        public virtual string Tester { get; set; }

        /// <summary>
        /// Gets or sets the conditions of satisfaction
        /// </summary>
        public virtual string ConditionsOfSatisfaction { get; set; }

        /// <summary>
        /// Gets or sets the project the story is part of
        /// </summary>
        public virtual Project Project { get; set; }
    }
}
