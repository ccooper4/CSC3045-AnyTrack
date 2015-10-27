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
    public class Project
    {

        #region Constructor

        /// <summary>
        /// Creates a new Project Entity
        /// </summary>
        public Project()
        {
            ProjectId = new Guid();
        }

        #endregion

        /// <summary>
        /// Gets or Sets Project ID
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets Version Control
        /// </summary>
        public string VersionControl { get; set; }

        /// <summary>
        /// Gets or Sets date Started On
        /// </summary>
        public DateTime StartedOn { get; set; }

        /// <summary>
        /// Gets or Sets Project Manager
        /// </summary>
        public User ProjectManager { get; set; }

        /// <summary>
        /// Gets or Sets Product Owner
        /// </summary>
        public User ProductOwner { get; set; }

        /// <summary>
        /// Gets or Sets ScrumMasters
        /// </summary>
        public List<User> ScrumMasters { get; set; }

        /*
        /// <summary>
        /// Gets or Sets Sprints
        /// </summary>
       public ICollection<Sprint> Sprints { get; set; }
         
        /// <summary>
        /// Gets or Sets Stories
        /// </summary>
        public ICollection<Story> Stories { get; set; }
         */
    }
}
