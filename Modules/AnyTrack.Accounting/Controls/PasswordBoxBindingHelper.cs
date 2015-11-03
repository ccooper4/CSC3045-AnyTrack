using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AnyTrack.Accounting.Controls
{
    /// <summary>
    /// Provides a method of binding a password box to a view model.
    /// </summary>
    public static class PasswordBoxBindingHelper
    {
        #region Fields 

        /// <summary>
        /// The binding property that can be used to expose a password as a Dependency Property
        /// </summary>
        public static readonly DependencyProperty Binding = DependencyProperty.RegisterAttached("Binding", typeof(string), typeof(PasswordBoxBindingHelper), new PropertyMetadata(string.Empty, OnPasswordBindingChanged));

        /// <summary>
        /// A flag indicating if this helper is enabled on the password box.
        /// </summary>
        public static readonly DependencyProperty EnablePasswordBoxBinding = DependencyProperty.RegisterAttached("EnablePasswordBoxBinding", typeof(bool), typeof(PasswordBoxBindingHelper), new PropertyMetadata(false, OnEnablePasswordBoxBindingChanged));

        #endregion 

        #region Methods 

        /// <summary>
        /// Sets the value of the Binding dependency object.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetBinding(DependencyObject element, string value)
        {
            element.SetValue(Binding, value);
        }

        /// <summary>
        /// Gets the value of the dependency object.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The current value of the dependency object.</returns>
        public static string GetBinding(DependencyObject element)
        {
            return (string)element.GetValue(Binding);
        }

        /// <summary>
        /// Sets the value of the Binding dependency object.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetEnablePasswordBoxBinding(DependencyObject element, bool value)
        {
            element.SetValue(EnablePasswordBoxBinding, value);
        }

        /// <summary>
        /// Gets the value of the dependency object.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The current value of the dependency object.</returns>
        public static bool GetEnablePasswordBoxBinding(DependencyObject element)
        {
            return (bool)element.GetValue(EnablePasswordBoxBinding);
        }

        /// <summary>
        /// Handles the event that is raised when the EnablePasswordBoxBinding DP is changed. Here, we can configure the helper.
        /// </summary>
        /// <param name="d">The dependency object, in this case the password box.</param>
        /// <param name="e">The evenr args.</param>
        public static void OnEnablePasswordBoxBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox; 

            var newStatus = (bool)e.NewValue;

            if (newStatus)
            {
                passwordBox.PasswordChanged += PushPasswordToDependencyObject;
            }
            else
            {
                passwordBox.PasswordChanged -= PushPasswordToDependencyObject;
            }
        }

        /// <summary>
        /// Handles the event that is raised when the Binding DP is changed. 
        /// </summary>
        /// <param name="d">The dependency object, in this case the password box.</param>
        /// <param name="e">The evenr args.</param>
        public static void OnPasswordBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;

            if (!GetEnablePasswordBoxBinding(passwordBox))
            {
                return; 
            }

            // Need to disable the password boxes event to prevent a potential loop
            passwordBox.PasswordChanged -= PushPasswordToDependencyObject;

            passwordBox.Password = (string)e.NewValue;

            passwordBox.PasswordChanged += PushPasswordToDependencyObject;
        }

        /// <summary>
        /// Pushes the password to the helper's dependency object. 
        /// </summary>
        /// <param name="sender">The password box that raised this event.</param>
        /// <param name="e">The event args.</param>
        public static void PushPasswordToDependencyObject(object sender, RoutedEventArgs e)
        {
            var box = sender as PasswordBox;
            var dependencyObjectBox = box as DependencyObject;
            dependencyObjectBox.SetValue(Binding, box.Password);
        }

        #endregion 
    }
}
