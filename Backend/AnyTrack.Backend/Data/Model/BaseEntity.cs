using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Data.Model
{
    /// <summary>
    /// Provides a base class for data models
    /// </summary>
    public abstract class BaseEntity
    {
        #region Constructor 

        /// <summary>
        /// Creates a new base entity.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Setting ID equal to a new GUID.")]
        public BaseEntity()
        {
            this.Id = Guid.NewGuid(); 
        }

        #endregion 

        #region Properties

        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the updated date
        /// </summary>
        public virtual DateTime Updated { get; set; }

        /// <summary>
        /// Gets or sets the created date
        /// </summary>
        public virtual DateTime Created { get; set; }

        #endregion 
    }
}
