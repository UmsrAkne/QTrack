using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using QTrack.Models;
using QTrack.Utils;

namespace QTrack.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApiCredentials credentials;
        private readonly HttpClient httpClient;
        private readonly IIssueService issueService;

        public ProjectService(ApiCredentials credentials, HttpClient? httpClient = null, IIssueService? issueService = null)
        {
            this.credentials = credentials;
            this.httpClient = httpClient ?? new HttpClient();
            this.issueService = issueService ?? new IssueService(credentials, httpClient);
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            // 明示的に取得数を指定しなければ、中途半端な数で打ち切られる
            const string query = "fields=id,name,shortName,archived&$top=100";
            var url = $"{credentials.YoutrackProjectsEndpoint}?{query}";

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", credentials.YoutrackApiKey);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var rawProjects = await response.Content.ReadFromJsonAsync<List<YouTrackProjectDto>>();

            if (rawProjects == null)
            {
                return Enumerable.Empty<Project>();
            }

            return rawProjects.Select(dto => new Project
            {
                Id = dto.Id ?? string.Empty,
                Name = dto.Name ?? string.Empty,
                ShortName = dto.ShortName ?? string.Empty,
                IsArchive = dto.Archived,
                IsFavorite = false,
            });
        }

        public async Task PopulateUpdatedAt(IEnumerable<Project> projects)
        {
            var searchCriteria = new IssueSearchCriteria
            {
                Top = 100,
                SortByUpdatedDesc = true,
            };

            var issues = await issueService.GetIssuesAsync(searchCriteria);

            // 1. プロジェクトコード（ShortName）ごとに最新の UpdatedAt を抽出して辞書化
            var latestUpdatedByProject = issues
                .Where(issue => !string.IsNullOrEmpty(issue.IdReadable))
                .GroupBy(issue => issue.IdReadable[..issue.IdReadable.LastIndexOf('-')]) // "QTR-37" -> "QTR"
                .ToDictionary(
                    group => group.Key,
                    group => group.Max(issue => issue.UpdatedAt));

            // 2. 各 Project の ShortName と照合して UpdatedAt を書き込み
            foreach (var project in projects)
            {
                if (project.ShortName != null && latestUpdatedByProject.TryGetValue(project.ShortName, out var latestUpdatedAt))
                {
                    project.UpdatedAt = latestUpdatedAt;
                }
            }
        }

        private sealed class YouTrackProjectDto
        {
            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("shortName")]
            public string? ShortName { get; set; }

            [JsonPropertyName("archived")]
            public bool Archived { get; set; }
        }
    }
}