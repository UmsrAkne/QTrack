using QTrack.Models;
using QTrack.Services;
using QTrack.Utils;

namespace QTrack.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class IssuesViewModel : BindableBase, ITabViewModels
    {
        private readonly IIssueService issueService;
        private readonly ApiCredentials credentials;

        public IssuesViewModel(IIssueService issueService, ApiCredentials credentials)
        {
            AppLogger.Info("IssuesViewModel created");
            AppLogger.Info(issueService.ToString() ?? string.Empty);

            this.credentials = credentials;
            this.issueService = issueService;
        }

        public string Header { get; set; } = "Issues";

        public Project? CurrentProject { get; set; }

        public async Task InitializeAsync(Project project)
        {
            await Task.Delay(2000);
            CurrentProject = project;
            Header = $"{project.Name} の課題";
        }
    }
}