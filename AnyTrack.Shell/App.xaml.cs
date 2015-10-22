using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AnyTrack.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Allows custom logic to be ran at application startup.
        /// </summary>
        /// <param name="e">The event information.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new ApplicationBootstrapper();
            bootstrapper.Run();
        }
    }
}
