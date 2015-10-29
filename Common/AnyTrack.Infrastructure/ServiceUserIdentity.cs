using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Infrastructure
{
    /// <summary>
    /// Represents a service user's identity.
    /// </summary>
    public class ServiceUserIdentity : IIdentity
    {
        #region Fields 

        /// <summary>
        /// The user's email address.
        /// </summary>
        private string name; 

        #endregion 

        #region Constructor

        /// <summary>
        /// Constructs a new identity for the specified user.
        /// </summary>
        /// <param name="name">The user's email addres.</param>
        public ServiceUserIdentity(string name)
        {
            this.name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the authentication type.
        /// </summary>
        public string AuthenticationType
        {
            get { return "FormsAuthentication"; }
        }

        /// <summary>
        /// Gets a value indicating whether the user is authenticated.
        /// </summary>
        public bool IsAuthenticated
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the current user's name.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        #endregion
    }
}
