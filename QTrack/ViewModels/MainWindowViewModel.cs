using QTrack.Utils;

namespace QTrack.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private string title = "QTrack";

        public MainWindowViewModel()
        {
            AppLogger.Info("MainWindowViewModel created");
        }

        public MainWindowViewModel(ProjectsViewModel projectsVm, IssuesViewModel issuesVm)
        {
            AppLogger.Info("MainWindowViewModel created");

            // 引数に欠けがあった場合はまともに動かないため、例外をスローして落とす
            ArgumentNullException.ThrowIfNull(projectsVm);
            ArgumentNullException.ThrowIfNull(issuesVm);

            AppLogger.Info(projectsVm.ToString() ?? string.Empty);
            AppLogger.Info(issuesVm.ToString() ?? string.Empty);
        }

        public string Title { get => title; set => SetProperty(ref title, value); }
    }
}