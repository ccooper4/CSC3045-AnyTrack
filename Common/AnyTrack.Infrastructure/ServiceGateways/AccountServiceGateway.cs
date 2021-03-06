﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Infrastructure.Security;
using Microsoft.Practices.Unity;

namespace AnyTrack.Infrastructure.ServiceGateways
{
    /// <summary>
    /// Provides a service gateway to the Account Service.
    /// </summary>
    public class AccountServiceGateway : IAccountServiceGateway
    {
        #region Fields 

        /// <summary>
        /// The web service client.
        /// </summary>
        private readonly IAccountService client;

        /// <summary>
        /// The unity container.
        /// </summary>
        private readonly IUnityContainer container; 

        #endregion

        #region Constructor 

        /// <summary>
        /// Constructs a new AccountServiceGateway with the specified web service client.
        /// </summary>
        /// <param name="container">The unity container.</param>
        /// <param name="client">The web service client.</param>
        public AccountServiceGateway(IUnityContainer container, IAccountService client)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            this.container = container;
            this.client = client;
        }

        #endregion

        #region Methods 

        /// <summary>
        /// Registers an account given the user details.
        /// </summary>
        /// <param name="registration">The registration details.</param>
        public void RegisterAccount(ServiceUser registration)
        {
            try
            {
                client.CreateAccount(registration);
            }
            catch (FaultException<UserAlreadyExistsFault> e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Logins in a user with their provided details
        /// </summary>
        /// <param name="login">The login details.</param>
        /// <returns>The result of the login operation.</returns>
        public ServiceLoginResult LoginAccount(ServiceUserCredential login)
        {
            // We need to unit test this method, however, OperationContext doesn't lend it's self well to being mocked. 
            // As a result, for this one occurance, we are best working around the issue. 
            OperationContextScope scope = null;
            try
            {
                if (client as AccountServiceClient != null)
                {
                    scope = new OperationContextScope((client as AccountServiceClient).InnerChannel);
                }

                var result = client.LogIn(login);

                var cookie = string.Empty;

                if (result.Success)
                {
                    if (scope != null)
                    {
                        var responseMessageProperty = (HttpResponseMessageProperty)OperationContext.Current.IncomingMessageProperties[HttpResponseMessageProperty.Name];
                        cookie = responseMessageProperty.Headers.Get("Set-Cookie");
                    }

                    var principal = new ServiceUserPrincipal(result, cookie);

                    UserDetailsStore.AuthCookie = cookie;
                    UserDetailsStore.LoggedInUserPrincipal = principal;
                }

                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (scope != null)
                {
                    scope.Dispose();
                }
            }
        }

        /// <summary>
        /// Refreshes the user's principal.
        /// </summary>
        /// <returns>The login result.</returns>
        public ServiceLoginResult RefreshLoginPrincipal()
        {
            return client.RefreshLoginPrincipal();
        }

        #endregion 
    }
}
