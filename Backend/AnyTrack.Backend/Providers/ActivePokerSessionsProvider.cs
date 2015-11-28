using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Service;
using AnyTrack.Backend.Service.Model;

namespace AnyTrack.Backend.Providers
{
    /// <summary>
    /// Provides the singleton list of active sessions for planning poker.
    /// </summary>
    public class ActivePokerSessionsProvider
    {
        #region Fields 

        /// <summary>
        /// The connected clients.
        /// </summary>
        private static Dictionary<Guid, ServicePlanningPokerSession> activeSessions; 

        #endregion 

        #region Methods 

        /// <summary>
        /// Retrieves the singleton connected clients list.
        /// </summary>
        /// <returns>The list of connected clients.</returns>
        public virtual Dictionary<Guid, ServicePlanningPokerSession> GetListOfSessions()
        {
            if (activeSessions == null)
            {
                activeSessions = new Dictionary<Guid, ServicePlanningPokerSession>();
            }

            return activeSessions;
        }

        #endregion 
    }
}
