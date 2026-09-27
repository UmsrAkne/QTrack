using System.Windows.Controls;
using System.Windows.Threading;

namespace QTrack.Views.Controls
{
    public partial class CommandPaletteView : UserControl
    {
        public CommandPaletteView()
        {
            InitializeComponent();
            IsVisibleChanged += (s, e) =>
            {
                if (IsVisible)
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Input, () =>
                    {
                        // CommandTextBox.Focus();
                        // CommandTextBox.SelectAll();
                    });
                }
            };
        }
    }
}