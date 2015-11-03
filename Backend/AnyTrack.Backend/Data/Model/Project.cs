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
        /// Gets or sets Name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets Version Control
        /// </summary>
        public virtual string VersionControl { get; set; }

        /// <summary>
        /// Gets or sets date Started On
        /// </summary>
        public virtual DateTime StartedOn { get; set; }

        /// <summary>
        /// Gets or sets Project Manager
        /// </summary>
        public virtual User ProjectManager { get; set; }

        /// <summary>
        /// Gets or sets Product Owner
        /// </summary>
        public virtual User ProductOwner { get; set; }

        /// <summary>
        /// Gets or sets ScrumMasters
        /// </summary>
        public virtual List<User> ScrumMasters { get; set; }

        /*
        /// <summary>
        /// Gets or sets Sprints
        /// </summary>
       public virtual ICollection<Sprint> Sprints { get; set; }

        /// <summary>
        /// Gets or sets Stories
        /// </summary>
        public virtual ICollection<Story> Stories { get; set; }
         */
    }
}
