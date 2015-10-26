using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace AnyTrack.Backend.Providers
{
    /// <summary>
    /// Provides a wrapper over the standard Forms Authentication functionality. 
    /// </summary>
    public class FormsAuthenticationProvider
    {
        /// <summary>
        /// Sets the authentication cookie in the outgoing HTTP request after a successful login.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <param name="persistentCookie">A flag indicating if the cookie should be persistent.</param>
        public virtual void SetAuthCookie(string userName, bool persistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, persistentCookie);
        }

        /// <summary>
        /// Clears the current Forms Authentication Cookie. 
        /// </summary>
        public virtual void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
