using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure.Security;

namespace AnyTrack.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions to the IPrincipal interface. 
    /// </summary>
    public static class IPrincipalExtensions
    {
        #region Methods 

        /// <summary>
        /// Returns the auth cookie from a Service User principal.
        /// </summary>
        /// <param name="principal">The current principal.</param>
        /// <returns>The auth cookie.</returns>
        public static string GetAuthCookie(this IPrincipal principal)
        {
            var serviceUserPrincipal = principal as ServiceUserPrincipal; 
            if (serviceUserPrincipal != null)
            {
                return serviceUserPrincipal.AuthCookie;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the full name from a Service User principal.
        /// </summary>
        /// <param name="principal">The current principal.</param>
        /// <returns>The full name.</returns>
        public static string GetFullName(this IPrincipal principal)
        {
            var serviceUserPrincipal = principal as ServiceUserPrincipal;
            if (serviceUserPrincipal != null)
            {
                return serviceUserPrincipal.FullName;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a value indicating if this user is in the specificed role with the given ids. 
        /// </summary>
        /// <param name="principal">The principal object.</param>
        /// <param name="role">The role.</param>
        /// <param name="projectId">The project id.</param>
        /// <param name="sprintId">The sprint id.</param>
        /// <returns>A true or false value.</returns>
        public static bool IsUserInRole(this IPrincipal principal, string role, Guid? projectId = null, Guid? sprintId = null)
        {
            var serviceUserPrincipal = principal as ServiceUserPrincipal;
            if (serviceUserPrincipal != null)
            {
                return serviceUserPrincipal.IsInRole(role, projectId, sprintId);
            }
            else
            {
                return false;
            }
        }

        #endregion 
    }
}
