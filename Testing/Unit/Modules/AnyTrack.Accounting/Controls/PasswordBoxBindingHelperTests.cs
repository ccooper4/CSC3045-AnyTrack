using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using System.Windows.Controls;
using AnyTrack.Accounting.Controls;
using System.Windows;

namespace Unit.Modules.AnyTrack.Accounting.Controls.PasswordBoxBindingHelperTests
{
    #region Tests 

    public class PasswordBoxBindingHelperTests
    {
        #region SetBinding(DependencyObject element, string value) Tests 

        [Test, RequiresSTA]
        public void CallSetBinding()
        {
            var control = new PasswordBox();
            var value = "test";

            PasswordBoxBindingHelper.SetBinding(control, value);

            control.GetValue(PasswordBoxBindingHelper.Binding).ToString().Should().Be(value);
        }

        #endregion 

        #region GetBinding(DependencyObject element) Tests

        [Test, RequiresSTA]
        public void CallGetBinding()
        {
            var control = new PasswordBox();
            var value = "Test";
            control.SetValue(PasswordBoxBindingHelper.Binding, value);

            var result = PasswordBoxBindingHelper.GetBinding(control).ToString();
            result.Should().Be(value);
        }

        #endregion 

        #region SetEnablePasswordBoxBinding(DependencyObject element, bool value) Tests

        [Test, RequiresSTA]
        public void SetEnablePasswordBoxBinding()
        {
            var control = new PasswordBox();

            PasswordBoxBindingHelper.SetEnablePasswordBoxBinding(control, false);

            ((bool)control.GetValue(PasswordBoxBindingHelper.EnablePasswordBoxBinding)).Should().BeFalse();
        }

        #endregion

        #region GetEnablePasswordBoxBinding(DependencyObject element) Tests

        [Test, RequiresSTA]
        public void CallGetEnablePasswordBoxBinding()
        {
            var control = new PasswordBox();
            control.SetValue(PasswordBoxBindingHelper.EnablePasswordBoxBinding, true);

            var result = (bool)PasswordBoxBindingHelper.GetEnablePasswordBoxBinding(control);
            result.Should().BeTrue();
        }

        #endregion 

        #region PushPasswordToDependencyObject(object sender, RoutedEventArgs e) Tests 

        [Test, RequiresSTA]
        public void CallPushPasswordToDependencyObject()
        {
            var control = new PasswordBox();
            control.Password = "Test";
            PasswordBoxBindingHelper.PushPasswordToDependencyObject(control, new RoutedEventArgs());
            control.GetValue(PasswordBoxBindingHelper.Binding).ToString().Should().Be("Test");
        }

        #endregion 

    }

    #endregion 
}
