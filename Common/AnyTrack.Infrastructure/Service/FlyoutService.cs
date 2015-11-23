using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AnyTrack.Infrastructure.Providers;
using AnyTrack.SharedUtilities.Extensions;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace AnyTrack.Infrastructure.Service
{
    /// <summary>
    /// Provides an implementation of the flyout service. 
    /// </summary>
    public class FlyoutService : IFlyoutService
    {
        #region Fields 

        /// <summary>
        /// The provider used to access the shell.
        /// </summary>
        private readonly WindowProvider mainWindow;

        /// <summary>
        /// The unity container.
        /// </summary>
        private readonly IUnityContainer container;

        /// <summary>
        /// The navigation service.
        /// </summary>
        private readonly IRegionNavigationService navService;

        #endregion 

        #region Constructor

        /// <summary>
        /// Constructs the flyout service with the specified dependencies.
        /// </summary>
        /// <param name="provider">The window provider.</param>
        /// <param name="container">The unity container.</param>
        /// <param name="navService">The navigation service.</param>
        public FlyoutService(WindowProvider provider, IUnityContainer container, IRegionNavigationService navService)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            if (navService == null)
            {
                throw new ArgumentNullException("navService");
            }

            this.mainWindow = provider;
            this.container = container;
            this.navService = navService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Shows the specified metro flyout on the Shell.
        /// </summary>
        /// <param name="viewName">The view to show.</param>
        /// <param name="navParams">Any paramaters to pass into the view model.</param>
        public void ShowMetroFlyout(string viewName, NavigationParameters navParams = null)
        {
            UserControl flyout = null;

            flyout = mainWindow.GetCurrentFlyouts(viewName).SingleOrDefault();

            // A flyout already exists with this name - check if it can still be used - IRegionMemberLifeTime. 
            // Also check IsNavigation Target on INavigation Aware. 
            if (flyout != null)
            {
                var iRegionMemberLiftTimeFlyout = flyout.DataContext as IRegionMemberLifetime;
                var iNavigationAwareFlyout = flyout.DataContext as INavigationAware;

                var destroyFlyout = false;

                if (iRegionMemberLiftTimeFlyout != null)
                {
                    destroyFlyout = destroyFlyout || !iRegionMemberLiftTimeFlyout.KeepAlive;
                }

                if (iNavigationAwareFlyout != null)
                {
                    destroyFlyout = destroyFlyout || !iNavigationAwareFlyout.IsNavigationTarget(new NavigationContext(navService, new Uri(viewName, UriKind.Relative), navParams));
                }

                if (destroyFlyout)
                {
                    mainWindow.DestroyExistingFlyout(viewName);
                    flyout = null;
                }
            }

            // Flyout doesn't exist yet - make it and add it. 
            if (flyout == null)
            {
                flyout = container.Resolve<object>(viewName) as UserControl;
                if (flyout == null)
                {
                    throw new ArgumentException("Could not resolve a view with the name '{0}'".Substitute(viewName));
                }

                var vm = flyout.DataContext as IFlyoutCompatibleViewModel;

                if (vm == null)
                {
                    throw new ArgumentException("The view '{0}' cannot be displayed in a flyout - the view model must inherit from FlyoutViewModelBase".Substitute(viewName));
                }

                mainWindow.AddFlyout(flyout);
            }

            // Call any navigation aware handlers. 
            var iNavigationAwareFinalFlyout = flyout.DataContext as INavigationAware;
            if (iNavigationAwareFinalFlyout != null)
            {
                var context = new NavigationContext(navService, new Uri(viewName, UriKind.Relative));
                if (navParams != null)
                {
                    foreach (var pair in navParams)
                    {
                        context.Parameters.Add(pair.Key, pair.Value);
                    }
                }

                iNavigationAwareFinalFlyout.OnNavigatedTo(context);
            }

            (flyout.DataContext as IFlyoutCompatibleViewModel).IsOpen = true; 
        }

    #endregion
    }
}
