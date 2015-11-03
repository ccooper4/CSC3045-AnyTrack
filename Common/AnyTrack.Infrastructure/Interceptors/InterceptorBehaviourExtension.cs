using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Infrastructure.Interceptors
{
    /// <summary>
    /// InterceptorBehaviourExtension class
    /// </summary>
    public class InterceptorBehaviorExtension : BehaviorExtensionElement, IEndpointBehavior
    {
        /// <summary>
        /// Override Behaviour Type
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(InterceptorBehaviorExtension); }
        }

        /// <summary>
        /// AddBindingParameters method
        /// </summary>
        /// <param name="serviceDescription">An description of the service</param>
        /// <param name="serviceHostBase">ServiceHostBase class to implement hosts</param>
        /// <param name="endpoints">End points of service to allow clients to communicate</param>
        /// <param name="bindingParameters">Stores binding elements to build factories</param>
        public void AddBindingParameters(
            ServiceDescription serviceDescription, 
            System.ServiceModel.ServiceHostBase serviceHostBase,
            System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints,
            System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }
      
        /// <summary>
        /// Validate method
        /// </summary>
        /// <param name="serviceDescription">Description of service</param>
        /// <param name="serviceHostBase">ServiceHostBase class to implement hosts</param>
        public void Validate(
            ServiceDescription serviceDescription,
            System.ServiceModel.ServiceHostBase serviceHostBase)
        {
        }

        /// <summary>
        /// Validate method
        /// </summary>
        /// <param name="endpoint">Endpoint for clients to communicate to</param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        /// <summary>
        /// Add binding parameters method
        /// </summary>
        /// <param name="endpoint">Service end point to allow clients to communicate</param>
        /// <param name="bindingParameters">Collection of parameters to build factories</param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Apply dispatch behaviour method
        /// </summary>
        /// <param name="endpoint">Service end point to allow clients to communicate</param>
        /// <param name="endpointDispatcher">Exposes properties to enable insertion of run-time extensions</param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        /// <summary>
        /// ApplyClientBehaviour method
        /// </summary>
        /// <param name="serviceEndpoint">Service end point to allow clients to communicate</param>
        /// <param name="behavior">Insertion point for classes</param>
        public void ApplyClientBehavior(
            ServiceEndpoint serviceEndpoint,
            System.ServiceModel.Dispatcher.ClientRuntime behavior)
        {
            behavior.MessageInspectors.Add(new CookieMessageInspector());
        }

        /// <summary>
        /// Create behaviour method
        /// </summary>
        /// <returns>returns new InterceptorBehaviourExtension</returns>
        protected override object CreateBehavior()
        {
            return new InterceptorBehaviorExtension();
        }
    }
}