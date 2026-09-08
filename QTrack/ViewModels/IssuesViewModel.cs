using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using QTrack.Models;
using QTrack.Services;
using QTrack.Utils;

namespace QTrack.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class IssuesViewModel : BindableBase, ITabViewModels
    {
        private readonly IIssueService issueService;
        private Issue pendingIssue = new ();
        private AsyncRelayCommand? quickAddCommand;
        private AsyncRelayCommand? addIssueCommand;

        public IssuesViewModel()
        {
            // xaml プレビューを表示するためのコンストラクタ
            AppLogger.Warn("IssuesViewModel() が実行されました。");
            AppLogger.Warn("通常、このコンストラクタは実行されません。オーバーロードの実行に問題がないか確認してください。");
            issueService = new MockIssueService();
            var l = issueService.GetIssuesAsync(new Project(), 10);
            Issues.AddRange(l.Result);
        }

        public IssuesViewModel(IIssueService issueService)
        {
            AppLogger.Info("IssuesViewModel created");
            AppLogger.Info(issueService.ToString() ?? string.Empty);

            this.issueService = issueService;
        }

        public string Header { get; set; } = "Issues";

        public Project? CurrentProject { get; set; }

        public ObservableCollection<Issue> Issues { get; set; } = new ();

        public Issue PendingIssue { get => pendingIssue; set => SetProperty(ref pendingIssue, value); }

        public AsyncRelayCommand QuickAddAsyncCommand =>
            quickAddCommand ??= new AsyncRelayCommand(async () =>
            {
                await Task.CompletedTask;
            });

        public AsyncRelayCommand AddIssueAsyncCommand =>
            addIssueCommand ??= new AsyncRelayCommand(async () =>
            {
                await Task.CompletedTask;
            });

        public async Task InitializeAsync(Project project)
        {
            await Task.Delay(2000);
            CurrentProject = project;

            var issues = await issueService.GetIssuesAsync(project, 10);
            Issues.AddRange(issues);

            Header = $"{project.Name} の課題";
        }
    }
}