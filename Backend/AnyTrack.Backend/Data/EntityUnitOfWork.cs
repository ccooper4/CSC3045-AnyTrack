﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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
            UserEntitySet = base.Set<User>();
            UserRepository = new EntityRepository<User>(UserEntitySet);

            RoleEntitySet = base.Set<Role>();
            RoleRepository = new EntityRepository<Role>(RoleEntitySet);
            
            ProjectEntitySet = base.Set<Project>();
            ProjectRepository = new EntityRepository<Project>(ProjectEntitySet);

            StoryEntitySet = base.Set<Story>();
            StoryRepository = new EntityRepository<Story>(StoryEntitySet);

            SprintStoryEntitySet = base.Set<SprintStory>();
            SprintStoryRepository = new EntityRepository<SprintStory>(SprintStoryEntitySet);

            SprintEntitySet = base.Set<Sprint>();
            SprintRepository = new EntityRepository<Sprint>(SprintEntitySet);

            TaskEntitySet = base.Set<Task>();
            TaskRepository = new EntityRepository<Task>(TaskEntitySet);

            TaskHourEstimateEntitySet = base.Set<TaskHourEstimate>();
            TaskHourEstimateRepository = new EntityRepository<TaskHourEstimate>(TaskHourEstimateEntitySet);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EntityUnitOfWork, Configuration>());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the user repository controlled by this unit of work.
        /// </summary>
        public IRepository<User> UserRepository { get; private set; }

        /// <summary>
        /// Gets the project repository controlled by this unit of work.
        /// </summary>
        public IRepository<Project> ProjectRepository { get; private set; }

        /// <summary>
        /// Gets the role repository controlled by this unit of work.
        /// </summary>
        public IRepository<Role> RoleRepository { get; private set; }

        /// <summary>
        /// Gets the story repository controlled by this unit of work.
        /// </summary>
        public IRepository<Story> StoryRepository { get; private set; }

        /// <summary>
        /// Gets the task repository controlled by this unit of work.
        /// </summary>
        public IRepository<Model.Task> TaskRepository { get; private set; }

        /// <summary>
        /// Gets the task hours estimate repository controlled by this unit of work.
        /// </summary>
        public IRepository<TaskHourEstimate> TaskHourEstimateRepository { get; private set; }

        /// <summary>
        /// Gets the sprint repository controlled by this unit of work.
        /// </summary>
        public IRepository<Sprint> SprintRepository { get; private set; }

        /// <summary>
        /// Gets the sprint repository controlled by this unit of work.
        /// </summary>
        public IRepository<SprintStory> SprintStoryRepository { get; private set; } 

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets the User's Entity Set, as provided by Entity Framework.
        /// </summary>
        private DbSet<User> UserEntitySet { get; set; }

        /// <summary>
        /// Gets or sets the Project's Entity Set, as provided by Entity Framework.
        /// </summary>
        private IDbSet<Project> ProjectEntitySet { get; set; }

        /// <summary>
        /// Gets or sets the Role Entity Set, as provided by Entity Framework.
        /// </summary>
        private DbSet<Role> RoleEntitySet { get; set; }

        /// <summary>
        /// Gets or sets the Story Entity Set, as provided by Entity Framework.
        /// </summary>
        private DbSet<Story> StoryEntitySet { get; set; }

        /// <summary>
        /// Gets or sets the Task Entity Set, as provided by Entity Framework.
        /// </summary>
        private DbSet<Task> TaskEntitySet { get; set; }

        /// <summary>
        /// Gets or sets the Task Hours Estimate Entity Set, as provided by Entity Framework.
        /// </summary>
        private DbSet<TaskHourEstimate> TaskHourEstimateEntitySet { get; set; }

        /// <summary>
        /// Gets or sets the Sprint Entity Set, as provided by Entity Framework.
        /// </summary>
        private DbSet<Sprint> SprintEntitySet { get; set; }

        /// <summary>
        /// Gets or sets the Sprint Entity Set, as provided by Entity Framework.
        /// </summary>
        private DbSet<SprintStory> SprintStoryEntitySet { get; set; }

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

        /// <summary>
        /// Provides a method by which the model creation can be modified.
        /// </summary>
        /// <param name="modelBuilder">The EF model builder.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasMany(p => p.ScrumMasters).WithMany();
            modelBuilder.Entity<Sprint>().HasMany(d => d.Team).WithMany();
            base.OnModelCreating(modelBuilder);        
        }

        #endregion   
    }
}
