using AnyTrack.Infrastructure.Extensions;
using AnyTrack.Infrastructure.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure.BackendAccountService;
using FluentAssertions;
using System.Security.Principal;

namespace Unit.Common.AnyTrack.Infrastructure.Extensions.IPrincipalExtensionsTests
{
    #region Context

    public class Context
    {
        public static ServiceUserPrincipal principal;

        [SetUp]
        public void SetUp()
        {
            principal = new ServiceUserPrincipal(new ServiceLoginResult(), "test");
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
            principal = new ServiceUserPrincipal(new ServiceLoginResult(), authCookie);

            principal.GetAuthCookie().Should().Be(authCookie);
        }

        #endregion

        #region GetFullName(this IPrincipal principal) Tests

        [Test]
        public void CallGetFullName()
        {
            var loginResult = new ServiceLoginResult
            {
                FirstName = "David",
                LastName = "Tester"
            };
            principal = new ServiceUserPrincipal(loginResult, "");

            principal.GetFullName().Should().Be(loginResult.FirstName + " " + loginResult.LastName);
        }

        #endregion

        #region IsUserInRole(this IPrincipal principal, string role, Guid? projectId = null, Guid? sprintId = null) Tests 

        [Test]
        public void CallIsUserInRoleOnServicePrincipal()
        {
            var loginResult = new ServiceLoginResult
            {
                AssignedRoles =  new List<ServiceRoleInfo>()
                {
                    new ServiceRoleInfo { Role = "Tester"}
                }.ToArray()
            };

            var servicePrincipal = new ServiceUserPrincipal(loginResult, "test");

            var userInRole = servicePrincipal.IsUserInRole("Tester", null, null);
            userInRole.Should().BeTrue();
        }

        [Test]
        public void CallIsUserInRoleOnOtherPrincipal()
        {
            var genericPrincipal = new GenericPrincipal(new GenericIdentity("Test"), null);

            var userInRole = genericPrincipal.IsUserInRole("Tester", null, null);
            userInRole.Should().BeFalse();
        }

        #endregion 

    }

    #endregion
}
