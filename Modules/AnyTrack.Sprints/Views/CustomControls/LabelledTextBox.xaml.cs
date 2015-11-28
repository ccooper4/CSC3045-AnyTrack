using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace AnyTrack.Sprints.Views.CustomControls
{
    /// <summary>
    /// Interaction logic for LabelledTextBox.xaml
    /// </summary>
    public partial class LabelledTextBox
    {
        /// <summary>
        /// constructor for making a label
        /// </summary>
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(LabelledTextBox), new FrameworkPropertyMetadata("Unnamed Label"));

        /// <summary>
        /// constructor for making a textbox
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(LabelledTextBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// constructor for making a labeled textbox
        /// </summary>
        public LabelledTextBox()
        {
            InitializeComponent();
            Root.DataContext = this;
        }

        /// <summary>
        /// Gets or sets Label property.
        /// </summary>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets Text property.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }

    /// <summary>
    /// LayoutGroup for sharing scope
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public partial class LayoutGroup : StackPanel
    {
        /// <summary>
        /// sharing the scope for the layoutgroup
        /// </summary>
        public LayoutGroup()
        {
            Grid.SetIsSharedSizeScope(this, true);
        }
    }
}