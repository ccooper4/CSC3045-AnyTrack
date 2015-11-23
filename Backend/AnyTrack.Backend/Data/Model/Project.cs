using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace AnyTrack.Backend.Data.Model
{
    /// <summary>
    /// Represents a Project Entity.
    /// </summary>
    public class Project : BaseEntity
    {
        /// <summary>
        /// Gets or sets the project's Name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the project's Description.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the project's Version Control.
        /// </summary>
        public virtual string VersionControl { get; set; }

        /// <summary>
        /// Gets or sets the date project is Started On.
        /// </summary>
        public virtual DateTime StartedOn { get; set; }

        /// <summary>
        /// Gets or sets the project's Project Manager.
        /// </summary>
        public virtual User ProjectManager { get; set; }

        /// <summary>
        /// Gets or sets the project's Product Owner.
        /// </summary>
        public virtual User ProductOwner { get; set; }

        /// <summary>
        /// Gets or sets the project's ScrumMasters.
        /// </summary>
        public virtual ICollection<User> ScrumMasters { get; set; }

        /// <summary>
        /// Gets or sets the projects Stories (Product Backlog).
        /// </summary>
        public virtual ICollection<Story> Stories { get; set; }
     
        /// <summary>
        /// Gets or sets the project's sprints.
        /// </summary>
       public virtual ICollection<Sprint> Sprints { get; set; }     
    }
}
