using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Regions;

namespace AnyTrack.Infrastructure.Service
{
    /// <summary>
    /// Outlines the interface for the flyout servie.
    /// </summary>
    public interface IFlyoutService
    {
        /// <summary>
        /// Shows the specified view in a metro flyout.
        /// </summary>
        /// <param name="viewName">The view name.</param>
        /// <param name="navParams">Any navigation paramaters.</param>
        void ShowMetroFlyout(string viewName, NavigationParameters navParams = null); 
    }
}
