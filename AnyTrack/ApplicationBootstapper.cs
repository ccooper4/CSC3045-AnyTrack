﻿using System;
using System.Globalization;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using AnyTrack.Accounting;
using AnyTrack.Client.Views;
using AnyTrack.Infrastructure.BackendAccountService;
using AnyTrack.Infrastructure.BackendProjectService;
using AnyTrack.Infrastructure.BackendSprintService;
using AnyTrack.Infrastructure.Providers;
using AnyTrack.Infrastructure.Service;
using AnyTrack.Infrastructure.ServiceGateways;
using AnyTrack.PlanningPoker;
using AnyTrack.Projects;
using AnyTrack.Sprints;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;

namespace AnyTrack.Client
{
    /// <summary>
    /// Bootstraps the application using the Unity Container
    /// </summary>
    public class ApplicationBootstrapper : UnityBootstrapper
    {
        #region Methods 

        /// <summary>
        /// Creates the application's shell.
        /// </summary>
        /// <returns>The shell as a dependency object.</returns>
        protected override DependencyObject CreateShell()
        {
            return Container.TryResolve<Shell>();
        }

        /// <summary>
        /// Shows the shell and sets it as the default window.
        /// </summary>
        protected override void InitializeShell()
        {
            base.InitializeShell();

            App.Current.MainWindow = (Window)this.Shell;
            App.Current.MainWindow.Show();
        }

        /// <summary>
        /// Configures the application's module catalog.
        /// </summary>
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            var moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(AccountingModule));
            moduleCatalog.AddModule(typeof(ProjectModule));
            moduleCatalog.AddModule(typeof(SprintModule));
            moduleCatalog.AddModule(typeof(PlanningPokerModule));
        }

        /// <summary>
        /// Configures the application's ability to automatically locate a view's viewModel.
        /// </summary>
        protected override void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewName = viewType.FullName;
                var viewAssemblyName = viewType.Assembly.FullName;
                var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}ViewModel, {1}", viewName, viewAssemblyName);
                return Type.GetType(viewModelName);
            });

            ViewModelLocationProvider.SetDefaultViewModelFactory((type) =>
            {
                return this.Container.Resolve(type);
            });
        }

        /// <summary>
        /// Configures the application's container.
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            // AnyTrack.Infrastrucutre Service References 
            this.Container.RegisterType<IProjectService, ProjectServiceClient>(new InjectionConstructor());
            this.Container.RegisterType<ISprintService, SprintServiceClient>(new InjectionConstructor());
            this.Container.RegisterType<IAccountService, AccountServiceClient>(new InjectionConstructor());

            // AnyTrack.Infrastructure.Services 
            this.Container.RegisterType<IMenuService, MenuService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IFlyoutService, FlyoutService>();

            // AnyTrack.Infrastructure.ServiceGateways
            this.Container.RegisterType<IProjectServiceGateway, ProjectServiceGateway>();
            this.Container.RegisterType<ISprintServiceGateway, SprintServiceGateway>();
            this.Container.RegisterType<IAccountServiceGateway, AccountServiceGateway>();

            // AnyTrack.Views
            this.Container.RegisterType<object, MainAppArea>("MainAppArea");

            // IPrincipal default. 
            this.Container.RegisterInstance<IPrincipal>(Thread.CurrentPrincipal);
        }

        #endregion 
    }
}
