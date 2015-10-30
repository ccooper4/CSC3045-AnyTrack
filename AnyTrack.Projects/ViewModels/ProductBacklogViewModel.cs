using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using AnyTrack.Infrastructure;
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
            this.project = BuildMockProject();
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
        /// Gets or sets the story name
        /// </summary>
        public string StoryName
        {
            get { return storyName; }
            set { SetProperty(ref storyName, value); }
        }
        #endregion Properties

        #region Methods

        /// <summary>
        /// Method to construct mock project for purpose of Backlog creation
        /// </summary>
        /// <returns>retruns a mock project</returns>
        public Project BuildMockProject()
        {
            Project project = new Project
            {
                Name = "Project Name",
                Description = "Description",
                ProjectManager = new NewUser
                {
                    EmailAddress = "rmoorhead03@qub.ac.uk",
                    FirstName = "Richard",
                    LastName = "Moorhead",
                    Password = "abc123",
                    Developer = true
                },
                StartedOn = new DateTime(2015, 10, 12)
            };

            return project;
        }
        #endregion Methods
    }
}
