using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel;
using System.Windows;
using AnyTrack.Accounting.ServiceGateways;
using AnyTrack.Accounting.ServiceGateways.Models;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendAccountService;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace AnyTrack.Accounting.Views
{
    /// <summary>
    /// The view model for the registration page. 
    /// </summary>
    public class RegistrationViewModel : BaseViewModel, IRegionMemberLifetime
    {
        #region Fields 

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
        /// The currently selected secret question.
        /// </summary>
        private string secretQuestion;

        /// <summary>
        /// The secret answer.
        /// </summary>
        private string secretAnswer; 

        #endregion

        #region Constructor 
        /// <summary>
        /// RegistrationViewModel constructor.
        /// </summary>
        /// <param name="regionManager">The region manager.</param>
        /// <param name="gateway">The account service gateway.</param>
        public RegistrationViewModel(IRegionManager regionManager, IAccountServiceGateway gateway)
            : base(gateway, regionManager)
        {
            this.SecretQuestions = new ObservableCollection<string>(ClientAvailableSecretQuestions.All());
            this.Skills = new ObservableCollection<string>();

            // SecretQuestions = serviceGateway.SecretQuestions();
            RegisterUserCommand = new DelegateCommand(this.RegisterUser);
            AddSkillCommand = new DelegateCommand(this.AddSkill);
            CancelRegisterUserCommand = new DelegateCommand(this.CancelRegisterUser);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Email property.
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "The email address is required")]
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
        [Required]
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
        [Required]
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
        /// Gets or sets Confirm Password property.
        /// </summary>
        [Required]
        [Compare("Password", ErrorMessage = "The passwords must match")]
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
        /// Gets or sets Secret Question.
        /// </summary>
        [Required]
        public string SecretQuestion
        {
            get
            {
                return secretQuestion; 
            }

            set
            {
                SetProperty(ref secretQuestion, value);
            }
        }

        /// <summary>
        /// Gets or sets Secret Answer.
        /// </summary>
        [Required]
        public string SecretAnswer
        {
            get
            {
                return secretAnswer;
            }

            set
            {
                SetProperty(ref secretAnswer, value);
            }
        }

        /// <summary>
        /// Gets or sets the Skills property.
        /// </summary>
        public ObservableCollection<string> Skills { get; set; }

        /// <summary>
        /// Gets or sets the SecretQuestions property.
        /// </summary>
        public ObservableCollection<string> SecretQuestions { get; set; }

        /// <summary>
        /// Gets the command used to register a user. 
        /// </summary>
        public DelegateCommand RegisterUserCommand { get; private set; }

        /// <summary>
        /// Gets the command used to register a user. 
        /// </summary>
        public DelegateCommand CancelRegisterUserCommand { get; private set; }

        /// <summary>
        /// Gets the command used to register add a Skill.
        /// </summary>
        public DelegateCommand AddSkillCommand { get; private set; }

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
        /// Perform registration.
        /// </summary>
        private void RegisterUser()
        {
            this.ValidateViewModelNow();

            if (!this.HasErrors)
            {
                var newUser = new ServiceUser
                {
                    EmailAddress = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Password = password,
                    ProductOwner = productOwner,
                    ScrumMaster = scrumMaster,
                    Developer = developer,
                    SecretQuestion = secretQuestion,
                    SecretAnswer = secretAnswer,
                    Skills = string.Join(",", Skills)
                };

                try
                {
                    ServiceGateway.RegisterAccount(newUser);

                    var callback = new Action<MessageDialogResult>(m =>
                    {
                        if (m == MessageDialogResult.Affirmative)
                        {
                            RegionManager.RequestNavigate(Infrastructure.RegionNames.AppContainer, "Login");
                        }
                    });

                    this.ShowMetroDialog("Registration successful", "Your user account has been successfully registered.", MessageDialogStyle.Affirmative, callback);
                }
                catch (FaultException<UserAlreadyExistsFault>)
                {
                    this.ShowMetroDialog("Registration was not successful", "Your user account could not be registered because an account with that email address already exists. Please try logging in.", MessageDialogStyle.Affirmative);
                }
            }
        }

        /// <summary>
        /// Cancel registration.
        /// </summary>
        private void CancelRegisterUser()
        {
            RegionManager.RequestNavigate(RegionNames.AppContainer, "Login");
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
