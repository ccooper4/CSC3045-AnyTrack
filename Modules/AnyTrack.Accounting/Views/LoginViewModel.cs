using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Accounting.BackendAccountService;
using AnyTrack.Accounting.ServiceGateways;
using AnyTrack.Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace AnyTrack.Accounting.Views
{
    /// <summary>
    /// The view model for the login page. 
    /// </summary>
    public class LoginViewModel : ValidatedBindableBase
    {
        #region Fields

        /// <summary>
        /// The region manager.
        /// </summary>
        private readonly IRegionManager regionManager;

        /// <summary>
        /// The account service gateway
        /// </summary>
        private readonly IAccountServiceGateway serviceGateway;

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
        {
            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }
            
            if (gateway == null)
            {
                throw new ArgumentNullException("gateway");
            }

            this.regionManager = regionManager;
            this.serviceGateway = gateway;

            LoginUserCommand = new DelegateCommand(this.LoginUser, this.CanLogin);
            SignUpCommand = new DelegateCommand(this.SignUp, this.CanSignUp);
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
        #endregion

        #region Methods

        /// <summary>
        /// Detects whether the login can take place.
        /// </summary>
        /// <returns>Proceed with login or not.</returns>
        private bool CanLogin()
        {
            return true;
        }

        /// <summary>
        /// Detects whether the SignUp can take place.
        /// </summary>
        /// <returns>Proceed with SignUp or not.</returns>
        private bool CanSignUp()
        {
            return true;
        }

        /// <summary>
        /// Perform login.
        /// </summary>
        private void LoginUser()
        {
            var user = new UserCredential
            {
                EmailAddress = email,
                Password = password
            };

            serviceGateway.LoginAccount(user);
            regionManager.RequestNavigate(Infrastructure.RegionNames.MainRegion, "Login");
        }

        /// <summary>
        /// Navigate to SignUp
        /// </summary>
        private void SignUp()
        {
            regionManager.RequestNavigate(RegionNames.AppContainer, "Registration");
        }

        #endregion
    }
}
