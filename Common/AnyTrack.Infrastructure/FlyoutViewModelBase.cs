using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;

namespace AnyTrack.Infrastructure
{
    /// <summary>
    /// Provides a base view model for Metro Flyouts. 
    /// </summary>
    public abstract class FlyoutViewModelBase : ValidatedBindableBase
    {
        #region Fields 

        /// <summary>
        /// A field indicating if a flyout is open. 
        /// </summary>
        private bool isOpen;

        /// <summary>
        /// The header text for a flyout. 
        /// </summary>
        private string header;

        /// <summary>
        /// The position of a flyout on the UI.
        /// </summary>
        private Position position;

        /// <summary>
        /// The theme used to display this flyout. 
        /// </summary>
        private FlyoutTheme theme;

        /// <summary>
        /// A flag indicating if this is a modal flyout. 
        /// </summary>
        private bool isModal; 

        #endregion 

        #region Constructor 

        /// <summary>
        /// Creates a new instance of the flyout base. 
        /// </summary>
        public FlyoutViewModelBase()
        {
            Position = MahApps.Metro.Controls.Position.Right;
            Theme = FlyoutTheme.Accent;
            IsModal = true;
        }

        #endregion 

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not a flyout is open at the moment.
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
        /// Gets or sets the flyout header.
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
        /// Gets or sets the position of the flyout.
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
        /// Gets or sets the theme used when displaying this flyout.
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
        /// Gets or sets a value indicating whether this flyout is a modal or not.
        /// </summary>
        public bool IsModal
        {
            get
            {
                return isModal; 
            }

            set
            {
                SetProperty(ref isModal, value);
            }
        }

        #endregion
    }
}
