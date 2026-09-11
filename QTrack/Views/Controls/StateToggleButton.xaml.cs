using System.Windows;
using System.Windows.Input;
using QTrack.Models;

namespace QTrack.Views.Controls
{
    public partial class StateToggleButton
    {
        public readonly static DependencyProperty StateProperty =
            DependencyProperty.Register(
                nameof(State),
                typeof(IssueState),
                typeof(StateToggleButton),
                new FrameworkPropertyMetadata(IssueState.Created, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public readonly static DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(StateToggleButton));

        public StateToggleButton()
        {
            InitializeComponent();
        }

        public IssueState State
        {
            get => (IssueState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            // 状態遷移のロジック
            State = State switch
            {
                IssueState.Created => IssueState.InProgress,
                IssueState.InProgress => IssueState.Pausing,
                IssueState.Pausing => IssueState.InProgress,
                _ => State,
            };
        }
    }
}