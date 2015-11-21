using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Projects.BackendProjectService;
using AnyTrack.Projects.ServiceGateways;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Regions;

namespace AnyTrack.Projects.Views
{
    /// <summary>
    /// View model to represent a story.
    /// </summary>
    public class StoryViewModel : FlyoutViewModelBase, INavigationAware, IRegionMemberLifetime
    {
        #region Fields

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

        /// <summary>
        /// The project Id
        /// </summary>
        private Guid storyId;

        /// <summary>
        /// summary field
        /// </summary>
        private string summary;

        /// <summary>
        /// as a
        /// </summary>
        private string asA;

        /// <summary>
        /// I want
        /// </summary>
        private string iWant;

        /// <summary>
        /// so that
        /// </summary>
        private string soThat;

        /// <summary>
        /// conditionsOfSatisfaction var
        /// </summary>
        private string conditionsOfSatisfaction;        

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Create Story View Model
        /// </summary>
        /// <param name="serviceGateway">The project service gateway</param>
        public StoryViewModel(IProjectServiceGateway serviceGateway)
        {
            if (serviceGateway == null)
            {
                throw new ArgumentNullException("serviceGateway");
            }

            this.serviceGateway = serviceGateway;
            this.Projects = new ObservableCollection<ProjectDetails>();
            this.Header = "Story";
            
            SaveUpdateStoryCommand = new DelegateCommand(this.SaveUpdateStory);           
        }

        /// <summary>
        /// Gets a value indicating whether it should refresh everytime
        /// </summary>
        public bool KeepAlive
        {
            get { return false; }
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
        [Required]
        public string Summary
        {
            get { return summary; }
            set { SetProperty(ref summary, value); }
        }

        /// <summary>
        /// Gets or sets AsA property represented by this view.
        /// </summary>
        [Required]
        public string AsA
        {
            get { return asA; }
            set { SetProperty(ref asA, value); }
        }

        /// <summary>
        /// Gets or sets IWant property represented by this view.
        /// </summary>
        [Required]
        public string IWant
        {
            get { return iWant; }
            set { SetProperty(ref iWant, value); }
        }

        /// <summary>
        /// Gets or sets SoThat property represented by this view.
        /// </summary>
        [Required]
        public string SoThat
        {
            get { return soThat; }
            set { SetProperty(ref soThat, value); }
        }

        /// <summary>
        /// Gets or sets Conditions of satisfaction property represented by this view.
        /// </summary>
        [Required]
        public string ConditionsOfSatisfaction
        {
            get { return conditionsOfSatisfaction; }
            set { SetProperty(ref conditionsOfSatisfaction, value); }
        }

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
        public DelegateCommand SaveUpdateStoryCommand { get; private set; }

        #endregion Properties

        #region Methods
        
        /// <summary>
        /// handles navigated to
        /// </summary>
        /// <param name="navigationContext">navigation Context</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.Projects.Clear();
            this.Projects.AddRange(serviceGateway.GetProjectNames(false, true, false));

            if (navigationContext.Parameters.ContainsKey("projectId"))
            {
                var projectId = (Guid)navigationContext.Parameters["projectId"];
                this.ProjectId = projectId;

                if (navigationContext.Parameters.ContainsKey("storyId"))
                {
                    var storyId = (Guid)navigationContext.Parameters["storyId"];
                    this.storyId = storyId;

                    var existingDetails = serviceGateway.GetProjectStory(projectId, storyId);

                    this.Summary = existingDetails.Summary;
                    this.AsA = existingDetails.AsA;
                    this.IWant = existingDetails.IWant;
                    this.SoThat = existingDetails.SoThat;
                    this.ConditionsOfSatisfaction = existingDetails.ConditionsOfSatisfaction;

                    this.storyId = storyId;
                }
            }
        }

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
        /// Save a story to the database.
        /// </summary>
        private void SaveUpdateStory()
        {
            this.ValidateViewModelNow();
            if (!HasErrors)
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

                serviceGateway.SaveUpdateStory(projectId, storyId, Story);
                this.ShowMetroDialog("Story has been saved!", "Success", MessageDialogStyle.Affirmative);

                var navParams = new NavigationParameters();
                navParams.Add("projectId", projectId);
                NavigateToItem("ProductBacklog", navParams);
                this.IsOpen = false;
            }
        }

        #endregion
    }
}
