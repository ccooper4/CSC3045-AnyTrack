using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using Prism.Regions;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// The sprint story view model.
    /// </summary>
    public class SprintStoryViewModel : ValidatedBindableBase, INavigationAware
    {
        /// <summary>
        /// On navigated to.
        /// </summary>
        /// <param name="navigationContext"> The navigation context </param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Is navigation target.
        /// </summary>
        /// <param name="navigationContext"> The navigation context isNavigationTarget</param>
        /// <returns> The navigation context </returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// On navigated from.
        /// </summary>
        /// <param name="navigationContext"> The navigation context onNavigatedFrom</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }
    }
}
