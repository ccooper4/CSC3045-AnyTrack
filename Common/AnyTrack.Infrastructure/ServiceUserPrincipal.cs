using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure.BackendAccountService;

namespace AnyTrack.Infrastructure
{
    /// <summary>
    /// Provides a custom IPrincipal that can store the data required to authenticate against the backend. 
    /// </summary>
    public class ServiceUserPrincipal : IPrincipal
    {
        #region Fields 

        /// <summary>
        /// The result of the login operation.
        /// </summary>
        private readonly LoginResult loginResult;

        /// <summary>
        /// The auth cookie. 
        /// </summary>
        private readonly string authCookie; 

        #endregion 

        #region Constructor

        /// <summary>
        /// Constructs the Service Principal using the specified auth details.
        /// </summary>
        /// <param name="result">The login result.</param>
        /// <param name="authCookie">The auth cookie.</param>
        public ServiceUserPrincipal(LoginResult result, string authCookie)
        {
            this.loginResult = result;
            this.authCookie = authCookie;
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
                return new ServiceUserIdentity(loginResult.EmailAddress);
            }
        }

        /// <summary>
        /// Gets the auth cookie.
        /// </summary>
        public string AuthCookie
        {
            get
            {
                return authCookie;
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
