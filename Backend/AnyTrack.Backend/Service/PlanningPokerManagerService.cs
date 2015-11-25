using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Data;
using AnyTrack.Backend.Providers;
using AnyTrack.Backend.Security;

namespace AnyTrack.Backend.Service
{
    /// <summary>
    /// The implementation of the planning poker manager service. 
    /// </summary>
    [CreatePrincipal]
    public class PlanningPokerManagerService : IPlanningPokerManagerService
    {
        #region Fields 

        /// <summary>
        /// The database unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The operation context provider.
        /// </summary>
        private readonly OperationContextProvider contextProvider;

        /// <summary>
        /// The currently connected clients.
        /// </summary>
        private readonly ConnectedClientsProvider connectedClients;

        #endregion 

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the planning poker manager service.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="context">The operation context provider.</param>
        /// <param name="connectedClients">The connected clients provider.</param>
        public PlanningPokerManagerService(IUnitOfWork unitOfWork, OperationContextProvider context, ConnectedClientsProvider connectedClients)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (connectedClients == null)
            {
                throw new ArgumentNullException("connectedClients");
            }

            this.unitOfWork = unitOfWork;
            this.contextProvider = context;
            this.connectedClients = connectedClients;
        }

        #endregion 

        #region Methods 

        /// <summary>
        /// Allows the client to subscribe to messages about new sessions for the given project and sprint ids. 
        /// </summary>
        /// <param name="sprintId">The sprint id.</param>
        public void SubscribeToNewSessionMessages(Guid sprintId)
        {
        }

        /// <summary>
        /// Starts a new planning poker session. 
        /// </summary>
        /// <param name="sprintId">The sprint id.</param>
        /// <returns>The new session id.</returns>
        public Guid StartNewPokerSession(Guid sprintId)
        {
            return Guid.NewGuid();
        }

        #endregion 
    }
}
