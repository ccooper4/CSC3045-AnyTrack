using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Providers;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Provides a service for getting the time.
    /// </summary>
    public class DateTimeService : PrincipalBuilderService, IDateTimeService
    {
        #region Constructor 

        /// <summary>
        /// Constructs a new Date Time service.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="provider">The forms auth provider.</param>
        /// <param name="context">The operation context provider.</param>
        public DateTimeService(IUnitOfWork unitOfWork, FormsAuthenticationProvider provider, OperationContextProvider context) : base(unitOfWork, provider, context)
        {
        }

        #endregion 

        #region Methods

        /// <summary>
        /// Gets the current date. 
        /// </summary>
        /// <returns>The current date.</returns>
        [PrincipalPermission(SecurityAction.Demand, Name = "andrew.fletcher16@gmail.com")]
        public DateTime GetCurrentDate()
        {
            return DateTime.Now;
        }

        #endregion 
    }
}
