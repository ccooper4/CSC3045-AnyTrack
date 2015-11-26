using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data.Model;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Service task class
    /// </summary>
    public class ServiceTask
    {
        #region Constructor

        /// <summary>
        /// Service task constructor
        /// </summary>
        public ServiceTask()
        {
            UpdatedHours = new List<ServiceUpdatedHours>();
            TaskId = new Guid();
        }

        #endregion

        /// <summary>
        /// Gets or sets Task ID
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the story this task is part of.
        /// </summary>
        public SprintStory SprintStory { get; set; }

        /// <summary>
        /// Gets or sets the Conditions Of Satisfaction
        /// </summary>
        public string ConditionsOfSatisfaction { get; set; }

        /// <summary>
        /// Gets or sets the Summary
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the Summary
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Assignee
        /// </summary>
        public ServiceUser Assignee { get; set; }

        /// <summary>
        /// Gets or sets the Tester
        /// </summary>
        public ServiceUser Tester { get; set; }

        /// <summary>
        /// Gets or sets the Hours Remaining
        /// </summary>
        public double HoursRemaining { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether task is blocked or not
        /// </summary>
        public bool Blocked { get; set; }

        /// <summary>
        /// Gets or sets the task's updated hours
        /// </summary>
        public ICollection<ServiceUpdatedHours> UpdatedHours { get; set; }
    }
}
