using System.Windows;
using System.Windows.Controls.Primitives;

namespace QTrack.Views.Controls
{
    public class IconToggleButton : ToggleButton
    {
        static IconToggleButton()
        {
            // デフォルトのスタイルを Generic.xaml から適用
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(IconToggleButton),
                new FrameworkPropertyMetadata(typeof(IconToggleButton)));
        }
    }
}