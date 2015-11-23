using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.Extensions;
using AnyTrack.Infrastructure.Service;
using AnyTrack.Infrastructure.Service.Model;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace AnyTrack.Client.Views
{
    /// <summary>
    /// The view model for the Main App area..
    /// </summary>
    public class MainAppAreaViewModel : ValidatedBindableBase
    {
        #region Fields 

        /// <summary>
        /// The menu service.
        /// </summary>
        private readonly IMenuService menuService;

        /// <summary>
        /// The region manager.
        /// </summary>
        private readonly IRegionManager regionManager;

        #endregion 

        #region Constructor

        /// <summary>
        /// Constructs a new view model for the application's main area.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="regionManager">The region manager.</param>
        public MainAppAreaViewModel(IMenuService service, IRegionManager regionManager)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }

            this.menuService = service;
            this.regionManager = regionManager;

            LogoutUserCommand = new DelegateCommand(this.Logout);
            NavigateCommand = new DelegateCommand<string>(NavigateToItemFromMenu);
        }

        #endregion

        #region Properties 

        /// <summary>
        /// Gets the menu items to be displayed.
        /// </summary>
        public ObservableCollection<MenuItem> MenuItems
        {
            get
            {
                return menuService.MenuItems;
            }
        }

        /// <summary>
        /// Gets the user's full name.
        /// </summary>
        public string FullName
        {
            get
            {
                return Thread.CurrentPrincipal.GetFullName();
            }
        }

        /// <summary>
        /// Gets the navigate command.
        /// </summary>
        public DelegateCommand<string> NavigateCommand { get; private set; }

        /// <summary>
        /// Gets the command used to logout a user. 
        /// </summary>
        public DelegateCommand LogoutUserCommand { get; private set; }

        #endregion 

        #region Methods 

        /// <summary>
        /// Navigates to the region specified in the menu item.
        /// </summary>
        /// <param name="view">The view to navigate to.</param>
        private void NavigateToItemFromMenu(string view)
        {
            regionManager.RequestNavigate(RegionNames.MainRegion, view);
        }

        /// <summary>
        /// Perform login.
        /// </summary>
        private void Logout()
        {
            Thread.CurrentPrincipal = null;
            RegionManager.RequestNavigate(RegionNames.AppContainer, "Login");
        }

        #endregion 
    }
}
