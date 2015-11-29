using System;
using System.Globalization;
using System.Windows.Data;

namespace AnyTrack.Sprints.Converters
{
    /// <summary>
    /// Converts between a DateTime and a String.
    /// </summary>
    public class DateConverter : IValueConverter
    {
        /// <summary>
        /// Converts from the viewmodel to the view.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">Any paramaters.</param>
        /// <param name="culture">The current culture</param>
        /// <returns>A date as a string.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? date = (DateTime?)value;
            if (date == null)
            {
                return string.Empty;
            }
            else
            {
                return date.Value.ToString("d", culture);
            }
        }

        /// <summary>
        /// Converts from the view to the viewmodel.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">Any paramaters.</param>
        /// <param name="culture">The current culture</param>
        /// <returns>A string parsed to a date.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value.ToString();
            DateTime resultDateTime;
            if (DateTime.TryParse(strValue, culture, DateTimeStyles.None, out resultDateTime))
            {
                return new DateTime?(resultDateTime);
            }

            return new DateTime?();
        }
    }
}
