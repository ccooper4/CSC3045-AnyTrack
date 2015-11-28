﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Backend.Service.Model
{
    /// <summary>
    /// Outlines the details about an active planning poker session. 
    /// </summary>
    public class ServicePlanningPokerSession
    {
        /// <summary>
        /// Gets or sets the planning poker session id.
        /// </summary>
        public Guid SessionID { get; set; }

        /// <summary>
        /// Gets or sets the Sprint ID.
        /// </summary>
        public Guid SprintID { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        public List<ServicePlanningPokerUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the ID of the session host.
        /// </summary>
        public Guid HostID { get; set; }

        /// <summary>
        /// Gets or sets the session state.
        /// </summary>
        public ServicePlanningPokerSessionState State { get; set; }
    }
}
