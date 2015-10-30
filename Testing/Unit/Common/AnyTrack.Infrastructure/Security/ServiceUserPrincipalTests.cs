using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Infrastructure.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Unit.Common.AnyTrack.Infrastructure.Security.ServiceUserPrincipalTests
{
    #region Context

    public class Context
    {
        public static ServiceUserPrincipal principal;

        [SetUp]
        public void SetUp()
        {
            principal = new ServiceUserPrincipal(new global::AnyTrack.Infrastructure.BackendAccountService.LoginResult(), "");
        }
    }

    #endregion 

    #region Tests 

    public class ServiceUserPrincipalTests: Context
    {
        #region Identity Tests 

        [Test]
        public void GetIdentity()
        {
            var loginResult = new LoginResult { EmailAddress = "test" };
            var cookie = "test";

            principal = new ServiceUserPrincipal(loginResult, cookie);

            var identity = principal.Identity;

            identity.Should().NotBeNull();
            identity.Should().BeOfType<ServiceUserIdentity>();
            identity.Name.Should().Be(loginResult.EmailAddress);
        }

        #endregion 

        #region AuthCookie Tests

        [Test]
        public void GetAuthCookie()
        {
            var loginResult = new LoginResult { EmailAddress = "test" };
            var cookie = "test";

            principal = new ServiceUserPrincipal(loginResult, cookie);

            principal.AuthCookie.Should().Be(cookie);
        }

        #endregion 

        #region IsInRole(string role) Tests 

        [Test]
        public void IsInRoleWhenUserIsInRole()
        {
            var role = "Tester";
            var loginResult = new LoginResult
            {
                AssignedRoles = new List<RoleInfo>()
                {
                    new RoleInfo { Role = role}
                }.ToArray()
            };
            var cookie = "test";

            principal = new ServiceUserPrincipal(loginResult, cookie);

            principal.IsInRole(role).Should().BeTrue();
        }

        [Test]
        public void IsInRoleWhenUserIsNotInRole()
        {
            var role = "Tester2";
            var loginResult = new LoginResult
            {
                AssignedRoles = new List<RoleInfo>()
                {
                    new RoleInfo { Role = role}
                }.ToArray()
            };
            var cookie = "test";

            principal = new ServiceUserPrincipal(loginResult, cookie);

            principal.IsInRole("Not").Should().BeFalse();
        }

        #endregion 
    }

    #endregion
}
