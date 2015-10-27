using AnyTrack.Backend.Providers;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit.Backend.AnyTrack.Backend.Providers.RoleProviderTests
{
    public class Context
    {
        public static RoleProvider provider; 

        [SetUp]
        public void ContextSetup()
        {
            provider = new RoleProvider();
        }
    }

    public class RoleProviderTests: Context
    {
        #region ApplicationName Tests 

        [Test]
        public void GetApplicationName()
        {
            var applicationName = provider.ApplicationName;

            applicationName.Should().Be("AnyTrack");
        }

        #endregion

        #region GetRolesForUser(string username) Tests 

        [Test]
        public void GetRolesForUser()
        {
            var user = "tester001";

            var roles = provider.GetRolesForUser(user);

            roles.Length.Should().Be(0);
        }


        #endregion

        #region AddUsersToRoles(string[] usernames, string[] roleNames) Tests 

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void AddUsersToRoles()
        {
            provider.AddUsersToRoles(new string[0], new string[0]);
        }


        #endregion

        #region CreateRole(string roleName) Tests 

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void CreateRole()
        {
            provider.CreateRole("test");
        }

        #endregion

        #region DeleteRole(string roleName, bool throwOnPopulatedRole) Tests 

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void DeleteRole()
        {
            provider.DeleteRole("test", false);
        }

        #endregion

        #region FindUsersInRole(string roleName, string usernameToMatch) Tests 

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void FindUsersInRole()
        {
            provider.FindUsersInRole("test", "test");
        }

        #endregion

        #region GetAllRoles() Tests 

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetAllRoles()
        {
            provider.GetAllRoles();
        }

        #endregion

        #region GetUsersInRole(string roleName) Tests 

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetUsersInRole()
        {
            provider.GetUsersInRole("test");
        }

        #endregion

        #region IsUserInRole(string username, string roleName) Tests 

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void IsUserInRole()
        {
            provider.IsUserInRole("test", "test");
        }

        #endregion

        #region RemoveUsersFromRoles(string[] usernames, string[] roleNames) Tests 

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void RemoveUsersFromRoles()
        {
            provider.RemoveUsersFromRoles(new string[0], new string[0]);
        }

        #endregion

        #region RoleExists(string roleName) Tests 

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void RoleExists()
        {
            provider.RoleExists("test");
        }

        #endregion 
    }
}
