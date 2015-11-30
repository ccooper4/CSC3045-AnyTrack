﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Backend.Service.Model;

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
        /// <param name="sessionInfo">The information about the new session state.</param>
        [OperationContract]
        void NotifyClientOfSession(ServiceSessionChangeInfo sessionInfo); 

        /// <summary>
        /// Notifies the cient that the session they are in has been terminated. 
        /// </summary>
        [OperationContract]
        void NotifyClientOfTerminatedSession();

        /// <summary>
        /// Notifies any conected clients with a new message that has been sent
        /// </summary>
        /// <param name="msg">The message being sent to the clients</param>
        [OperationContract]
        void SendMessageToClients(ServiceChatMessage msg);
    }    
}
