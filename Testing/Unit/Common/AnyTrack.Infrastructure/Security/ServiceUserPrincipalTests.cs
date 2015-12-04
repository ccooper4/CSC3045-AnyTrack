using AnyTrack.Infrastructure.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure.BackendAccountService;
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
            principal = new ServiceUserPrincipal(new ServiceLoginResult(), "");
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
            var loginResult = new ServiceLoginResult { EmailAddress = "test" };
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
            var loginResult = new ServiceLoginResult { EmailAddress = "test" };
            var cookie = "test";

            principal = new ServiceUserPrincipal(loginResult, cookie);

            principal.AuthCookie.Should().Be(cookie);
        }

        #endregion 

        #region FullName Tests

        [Test]
        public void GetFullName()
        {
            var loginResult = new ServiceLoginResult { FirstName = "David", LastName = "Tester" };
            var cookie = "test";

            principal = new ServiceUserPrincipal(loginResult, cookie);

            principal.FullName.Should().Be(loginResult.FirstName + " " + loginResult.LastName);
        }

        #endregion 

        #region IsInRole(string role) Tests 

        [Test]
        public void IsInRoleWhenUserIsInRole()
        {
            var role = "Tester";
            var loginResult = new ServiceLoginResult
            {
                AssignedRoles = new List<ServiceRoleInfo>()
                {
                    new ServiceRoleInfo { Role = role}
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
            var loginResult = new ServiceLoginResult
            {
                AssignedRoles = new List<ServiceRoleInfo>()
                {
                    new ServiceRoleInfo { Role = role}
                }.ToArray()
            };
            var cookie = "test";

            principal = new ServiceUserPrincipal(loginResult, cookie);

            principal.IsInRole("Not").Should().BeFalse();
        }

        #endregion 

        #region IsInRole(string role, Guid? projectId, Guid? sprintId) Tests

        [Test]
        public void IsInRoleWhenUserIsInRoleWithPrjectId()
        {
            var role = "Tester";
            var projectId = Guid.NewGuid();
            var loginResult = new ServiceLoginResult
            {
                AssignedRoles = new List<ServiceRoleInfo>()
                {
                    new ServiceRoleInfo { Role = role, ProjectId = projectId}
                }.ToArray()
            };
            var cookie = "test";

            principal = new ServiceUserPrincipal(loginResult, cookie);

            principal.IsInRole(role, projectId, null).Should().BeTrue();
        }

        [Test]
        public void IsInRoleWhenUserIsInRoleWithSprintId()
        {
            var role = "Tester";
            var sprintId = Guid.NewGuid();
            var loginResult = new ServiceLoginResult
            {
                AssignedRoles = new List<ServiceRoleInfo>()
                {
                    new ServiceRoleInfo { Role = role, SprintId = sprintId}
                }.ToArray()
            };
            var cookie = "test";

            principal = new ServiceUserPrincipal(loginResult, cookie);

            principal.IsInRole(role, null, sprintId).Should().BeTrue();
        }


        #endregion 
    }

    #endregion
}
