using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data.Model;

namespace AnyTrack.Backend.Security
{
    /// <summary>
    /// Provides a principal that can be generated from the forms auth cookie.
    /// </summary>
    public class GeneratedServiceUserPrincipal : IPrincipal
    {
        #region Fields 

        /// <summary>
        /// The user roles. 
        /// </summary>
        private string[] roles;

        /// <summary>
        /// The user name.
        /// </summary>
        private string name; 

        #endregion 

        #region Constructor

        /// <summary>
        /// Constructs the Service Principal using the specified auth details.
        /// </summary>
        /// <param name="user">The user.</param>
        public GeneratedServiceUserPrincipal(User user)
        {
            this.name = user.EmailAddress;
            this.roles = user.Roles.Select(r => r.RoleName).ToArray();
        }

        #endregion 

        #region Properties 

        /// <summary>
        /// Gets the current user's identity.
        /// </summary>
        public IIdentity Identity
        {
            get
            {
                return new GeneratedServiceUserIdentity(name);
            }
        }

        #endregion 

        #region Methods 

        /// <summary>
        /// Returns a flag indicating if the user is in the given role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>A true or false flag.</returns>
        public bool IsInRole(string role)
        {
            return true;
        }

        #endregion 
    }
}
