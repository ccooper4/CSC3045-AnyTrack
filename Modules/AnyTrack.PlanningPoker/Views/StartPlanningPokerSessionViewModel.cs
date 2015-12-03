using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.ServiceGateways;
using AnyTrack.PlanningPoker.ServiceGateways;
using Prism.Commands;
using Prism.Regions;
using SprintModels = AnyTrack.Infrastructure.BackendSprintService;

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
        /// The sprint service gateway.
        /// </summary>
        private readonly ISprintServiceGateway sprintServiceGateway;

        /// <summary>
        /// The selected project id.
        /// </summary>
        private Guid? projectId;

        /// <summary>
        /// The selected sprint id.
        /// </summary>
        private Guid? sprintId;

        /// <summary>
        /// The selected Project.
        /// </summary>
        private ServiceProjectSummary selectedProject;

        /// <summary>
        /// The selected Sprint.
        /// </summary>
        private ServiceSprintSummary selectedSprint;

        #endregion 

        #region Constructor 

        /// <summary>
        /// Constructs a new instance of the start planning poker view model with the specified dependencies.
        /// </summary>
        /// <param name="gateway">The service gateway.</param>
        /// <param name="projectServiceGateway">The project service gateway.</param>
        /// <param name="sprintServiceGateway">The sprint service gateway.</param>
        public StartPlanningPokerSessionViewModel(IPlanningPokerManagerServiceGateway gateway, IProjectServiceGateway projectServiceGateway, ISprintServiceGateway sprintServiceGateway)
        {
            if (gateway == null)
            {
                throw new ArgumentNullException("gateway");
            }

            if (projectServiceGateway == null)
            {
                throw new ArgumentNullException("projectServiceGateway"); 
            }

            if (sprintServiceGateway == null)
            {
                throw new ArgumentNullException("sprintServiceGateway");
            }

            this.serviceGateway = gateway;
            this.projectServiceGateway = projectServiceGateway;
            this.sprintServiceGateway = sprintServiceGateway;

            this.Projects = new ObservableCollection<ServiceProjectSummary>();
            this.Sprints = new ObservableCollection<SprintModels.ServiceSprintSummary>();

            this.StartPokerSession = new DelegateCommand(EstablishPokerSession);
        }

        #endregion 

        #region Properties 

        /// <summary>
        /// Gets or sets the command used to start the poker session.
        /// </summary>
        public DelegateCommand StartPokerSession { get; set; }

        /// <summary>
        /// Gets or sets the list of projects.
        /// </summary>
        public ObservableCollection<ServiceProjectSummary> Projects { get; set; }

        /// <summary>
        /// Gets or sets the list of sprints.
        /// </summary>
        public ObservableCollection<SprintModels.ServiceSprintSummary> Sprints { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        [Required]
        public Guid? ProjectId
        {
            get
            {
                return projectId; 
            }

            set
            {
                SetProperty(ref projectId, value);
                this.Sprints.Clear();
                var sprints = sprintServiceGateway.GetSprintNames(value, true, false);
                Sprints.AddRange(sprints);
            }
        }

        /// <summary>
        /// Gets or sets the sprint id.
        /// </summary>
        [Required]
        public Guid? SprintId
        {
            get
            {
                return sprintId;
            }

            set
            {
                SetProperty(ref sprintId, value);
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

            if (navigationContext.Parameters.ContainsKey("ProjectId"))
            {
                this.ProjectId = new Guid(navigationContext.Parameters["ProjectId"].ToString());
            }

            if (navigationContext.Parameters.ContainsKey("SprintId"))
            {
                this.SprintId = new Guid(navigationContext.Parameters["SprintId"].ToString());
            }
        }

        /// <summary>
        /// Starts the planning poker session for the current sprint and project id 
        /// </summary>
        private void EstablishPokerSession()
        {
            ValidateViewModelNow();

            if (!this.HasErrors)
            {
                var sessonId = serviceGateway.StartNewPokerSession(sprintId.Value);
                this.ShowMetroDialog("Session started", "The planning poker session has been started");
            }
        }

        #endregion 
    }
}
