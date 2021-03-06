﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Infrastructure.Extensions;
using AnyTrack.Infrastructure.ServiceGateways;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace AnyTrack.Accounting.Views
{
    /// <summary>
    /// The view model for the login page. 
    /// </summary>
    public class LoginViewModel : BaseViewModel, IRegionMemberLifetime
    {
        #region Fields

        /// <summary>
        /// The specified email address.
        /// </summary>
        private string email;        

        /// <summary>
        /// The specified password.
        /// </summary>
        private string password;             

        #endregion

        /// <summary>
        /// LoginViewModel constructor
        /// </summary>
        /// <param name="regionManager">The region manager.</param>
        /// <param name="gateway">The account service gateway.</param>
        public LoginViewModel(IRegionManager regionManager, IAccountServiceGateway gateway)
            : base(gateway, regionManager)
        {
            LoginUserCommand = new DelegateCommand(this.LoginUser);
            SignUpCommand = new DelegateCommand(this.SignUp);
        }
        #region Properties

        /// <summary>
        /// Gets or sets Email property.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                SetProperty(ref email, value);
            }
        }                

        /// <summary>
        /// Gets or sets Password property.
        /// </summary>
        [Required]
        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                SetProperty(ref password, value);
            }
        }
            
        /// <summary>
        /// Gets the command used to login a user. 
        /// </summary>
        public DelegateCommand LoginUserCommand { get; private set; }

        /// <summary>
        /// Gets the command used to login a user. 
        /// </summary>
        public DelegateCommand SignUpCommand { get; private set; }

        /// <summary>
        /// Gets a value indicating whether it should refresh everytime
        /// </summary>
        public bool KeepAlive
        {
            get { return false; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Perform login.
        /// </summary>
        private void LoginUser()
        {
            this.ValidateViewModelNow();
            if (!this.HasErrors)
            {
                var user = new ServiceUserCredential
                {
                    EmailAddress = email,
                    Password = password
                };

                var response = ServiceGateway.LoginAccount(user);

                if (response.Success)
                {
                    RegionManager.RequestNavigate(RegionNames.AppContainer, "MainAppArea");
                }
                else
                {
                    this.ShowMetroDialog("Unable to login!", "Sorry! We were unable to log you into AnyTrack using the details provided. Please check them and try again. Alternatively, rest your password or create an account");
                }
            }
        }

        /// <summary>
        /// Navigate to SignUp
        /// </summary>
        private void SignUp()
        {
            RegionManager.RequestNavigate(RegionNames.AppContainer, "Registration");
        }

        #endregion
    }
}
