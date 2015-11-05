using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Projects.BackendProjectService;
using AnyTrack.Projects.ServiceGateways;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace AnyTrack.Projects.Views
{
    /// <summary>
    /// The view model for the product backlog
    /// </summary>
    public class ProductBacklogViewModel : ValidatedBindableBase
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
        /// The project Id
        /// </summary>
        private Guid projectId;
                
        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Product Backlog View Model
        /// </summary>
        /// <param name="regionManager">The region manager</param>
        /// <param name="serviceGateway">The project service gateway</param>
        public ProductBacklogViewModel(IRegionManager regionManager, IProjectServiceGateway serviceGateway)
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
            this.Stories = new ObservableCollection<StoryDetails>();
            this.Projects = new ObservableCollection<ProjectDetails>();
            this.Projects.AddRange(serviceGateway.GetProjectNames());
            OpenStoryViewCommand = new DelegateCommand(this.OpenStoryView, this.CanOpenStoryView);
            EditStoryCommand = new DelegateCommand<StoryDetails>(this.EditStory);
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets a given story from the backlog
        /// </summary>
        public DelegateCommand<StoryDetails> DeleteStoryFromProductBacklogCommand { get; set; }

        /// <summary>
        /// Gets or sets a given story from the backlog
        /// </summary>
        public DelegateCommand<StoryDetails> EditStoryCommand { get; set; }

        /// <summary>
        /// Gets the command used to open story view. 
        /// </summary>
        public DelegateCommand OpenStoryViewCommand { get; private set; }

        #endregion Commands

        #region Properties

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
                var result = SetProperty(ref projectId, value);
                if (result)
                {
                    Stories.Clear();
                    Stories.AddRange(serviceGateway.GetProjectStories(value));
                }
            }
        }

        /// <summary>
        /// Gets or sets the stories
        /// </summary>
        public ObservableCollection<StoryDetails> Stories { get; set; }

        /// <summary>
        /// Gets or sets the project details.
        /// </summary>
        public ObservableCollection<ProjectDetails> Projects { get; set; }

        #endregion Properties

        #region Methods
        
        /// <summary>
        /// Detects whether the story view can open.
        /// </summary>
        /// <returns>Open story view or not.</returns>
        private bool CanOpenStoryView()
        {
            return true;
        }

        /// <summary>
        /// Open story view.
        /// </summary>
        private void OpenStoryView()
        {
            regionManager.RequestNavigate(RegionNames.AppContainer, "Story");
        }

        /// <summary>
        /// Open story view.
        /// </summary>
        /// <param name="story">story object</param>
        private void EditStory(StoryDetails story)
        {
            this.ShowMetroDialog("Called EditStorySuccessfully", "SID:" + story.StoryId + ". PID:" + projectId, MessageDialogStyle.Affirmative);

            var navParams = new NavigationParameters();
            navParams.Add("projectId", projectId);
            navParams.Add("storyId", story.StoryId);
            regionManager.RequestNavigate(RegionNames.AppContainer, "Story", navParams);
        }             

        #endregion
    }
}
