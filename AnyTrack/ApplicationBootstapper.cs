using System;
using System.Globalization;
using System.Windows;
using AnyTrack.Accounting;
using AnyTrack.Client.Views;
using AnyTrack.Projects;
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
                return this.Container.TryResolve(type);
            });
        }

        #endregion 
    }
}
