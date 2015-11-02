using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Infrastructure.Interceptors;
using NSubstitute;
using NUnit.Framework;

namespace Unit.Common.AnyTrack.Infrastructure
{
    #region Context

    public class Context
    {
        public static CookieMessageInspector cookieMessageInspector;

        [SetUp]
        public void ContextSetup()
        {
            cookieMessageInspector = new CookieMessageInspector();
        }
    }

    #endregion
    
    #region Tests

    public class InterceptorBehaviourExtensionTests : Context
    {
        [Test]
        public void BeforeSendRequestReturnsNull()
        {
            var request = Substitute.For<System.ServiceModel.Channels.Message>();
            cookieMessageInspector.BeforeSendRequest(ref request, Substitute.For<IClientChannel>());
        }
    }

    #endregion
}


