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
    public class StoryViewModel : ValidatedBindableBase
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

            // SaveStoryCommand = new DelegateCommand(this.SaveStory, this.CanAddStory);
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
        /// Gets the Save Story Command.
        /// </summary>
        public DelegateCommand SaveStoryCommand { get; private set; }

        #endregion Properties
    }
}
