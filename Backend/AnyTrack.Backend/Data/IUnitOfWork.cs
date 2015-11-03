using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data.Model;

namespace AnyTrack.Backend.Data
{
    /// <summary>
    /// Provides an interface to the data-context.
    /// </summary>
    public interface IUnitOfWork
    {
        #region Properties 
        /// <summary>
        /// Gets the user repository controlled by this unit of work.
        /// </summary>
        IRepository<User> UserRepository { get; }

        /// <summary>
        /// Gets the role repository controlled by this unit of work.
        /// </summary>
        IRepository<Role> RoleRepository { get; }

        /// <summary>
        /// Gets the Project Repository
        /// </summary>
        IRepository<Project> ProjectRepository { get; }

        #endregion 

        #region Methods 

        /// <summary>
        /// Saves any pending changes to the database.
        /// </summary>
        void Commit(); 

        #endregion 
    }
}
