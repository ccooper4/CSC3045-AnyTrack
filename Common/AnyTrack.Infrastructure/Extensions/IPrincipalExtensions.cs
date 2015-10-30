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

        #endregion 
    }
}
