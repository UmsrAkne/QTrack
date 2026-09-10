using System.Windows;

namespace QTrack.Views.Controls
{
    public partial class CustomCheckBox
    {
        public readonly static DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(
                nameof(IsChecked),
                typeof(bool?),
                typeof(CustomCheckBox),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public CustomCheckBox()
        {
            InitializeComponent();
        }

        public bool? IsChecked
        {
            get => (bool?)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }
    }
}