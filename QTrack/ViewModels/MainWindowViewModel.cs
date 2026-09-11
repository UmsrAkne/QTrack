using System.Collections.ObjectModel;
using System.Windows;
using QTrack.Models;
using QTrack.Services;
using QTrack.Utils;

namespace QTrack.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private readonly Func<IssuesViewModel> issuesVmFactory;
        private string title = "QTrack";
        private ITabViewModels? selectedTab;

        public MainWindowViewModel()
        {
            AppLogger.Info("MainWindowViewModel created");
            issuesVmFactory = () => new IssuesViewModel(new MockIssueService());
        }

        public MainWindowViewModel(ProjectsViewModel projectsVm, Func<IssuesViewModel> issuesVmFactory, AppSettings appSettings)
        {
            AppLogger.Info("MainWindowViewModel created");

            // 引数に欠けがあった場合はまともに動かないため、例外をスローして落とす
            ArgumentNullException.ThrowIfNull(projectsVm);
            ArgumentNullException.ThrowIfNull(issuesVmFactory);

            AppLogger.Info(projectsVm.ToString() ?? string.Empty);
            this.issuesVmFactory = issuesVmFactory;
            TabViewModels.Add(projectsVm);
            SelectedTab = projectsVm;

            projectsVm.OpenProjectEvent += OnOpenProjectEvent;

            #if DEBUG
            _ = InitializeDebugStateAsync(appSettings);
            #endif
        }

        public string Title { get => title; set => SetProperty(ref title, value); }

        public ObservableCollection<ITabViewModels> TabViewModels { get; set; } = new ();

        public ITabViewModels? SelectedTab { get => selectedTab; set => SetProperty(ref selectedTab, value); }

        private async void OnOpenProjectEvent(object? sender, Project project)
        {
            try
            {
                await OnOpenProjectIssuesAsync(project);
            }
            catch (Exception ex)
            {
                // 必要に応じてログ出力やユーザーへのエラー通知
                var msg = $"プロジェクト \"{project.Name}\" のオープンに失敗しました: {ex.Message}";
                AppLogger.Warn(msg);

                MessageBox.Show(
                    msg,
                    "エラー",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task OnOpenProjectIssuesAsync(Project project)
        {
            // 1. 既に開いているタブがあればそれをアクティブにする（重複オープンを防ぐ場合）
            var existingTab = TabViewModels
                .OfType<IssuesViewModel>()
                .FirstOrDefault(vm => vm.CurrentProject?.Name == project.Name);

            if (existingTab != null)
            {
                SelectedTab = existingTab;
                return;
            }

            // 2. DI経由で新しいインスタンスを作成
            var newIssuesVm = issuesVmFactory();

            // 3. データを初期化
            await newIssuesVm.InitializeAsync(project);

            // 4. タブ一覧に追加して選択状態にする
            TabViewModels.Add(newIssuesVm);
            SelectedTab = newIssuesVm;
        }

        #if DEBUG
        private async Task InitializeDebugStateAsync(AppSettings appSettings)
        {
            // コンストラクタ内では await できないため、インデックスの設定まで含めて非同期メソッドで実行
            try
            {
                await OnOpenProjectIssuesAsync(new Project { Name = "Auto Generated Project 1", });

                if (appSettings.InitialTabIndexForDebug >= 0 &&
                    appSettings.InitialTabIndexForDebug < TabViewModels.Count)
                {
                    SelectedTab = TabViewModels[appSettings.InitialTabIndexForDebug];
                }
            }
            catch (Exception ex)
            {
                AppLogger.Warn($"Debug初期化に失敗しました: {ex.Message}");
            }
        }
        #endif
    }
}