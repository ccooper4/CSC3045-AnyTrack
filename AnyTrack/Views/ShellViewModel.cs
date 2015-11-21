using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyTrack.Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using Prism.Regions;

namespace AnyTrack.Client.Views
{
    /// <summary>
    /// The view model for the Shell.
    /// </summary>
    public class ShellViewModel : BindableBase
    {
        #region Fields 

        /// <summary>
        /// The collection of flyouts. 
        /// </summary>
        private ObservableCollection<FlyoutViewModelBase> flyouts; 

        #endregion 

        #region Properties

        /// <summary>
        /// Gets or sets the flyouts on the shell.
        /// </summary>
        public ObservableCollection<FlyoutViewModelBase> Flyouts
        {
            get
            {
                return flyouts;
            }

            set
            {
                SetProperty(ref flyouts, value);
            }
        }

        #endregion 
    }
}
