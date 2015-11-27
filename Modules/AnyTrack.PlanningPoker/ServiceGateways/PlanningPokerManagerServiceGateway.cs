using AnyTrack.Infrastructure.Providers;
using AnyTrack.PlanningPoker.BackendPlanningPokerManagerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.PlanningPoker.ServiceGateways
{
    /// <summary>
    /// Provides an implementation for the Planning Poker Manager service gateway.
    /// </summary>
    public class PlanningPokerManagerServiceGateway
    {
        #region Fields 

        /// <summary>
        /// The provider used to access the operation context. 
        /// </summary>
        private readonly ClientSideOperationContextProvider operationContextProvider;

        /// <summary>
        /// The web service client.
        /// </summary>
        private readonly IPlanningPokerManagerService client; 
        
        #endregion 

        #region Constructor 

        /// <summary>
        /// Constructs a new planning poker manager service gateway with the specified dependencies. 
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="contextProvider">The context provider.</param>
        public PlanningPokerManagerServiceGateway(IPlanningPokerManagerService client, ClientSideOperationContextProvider contextProvider)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            if (contextProvider == null)
            {
                throw new ArgumentNullException("contextProvider");
            }

            this.client = client;
            this.operationContextProvider = contextProvider;
        }

        #endregion 

    }
}
