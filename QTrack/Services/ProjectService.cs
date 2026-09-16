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
        private readonly AppSettings appSettings;
        private readonly ILiteDbService? dbService;

        public ProjectService(
            ApiCredentials credentials,
            HttpClient httpClient,
            IIssueService issueService,
            ILiteDbService dbService,
            AppSettings appSettings)
        {
            this.credentials = credentials;
            this.httpClient = httpClient;
            this.issueService = issueService;
            this.appSettings = appSettings;
            this.dbService = dbService;
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
            var projectList = projects.ToList();
            if (dbService != null)
            {
                foreach (var p in projectList)
                {
                    var cachedProject = dbService.Get<Project>(p.Id);
                    if (cachedProject != null)
                    {
                        p.UpdatedAt = cachedProject.UpdatedAt;
                    }
                }
            }

            var issues = await issueService.FetchRecentlyUpdatedIssuesAsync();

            // 1. プロジェクトコード（ShortName）ごとに最新の UpdatedAt を抽出して辞書化
            var latestUpdatedByProject = issues
                .Where(issue => !string.IsNullOrEmpty(issue.IdReadable) && issue.IdReadable.Contains('-'))
                .GroupBy(issue => issue.IdReadable[..issue.IdReadable.LastIndexOf('-')]) // "QTR-37" -> "QTR"
                .ToDictionary(
                    group => group.Key,
                    group => group.Max(issue => issue.UpdatedAt));

            // 2. 各 Project の ShortName と照合して UpdatedAt を書き込み
            foreach (var project in projectList)
            {
                if (project.ShortName != null && latestUpdatedByProject.TryGetValue(project.ShortName, out var latestUpdatedAt))
                {
                    if (project.UpdatedAt < latestUpdatedAt)
                    {
                        project.UpdatedAt = latestUpdatedAt;
                    }
                }

                dbService?.Upsert(project);
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