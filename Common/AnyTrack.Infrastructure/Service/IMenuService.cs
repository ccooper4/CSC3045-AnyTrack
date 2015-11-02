using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure.Service.Model;

namespace AnyTrack.Infrastructure.Service
{
    /// <summary>
    /// Outlines the interface for the application's menu service.
    /// </summary>
    public interface IMenuService
    {
        #region Properties 

        /// <summary>
        /// Gets the items to be displayed on the menu.
        /// </summary>
        ObservableCollection<MenuItem> MenuItems { get; }

        #endregion 

        #region Methods 

        /// <summary>
        /// Adds an item to the menu.
        /// </summary>
        /// <param name="item">The menu item to add.</param>
        void AddMenuItem(MenuItem item);

        #endregion
    }
}
