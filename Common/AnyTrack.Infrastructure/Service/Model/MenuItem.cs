using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyTrack.Infrastructure.Service.Model
{
    /// <summary>
    /// Represents a single item on the Shell's menu.
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the tile color.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the navigation view name.
        /// </summary>
        public string NavigationViewName { get; set; }

        /// <summary>
        /// Gets or sets the order in which the items will be displayed on the UI.
        /// </summary>
        public int Order { get; set; }
    }
}
