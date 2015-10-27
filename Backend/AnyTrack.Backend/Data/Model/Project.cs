using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace AnyTrack.Backend.Data.Model
{
    /// <summary>
    /// Represents a Project Entity
    /// </summary>
    public class Project : BaseEntity
    {
        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or Sets Version Control
        /// </summary>
        public virtual string VersionControl { get; set; }

        /// <summary>
        /// Gets or Sets date Started On
        /// </summary>
        public virtual DateTime StartedOn { get; set; }

        /// <summary>
        /// Gets or Sets Project Manager
        /// </summary>
        public virtual User ProjectManager { get; set; }

        /// <summary>
        /// Gets or Sets Product Owner
        /// </summary>
        public virtual User ProductOwner { get; set; }

        /// <summary>
        /// Gets or Sets ScrumMasters
        /// </summary>
        public virtual List<User> ScrumMasters { get; set; }

        /*
        /// <summary>
        /// Gets or Sets Sprints
        /// </summary>
       public virtual ICollection<Sprint> Sprints { get; set; }

        /// <summary>
        /// Gets or Sets Stories
        /// </summary>
        public virtual ICollection<Story> Stories { get; set; }
         */
    }
}
