using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data.Model;

namespace AnyTrack.Backend.Data
{
    /// <summary>
    /// Represents a repository in the unit of work.
    /// </summary>
    /// <typeparam name="T">The type of item in this repository.</typeparam>
    public class EntityRepository<T> : IRepository<T> where T : BaseEntity
    {
        #region Fields

        /// <summary>
        /// The underlaying EF items set.
        /// </summary>
        private readonly IDbSet<T> items;

        #endregion 

        #region Constructor 

        /// <summary>
        /// Creates a new Entity Repository with the specified items.
        /// </summary>
        /// <param name="items">The DB items.</param>
        public EntityRepository(IDbSet<T> items)
        {
            if (items == null) 
            {
                throw new ArgumentNullException("items");
            }

            this.items = items;
        }

        #endregion 

        #region Properties

        /// <summary>
        /// Gets the items in the repository.
        /// </summary>
        public IQueryable<T> Items
        {
            get
            {
                return items;
            }
        }

        #endregion 

        #region Methods

        /// <summary>
        /// Inserts a new item into the repository, 
        /// </summary>
        /// <param name="item">The item to insert.</param>
        public void Insert(T item)
        {
            items.Add(item);
        }

        /// <summary>
        /// Deletes the specified item from the repository. 
        /// </summary>
        /// <param name="item">The item to delete.</param>
        public void Delete(T item)
        {
            items.Remove(item);
        }

        #endregion
    }
}
