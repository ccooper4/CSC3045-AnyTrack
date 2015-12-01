using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.ServiceGateways;
using MahApps.Metro.Controls;
using Prism.Regions;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// The sprint story view model.
    /// </summary>
    public class TaskViewModel : ValidatedBindableBase, INavigationAware, IRegionMemberLifetime, IFlyoutCompatibleViewModel
    {
        #region Fields

        /// <summary>
        /// The is open field.
        /// </summary>
        private bool isOpen;

        /// <summary>
        /// The header field.
        /// </summary>
        private string header;

        /// <summary>
        /// The position field.
        /// </summary>
        private Position position;

        /// <summary>
        /// The is model field. 
        /// </summary>
        private bool isModel;

        /// <summary>
        /// The flyout theme field.
        /// </summary>
        private FlyoutTheme theme;

        #endregion

        /// <summary>
        /// Creates a new Sprint Story View Model
        /// </summary>
        /// <param name="iSprintServiceGateway">The sprint Service Gateway</param>
        public TaskViewModel(ISprintServiceGateway iSprintServiceGateway)
        {
            this.Header = null;
            this.Theme = FlyoutTheme.Accent;
            this.Position = Position.Left;
            this.IsModal = true;
        }

        #region Flyouts

        /// <summary>
        /// Gets a value indicating whether it should refresh everytime
        /// </summary>
        public bool KeepAlive
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this flyout is open.
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return isOpen;
            }

            set
            {
                SetProperty(ref isOpen, value);
            }
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public string Header
        {
            get
            {
                return header;
            }

            set
            {
                SetProperty(ref header, value);
            }
        }

        /// <summary>
        /// Gets or sets position.
        /// </summary>
        public Position Position
        {
            get
            {
                return position;
            }

            set
            {
                SetProperty(ref position, value);
            }
        }

        /// <summary>
        /// Gets or sets the flyout theme.
        /// </summary>
        public FlyoutTheme Theme
        {
            get
            {
                return theme;
            }

            set
            {
                SetProperty(ref theme, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this flyout is a model.
        /// </summary>
        public bool IsModal
        {
            get
            {
                return isModel;
            }

            set
            {
                SetProperty(ref isModel, value);
            }
        }

        #endregion

        /// <summary>
        /// On navigated to.
        /// </summary>
        /// <param name="navigationContext"> The navigation context </param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// Is navigation target.
        /// </summary>
        /// <param name="navigationContext"> The navigation context isNavigationTarget</param>
        /// <returns> The navigation context </returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        /// <summary>
        /// On navigated from.
        /// </summary>
        /// <param name="navigationContext"> The navigation context onNavigatedFrom</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
