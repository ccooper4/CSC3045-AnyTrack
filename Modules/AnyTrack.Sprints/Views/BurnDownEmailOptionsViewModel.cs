using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using AnyTrack.Infrastructure;
using AnyTrack.Infrastructure.ServiceGateways;
using MahApps.Metro.Controls;
using OxyPlot.Wpf;
using Prism.Commands;
using Prism.Regions;

namespace AnyTrack.Sprints.Views
{
    /// <summary>
    /// ViewModel for Burndown Email
    /// </summary>
    public class BurnDownEmailOptionsViewModel : ValidatedBindableBase, IFlyoutCompatibleViewModel, INavigationAware
    {
        /// <summary>
        /// Variable for the sprint service
        /// </summary>
        private ISprintServiceGateway sprintService;

        /// <summary>
        /// The project service
        /// </summary>
        private IProjectServiceGateway projectService;

        /// <summary>
        /// Constructor for the BurnDownEmailViewModel
        /// </summary>
        /// <param name="sprintGateway">The sprint gateway</param>
        /// <param name="projectGateway">The project gateway</param>
        public BurnDownEmailOptionsViewModel(ISprintServiceGateway sprintGateway, IProjectServiceGateway projectGateway)
        {
            this.Header = "Share Burndown Charts";
            this.IsModal = true;
            this.Position = Position.Right;
            this.Theme = FlyoutTheme.Accent;
            this.IsOpen = true;

            sprintService = sprintGateway;
            projectService = projectGateway;
            SendEmailCommand = new DelegateCommand(SendEmail);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the flyout is open
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Gets or sets Flyout header
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the flyout position
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// Gets or sets the flyout theme
        /// </summary>
        public FlyoutTheme Theme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the flyout IsModal
        /// </summary>
        public bool IsModal { get; set; }

        /// <summary>
        /// Gets or sets the close Button Visibility
        /// </summary>
        public Visibility CloseButtonVisibility { get; set; }

        /// <summary>
        /// Gets or sets the title Visiblity
        /// </summary>
        public Visibility TitleVisibility { get; set; }

        /// <summary>
        /// Gets or sets the command to send an email burndown chart
        /// </summary>
        public DelegateCommand SendEmailCommand { get; set; }

        /// <summary>
        /// Gets or sets the email address to be sent to
        /// </summary>
        public string SenderEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the email address to be sent to
        /// </summary>
        public string RecipientEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the email message to be sent
        /// </summary>
        public string EmailMessage { get; set; }

        /// <summary>
        /// Gets or sets the email attachment
        /// </summary>
        public Attachment EmailAttachment { get; set; }

        /// <summary>
        /// Navigate to burndown email
        /// </summary>
        /// <param name="navigationContext">Context of email</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// Navigate to burndown
        /// </summary>
        /// <param name="navigationContext">Context of the navigation</param>
        /// <returns>the navigation target</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        /// <summary>
        /// On navigated from
        /// </summary>
        /// <param name="navigationContext">Context of the navigation</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        
        /// <summary>
        /// Method to send an email request
        /// </summary>
        private void SendEmail()
        {
            ////sprintService.SendEmailRequest(SenderEmailAddress, RecipientEmailAddress, EmailMessage, EmailAttachment);
            ////Attachment attachment = new Attachment("C://Users//User//Desktop//91.png");
            MemoryStream ms = new MemoryStream();
            Bitmap bmp = new Bitmap("C://Users//User//Desktop//91.pn");
            ////sprintService.SendEmailRequest("seanhaughian@live.co.uk", "seanhaughian@live.co.uk", "Hello", attachment);
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            using (var stream = new MemoryStream())
            {
                var pngExporter = new PngExporter();
                ////pngExporter.Export(plotModel, stream, 600, 400, Brushes.White);
            }
        }
    }
}