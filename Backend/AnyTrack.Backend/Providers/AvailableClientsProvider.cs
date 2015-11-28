﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Service;
using AnyTrack.Backend.Service.Model;

namespace AnyTrack.Backend.Providers
{
    /// <summary>
    /// Provides the singleton list of connected clients for planning poker.
    /// </summary>
    public class AvailableClientsProvider
    {
        #region Fields 

        /// <summary>
        /// The connected clients.
        /// </summary>
        private static Dictionary<Guid, List<ServicePlanningPokerPendingUser>> connectedClients; 

        #endregion 

        #region Methods 

        /// <summary>
        /// Retrieves the singleton connected clients list.
        /// </summary>
        /// <returns>The list of connected clients.</returns>
        public virtual Dictionary<Guid, List<ServicePlanningPokerPendingUser>> GetListOfClients()
        {
            if (connectedClients == null)
            {
                connectedClients = new Dictionary<Guid, List<ServicePlanningPokerPendingUser>>();
            }

            return connectedClients;
        }

        #endregion 
    }
}
