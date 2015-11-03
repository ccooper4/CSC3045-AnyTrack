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
        /// Project Name
        /// </summary>
        [Required]
        private string projectName;

        /// <summary>
        /// Story Name
        /// </summary>
        [Required]
        private string storyName;

        /// <summary>
        /// The project
        /// </summary>
        [Required]
        private Project project;

        /// <summary>
        /// The project Id
        /// </summary>
        private string projectId;

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
            this.project = new Project { Stories = new ObservableCollection<Story>() };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the project name
        /// </summary>
        public string ProjectName
        {
            get { return projectName; }
            set { SetProperty(ref projectName, value); }
        }

        /// <summary>
        /// Gets or sets the project name
        /// </summary>
        public string ProjectId
        {
            get { return projectId; }
            set { SetProperty(ref projectId, value); }
        }

        /// <summary>
        /// Gets or sets the story title
        /// </summary>
        public string StoryTitle
        {
            get { return storyName; }
            set { SetProperty(ref storyName, value); }
        }

        /// <summary>
        /// Gets or sets the stories
        /// </summary>
        public ObservableCollection<Story> Stories
        {
            get
            {
                return project.Stories;
            }

            set
            {
                if (project.Stories != value)
                {
                    project.Stories = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected item
        /// </summary>
        public Project Project
        {
            get
            {
                return project;
            }

            set
            {
                project = value;
            }
        }

        /// <summary>
        /// Gets the projects
        /// </summary>
        public List<Project> Projects
        {
            get
            {
                return serviceGateway.GetProjects();
            }
        }

        #endregion 
    }
}
