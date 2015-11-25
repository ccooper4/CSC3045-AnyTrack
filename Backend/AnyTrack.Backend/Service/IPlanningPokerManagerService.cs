using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// Outlines the interface for the Server hosted component of the planning poker system.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IPlanningPokerClientService))]
    public interface IPlanningPokerManagerService
    {
        /// <summary>
        /// Allows the client to subscribe to messages about new sessions for the given project and sprint ids. 
        /// </summary>
        /// <param name="sprintId">The sprint id.</param>
        [OperationContract(IsOneWay = true)]
        void SubscribeToNewSessionMessages(Guid sprintId);

        /// <summary>
        /// Allows the scrum master to start a new planning poker session.
        /// </summary>
        /// <param name="sprintId">The sprint id.</param>
        /// <returns>The session id for the new poker session.</returns>
        Guid StartNewPokerSession(Guid sprintId);
    }
}
