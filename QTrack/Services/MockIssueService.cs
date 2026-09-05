using QTrack.Models;

namespace QTrack.Services
{
    public class MockIssueService : IIssueService
    {
        public List<Issue> GetIssues(Project project, int count)
        {
            return new List<Issue>();
        }
    }
}