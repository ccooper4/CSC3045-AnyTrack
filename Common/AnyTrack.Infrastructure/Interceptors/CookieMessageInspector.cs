using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AnyTrack.Infrastructure.Extensions;

namespace AnyTrack.Infrastructure.Interceptors
{
    /// <summary>
    /// CookieMessageInspector class
    /// </summary>
    public class CookieMessageInspector : IClientMessageInspector
    {
        /// <summary>
        /// String to store cookie
        /// </summary>
        private string cookie;

        /// <summary>
        /// Constructor for CookieMessageInspector
        /// </summary>
        public CookieMessageInspector()
        {
            cookie = Thread.CurrentPrincipal.GetAuthCookie();
        }

       /// <summary>
       /// Before send request method
       /// </summary>
       /// <param name="request">Request message</param>
       /// <param name="channel">Channel used by client application</param>
       /// <returns>Returns null value</returns>
        public object BeforeSendRequest(
            ref System.ServiceModel.Channels.Message request,
            System.ServiceModel.IClientChannel channel)
        {
            if (!string.IsNullOrEmpty(cookie))
            {
                HttpRequestMessageProperty requestMessageProperty = new HttpRequestMessageProperty();
                requestMessageProperty.Headers[HttpResponseHeader.SetCookie] = cookie;
                request.Properties[HttpRequestMessageProperty.Name] = requestMessageProperty;
            }

            return null;
        }

        /// <summary>
        /// AfterReceiveReply method
        /// </summary>
        /// <param name="reply">Message reply</param>
        /// <param name="correlationState">correlationState object</param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }
    }
}
