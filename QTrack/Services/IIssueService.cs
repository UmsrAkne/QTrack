using QTrack.Models;

namespace QTrack.Services
{
    public interface IIssueService
    {
        Task<List<Issue>> GetIssuesAsync(Project project, int count);
    }
}