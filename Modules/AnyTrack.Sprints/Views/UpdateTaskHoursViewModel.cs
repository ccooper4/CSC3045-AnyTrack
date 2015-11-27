using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AnyTrack.Infrastructure;
using AnyTrack.Sprints.ServiceGateways;
using Prism.Commands;
using Prism.Regions;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// The view model for the Update Task Hours ViewModel
    /// </summary>
    public class UpdateTaskHoursViewModel : ValidatedBindableBase
    {
        #region Fields

        /// <summary>
        /// The srpint service gateway
        /// </summary>
        private readonly ISprintServiceGateway serviceGateway;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Update Task Hours View Model
        /// </summary>
        /// <param name="serviceGateway">The project service gateway</param>
        public UpdateTaskHoursViewModel(ISprintServiceGateway serviceGateway)
        {
            if (serviceGateway == null)
            {
                throw new ArgumentNullException("serviceGateway");
            }

            this.serviceGateway = serviceGateway;
            var results = serviceGateway.GetAllTasksForSprint(new Guid("35892e17-80f6-415f-9c65-7395632f0223"));
        }

        #endregion
    }
}
