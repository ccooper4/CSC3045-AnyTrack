using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Outlines the callback interface for the duplex planning poker service.
    /// </summary>
    [ServiceContract]
    public interface IPlanningPokerClientService
    {
        /// <summary>
        /// Notifies any conected clients in the project/sprint group about a new session.
        /// </summary>
        /// <param name="sprintId">The sprint id.</param>
        /// <param name="sessionAvailable">A flag indicating if a session is available for this sprint.</param>
        /// <param name="sessionId">The session id for the available session.</param>
        void NotifyClientOfSession(Guid sprintId, bool sessionAvailable, Guid? sessionId); 

        /// <summary>
        /// Notifies the cient that the session they are in has been terminated. 
        /// </summary>
        void NotifyClientOfTerminatedSession();
    }
}
