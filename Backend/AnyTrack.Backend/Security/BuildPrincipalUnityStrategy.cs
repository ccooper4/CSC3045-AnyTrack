using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Providers;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace AnyTrack.Backend.Security
{
    /// <summary>
    /// The strategy used by Unity to build the principal.
    /// </summary>
    public class BuildPrincipalUnityStrategy : BuilderStrategy
    {
        #region Fields 

        /// <summary>
        /// The unity container.
        /// </summary>
        private readonly IUnityContainer container;

        /// <summary>
        /// Types that are excluded from being affected by this extension.
        /// </summary>
        private readonly Type[] excludedTypes = new Type[] { typeof(IUnitOfWork), typeof(OperationContextProvider), typeof(FormsAuthenticationProvider) }; 

        #endregion 

        #region Constructor

        /// <summary>
        /// Constructs a new instance of this strategy with the specified container.
        /// </summary>
        /// <param name="container">The unity container.</param>
        public BuildPrincipalUnityStrategy(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Post Build Up section of the Unity chain.
        /// </summary>
        /// <param name="context">The unity context.</param>
        public override void PostBuildUp(IBuilderContext context)
        {
            base.PostBuildUp(context);

            var obj = context.Existing;
            var objType = obj.GetType();

            //// We must not run this logic at all for classes this extension depends on,
            //// if we do that, a StackoverflowException happens - 
            //// The container attempts to resolve the extension dependency which then
            //// attempts to re-run this logic. 
            
            if (excludedTypes.Contains(objType))
            {
                return; 
            }

            var operationContext = container.Resolve<OperationContextProvider>();

            var objHasAttribute = objType.CustomAttributes.SingleOrDefault(a => a.AttributeType == typeof(CreatePrincipalAttribute)) != null;

            var callMethod = operationContext.GetMethodInfoForServiceCall();
            var callMethodHasAttribute = callMethod == null ? false : callMethod.CustomAttributes.SingleOrDefault(a => a.AttributeType == typeof(CreatePrincipalAttribute)) != null;

            if (objHasAttribute || callMethodHasAttribute)
            {
                var formsAuthProvider = container.Resolve<FormsAuthenticationProvider>();
                var unitOfWork = container.Resolve<IUnitOfWork>();

                if (operationContext.IncomingMessageProperties != null)
                {
                    var messageProperty = (HttpRequestMessageProperty)operationContext.IncomingMessageProperties[HttpRequestMessageProperty.Name];
                    string cookie = messageProperty.Headers.Get("Set-Cookie");
                    if (!string.IsNullOrWhiteSpace(cookie))
                    {
                        var cookieParts = cookie.Split(';');
                        var encryptedAuthCookie = cookieParts[0].Replace("AuthCookie=", string.Empty);
                        var formsTicket = formsAuthProvider.Decrypt(encryptedAuthCookie);

                        if (!formsTicket.Expired)
                        {
                            var user = unitOfWork.UserRepository.Items.Single(u => u.EmailAddress == formsTicket.Name);
                            var principal = new GeneratedServiceUserPrincipal(user);

                            Thread.CurrentPrincipal = principal;
                        }
                    }
                }
            }
        }

        #endregion 
    }
}
