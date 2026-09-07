using System.Collections.ObjectModel;
using QTrack.Models;
using QTrack.Services;
using QTrack.Utils;

namespace QTrack.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class IssuesViewModel : BindableBase, ITabViewModels
    {
        private readonly IIssueService issueService;

        public IssuesViewModel(IIssueService issueService)
        {
            AppLogger.Info("IssuesViewModel created");
            AppLogger.Info(issueService.ToString() ?? string.Empty);

            this.issueService = issueService;
        }

        public string Header { get; set; } = "Issues";

        public Project? CurrentProject { get; set; }

        public ObservableCollection<Issue> Issues { get; set; } = new ();

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