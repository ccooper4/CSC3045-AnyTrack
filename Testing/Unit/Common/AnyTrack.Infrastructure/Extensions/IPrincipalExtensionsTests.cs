using AnyTrack.Infrastructure.Extensions;
using AnyTrack.Infrastructure.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using AnyTrack.Infrastructure.BackendAccountService;

namespace Unit.Common.AnyTrack.Infrastructure.Extensions.IPrincipalExtensionsTests
{
    #region Context

    public class Context
    {
        public static ServiceUserPrincipal principal;

        [SetUp]
        public void SetUp()
        {
            principal = new ServiceUserPrincipal(new global::AnyTrack.Infrastructure.BackendAccountService.LoginResult(), "test");
        }
    }

    #endregion 

    #region Tests
    public class IPrincipalExtensionsTests: Context
    {
        #region GetAuthCookie(this IPrincipal principal) Tests

        [Test]
        public void CallGetAuthCookie()
        {
            var authCookie = "test";
            principal = new ServiceUserPrincipal(new global::AnyTrack.Infrastructure.BackendAccountService.LoginResult(), authCookie);

            principal.GetAuthCookie().Should().Be(authCookie);
        }

        #endregion

        #region GetFullName(this IPrincipal principal) Tests

        [Test]
        public void CallGetFullName()
        {
            var loginResult = new LoginResult
            {
                FirstName = "David",
                LastName = "Tester"
            };
            principal = new ServiceUserPrincipal(loginResult, "");

            principal.GetFullName().Should().Be(loginResult.FirstName + " " + loginResult.LastName);
        }

        #endregion
    
    }

    #endregion
}
