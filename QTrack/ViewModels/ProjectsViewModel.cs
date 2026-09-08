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
        private AsyncRelayCommand? fetchProjectsCommand;
        private Project? selectedProject;
        private bool isLoading;

        public ProjectsViewModel()
        {
            // xaml プレビューを表示するためのコンストラクタ
            AppLogger.Warn("ProjectsViewModel() が実行されました。");
            AppLogger.Warn("通常、このコンストラクタは実行されません。オーバーロードの実行に問題がないか確認してください。");
            projectService = new MockProjectService();
            var list = projectService.GetAllProjectsAsync();
            Projects.AddRange(list.Result);
        }

        public ProjectsViewModel(IProjectService projectService)
        {
            AppLogger.Info("IssuesViewModel created");
            AppLogger.Info(projectService.ToString() ?? string.Empty);
            this.projectService = projectService;
        }

        public event EventHandler<Project>? OpenProjectEvent;

        public string Header { get; set; } = "Projects";

        public ObservableCollection<Project> Projects { get; set; } = new ();

        public Project? SelectedProject
        {
            get => selectedProject;
            set => SetProperty(ref selectedProject, value);
        }

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

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

        public DelegateCommand RaiseOpenProjectEventCommand => new (() =>
        {
            if (SelectedProject is not null)
            {
                OpenProjectEvent?.Invoke(this, SelectedProject);
                IsLoading = true;
            }
        });
    }
}