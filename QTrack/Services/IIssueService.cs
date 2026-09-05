using QTrack.Models;

namespace QTrack.Services
{
    public interface IIssueService
    {
        List<Issue> GetIssues(Project project, int count);
    }
}