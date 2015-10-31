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
    /// Provides a menu service for managing menu items on the Shell.
    /// </summary>
    public class MenuService : IMenuService
    {
        #region Constructor 

        /// <summary>
        /// Constructs a new Menu Service.
        /// </summary>
        public MenuService()
        {
            MenuItems = new ObservableCollection<MenuItem>();
        }

        #endregion 

        #region Properties

        /// <summary>
        /// Gets the items to be displayed on the menu.
        /// </summary>
        public ObservableCollection<MenuItem> MenuItems { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an item to the menu.
        /// </summary>
        /// <param name="item">The menu item to add to the collection.</param>
        public void AddMenuItem(MenuItem item)
        {
            MenuItems.Add(item);
        }

        #endregion
    }
}
