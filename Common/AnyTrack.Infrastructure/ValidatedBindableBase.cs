using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
        public bool SetProperty<T>(ref T storage, T newValue, [CallerMemberName()]string propertyName = null)
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

        #endregion
    }
}
