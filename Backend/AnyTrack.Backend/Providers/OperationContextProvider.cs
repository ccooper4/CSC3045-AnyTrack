using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.SharedUtilities.Extensions;

namespace AnyTrack.Backend.Providers
{
    /// <summary>
    /// Provides a wrapper around the WCF Operation Context
    /// </summary>
    public class OperationContextProvider
    {
        #region Properties 

        /// <summary>
        /// Gets the Incoming Message Properties from WCF.
        /// </summary>
        public virtual MessageProperties IncomingMessageProperties
        {
            get
            {
                if (OperationContext.Current != null)
                {
                    return OperationContext.Current.IncomingMessageProperties;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns the channel that can be used to call functions on the client.
        /// </summary>
        /// <typeparam name="T">The channel type.</typeparam>
        /// <returns>The client channel.</returns>
        public virtual T GetClientChannel<T>()
        {
            return OperationContext.Current.GetCallbackChannel<T>();
        }

        /// <summary>
        /// Retrieves the name of the method being called by a service operation.
        /// </summary>
        /// <returns>The reflected method info.</returns>
        public virtual MethodInfo GetMethodInfoForServiceCall()
        {
            var methodName = OperationContext.Current.IncomingMessageProperties["HttpOperationName"].ToString();

            if (methodName.IsEmpty())
            {
                string action = OperationContext.Current.IncomingMessageHeaders.Action;
                methodName = OperationContext.Current.EndpointDispatcher.DispatchRuntime.Operations.FirstOrDefault(o => o.Action == action).Name;
            }

            var serviceType = OperationContext.Current.Host.Description.ServiceType;
            var method = serviceType.GetMethod(methodName);
            return method;
        }

        #endregion 
    }
}
