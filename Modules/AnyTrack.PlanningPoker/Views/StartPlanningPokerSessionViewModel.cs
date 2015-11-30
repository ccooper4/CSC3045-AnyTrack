using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.ServiceGateways;
using AnyTrack.PlanningPoker.ServiceGateways;
using Prism.Regions;

namespace AnyTrack.PlanningPoker.Views
{
    /// <summary>
    /// The view model that allows a scrum master to start a planning poker session.
    /// </summary>
    public class StartPlanningPokerSessionViewModel : ValidatedBindableBase, INavigationAware, IRegionMemberLifetime
    {
        #region Fields 

        /// <summary>
        /// The planning poker service gateway.
        /// </summary>
        private readonly IPlanningPokerManagerServiceGateway serviceGateway;

        /// <summary>
        /// The project service gateway.
        /// </summary>
        private readonly IProjectServiceGateway projectServiceGateway;

        /// <summary>
        /// The selected project id.
        /// </summary>
        private Guid projectId; 

        #endregion 

        #region Constructor 

        /// <summary>
        /// Constructs a new instance of the start planning poker view model with the specified dependencies.
        /// </summary>
        /// <param name="gateway">The service gateway.</param>
        /// <param name="projectServiceGateway">The project service gateway.</param>
        public StartPlanningPokerSessionViewModel(IPlanningPokerManagerServiceGateway gateway, IProjectServiceGateway projectServiceGateway)
        {
            if (gateway == null)
            {
                throw new ArgumentNullException("gateway");
            }

            if (projectServiceGateway == null)
            {
                throw new ArgumentNullException("projectServiceGateway"); 
            }

            this.serviceGateway = gateway;
            this.projectServiceGateway = projectServiceGateway;

            this.Projects = new ObservableCollection<ServiceProjectSummary>();
        }

        #endregion 

        #region Properties 

        /// <summary>
        /// Gets or sets the list of projects.
        /// </summary>
        public ObservableCollection<ServiceProjectSummary> Projects { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public Guid ProjectId
        {
            get
            {
                return projectId; 
            }

            set
            {
                SetProperty(ref projectId, value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this view/view model should be reused.
        /// </summary>
        public bool KeepAlive
        {
            get
            {
                return false;
            }
        }

        #endregion 

        #region Methods 

        /// <summary>
        /// Allows this view model to specify if it can be re-used as a view model.
        /// </summary>
        /// <param name="navigationContext">The current navigation context.</param>
        /// <returns>A true or false value indicating if this view can be used as the new view.</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        /// <summary>
        /// Allows this view model to run any custom logic when the region system navigates from it.
        /// </summary>
        /// <param name="navigationContext">The current navigation context.</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// Allows this view model to run any custom logic when the region system navigates to it.
        /// </summary>
        /// <param name="navigationContext">The current navigation context.</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.Projects.Clear();
            var results = projectServiceGateway.GetProjectNames(true, false, false);
            this.Projects.AddRange(results);
        }

        #endregion 
    }
}
