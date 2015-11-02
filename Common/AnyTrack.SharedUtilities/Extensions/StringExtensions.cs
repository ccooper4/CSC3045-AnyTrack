using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.SharedUtilities.Extensions
{
    /// <summary>
    /// Provides a set of extensions to make it easier to work with strings. 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a value indicating if this string is empty.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>True if this string is not empty, false otherwise.</returns>
        public static bool IsNotEmpty(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// Returns a value indicating if this string is empty.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>True if this string is empty, false otherwise.</returns>
        public static bool IsEmpty(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// Provides a shorthand way to inject values into a string.
        /// </summary>
        /// <param name="s">The format string.</param>
        /// <param name="items">The items to inject.</param>
        /// <returns>The string, filled with the items.</returns>
        public static string Substitute(this string s, params object[] items)
        {
            return string.Format(s, items);
        }
    }
}
