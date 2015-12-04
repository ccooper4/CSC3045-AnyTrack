using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AnyTrack.Projects.Converters
{
    /// <summary>
    /// Provides a class to convert between the Visibility enum and a boolean flag.
    /// </summary>
    public class BooleanToInvisibilityConverter : IValueConverter
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
            bool val = (bool)value;
            if (!val)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
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
            var visibility = (Visibility)value; 

            if (visibility == Visibility.Visible)
            {
                return false; 
            }
            else
            {
                return true; 
            }
        }
    }
}
