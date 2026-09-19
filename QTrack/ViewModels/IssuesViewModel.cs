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
        private AsyncRelayCommand<Issue>? toggleCompleteFlagCommand;
        private IssueListItemViewModel? selectedIssue;

        public IssuesViewModel()
        {
            // xaml プレビューを表示するためのコンストラクタ
            AppLogger.Warn("IssuesViewModel() が実行されました。");
            AppLogger.Warn("通常、このコンストラクタは実行されません。オーバーロードの実行に問題がないか確認してください。");
            issueService = new MockIssueService();
            var l = issueService.GetIssuesAsync(new Project(), 10);
            var list = l.Result.Select(i => new IssueListItemViewModel(i));
            Issues.AddRange(list);
        }

        public IssuesViewModel(IIssueService issueService)
        {
            AppLogger.Info("IssuesViewModel created");
            AppLogger.Info(issueService.ToString() ?? string.Empty);

            this.issueService = issueService;
        }

        public string Header { get; set; } = "Issues";

        public Project? CurrentProject { get; set; }

        public ObservableCollection<IssueListItemViewModel> Issues { get; set; } = new ();

        public IssueListItemViewModel? SelectedIssue { get => selectedIssue; set => SetProperty(ref selectedIssue, value); }

        public Issue PendingIssue { get => pendingIssue; set => SetProperty(ref pendingIssue, value); }

        public AsyncRelayCommand QuickAddAsyncCommand =>
            quickAddCommand ??= new AsyncRelayCommand(async () =>
            {
                await Task.CompletedTask;
            });

        public AsyncRelayCommand AddIssueAsyncCommand =>
            addIssueCommand ??= new AsyncRelayCommand(async () =>
            {
                var summary = PendingIssue.Summary.Trim();
                var description = PendingIssue.Description.Trim();
                var project = CurrentProject;

                if (project is null || string.IsNullOrWhiteSpace(summary))
                {
                    return;
                }

                // まず画面に表示する仮の Issue
                var temporaryIssue = new Issue
                {
                    Id = $"client-{Guid.NewGuid():N}",
                    IdReadable = "Posting...",
                    Summary = summary,
                    Description = description,
                    UpdatedAt = DateTime.Now,
                    Priority = "Normal",
                    State = IssueState.Created,
                };

                var item = new IssueListItemViewModel(temporaryIssue)
                {
                    IsPosting = true,
                };

                // 先頭に表示する場合
                Issues.Insert(0, item);

                // 入力欄をクリアする場合
                PendingIssue.Summary = string.Empty;
                PendingIssue.Description = string.Empty;

                try
                {
                    // 実際の投稿処理
                    var createdIssue =
                        await issueService.CreateIssueAsync(project, summary, description);

                    // 投稿成功後、仮データを実データに更新
                    item.Issue = createdIssue;
                    item.IsPosting = false;
                }
                catch (Exception ex)
                {
                    // 投稿失敗の場合は削除
                    Issues.Remove(item);

                    PendingIssue.Summary = summary;
                    PendingIssue.Description = description;

                    // 必要に応じて通知
                    // MessageBox、通知領域、Snackbar など
                    // await notificationService.ShowErrorAsync(...);
                }
            });

        public AsyncRelayCommand<Issue> ToggleCompleteFlagAsyncCommand =>
            toggleCompleteFlagCommand ??= new AsyncRelayCommand<Issue>(async (issue) =>
            {
                if (issue == null)
                {
                    return;
                }

                await issueService.CompleteIssueAsync(issue);
            });

        public async Task InitializeAsync(Project project)
        {
            CurrentProject = project;

            var issues = await issueService.GetIssuesAsync(project, 10);
            Issues.AddRange(issues.Select(i => new IssueListItemViewModel(i)));

            Header = $"{project.Name} の課題";
        }
    }
}