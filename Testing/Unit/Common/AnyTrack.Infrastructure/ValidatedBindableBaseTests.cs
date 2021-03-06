﻿using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.Providers;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Threading;
using Prism.Regions;
using AnyTrack.Infrastructure.Service;

namespace Unit.Common.AnyTrack.Infrastructure.InterceptorBehaviourTests
{
    #region Supporting Types 

    public class TestViewModel : ValidatedBindableBase
    {
        private string name;

        public bool setResult = false;

        [Required]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                setResult = SetProperty(ref name, value);
            }
        }
    }

    #endregion 

    #region Context

    public class Context
    {
        public static TestViewModel vm;
        public static IRegionManager regionManager; 

        [SetUp]
        public void Setup()
        {
            regionManager = Substitute.For<IRegionManager>();
            vm = new TestViewModel();
            vm.RegionManager = regionManager;
        }
    }

    #endregion

    #region ValidatedBindableBaseTests

    public class ValidatedBindableBaseTests: Context
    {
        #region HasErrors Tests 

        [Test]
        public void GetHasErrorsForAnInvalidModel()
        {
            vm = new TestViewModel { Name = "test" };

            vm.Name = null;

            vm.HasErrors.Should().BeTrue();
        }

        [Test]
        public void GetHasErrorsForAValidModel()
        {
            vm = new TestViewModel { Name = "test" };

            vm.HasErrors.Should().BeFalse();
        }

        #endregion

        #region SetProperty<T>(ref T storage, T newValue, [CallerMemberName()]string propertyName = null) Tests 

        [Test]
        public void SetPropertyTheSame()
        {
            vm = new TestViewModel { Name = "test" };

            vm.Name = "test";

            vm.setResult.Should().BeFalse();
        }

        [Test]
        public void SetPropertyDifferent()
        {
            string propertyChanged = "";
            vm = new TestViewModel { Name = "test" };

            vm.PropertyChanged += (o, s) => propertyChanged = s.PropertyName;

            vm.Name = "testnew";

            vm.setResult.Should().BeTrue();
            propertyChanged.Should().Be("Name");
        }

        [Test]
        public void SetPropertyInvalid()
        {
            string propertyChanged = "";
            string errorProperty = "";
            vm = new TestViewModel { Name = "test" };

            vm.PropertyChanged += (o, s) => propertyChanged = s.PropertyName;
            vm.ErrorsChanged += (o, s) => errorProperty = s.PropertyName;
            vm.Name = null;

            vm.setResult.Should().BeTrue();
            propertyChanged.Should().Be("Name");
            errorProperty.Should().Be("Name");
            vm.HasErrors.Should().BeTrue();
            var errors = vm.GetErrors("Name") as IEnumerable<string>;
            errors.Count().Should().Be(1);
        }



        #endregion

        #region  GetErrors(string propertyName) Tests 

        [Test]
        public void GetErrorsForAnInvalidModel()
        {
            vm = new TestViewModel { Name = "test" };

            vm.Name = null;

            var errors = vm.GetErrors("Name") as List<string>;

            errors.Should().NotBeNull();
            errors.Count.Should().Be(1);
        }

        [Test]
        public void GetErrorsForAValidModel()
        {
            vm = new TestViewModel { Name = "test" };

            var errors = vm.GetErrors("Name") as List<string>;

            errors.Should().BeNull();
        }

        #endregion 

        #region ValidateViewModelNow() Tests 

        [Test]
        public void CallValidateViewModelNowWithNoErrors()
        {
            vm = new TestViewModel { Name = "Test"};
            vm.ValidateViewModelNow();
            vm.HasErrors.Should().BeFalse();
        }

        [Test]
        public void CallValidateViewModelNowWithErrors()
        {
            vm = new TestViewModel { Name = "" };
            vm.ValidateViewModelNow();
            vm.HasErrors.Should().BeTrue();
        }

        #endregion 

        #region ShowMetroDialog(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, Action<MessageDialogResult> callback = null) Tests

        [Test]
        public void ShowMetroDialogWithNoCallBack()
        {
            var title = "Test";
            var message = "Message";
            vm.MainWindow = Substitute.For<WindowProvider>();

            vm.MainWindow.ShowMessageAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(MessageDialogResult.Affirmative);

            vm.ShowMetroDialog(title, message);

            vm.MainWindow.Received().ShowMessageAsync(title, message);
        }

        [Test]
        public void ShowMetroDialogWithCallBack()
        {
            var wait = new ManualResetEvent(false); 

            var title = "Test";
            var message = "Message";
            var style = MessageDialogStyle.Affirmative;
            var callback = new Action<MessageDialogResult>(m =>
            {
                m.Should().Be(MessageDialogResult.Affirmative);
                wait.Set();
            });

            var window = Substitute.For<WindowProvider>();

            window.ShowMessageAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(MessageDialogResult.Affirmative);
            window.InvokeAction(Arg.Do<Action>(a => a()));
            vm.MainWindow = window;

            vm.ShowMetroDialog(title, message, style, callback);

            vm.MainWindow.Received().ShowMessageAsync(title, message, style);
            wait.WaitOne();
        }

        #endregion 

        #region ShowMetroFlyout(string viewName, NavigationParameters navParams = null) Tests 

        [Test]
        public void CallShowMetroFlyout()
        {
            var viewName = "Test";
            var navParams = new NavigationParameters();

            vm.FlyoutService = Substitute.For<IFlyoutService>();

            vm.ShowMetroFlyout(viewName, navParams);

            vm.FlyoutService.Received().ShowMetroFlyout(viewName, navParams);            
        }

        #endregion 

        #region NavigateToItem(string view, NavigationParameters navParams = null) Tests

        [Test]
        public void CallNavigateToItemWithParams()
        {
            var np = new NavigationParameters();
            vm.NavigateToItem("test", np);
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "test", np);
        }

        [Test]
        public void CallNavigateToItemWithNoParams()
        {
            vm.NavigateToItem("test");
            regionManager.Received().RequestNavigate(RegionNames.MainRegion, "test");
        }

        #endregion 
    }

    #endregion 
}
