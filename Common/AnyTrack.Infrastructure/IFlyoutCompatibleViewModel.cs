using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;

namespace AnyTrack.Infrastructure
{
    /// <summary>
    /// Provides an interface that outlines the requirements to use a view as a flyout. 
    /// </summary>
    public interface IFlyoutCompatibleViewModel
    {
        #region Properties 

        /// <summary>
        /// Gets or sets a value indicating whether or not the flyout is open. 
        /// </summary>
        bool IsOpen { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        string Header { get; set; }

        /// <summary>
        /// Gets or sets the position of a flyout.
        /// </summary>
        Position Position { get; set; }

        /// <summary>
        /// Gets or sets the flyout theme.
        /// </summary>
        FlyoutTheme Theme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this flyout is a model flyout.
        /// </summary>
        bool IsModal { get; set; }

        /// <summary>
        /// Gets or sets the visibility of the close button
        /// </summary>
        Visibility CloseButtonVisibility { get; set; }
       
        /// <summary>
        /// Gets or sets the visibility of the title
        /// </summary>
        Visibility TitleVisibility { get; set; }

        #endregion 
    }
}
