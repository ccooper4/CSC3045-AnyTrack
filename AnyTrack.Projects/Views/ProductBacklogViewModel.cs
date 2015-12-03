using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.ServiceGateways;
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
    public class ProductBacklogViewModel : ValidatedBindableBase, IRegionMemberLifetime, INavigationAware
    {
        #region Fields

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
        /// <param name="serviceGateway">The project service gateway</param>
        public ProductBacklogViewModel(IProjectServiceGateway serviceGateway)
        {
            if (serviceGateway == null)
            {
                throw new ArgumentNullException("serviceGateway");
            }

            this.serviceGateway = serviceGateway;

            this.Stories = new ObservableCollection<ServiceStorySummary>();
            this.Projects = new ObservableCollection<ServiceProjectSummary>();

            var results = serviceGateway.GetProjectNames(false, true, false);
            this.Projects.AddRange(results);

            OpenStoryViewCommand = new DelegateCommand(this.OpenStoryView);
            DeleteStoryCommand = new DelegateCommand<string>(DeleteStory);
            EditStoryCommand = new DelegateCommand<ServiceStorySummary>(this.EditStory);
        }

        #endregion

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
        public ObservableCollection<ServiceStorySummary> Stories { get; set; }

        /// <summary>
        /// Gets or sets the project details.
        /// </summary>
        public ObservableCollection<ServiceProjectSummary> Projects { get; set; }

        /// <summary>
        /// Gets a value indicating whether it should refresh everytime
        /// </summary>
        public bool KeepAlive
        {
            get { return false; }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets a given story to delete from the backlog
        /// </summary>
        public DelegateCommand<string> DeleteStoryCommand { get; set; }

        /// <summary>
        /// Gets or sets a given story from the backlog
        /// </summary>
        public DelegateCommand<ServiceStorySummary> EditStoryCommand { get; set; }

        /// <summary>
        /// Gets the command used to open story view. 
        /// </summary>
        public DelegateCommand OpenStoryViewCommand { get; private set; }

        #endregion Commands

        #region Methods

        /// <summary>
        /// Handles the Is Navigation target event. 
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns>A true or false value indicating if this viewmodel can handle the navigation request.</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        /// <summary>
        /// Handles the on navigated from event. 
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// Handles the navigated to.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("projectId"))
            {
                ProjectId = (Guid)navigationContext.Parameters["projectId"];
            }
        }

        /// <summary>
        /// Deleting a story from the product backlog view
        /// </summary>
        /// <param name="storyId">The story id.</param>
        public void DeleteStory(string storyId)
        {
            var guid = Guid.Parse(storyId);
            var callbackAction = new Action<MessageDialogResult>(mdr =>
            {
                if (mdr == MessageDialogResult.Affirmative)
                {
                    serviceGateway.DeleteStoryFromProductBacklog(projectId, guid);

                    Stories.Clear();
                    Stories.AddRange(serviceGateway.GetProjectStories(projectId));
                }
            });

            this.ShowMetroDialog("Delete story - confirmation", "Are you sure that you want to delete this story from the backlog?", MessageDialogStyle.AffirmativeAndNegative, callbackAction); 
        }

        /// <summary>
        /// Open story view.
        /// </summary>
        private void OpenStoryView()
        {
            var navParams = new NavigationParameters();
            navParams.Add("projectId", projectId);
            this.ShowMetroFlyout("Story", navParams);
        }

        /// <summary>
        /// Open story view.
        /// </summary>
        /// <param name="story">story object</param>
        private void EditStory(ServiceStorySummary story)
        {
            var navParams = new NavigationParameters();
            navParams.Add("projectId", projectId);
            navParams.Add("storyId", story.StoryId);
            this.ShowMetroFlyout("Story", navParams);
        }

        #endregion
    }
}
