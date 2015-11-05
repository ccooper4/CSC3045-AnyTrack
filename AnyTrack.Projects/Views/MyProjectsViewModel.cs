using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Projects.BackendProjectService;
using AnyTrack.Projects.ServiceGateways;
using AnyTrack.SharedUtilities.Extensions;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace AnyTrack.Projects.Views
{
    /// <summary>
    /// The view model for the project
    /// </summary>
    public class MyProjectsViewModel : ValidatedBindableBase
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

        private ObservableCollection<ServiceProject> tiles;

        public ObservableCollection<ServiceProject> Tiles { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Create Project View Model
        /// </summary>
        /// <param name="regionManager">The region manager</param>
        /// <param name="serviceGateway">The project service gateway</param>
        public MyProjectsViewModel(IRegionManager regionManager, IProjectServiceGateway serviceGateway)
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

            tiles = new List<ServiceProject>()
            {
                new ServiceProject()
                {
                    Name = "TestProject1",
                    Description = "Desc1"
                },
                new ServiceProject()
                {
                    Name = "TestProject2",
                    Description = "Desc2"
                }
            };
        }

        #endregion


    }
}
