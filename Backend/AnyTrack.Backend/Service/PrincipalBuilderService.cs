using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Security;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Providers;
using AnyTrack.Backend.Security;
using AnyTrack.Backend.Service.Model;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Provides a base class for services that can build the IPrincipal from the request.
    /// </summary>
    public abstract class PrincipalBuilderService
    {
        #region Constructor

        /// <summary>
        /// Constructs the PrincipalBuilderService. 
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="provider">The forms provider.</param>
        /// <param name="context">The operation context provider.</param>
        public PrincipalBuilderService(IUnitOfWork unitOfWork, FormsAuthenticationProvider provider, OperationContextProvider context)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }
            
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (context.IncomingMessageProperties != null)
            {
                var messageProperty = (HttpRequestMessageProperty)context.IncomingMessageProperties[HttpRequestMessageProperty.Name];
                string cookie = messageProperty.Headers.Get("Set-Cookie");
                if (!string.IsNullOrWhiteSpace(cookie))
                {
                    var cookieParts = cookie.Split(';');
                    var encryptedAuthCookie = cookieParts[0].Replace("AuthCookie=", string.Empty);
                    var formsTicket = provider.Decrypt(encryptedAuthCookie);

                    if (!formsTicket.Expired)
                    {
                        var user = unitOfWork.UserRepository.Items.Single(u => u.EmailAddress == formsTicket.Name);
                        var principal = new GeneratedServiceUserPrincipal(user);

                        Thread.CurrentPrincipal = principal;
                    }
                }
            }
        }

        #endregion 
    }
}
