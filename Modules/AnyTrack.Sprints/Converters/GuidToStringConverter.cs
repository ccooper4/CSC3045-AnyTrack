using System;
using System.Globalization;
using System.Windows.Data;

namespace AnyTrack.Sprints.Converters
{
    /// <summary>
    /// Provides a class to convert between the a guid and a string.
    /// </summary>
    public class GuidToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts the object from the view model to the view format.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The paramater.</param>
        /// <param name="culture">The current culture.</param>
        /// <returns>A visibility enum value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (Guid)value;
            return val.ToString();
        }

        /// <summary>
        /// Converts the object from the view to the view model format.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The paramater.</param>
        /// <param name="culture">The current culture.</param>
        /// <returns>A bool value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var guidString = value.ToString();
            return Guid.Parse(guidString);
        }
    }
}
