using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Infrastructure
{
    /// <summary>
    /// Provides a global container for the current cookie.
    /// </summary>
    public static class UserDetailsStore
    {
        /// <summary>
        /// Gets or sets the Auth Cookie.
        /// </summary>
        public static string AuthCookie { get; set; }
    }
}
