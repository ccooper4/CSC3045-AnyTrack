using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data.Model;
using AnyTrack.Backend.Migrations;

namespace AnyTrack.Backend.Data
{
    /// <summary>
    /// Represents the object used to manage the database.
    /// </summary>
    public class EntityUnitOfWork : DbContext, IUnitOfWork
    {
        #region Constructor 

        /// <summary>
        /// Creates a new Entity Unit of Work with the default connection string.
        /// </summary>
        public EntityUnitOfWork() : base("AnyTrackBackend")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EntityUnitOfWork, Configuration>());
        }

        #endregion 

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Saves any pending changes to the database.
        /// </summary>
        public void Commit()
        {
            foreach (var e in ChangeTracker.Entries().Where(e => e.State == EntityState.Added).Select(e => e.Entity as BaseEntity))
            {
                e.Created = DateTime.Now;    
            }

            foreach (var e in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).Select(e => e.Entity as BaseEntity))
            {
                e.Updated = DateTime.Now;
            }

            this.SaveChanges();
        }

        #endregion 
    }
}
