using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data;

namespace AnyTrack.Backend.Providers
{
    /// <summary>
    /// Provides a custom implementation of the RoleProvider class. 
    /// </summary>
    public class RoleProvider : System.Web.Security.RoleProvider
    {
        #region Properties 

        /// <summary>
        /// Gets or sets the unit of work used for database access.
        /// </summary>
        public IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// Gets or sets the Application Name.
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                return "AnyTrack";
            }

            set
            {
            }
        }

        #endregion

        #region Methods 

        /// <summary>
        /// Adds the specified users to the specified roles.
        /// </summary>
        /// <param name="usernames">The users.</param>
        /// <param name="roleNames">The roles.</param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Creates the specified role.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        public override void CreateRole(string roleName)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Deletes the specified role.
        /// </summary>
        /// <param name="roleName">The role to delete.</param>
        /// <param name="throwOnPopulatedRole">A flag indicating if an exception should be thrown if no role is found.</param>
        /// <returns>A flag containing the result of the deletion.</returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns the users in the given role.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <param name="usernameToMatch">The matching pattern for the usernames.</param>
        /// <returns>A collection of users.</returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns all roles in the system.
        /// </summary>
        /// <returns>An array of the available roles.</returns>
        public override string[] GetAllRoles()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the roles for the given user. 
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The roles for the user.</returns>
        public override string[] GetRolesForUser(string username)
        {
            var user = UnitOfWork.UserRepository.Items.Single(u => u.EmailAddress == username);

            return user.Roles.Select(r => r.RoleName).ToArray();
        }

        /// <summary>
        /// Gets the users in the given role.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <returns>Returns the users in the given role.</returns>
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns a flag to show if the user is in the provided role.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="roleName">The rolename.</param>
        /// <returns>A flag to show if the user is in a role.</returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes the users from the provided roles.
        /// </summary>
        /// <param name="usernames">The usernames.</param>
        /// <param name="roleNames">The roles.</param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns true if the role exists.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <returns>True or false to show if a role exists.</returns>
        public override bool RoleExists(string roleName)
        {
            throw new NotSupportedException();
        }

        #endregion 
    }
}
