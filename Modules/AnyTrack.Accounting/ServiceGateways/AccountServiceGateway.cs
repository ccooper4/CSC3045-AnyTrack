using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnyTrack.Accounting.ServiceGateways.Models;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendAccountService;

namespace AnyTrack.Accounting.ServiceGateways
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

        #endregion

        #region Constructor 

        /// <summary>
        /// Constructs a new AccountServiceGateway with the specified web service client.
        /// </summary>
        /// <param name="client">The web service client.</param>
        public AccountServiceGateway(IAccountService client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            this.client = client;
        }

        #endregion

        #region Methods 

        /// <summary>
        /// Registers an account given the user details.
        /// </summary>
        /// <param name="registration">The registration details.</param>
        public void RegisterAccount(NewUserRegistration registration)
        {
            var newUser = new NewUser
            {
                EmailAddress = registration.EmailAddress,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                Password = registration.Password,
                ProductOwner = registration.ProductOwner,
                ScrumMaster = registration.ScrumMaster,
                Developer = registration.Developer,
            };

            try
            {
                client.CreateAccount(newUser);
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
        public LoginResult LoginAccount(UserCredential login)
        {
            var user = new UserCredential
            {
                EmailAddress = login.EmailAddress,
                Password = login.Password
            };

            using (new OperationContextScope((client as AccountServiceClient).InnerChannel))
            {
                var result = client.LogIn(user);

                if (result.Success)
                {
                    var responseMessageProperty = (HttpResponseMessageProperty)OperationContext.Current.IncomingMessageProperties[HttpResponseMessageProperty.Name];
                    var cookie = responseMessageProperty.Headers.Get("Set-Cookie");

                    var principal = new ServiceUserPrincipal(result, cookie);
                    Thread.CurrentPrincipal = principal;
                }

                return result;
            }
        }

        /// <summary>
        /// Returns the supported list of secret questions.
        /// </summary>
        /// <returns> the supported list of secret questions. </returns>
        public List<AvailableSecretQuestion> SecretQuestions()
        {
            throw new NotImplementedException();
        }

        #endregion 
    }
}
