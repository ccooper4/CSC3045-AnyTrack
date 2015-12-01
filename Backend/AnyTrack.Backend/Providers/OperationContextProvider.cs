using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
        /// Gets the message headers from the current operation context.
        /// </summary>
        public virtual MessageHeaders IncomingMessageHeaders
        {
            get
            {
                if (OperationContext.Current != null)
                {
                    return OperationContext.Current.IncomingMessageHeaders;
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
            if (OperationContext.Current == null)
            {
                return default(T);
            }

            return OperationContext.Current.GetCallbackChannel<T>();
        }

        /// <summary>
        /// Retrieves the name of the method being called by a service operation.
        /// </summary>
        /// <returns>The reflected method info.</returns>
        public virtual MethodInfo GetMethodInfoForServiceCall()
        {
            //// Operation Context current may be null, if so, there is no method. Return null.
            if (OperationContext.Current == null)
            {
                return null; 
            }

            var methodName = string.Empty;

            try
            {
                methodName = OperationContext.Current.IncomingMessageProperties["HttpOperationName"].ToString();
            }
            catch
            {
            }

            if (methodName.IsEmpty())
            {
                string action = OperationContext.Current.IncomingMessageHeaders.Action;
                methodName = OperationContext.Current.EndpointDispatcher.DispatchRuntime.Operations.FirstOrDefault(o => o.Action == action).Name;
            }

            var serviceType = OperationContext.Current.Host.Description.ServiceType;
            var method = serviceType.GetMethod(methodName);
            return method;
        }
        
        /// <summary>
        /// Returns the user in the HTTP Context.
        /// </summary>
        /// <returns>An IPrincipal object.</returns>
        public virtual IPrincipal GetHttpContextUser()
        {
            var context = HttpContext.Current;
            if (context != null && context.User != null)
            {
                return context.User;
            }

            return null;
        }

        #endregion 
    }
}
