using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Accounting.ServiceGateways;
using AnyTrack.Accounting.ServiceGateways.Models;
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
        /// The account service gateway
        /// </summary>
        private readonly IAccountServiceGateway serviceGateway;

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
        /// The currently entered skill.
        /// </summary>
        private string currentSkill;

        /// <summary>
        /// The specified user skills.
        /// </summary>
        private ObservableCollection<string> skills;

        #endregion

        #region Constructor 
        /// <summary>
        /// RegistrationViewModel constructor.
        /// </summary>
        /// <param name="regionManager">The region manager.</param>
        /// <param name="gateway">The account service gateway.</param>
        public RegistrationViewModel(IRegionManager regionManager, IAccountServiceGateway gateway)
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

            RegisterUserCommand = new DelegateCommand(this.RegisterUser, this.CanRegister);
            AddSkillCommand = new DelegateCommand(this.AddSkill, this.CanAddSkill);
        }

        #endregion

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
        /// Gets or sets CurrentSkill property.
        /// </summary>
        public string CurrentSkill
        {
            get
            {
                return currentSkill;
            }

            set
            {
                SetProperty(ref currentSkill, value);
            }
        }

        /// <summary>
        /// Gets the Skills property.
        /// </summary>
        public ObservableCollection<string> Skills
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

        /// <summary>
        /// Gets the command used to register add a Skill.
        /// </summary>
        public DelegateCommand AddSkillCommand { get; private set; }
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
            var newUser = new NewUserRegistration
            {
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName,
                Password = password,
                ProductOwner = productOwner,
                ScrumMaster = scrumMaster,
                Developer = developer
            };

            serviceGateway.RegisterAccount(newUser);

            regionManager.RequestNavigate(Infrastructure.RegionNames.AppContainer, "Login");
        }

        /// <summary>
        /// Detects whether the entered skill can be added.
        /// </summary>
        /// <returns>Proceed with adding the skill or not.</returns>
        private bool CanAddSkill()
        {
            return true;
        }

        /// <summary>
        /// Add a new skill.
        /// </summary>
        private void AddSkill()
        {
            Skills.Add(CurrentSkill);
            CurrentSkill = string.Empty;
        }

        #endregion 
    }
}
