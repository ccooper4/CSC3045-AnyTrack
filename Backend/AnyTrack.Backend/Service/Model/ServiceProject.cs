using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Represents a Project entity
    /// </summary>
    public class ServiceProject
    {
        #region Constructor

        /// <summary>
        /// Creates a new Project Entity
        /// </summary>
        public ServiceProject()
        {
            ScrumMasterEmailAddresses = new List<string>();
        }
        #endregion

        /// <summary>
        /// Gets or sets the Project Id.
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the project's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the project's version control.
        /// </summary>
        public string VersionControl { get; set; }

        /// <summary>
        /// Gets or sets the project start date.
        /// </summary>
        public DateTime StartedOn { get; set; }

        /// <summary>
        /// Gets or sets the Project Manager's email address.
        /// </summary>
        public string ProjectManagerEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the Product Owner's email address.
        /// </summary>
        public string ProductOwnerEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the list of the scrumMasters' email addresses.
        /// </summary>
        public List<string> ScrumMasterEmailAddresses { get; set; }

        /*
        /// <summary>
        /// Gets or sets Sprints
        /// </summary>
       public ICollection<Sprint> Sprints { get; set; }
        */

        /// <summary>
        /// Gets or sets Stories
        /// </summary>
        public List<ServiceStory> Stories { get; set; }
    }
}
