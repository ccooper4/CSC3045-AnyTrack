using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Infrastructure.CustomValidationAttributes
{
    /// <summary>
    /// A custom validation Attribute that compares a DateTime object to another DateTime object.
    /// </summary>
    public class DateTimeCompareAttribute : ValidationAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructs a DateTimeCompare Attribute
        /// </summary>
        /// <param name="operation">Operation to use to compare dates</param>
        /// <param name="dateToCompareVariableName">Name of variable containing date to be compared</param>
        /// <param name="errorMessage">Error message to display of invalid</param>
        public DateTimeCompareAttribute(string operation, string dateToCompareVariableName, string errorMessage)
        {
            Operation = operation;
            DateToCompareVariableName = dateToCompareVariableName;
            ErrorMessage = errorMessage;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets date to compare the object to.
        /// </summary>
        public DateTime DateToCompare { get; set; }

        /// <summary>
        /// Gets or sets the variable name on the view model that contains the date to compare.
        /// </summary>
        public string DateToCompareVariableName { get; set; }
        
        /// <summary>
        /// Gets or sets the operation to compare the 2 dates.
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the object is valid.
        /// </summary>
        private bool Result { get; set; }

        #endregion

        /// <summary>
        /// Checks if the object has a valid value.
        /// </summary>
        /// <param name="value">object to be checked</param>
        /// <param name="validationContext">validation context</param>
        /// <returns>Validation Result</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date = Convert.ToDateTime(value);

            PropertyInfo endTimeInfo = validationContext.ObjectType.GetProperty(DateToCompareVariableName);

            var dateToCompare = endTimeInfo.GetValue(validationContext.ObjectInstance);

            DateToCompare = (DateTime)dateToCompare;

            switch (Operation)
            {
                case ">":
                    Result = date > DateToCompare;
                    break;
                case ">=":
                    Result = date >= DateToCompare;
                    break;
                case "==":
                    Result = date == DateToCompare;
                    break;
                case "<=":
                    Result = date <= DateToCompare;
                    break;
                case "<":
                    Result = date < DateToCompare;
                    break;
            }

            if (Result)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}
