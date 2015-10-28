﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Accounting.BackendAccountService;
using AnyTrack.Accounting.ServiceGateways.Models;

namespace AnyTrack.Accounting.ServiceGateways
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
        void RegisterAccount(NewUserRegistration registration);

        /// <summary>
        /// Logins in a user with their provided details
        /// </summary>
        /// <param name="login">The login details.</param>
        void LoginAccount(UserCredential login);

        /// <summary>
        /// Returns the supported list of secret questions.
        /// </summary>
        /// <returns>the supported list of secret questions.</returns>
        List<AvailableSecretQuestion> SecretQuestions();
    }
}
