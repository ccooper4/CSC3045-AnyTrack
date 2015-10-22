using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data.Model;

namespace AnyTrack.Backend.Data
{
    /// <summary>
    /// Outlines the interface used to access a specific database repository.
    /// </summary>
    /// <typeparam name="T">The type of item in the repository.</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets the items in the repository.
        /// </summary>
        IQueryable<T> Items { get; }

        #endregion 

        #region Methods 

        /// <summary>
        /// Inserts a new item into the repository, 
        /// </summary>
        /// <param name="item">The item to insert.</param>
        void Insert(T item);

        /// <summary>
        /// Deletes the specified item from the repository. 
        /// </summary>
        /// <param name="item">The item to delete.</param>
        void Delete(T item); 

        #endregion 
    }
}
