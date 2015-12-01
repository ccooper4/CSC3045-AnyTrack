using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.ServiceGateways;
using MahApps.Metro.Controls;
using Prism.Commands;
using Prism.Regions;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// The sprint story view model.
    /// </summary>
    public class SprintStoryViewModel : ValidatedBindableBase, INavigationAware, IRegionMemberLifetime, IFlyoutCompatibleViewModel
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

        /// <summary>
        /// The close button visibility field.
        /// </summary>
        private Visibility closeButtonVisibility;

        /// <summary>
        /// The close button visibility field.
        /// </summary>
        private Visibility titleVisibility;

        #endregion

        /// <summary>
        /// Creates a new Sprint Story View Model
        /// </summary>
        /// <param name="iSprintServiceGateway">The sprint Service Gateway</param>
        public SprintStoryViewModel(ISprintServiceGateway iSprintServiceGateway)
        {
            this.Header = null;
            this.Theme = FlyoutTheme.Accent;
            this.Position = Position.Right;
            this.IsModal = true;
            this.CloseButtonVisibility = Visibility.Hidden;
            this.TitleVisibility = Visibility.Hidden;

            OpenTaskViewCommand = new DelegateCommand(this.OpenTaskView);
        }

        /// <summary>
        /// Gets the command used to open a sprint story view. 
        /// </summary>
        public DelegateCommand OpenTaskViewCommand { get; private set; }

        #region Flyouts

        /// <summary>
        /// Gets or sets the close button visibility
        /// </summary>
        public Visibility CloseButtonVisibility
        {
            get
            {
                return closeButtonVisibility;
            }

            set
            {
                SetProperty(ref closeButtonVisibility, value);
            }
        }

        /// <summary>
        /// Gets or sets the title visibility
        /// </summary>
        public Visibility TitleVisibility
        {
            get
            {
                return titleVisibility;
            }

            set
            {
                SetProperty(ref titleVisibility, value);
            }
        }

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

        /// <summary>
        /// Open story view.
        /// </summary>
        private void OpenTaskView()
        {
            var navParams = new NavigationParameters();

            ////navParams.Add("projectId", projectId);
            this.ShowMetroFlyout("Task", navParams);
        }
    }
}
