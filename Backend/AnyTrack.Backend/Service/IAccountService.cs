using System.ServiceModel;
using AnyTrack.Backend.Service.Model;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Outlines the contract for the Account Service.
    /// </summary>
    [ServiceContract]
    public interface IAccountService
    {
        /// <summary>
        /// Creates a new user account given the captured data.
        /// </summary>
        /// <param name="user">The user to register in the membership system.</param>
        [OperationContract]
        void CreateAccount(NewUser user);

        /// <summary>
        /// Validates a user's credentials against the user store.
        /// </summary>
        /// <param name="credential">The login credentials.</param>
        /// <returns>A flag indicating if the login was successfu</returns>
        [OperationContract]
        LoginResult LogIn(UserCredential credential);
    }
}
