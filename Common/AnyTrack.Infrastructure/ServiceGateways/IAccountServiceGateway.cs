﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure.BackendAccountService;

namespace AnyTrack.Infrastructure.ServiceGateways
{
    /// <summary>
    /// The interface outlining the account service gateway
    /// </summary>
    public interface IAccountServiceGateway
    {
        /// <summary>
        /// Registers an account given the user details.
        /// </summary>
        /// <param name="registration">The registration details.</param>
        void RegisterAccount(ServiceUser registration);

        /// <summary>
        /// Logins in a user with their provided details
        /// </summary>
        /// <param name="login">The login details.</param>
        /// <returns>The result from the login operation.</returns>
        ServiceLoginResult LoginAccount(ServiceUserCredential login);

        /// <summary>
        /// Refreshes the user's principal.
        /// </summary>
        /// <returns>The login result.</returns>
        ServiceLoginResult RefreshLoginPrincipal();
    }
}
