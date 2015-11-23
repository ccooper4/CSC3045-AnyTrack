using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Represents a summary of a Project and the roles a user possesses in that project
    /// </summary>
    public class ServiceProjectRoleSummary
    {
        /// <summary>
        /// Gets or sets the ProjectId.
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the Project Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Projects Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is Project Manager of the Project.
        /// </summary>
        public bool ProjectManager { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is Product Owner of the Project.
        /// </summary>
        public bool ProductOwner { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is a Scrum Master of the Project.
        /// </summary>
        public bool ScrumMaster { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is a Developer of the Project.
        /// </summary>
        public bool Developer { get; set; }
    }
}
