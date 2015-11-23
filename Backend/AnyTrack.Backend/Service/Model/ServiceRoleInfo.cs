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
    public class ServiceRoleInfo
    {
        #region Properties 

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the project Id.
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the sprint Id.
        /// </summary>
        public Guid? SprintId { get; set; }

        #endregion 
    }
}
