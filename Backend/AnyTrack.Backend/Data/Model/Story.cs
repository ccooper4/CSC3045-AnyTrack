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
        /// Gets or sets the conditions of satisfaction
        /// </summary>
        public virtual string ConditionsOfSatisfaction { get; set; }

        /// <summary>
        /// Gets or sets the asA value.
        /// </summary>
        public string AsA { get; set; }

        /// <summary>
        /// Gets or sets the iWant value.
        /// </summary>
        public string IWant { get; set; }

        /// <summary>
        /// Gets or sets the soThat value.
        /// </summary>
        public string SoThat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the story is in a sprint
        /// </summary>
        public bool InSprint { get; set; }

        /// <summary>
        /// Gets or sets the project the story is part of.
        /// </summary>
        public virtual Project Project { get; set; }
    }
}
