using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Projects.BackendProjectService;
using AnyTrack.Projects.ServiceGateways;
using Prism.Commands;
using Prism.Regions;

namespace AnyTrack.Projects.Views
{
    /// <summary>
    /// View model to represent a story.
    /// </summary>
    public class StoryViewModel : ValidatedBindableBase, INavigationAware
    {
        #region Fields

        /// <summary>
        /// The region manager
        /// </summary>
        private readonly IRegionManager regionManager;

        /// <summary>
        /// The project service gateway
        /// </summary>
        private readonly IProjectServiceGateway serviceGateway;

        /// <summary>
        /// Project Name
        /// </summary>
        private string projectName;

        /// <summary>
        /// The project Id
        /// </summary>
        private Guid projectId;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Create Story View Model
        /// </summary>
        /// <param name="regionManager">The region manager</param>
        /// <param name="serviceGateway">The project service gateway</param>
        public StoryViewModel(IRegionManager regionManager, IProjectServiceGateway serviceGateway)
        {
            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }

            if (serviceGateway == null)
            {
                throw new ArgumentNullException("serviceGateway");
            }

            this.regionManager = regionManager;
            this.serviceGateway = serviceGateway;
            this.Projects = new ObservableCollection<ProjectDetails>();
            this.Projects.AddRange(serviceGateway.GetProjectNames());

            SaveStoryCommand = new DelegateCommand(this.SaveStory, this.CanAddStory);
            CancelStoryViewCommand = new DelegateCommand(this.CancelStoryView, this.CanCancel);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the project details.
        /// </summary>
        public ObservableCollection<ProjectDetails> Projects { get; set; }

        /// <summary>
        /// Gets or sets story represented by this view.
        /// </summary>
        public ServiceStory Story { get; set; }

        /// <summary>
        /// Gets or sets Summary property represented by this view.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets AsA property represented by this view.
        /// </summary>
        public string AsA { get; set; }

        /// <summary>
        /// Gets or sets IWant property represented by this view.
        /// </summary>
        public string IWant { get; set; }

        /// <summary>
        /// Gets or sets SoThat property represented by this view.
        /// </summary>
        public string SoThat { get; set; }

        /// <summary>
        /// Gets or sets Conditions of satisfaction property represented by this view.
        /// </summary>
        public string ConditionsOfSatisfaction { get; set; }

        /// <summary>
        /// Gets or sets the project name
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
        /// Gets the Save Story Command.
        /// </summary>
        public DelegateCommand SaveStoryCommand { get; private set; }

        /// <summary>
        /// Gets the cancel Command.
        /// </summary>
        public DelegateCommand CancelStoryViewCommand { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the Is Navigation Target event.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns>A true or false flag indicating if this view model can handle the navigation request.</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false; 
        }

        /// <summary>
        /// Handles the navigation from event.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// Handles the on navigated to event.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("projectId"))
            {
                var projectId = (Guid)navigationContext.Parameters["projectId"];
                this.ProjectId = projectId;
            }
        }

        /// <summary>
        /// Save a story to the database.
        /// </summary>
        private void SaveStory()
        {
            Story = new ServiceStory()
            {
                Summary = this.Summary,
                AsA = this.AsA,
                IWant = this.IWant,
                SoThat = this.SoThat,
                ConditionsOfSatisfaction = this.ConditionsOfSatisfaction,
                ProjectId = this.projectId
            };

            serviceGateway.AddStory(projectId, Story);

            var navParams = new NavigationParameters();
            navParams.Add("projectId", projectId);
            regionManager.RequestNavigate(RegionNames.MainRegion, "ProductBacklog", navParams);
        }

        /// <summary>
        /// Check if a a story can be added.
        /// </summary>
        /// <returns>If a can story can be added</returns>
        private bool CanAddStory()
        {
            return true;
        }

        /// <summary>
        /// Cancel story creation.
        /// </summary>
        private void CancelStoryView()
        {
            regionManager.RequestNavigate(RegionNames.AppContainer, "ProductBacklog");
        }

        /// <summary>
        /// Detects whether the story view / create can cancel.
        /// </summary>
        /// <returns>Cancel the view or not.</returns>
        private bool CanCancel()
        {
            return true;
        }

        #endregion
    }
}
