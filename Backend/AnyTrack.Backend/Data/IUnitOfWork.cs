using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Data
{
    /// <summary>
    /// Provides an interface to the data-context.
    /// </summary>
    public interface IUnitOfWork
    {
        #region Properties 

        #endregion 

        #region Methods 

        /// <summary>
        /// Saves any pending changes to the database.
        /// </summary>
        void Commit(); 

        #endregion 
    }
}
