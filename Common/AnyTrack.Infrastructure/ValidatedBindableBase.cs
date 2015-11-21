using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AnyTrack.Infrastructure.Providers;
using AnyTrack.Infrastructure.Service;
using AnyTrack.SharedUtilities.Extensions;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace AnyTrack.Infrastructure
{
    /// <summary>
    /// Provides a custom view model base that supports raising validation events. 
    /// </summary>
    public abstract class ValidatedBindableBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region Fields 

        /// <summary>
        /// Gets the dictionary used to contain property keyed validation results.
        /// </summary>
        private Dictionary<string, List<ValidationResult>> validationResults;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new ValidatedBindableBase
        /// </summary>
        public ValidatedBindableBase()
        {
            validationResults = new Dictionary<string, List<ValidationResult>>();
        }

        #endregion 

        #region Events

        /// <summary>
        /// The event to be raised when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The event to be raised when an error occurs.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #endregion

        #region Properties 

        /// <summary>
        /// Gets a value indicating whether the view model has errors.
        /// </summary>
        public bool HasErrors
        {
            get
            {
                return validationResults.Count > 0;
            }
        }

        /// <summary>
        /// Gets or sets the Main Window of the application
        /// </summary>
        [Dependency]
        public WindowProvider MainWindow { get; set; }

        /// <summary>
        /// Gets or sets the region manager.
        /// </summary>
        [Dependency]
        public IRegionManager RegionManager { get; set; }

        /// <summary>
        /// Gets or sets the flyout service.
        /// </summary>
        [Dependency]
        public IFlyoutService FlyoutService { get; set; }

        #endregion

        #region Methods 

        /// <summary>
        /// Updates the property if the value has changed. Notifies the view if needs be.
        /// </summary>
        /// <param name="storage">The original value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyName">The property name.</param>
        /// <typeparam name="T">The property type.</typeparam>
        /// <returns>True or false to indicate if the property was changed.</returns>
        public bool SetProperty<T>(ref T storage, T newValue, [System.Runtime.CompilerServices.CallerMemberName()]string propertyName = null)
        {
            if (object.Equals(storage, newValue))
            {
                return false;
            }

            storage = newValue;

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            validationResults.Remove(propertyName);

            var validatorResults = new List<ValidationResult>();
            var context = new ValidationContext(this);
            context.MemberName = propertyName;
            Validator.TryValidateProperty(newValue, context, validatorResults);

            if (validatorResults.Count > 0)
            {
                validationResults.Add(propertyName, validatorResults);
                if (ErrorsChanged != null)
                {
                    ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the errors for the property name.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The list of errors.</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            if (validationResults.ContainsKey(propertyName))
            {
                var results = validationResults.Single(v => v.Key == propertyName).Value;
                return results.Select(t => t.ErrorMessage).ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Runs the validator logic for the entire viewmodel on-demand. 
        /// </summary>
        public void ValidateViewModelNow()
        {
            validationResults.Clear();

            var validatorResults = new List<ValidationResult>();
            var context = new ValidationContext(this);

            Validator.TryValidateObject(this, context, validatorResults, true);

            var memberNames = validatorResults.SelectMany(vr => vr.MemberNames).Distinct();
            foreach (var memberName in memberNames)
            {
                var errors = validatorResults.Where(vr => vr.MemberNames.Contains(memberName)).ToList();
                if (errors.Count > 0)
                {
                    validationResults.Add(memberName, errors);
                    if (ErrorsChanged != null)
                    {
                        ErrorsChanged(this, new DataErrorsChangedEventArgs(memberName));
                    }
                }
            }
        }

        /// <summary>
        /// Shows the given message on the UI using the MahMetro Dialog box.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message to show.</param>
        /// <param name="style">The dialog style.</param>
        /// <param name="callback">An action to execute when the user makes a selection. This callback is ran on the UI Thread.</param>
        public void ShowMetroDialog(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, Action<MessageDialogResult> callback = null)
        {
            var window = MainWindow; 

            var result = MainWindow.ShowMessageAsync(title, message, style).ContinueWith(t =>
            {
                if (callback != null)
                {
                    var dispatcherAction = new Action(() =>
                    {
                        callback(t.Result);
                    });

                    window.InvokeAction(dispatcherAction);
                }
            });
        }

        /// <summary>
        /// Navigates to the region specified in the menu item.
        /// </summary>
        /// <param name="view">The view to navigate to.</param>
        /// <param name="navParams">nav params</param>
        public void NavigateToItem(string view, NavigationParameters navParams = null)
        {
            if (navParams == null)
            {
                RegionManager.RequestNavigate(RegionNames.MainRegion, view);
            }
            else
            {
                RegionManager.RequestNavigate(RegionNames.MainRegion, view, navParams);
            }
        }

        /// <summary>
        /// Shows the specified metro flyout on the Shell.
        /// </summary>
        /// <param name="viewName">The view to show.</param>
        /// <param name="navParams">Any paramaters to pass into the view model.</param>
        public void ShowMetroFlyout(string viewName, NavigationParameters navParams = null)
        {
            FlyoutService.ShowMetroFlyout(viewName, navParams);
        }
        
        #endregion
    }
}
