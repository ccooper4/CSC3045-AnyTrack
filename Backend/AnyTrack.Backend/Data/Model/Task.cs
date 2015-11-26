using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Service.Model;

namespace AnyTrack.Backend.Data.Model
{
    /// <summary>
    /// Represents a Task entity.
    /// </summary>
    public class Task : BaseEntity
    {
        /// <summary>
        /// Gets or sets the story this task is part of.
        /// </summary>
        public virtual SprintStory SprintStory { get; set; }

        /// <summary>
        /// Gets or sets the Conditions Of Satisfaction
        /// </summary>
        public virtual string ConditionsOfSatisfaction { get; set; }

        /// <summary>
        /// Gets or sets the Summary
        /// </summary>
        public virtual string Summary { get; set; }

        /// <summary>
        /// Gets or sets the Summary
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the Assignee
        /// </summary>
        public virtual User Assignee { get; set; }

        /// <summary>
        /// Gets or sets the Tester
        /// </summary>
        public virtual User Tester { get; set; }

        /// <summary>
        /// Gets or sets the Hours Remaining
        /// </summary>
        public virtual double HoursRemaining { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether task is blocked or not
        /// </summary>
        public virtual bool Blocked { get; set; }
    }
}
