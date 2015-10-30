using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Provides information about the roles a user is assigned to.
    /// </summary>
    public class RoleInfo
    {
        #region Properties 

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the project ID
        /// </summary>
        public Guid? ProjectID { get; set; }

        /// <summary>
        /// Gets or sets the sprint id
        /// </summary>
        public Guid? SprintID { get; set; }

        #endregion 
    }
}
