using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using QTrack.Models;
using QTrack.Services;
using QTrack.Utils;

namespace QTrack.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ProjectsViewModel : BindableBase, ITabViewModels
    {
        private readonly IProjectService projectService;
        private readonly ApiCredentials credentials;
        private AsyncRelayCommand? fetchProjectsCommand;

        public ProjectsViewModel()
        {
            // xaml プレビューを表示するためのコンストラクタ
            AppLogger.Warn("ProjectsViewModel() が実行されました。");
            AppLogger.Warn("通常、このコンストラクタは実行されません。オーバーロードの実行に問題がないか確認してください。");
            projectService = new MockProjectService();
            var list = projectService.GetAllProjectsAsync();
            Projects.AddRange(list.Result);
        }

        public ProjectsViewModel(IProjectService projectService, ApiCredentials credentials)
        {
            AppLogger.Info("IssuesViewModel created");
            AppLogger.Info(projectService.ToString());
            this.credentials = credentials;
            this.projectService = projectService;
        }

        public string Header { get; set; } = "Projects";

        public ObservableCollection<Project> Projects { get; set; } = new ();

        public AsyncRelayCommand FetchProjectsAsyncCommand =>
            fetchProjectsCommand ??= new AsyncRelayCommand(async () =>
            {
                try
                {
                    var l = await projectService.GetAllProjectsAsync();
                    Projects.Clear();
                    Projects.AddRange(l);
                }
                catch (Exception e)
                {
                    AppLogger.Error("プロジェクトの読み込みに失敗しました", e);
                    throw;
                }
            });
    }
}