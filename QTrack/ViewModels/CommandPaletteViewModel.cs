using System.Windows.Input;
using QTrack.Services;

namespace QTrack.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CommandPaletteViewModel : BindableBase
    {
        private readonly IYouTrackCommandService youTrackCommandService;
        private string commandText = string.Empty;
        private bool isOpen;
        private string? targetContextDescription; // 例: "対象: QTRACK-123"

        public CommandPaletteViewModel(IYouTrackCommandService youTrackCommandService)
        {
            this.youTrackCommandService = youTrackCommandService;

            ExecuteCommand = new DelegateCommand(OnExecute, CanExecute)
                .ObservesProperty(() => CommandText);

            CloseCommand = new DelegateCommand(() => IsOpen = false);
        }

        public bool IsOpen { get => isOpen; set => SetProperty(ref isOpen, value); }

        public string CommandText { get => commandText; set => SetProperty(ref commandText, value); }

        public string? TargetContextDescription
        {
            get => targetContextDescription;
            set => SetProperty(ref targetContextDescription, value);
        }

        public ICommand ExecuteCommand { get; }

        public ICommand CloseCommand { get; }

        private bool CanExecute()
        {
            return !string.IsNullOrWhiteSpace(CommandText);
        }

        public void Open(string? contextDescription = null)
        {
            TargetContextDescription = contextDescription;
            IsOpen = true;
        }

        private async void OnExecute()
        {
            var text = CommandText;
            CommandText = string.Empty;
            IsOpen = false; // 実行時に閉じる

            await youTrackCommandService.ExecuteCommandAsync(text);
        }
    }
}