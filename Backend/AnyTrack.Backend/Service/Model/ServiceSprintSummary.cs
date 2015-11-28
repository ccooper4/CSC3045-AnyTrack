using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Represents the fields required to create a summary of a sprint.
    /// </summary>
    public class ServiceSprintSummary
    {
        /// <summary>
        /// Gets or sets the sprints id.
        /// </summary>
        public Guid SprintId { get; set; }

        /// <summary>
        /// Gets or sets the sprint's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sprint's descriptions.
        /// </summary>
        public string Description { get; set; }
    }
}
