using AnyTrack.Infrastructure.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Unit.Common.AnyTrack.Infrastructure.Security.ServiceUserIdentityTests
{
    #region Context 

    public class Context
    {
        public static ServiceUserIdentity identity;

        [SetUp]
        public void SetUp()
        {
            identity = new ServiceUserIdentity("");
        }
    }

    #endregion 

    #region Tests 

    public class ServiceUserIdentityTests: Context
    {
        #region AuthenticationType Tests 

        [Test]
        public void GetAuthenticationType()
        {
            identity.AuthenticationType.Should().Be("FormsAuthentication");
        }

        #endregion 

        #region IsAuthenticated Tests

        [Test]
        public void GetIsAuthenticated()
        {
            identity.IsAuthenticated.Should().BeTrue();
        }

        #endregion 

        #region Name Tests 

        [Test]
        public void GetName()
        {
            var name = "test";
            identity = new ServiceUserIdentity(name);

            identity.Name.Should().Be(name);
        }

        #endregion 
    }

    #endregion 
}
