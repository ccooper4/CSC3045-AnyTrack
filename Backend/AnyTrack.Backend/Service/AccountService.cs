﻿using System;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;
using System.Web.Helpers;
using System.Web.Security;
using System.Windows;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Backend.Faults;
using AnyTrack.Backend.Providers;
using AnyTrack.Backend.Security;
using AnyTrack.Backend.Service.Model;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Provides a service that can be used to work with user accounts. 
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class AccountService : IAccountService
    {
        #region Fields 

        /// <summary>
        /// The application's unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The provider for forms authentication.
        /// </summary>
        private readonly FormsAuthenticationProvider formsAuthProvider;

        #endregion 

        #region Constructor 

        /// <summary>
        /// Constructs a new AccountService using the specified dependencies. 
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="formsAuthProvider">The forms auth provider.</param>
        public AccountService(IUnitOfWork unitOfWork, FormsAuthenticationProvider formsAuthProvider)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (formsAuthProvider == null)
            {
                throw new ArgumentNullException("formsAuthProvider");
            }

            this.unitOfWork = unitOfWork;
            this.formsAuthProvider = formsAuthProvider;
        }

        #endregion

        #region Methods 

        /// <summary>
        /// Creates a new user account given the captured data.
        /// </summary>
        /// <param name="user">The user to register in the membership system.</param>
        public void CreateAccount(ServiceUser user)
        {
            if (unitOfWork.UserRepository.Items.Any(u => u.EmailAddress == user.EmailAddress))
            {
                throw new FaultException<UserAlreadyExistsFault>(new UserAlreadyExistsFault());
            }

            var dataUser = new User
            {
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = Crypto.HashPassword(user.Password),
                ProductOwner = user.ProductOwner,
                ScrumMaster = user.ScrumMaster,
                Developer = user.Developer,
                SecretQuestion = user.SecretQuestion,
                SecretAnswer = user.SecretAnswer,
                Skills = user.Skills
            };

            unitOfWork.UserRepository.Insert(dataUser);

            unitOfWork.Commit();
        }

        /// <summary>
        /// Validates a user's credentials against the user store.
        /// </summary>
        /// <param name="credential">The user's credential.</param>
        /// <returns>A flag indicating the outcome of the validation.</returns>
        public ServiceLoginResult LogIn(ServiceUserCredential credential)
        {
            var userAccount = unitOfWork.UserRepository.Items.SingleOrDefault(u => u.EmailAddress == credential.EmailAddress);

            if (userAccount == null)
            {
                return new ServiceLoginResult
                {
                    Success = false
                };  
            }

            if (!Crypto.VerifyHashedPassword(userAccount.Password, credential.Password))
            {
                return new ServiceLoginResult
                {
                    Success = false
                };  
            }

            formsAuthProvider.SetAuthCookie(credential.EmailAddress, false);

            return new ServiceLoginResult
            {
                EmailAddress = userAccount.EmailAddress,
                FirstName = userAccount.FirstName,
                LastName = userAccount.LastName,
                Developer = userAccount.Developer,
                ProductOwner = userAccount.ProductOwner,
                ScrumMaster = userAccount.ScrumMaster,
                AssignedRoles = userAccount.Roles.Select(r => new ServiceRoleInfo
                {
                    Role = r.RoleName,
                    ProjectId = r.ProjectId,
                    SprintId = r.SprintId
                }).ToList(),
                Success = true
            }; 
        }

        /// <summary>
        /// Allows a user to refresh their roles.
        /// </summary>
        /// <returns>A login result object.</returns>
        [CreatePrincipal]
        public ServiceLoginResult RefreshLoginPrincipal()
        {
            var currentUser = Thread.CurrentPrincipal.Identity.Name;

            var userAccount = unitOfWork.UserRepository.Items.Single(u => u.EmailAddress == currentUser);

            return new ServiceLoginResult
            {
                EmailAddress = userAccount.EmailAddress,
                FirstName = userAccount.FirstName,
                LastName = userAccount.LastName,
                Developer = userAccount.Developer,
                ProductOwner = userAccount.ProductOwner,
                ScrumMaster = userAccount.ScrumMaster,
                AssignedRoles = userAccount.Roles.Select(r => new ServiceRoleInfo
                {
                    Role = r.RoleName,
                    ProjectId = r.ProjectId,
                    SprintId = r.SprintId
                }).ToList(),
                Success = true
            }; 
        }

        #endregion 
    }
}
