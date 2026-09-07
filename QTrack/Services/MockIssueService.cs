using QTrack.Models;

namespace QTrack.Services
{
    public class MockIssueService : IIssueService
    {
        public async Task<List<Issue>> GetIssuesAsync(Project project, int count)
        {
            var l = new List<Issue>();
            for (var i = 0; i < 10; i++)
            {
                var issue = new Issue()
                {
                    IdReadable = $"{project}-00{i + 1}",
                    Id = $"000-0{i + 1}",
                    Summary = $"テスト用ダミー課題のタイトル {i + 1} ",
                    Description = $"テスト用課題の説明文\n改行つき {i + 1}",
                    Priority = "low",
                    State = "open",
                    UpdatedAt = new DateTime(2024, 1, 1 + i),
                };

                l.Add(issue);
            }

            return await Task.FromResult(l);
        }
    }
}