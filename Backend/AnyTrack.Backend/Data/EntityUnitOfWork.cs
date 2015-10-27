﻿using System;
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

            UserEntitySet = base.Set<User>();
            UserRepository = new EntityRepository<User>(UserEntitySet);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the user repository controlled by this unit of work.
        /// </summary>
        public IRepository<User> UserRepository { get; private set; }

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets the User's Entity Set, as provided by Entity Framework.
        /// </summary>
        private DbSet<User> UserEntitySet { get; set; }

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
