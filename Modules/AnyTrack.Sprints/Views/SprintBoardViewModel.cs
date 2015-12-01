using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.BackendSprintService;
using Prism.Commands;
using Prism.Regions;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// The sprint board view model.
    /// </summary>
    public class SprintBoardViewModel : ValidatedBindableBase, INavigationAware
    {
        /// <summary>
        /// The Sprint Service Gateway.
        /// </summary>
        private ISprintService serviceGateway;

        /// <summary>
        /// The sprint id
        /// </summary>
        private Guid sprintId;

        #region Constructor

        /// <summary>
        /// Creates a new Sprint Manager.
        /// </summary>
        /// <param name="serviceGateway">The Service Gateway</param>
        public SprintBoardViewModel(ISprintService serviceGateway)
        {
            if (serviceGateway == null)
            {
                throw new ArgumentNullException("serviceGateway");
            }

            this.serviceGateway = serviceGateway;

            OpenSprintStoryViewCommand = new DelegateCommand(this.OpenSprintStoryView);
            EditTaskHoursCommand = new DelegateCommand(this.GoToUpdateTaskHours);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the sprint id
        /// </summary>
        public Guid SprintId
        {
            get
            {
                return sprintId;
            }

            set
            {
                sprintId = value;
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets the command used to open a sprint story view. 
        /// </summary>
        public DelegateCommand OpenSprintStoryViewCommand { get; private set; }

        /// <summary>
        /// Gets or sets the command that can be used to add a sprint.
        /// </summary>
        public DelegateCommand EditTaskHoursCommand { get; set; }

        #endregion

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
        }

        /// <summary>
        /// Navigates to update task hours
        /// </summary>
        private void GoToUpdateTaskHours()
        {
            var navParams = new NavigationParameters();
            sprintId = new Guid("878b0202-24b0-4f9f-a147-620879a6e760");
            navParams.Add("sprintId", sprintId);
            NavigateToItem("UpdateTaskHours", navParams);
        }

        /// <summary>
        /// Open story view.
        /// </summary>
        private void OpenSprintStoryView()
        {
            var navParams = new NavigationParameters();

            ////navParams.Add("projectId", projectId);
            this.ShowMetroFlyout("SprintStory", navParams);
        }

        #endregion
    }
}
