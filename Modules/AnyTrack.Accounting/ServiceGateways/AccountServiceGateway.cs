using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Accounting.BackendAccountService;
using AnyTrack.Accounting.ServiceGateways.Models;

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

            client.CreateAccount(newUser);
        }

        #endregion 
    }
}
