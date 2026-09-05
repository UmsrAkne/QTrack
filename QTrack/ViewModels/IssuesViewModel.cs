using QTrack.Services;
using QTrack.Utils;

namespace QTrack.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class IssuesViewModel : BindableBase
    {
        private readonly IIssueService issueService;

        public IssuesViewModel(IIssueService issueService)
        {
            AppLogger.Info("IssuesViewModel created");
            AppLogger.Info(issueService.ToString());

            this.issueService = issueService;
        }
    }
}