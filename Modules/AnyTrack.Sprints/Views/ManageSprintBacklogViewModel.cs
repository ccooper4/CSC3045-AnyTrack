﻿// <auto-generated />
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.ServiceGateways;
using Prism.Commands;
using Prism.Regions;
using ServiceSprintStory = AnyTrack.Infrastructure.BackendSprintService.ServiceSprintStory;
using SprintServiceStory = AnyTrack.Infrastructure.BackendSprintService.ServiceStory;


namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// The view model for the Update Task Hours ViewModel
    /// </summary>
    public class ManageSprintBacklogViewModel : ValidatedBindableBase, INavigationAware
    {
        #region Fields
        private ISprintServiceGateway sprintService;
        private IProjectServiceGateway projectService;
        private ServiceSprintStory selectedSprint;
        private ServiceStorySummary selectedProductStory;
        private int selectedSprintIndex;
        private int selectedProductStoryIndex;
        private ServiceProject project;
        private ObservableCollection<ServiceStorySummary> productBacklog;
        private ObservableCollection<ServiceSprintStory> sprintBacklog;
        private string summary;
        private Guid projectId;
        private Guid sprintId;
        private DateTime sprintStartDate;
        private bool sprintActive;
        #endregion

        #region Properties
        public String Summary
        {
            get { return summary; }
            set { summary = value; }
        }
        /// <summary>
        /// Gets or sets the add to sprint command
        /// </summary>
        public DelegateCommand AddToSprintCommand { get; set; }

        public DelegateCommand RemoveFromSprintCommand { get; set; }

        public DelegateCommand AddAllToSprintCommand { get; set; }

        public DelegateCommand RemoveAllFromSprintCommand { get; set; }

        public DelegateCommand SaveCommand { get; set; }

        /// <summary>
        /// Gets or sets the stories
        /// </summary>
        public ObservableCollection<ServiceStorySummary> ProductBacklog
        {
            get { return productBacklog; }
            set
            {
                SetProperty(ref productBacklog, value);
            }
        }
        public ObservableCollection<ServiceSprintStory> SprintBacklog
        {
            get { return sprintBacklog; }
            set
            {
                SetProperty(ref sprintBacklog, value);
            }
        }

        public ServiceSprintStory SelectedSprint
        {
            get { return selectedSprint; }
            set
            {
                SetProperty(ref selectedSprint, value);
            }
        }

        public ServiceStorySummary SelectedProductStory
        {
            get { return selectedProductStory; }
            set
            {
                SetProperty(ref selectedProductStory, value);
            }
        }

        public int SelectedSprintIndex
        {
            get { return selectedSprintIndex; }
            set
            {
                if (selectedSprintIndex != value)
                {
                    SetProperty(ref selectedSprintIndex, value);
                    SelectedProductStoryIndex = -1;
                }
                SetProperty(ref selectedSprintIndex, value);
            }
        }

        public int SelectedProductStoryIndex
        {
            get { return selectedProductStoryIndex; }
            set
            {
                if (selectedProductStoryIndex != value)
                {
                    SetProperty(ref selectedProductStoryIndex, value);
                    SelectedSprintIndex = -1;
                }
                SetProperty(ref selectedProductStoryIndex, value);
            }
        }

        public bool SprintActive
        {
            get { return !sprintActive; }
            set { sprintActive = !value; }
        }

        #endregion

        #region Constructor

        public ManageSprintBacklogViewModel(ISprintServiceGateway sprintGateway, IProjectServiceGateway projectGateway)
        {
            projectService = projectGateway;
            if (projectService == null)
            {
                throw new ArgumentNullException("projectService");
            }

            sprintService = sprintGateway;
            if (sprintService == null)
            {
                throw new ArgumentNullException("sprintService");
            }

            AddToSprintCommand = new DelegateCommand(AddToSprint);
            RemoveFromSprintCommand = new DelegateCommand(RemoveFromSprint);
            AddAllToSprintCommand = new DelegateCommand(AddAllToSprint);
            RemoveAllFromSprintCommand = new DelegateCommand(RemoveAllFromSprint);
            SaveCommand = new DelegateCommand(Save);

            SelectedProductStoryIndex = -1;
            SelectedSprintIndex = -1;
        }

        #endregion


        #region Methods
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("ProjectId"))
            {
                projectId = (Guid)navigationContext.Parameters["ProjectId"];
                ProductBacklog = new ObservableCollection<ServiceStorySummary>(projectService.GetProjectStories(projectId));
            }
            if (navigationContext.Parameters.ContainsKey("SprintId"))
            {
                sprintId = (Guid)navigationContext.Parameters["SprintId"];
                SprintBacklog = new ObservableCollection<ServiceSprintStory>(sprintService.GetSprintStories(sprintId));
                PreventDuplicateStories();

                sprintStartDate = sprintService.GetSprint(sprintId).StartDate;
                SprintActive = sprintStartDate <= DateTime.Today;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void AddToSprint()
        {
            if (!SprintActive)
            {
                if (SelectedProductStory != null)
                {
                    SprintBacklog.Add(MapProductStoryToSprintStory(SelectedProductStory));
                    ProductBacklog.Remove(SelectedProductStory);
                }
            }
            else
            {
                ShowMetroDialog("Sprint is Active", "You cannot move stories during an active sprint");
            }
        }

        private void AddAllToSprint()
        {
            if (!SprintActive)
            {
                foreach (var story in ProductBacklog)
                {
                    SprintBacklog.Add(MapProductStoryToSprintStory(story));
                }
                ProductBacklog.Clear();
            }
            else
            {
                ShowMetroDialog("Sprint is Active", "You cannot move stories during an active sprint");
            }
        }

        private void RemoveFromSprint()
        {
            if (!SprintActive)
            {
                if (SelectedSprint != null)
                {
                    ProductBacklog.Add(MapSprintStoryToProductStory(SelectedSprint));
                    SprintBacklog.Remove(SelectedSprint);
                }
            }
            else
            {
                ShowMetroDialog("Sprint is Active", "You cannot move stories during an active sprint");
            }
        }

        private void RemoveAllFromSprint()
        {
            if (!SprintActive)
            {
                foreach (var story in SprintBacklog)
                {
                    ProductBacklog.Add(MapSprintStoryToProductStory(story));
                }
                SprintBacklog.Clear();
            }
            else
            {
                ShowMetroDialog("Sprint is Active", "You cannot move stories during an active sprint");
            }
        }

        private void PreventDuplicateStories()
        {
            foreach (var story in SprintBacklog)
            {
                if (story.Story.InSprint)
                {                     
                    ProductBacklog.Remove(ProductBacklog.Where(i => i.StoryId == story.Story.StoryId).Single());
                }
            }
        }

        private void Save()
        {
            sprintService.ManageSprintBacklog(projectId, sprintId, new List<ServiceSprintStory>(sprintBacklog));
        }

        private ServiceStorySummary MapSprintStoryToProductStory(ServiceSprintStory sprintStory)
        {
            ServiceStorySummary productStory = new ServiceStorySummary();
            productStory.Summary = sprintStory.Story.Summary;
            productStory.StoryId = sprintStory.Story.StoryId;
            productStory.InSprint = sprintStory.Story.InSprint;
            return productStory;
        }

        private ServiceSprintStory MapProductStoryToSprintStory(ServiceStorySummary productStory)
        {
            ServiceSprintStory sprintStory = new ServiceSprintStory();
            sprintStory.Story = new SprintServiceStory();
            sprintStory.Story.Summary = productStory.Summary;
            sprintStory.Story.StoryId = productStory.StoryId;
            sprintStory.Story.InSprint = (bool)productStory.InSprint;
            sprintStory.Status = ServiceSprintStoryStatus.NotStarted;
            return sprintStory;
        }
        #endregion
    }
}