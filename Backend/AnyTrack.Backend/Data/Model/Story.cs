using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Service.Model;

namespace AnyTrack.Backend.Data.Model
{
    /// <summary>
    /// Represents a story entity.
    /// </summary>
    public class Story : BaseEntity
    {
        /// <summary>
        /// Gets or sets the story summary
        /// </summary>
        public virtual string Summary { get; set; }

        /// <summary>
        /// Gets or sets the user story.
        /// </summary>
        public virtual string StoryBreakdown { get; set; }

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
