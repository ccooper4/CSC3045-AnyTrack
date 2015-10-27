using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Accounting.ServiceGateways;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace AnyTrack.Accounting.Views
{
    /// <summary>
    /// The view model for the registration page. 
    /// </summary>
    public class LoginViewModel : BindableBase
    {
        #region Fields

        /// <summary>
        /// The region manager.
        /// </summary>
        private readonly IRegionManager regionManager;

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
        /// RegistrationViewModel constructor.
        /// </summary>
        /// <param name="regionManager">The region manager.</param>
        public LoginViewModel(IRegionManager regionManager)
        {
            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }

            this.regionManager = regionManager;

            LoginUserCommand = new DelegateCommand(this.LoginUser, this.CanLogin);
        }

        #region Properties

        /// <summary>
        /// Gets or sets Email property.
        /// </summary>
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
        /// Gets the command used to register a user. 
        /// </summary>
        public DelegateCommand LoginUserCommand { get; private set; }
        #endregion

        #region Methods

        /// <summary>
        /// Detects whether the registration can take place.
        /// </summary>
        /// <returns>Proceed with registration or not.</returns>
        private bool CanLogin()
        {
            return true;
        }

        /// <summary>
        /// Perform registration.
        /// </summary>
        private void LoginUser()
        {
            regionManager.RequestNavigate(Infrastructure.RegionNames.MainRegion, "Login");
        }

        #endregion
    }
}
