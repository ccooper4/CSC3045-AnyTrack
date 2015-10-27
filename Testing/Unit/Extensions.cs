using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit
{
    /// <summary>
    /// General extension methods to help when Unit Testing. 
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Calls the specified method on the object. 
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="o">The object.</param>
        /// <param name="method">The method to call.</param>
        /// <param name="prms">The list of paramaters.</param>
        /// <returns>The result of the method.</returns>
        public static T Call<T>(this object o, string method, params object[] prms)
        {
            var type = o.GetType();

            var methodInfo = type.GetMethod(method, System.Reflection.BindingFlags.NonPublic);

            return (T)methodInfo.Invoke(o, prms); 
        }

        /// <summary>
        /// Calls the specified method on the object. 
        /// </summary>
        /// <param name="o">The object.</param>
        /// <param name="method">The method to call.</param>
        /// <param name="prms">The list of paramaters.</param>
        /// <returns>The result of the method.</returns>
        public static void Call(this object o, string method, params object[] prms)
        {
            var type = o.GetType();

            var methodInfo = type.GetMethod(method, System.Reflection.BindingFlags.NonPublic);

            methodInfo.Invoke(o, prms);
        }
    }
}
