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
    public class RegistrationViewModel : BindableBase
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
        /// The specified first name.
        /// </summary>
        private string firstName;

        /// <summary>
        /// The specified last name. 
        /// </summary>
        private string lastName;

        /// <summary>
        /// The specified password.
        /// </summary>
        private string password;

        /// <summary>
        /// The specified confirmation password.
        /// </summary>
        private string confirmPassword;

        /// <summary>
        /// The specified productOwner flag.
        /// </summary>
        private bool productOwner;

        /// <summary>
        /// The specified scrumMaster flag.
        /// </summary>
        private bool scrumMaster;

        /// <summary>
        /// The specified developer flag.
        /// </summary>
        private bool developer;

        /// <summary>
        /// The specified user skills.
        /// </summary>
        private ObservableCollection<UserSkills> skills;

        #endregion

        /// <summary>
        /// RegistrationViewModel constructor.
        /// </summary>
        /// <param name="regionManager">The region manager.</param>
        public RegistrationViewModel(IRegionManager regionManager)
        {
            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }

            this.regionManager = regionManager;

            RegisterUserCommand = new DelegateCommand(this.RegisterUser, this.CanRegister);
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
        /// Gets or sets First name property.
        /// </summary>
        public string FirstName
        {
            get
            {
                return firstName;
            }

            set
            {
                SetProperty(ref firstName, value);
            }
        }

        /// <summary>
        /// Gets or sets Last name property.
        /// </summary>
        public string LastName
        {
            get
            {
                return lastName;
            }

            set
            {
                SetProperty(ref lastName, value);
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
        /// Gets or sets Confirm Password property.
        /// </summary>
        public string ConfirmPassword
        {
            get
            {
                return confirmPassword;
            }

            set
            {
                SetProperty(ref confirmPassword, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Product Owner flag is set.
        /// </summary>
        public bool ProductOwner
        {
            get
            {
                return productOwner;
            }

            set
            {
                SetProperty(ref productOwner, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Scrum Master flag is set.
        /// </summary>
        public bool ScrumMaster
        {
            get
            {
                return scrumMaster;
            }

            set
            {
                SetProperty(ref scrumMaster, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Developer flag is set.
        /// </summary>
        public bool Developer
        {
            get
            {
                return developer;
            }

            set
            {
                SetProperty(ref developer, value);
            }
        }

        /// <summary>
        /// Gets the Skills property.
        /// </summary>
        public ObservableCollection<UserSkills> Skills
        {
            get
            {
                return skills;
            }
        } 

        /// <summary>
        /// Gets the command used to register a user. 
        /// </summary>
        public DelegateCommand RegisterUserCommand { get; private set; }
        #endregion

        #region Methods 

        /// <summary>
        /// Detects whether the registration can take place.
        /// </summary>
        /// <returns>Proceed with registration or not.</returns>
        private bool CanRegister()
        {
            return true;
        }

        /// <summary>
        /// Perform registration.
        /// </summary>
        private void RegisterUser()
        {
            regionManager.RequestNavigate(Infrastructure.RegionNames.MainRegion, "Login");
        }

        #endregion 
    }
}
