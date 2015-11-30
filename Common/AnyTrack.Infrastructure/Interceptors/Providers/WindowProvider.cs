using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using AnyTrack.SharedUtilities.Extensions;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace AnyTrack.Infrastructure.Providers
{
    /// <summary>
    /// Provides an abstracted method of accessing the WPF Main Window. 
    /// </summary>
    public class WindowProvider
    {
        #region Fields 

        /// <summary>
        /// The application's main window.
        /// </summary>
        private MetroWindow window;

        #endregion 

        #region Constructor 

        /// <summary>
        /// Constructs the Window Provider to provide a wrapper to the current window.
        /// </summary>
        public WindowProvider()
        {
            if (Application.Current != null)
            {
                window = Application.Current.MainWindow as MetroWindow;
            }
        }

        #endregion 

        #region Methods

        /// <summary>
        /// Provides a wrapper around the Metro ShowMessageAsync method.
        /// </summary>
        /// <param name="title">The message box title.</param>
        /// <param name="message">The message box message.</param>
        /// <param name="style">The style.</param>
        /// <param name="settings">Any settings.</param>
        /// <returns>The Async task that can be used to get the dialog result.</returns>
        public virtual Task<MessageDialogResult> ShowMessageAsync(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null)
        {
            return window.ShowMessageAsync(title, message, style, settings);
        }

        /// <summary>
        /// Invokes the specified action using the wrapped dispatcher.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        public virtual void InvokeAction(Action action)
        {
            window.Dispatcher.Invoke(action);
        }

        /// <summary>
        /// Returns a list of flyouts currently registered with the shell.
        /// </summary>
        /// <param name="flyoutName">The flyout name.</param>
        /// <returns>A list of flyouts.</returns>
        public virtual List<UserControl> GetCurrentFlyouts(string flyoutName)
        {
            var currentFlyouts = window.Flyouts.Items.Cast<UserControl>();

            if (flyoutName.IsNotEmpty())
            {
                currentFlyouts = currentFlyouts.Where(f => f.GetType().Name == flyoutName);
            }

            return currentFlyouts.ToList();
        }

        /// <summary>
        /// Removes the given flyout from the shell's flyout list.
        /// </summary>
        /// <param name="flyoutName">The flyout name.</param>
        public virtual void DestroyExistingFlyout(string flyoutName)
        {
            var flyout = window.Flyouts.Items.Cast<UserControl>().SingleOrDefault(f => f.GetType().Name == flyoutName);
            if (flyout != null)
            {
                window.Flyouts.Items.Remove(flyout);
            }
        }

        /// <summary>
        /// Adds the given flyout to the shell.
        /// </summary>
        /// <param name="flyout">The flyout to add to the shell.</param>
        public virtual void AddFlyout(UserControl flyout)
        {
            window.Flyouts.Items.Add(flyout);
        }

        #endregion 
    }
}
