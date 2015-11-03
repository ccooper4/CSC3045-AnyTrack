using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnyTrack.Projects.Views
{
    /// <summary>
    /// Interaction logic for ProductBacklog.xaml
    /// </summary>
    public partial class ProductBacklog : UserControl
    {
        /// <summary>
        /// Initialize the product backlog
        /// </summary>
        public ProductBacklog()
        {
            InitializeComponent();
        }

        #region Methods
        /// <summary>
        /// Handler to edit a given story
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the arguments</param>
        private void EditStory(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Handler to delete a given story
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the arguments</param>
        private void DeleteStory(object sender, RoutedEventArgs e)
        {
        }
        #endregion Methods
    }
}
