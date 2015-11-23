using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.Providers;
using AnyTrack.Infrastructure.Service;
using Microsoft.Practices.Unity;
using NSubstitute;
using NUnit.Framework;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using FluentAssertions;
using MahApps.Metro.Controls;

namespace Unit.Common.AnyTrack.Infrastructure.Service.FlyoutServiceTests
{
    #region Supporting Types 

    public class FlyoutViewModel : ValidatedBindableBase, IFlyoutCompatibleViewModel, IRegionMemberLifetime, INavigationAware
    {
        private bool reuseableView = false;
        private bool isOpen;
        private bool isModal;
        private Position position;
        private FlyoutTheme theme;
        private string header;

        public NavigationParameters SentParams; 

        public FlyoutViewModel(bool reuseable)
        {
            reuseableView = reuseable;
        }

        public bool KeepAlive
        {
            get { return reuseableView; }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return reuseableView;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            SentParams = navigationContext.Parameters;
        }

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

        public MahApps.Metro.Controls.Position Position
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
    }

    public class OtherViewModel: ValidatedBindableBase
    {
    }

    public class FlyoutControl : UserControl
    {
        public FlyoutControl(bool reuseableView)
        {
            this.DataContext = new FlyoutViewModel(reuseableView);
        }

        public FlyoutViewModel ViewModel
        {
            get
            {
                return this.DataContext as FlyoutViewModel;
            }
        }

    }

    public class FlyoutControlWrongViewModel : UserControl
    {
        public FlyoutControlWrongViewModel()
        {
            this.DataContext = new OtherViewModel();
        }

    }

    #endregion 

    #region Context 
    
    public class Context
    {
        public static WindowProvider provider;
        public static IUnityContainer container;
        public static IRegionNavigationService navService;

        public static FlyoutControl control;
        public static FlyoutControlWrongViewModel wrongControl;

        public static FlyoutService service;

        [SetUp]
        public void SetUp()
        {
            control = new FlyoutControl(false);
            wrongControl = new FlyoutControlWrongViewModel();

            provider = Substitute.For<WindowProvider>();
            container = Substitute.For<IUnityContainer>();
            navService = Substitute.For<IRegionNavigationService>();

            service = new FlyoutService(provider, container, navService);
        }
    }

    #endregion 

    #region Tests 

    public class FlyoutServiceTests: Context
    {
        #region Constructor Tests 
        
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoProvider()
        {
            service = new FlyoutService(null, container, navService);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoContainer()
        {
            service = new FlyoutService(provider, null, navService);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNoNavService()
        {
            service = new FlyoutService(provider, container, null);
        }

        #endregion 

        #region ShowMetroFlyout(string viewName, NavigationParameters navParams = null) Tests 

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ShowANewFlyoutThatTheContainerCannotResolve()
        {
            var viewName = "test";
            var navParams = new NavigationParameters();
            navParams.Add("test", true);

            provider.GetCurrentFlyouts(viewName).Returns(new List<UserControl>());

            container.Resolve<object>(viewName).Returns(a => { return null; });

            service.ShowMetroFlyout(viewName, navParams);

            provider.Received().GetCurrentFlyouts(viewName);
            container.Received().Resolve<object>(viewName);
            provider.DidNotReceive().AddFlyout(control);
            control.ViewModel.SentParams.Should().BeNull();
            control.ViewModel.IsOpen.Should().BeFalse();

        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ShowANewFlyoutWithTheWrongViewModelType()
        {
            var viewName = "test";
            var navParams = new NavigationParameters();
            navParams.Add("test", true);

            provider.GetCurrentFlyouts(viewName).Returns(new List<UserControl>());

            container.Resolve<object>(viewName).Returns(wrongControl);

            service.ShowMetroFlyout(viewName, navParams);

            provider.Received().GetCurrentFlyouts(viewName);
            container.Received().Resolve<object>(viewName);
            provider.DidNotReceive().AddFlyout(wrongControl);

        }

        [Test]
        public void ShowANewFlyout()
        {
            var viewName = "test";
            var navParams = new NavigationParameters();
            navParams.Add("test", true);

            provider.GetCurrentFlyouts(viewName).Returns(new List<UserControl>());

            container.Resolve<object>(viewName).Returns(control);

            service.ShowMetroFlyout(viewName, navParams);

            provider.Received().GetCurrentFlyouts(viewName);
            container.Received().Resolve<object>(viewName);
            provider.Received().AddFlyout(control);
            control.ViewModel.SentParams.ContainsKey("test").Should().BeTrue();
            control.ViewModel.IsOpen.Should().BeTrue();

        }

        [Test]
        public void ShowAnExistingFlyoutThatIsDestroyed()
        {
            control = new FlyoutControl(false);
            var viewName = "test";
            var navParams = new NavigationParameters();
            navParams.Add("test", true);

            provider.GetCurrentFlyouts(viewName).Returns(new List<UserControl>() { control});

            container.Resolve<object>(viewName).Returns(control);

            service.ShowMetroFlyout(viewName, navParams);

            provider.Received().GetCurrentFlyouts(viewName);
            provider.Received().DestroyExistingFlyout(viewName);
            container.Received().Resolve<object>(viewName);
            provider.Received().AddFlyout(control);
            control.ViewModel.SentParams.ContainsKey("test").Should().BeTrue();
            control.ViewModel.IsOpen.Should().BeTrue();
        }

        [Test]
        public void ShowAnExistingFlyoutThatIsReUsed()
        {
            control = new FlyoutControl(true);
            var viewName = "test";
            var navParams = new NavigationParameters();
            navParams.Add("test", true);

            provider.GetCurrentFlyouts(viewName).Returns(new List<UserControl>() { control });

            service.ShowMetroFlyout(viewName, navParams);

            provider.Received().GetCurrentFlyouts(viewName);
            provider.DidNotReceive().DestroyExistingFlyout(viewName);
            container.DidNotReceive().Resolve<object>(viewName);
            provider.DidNotReceive().AddFlyout(control);
            control.ViewModel.SentParams.ContainsKey("test").Should().BeTrue();
            control.ViewModel.IsOpen.Should().BeTrue();
        }

        #endregion
    }

    #endregion 
}
