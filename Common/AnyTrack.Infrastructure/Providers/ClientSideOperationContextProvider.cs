using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Infrastructure.Providers
{
    /// <summary>
    /// Provides a client side wrapper for the WCF Operation Context. 
    /// </summary>
    public class ClientSideOperationContextProvider
    {
        /// <summary>
        /// Returns the callback channel for the current service client.
        /// </summary>
        /// <typeparam name="T">The type of callback channel.</typeparam>
        /// <returns></returns>
        public virtual T GetCallbackChannel<T>()
        {
            var operationContextCurrent = OperationContext.Current;

            if (operationContextCurrent == null)
            {
                return default(T);
            }

            return operationContextCurrent.GetCallbackChannel<T>();
        }
    }
}
