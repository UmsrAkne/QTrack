using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using QTrack.Models;
using QTrack.Services.DTOs;
using QTrack.Utils;

namespace QTrack.Services
{
    public class IssueService : IIssueService
    {
        private readonly ApiCredentials credentials;
        private readonly HttpClient httpClient;

        public IssueService(ApiCredentials credentials, HttpClient? httpClient = null)
        {
            AppLogger.Info("IssueService created");

            this.credentials = credentials;
            this.httpClient = httpClient ?? new HttpClient();
        }

        public async Task<List<Issue>> GetIssuesAsync(Project project, int count)
        {
            var query = $"query=project:{project.ShortName}&fields=id,idReadable,summary,description,updated,customFields(name,value(name,minutes,presentation)),links(direction,linkType(name),issues(idReadable))";
            var url = $"{credentials.YoutrackIssuesEndpoint}?{query}";

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", credentials.YoutrackApiKey);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var rawIssues = await response.Content.ReadFromJsonAsync<List<IssueDto>>();

            return rawIssues == null
                ? new List<Issue>()
                : rawIssues.Select(dto => dto.ToModel()).ToList();
        }

        public Task<List<Issue>> GetIssuesAsync(IssueSearchCriteria criteria)
        {
            throw new NotImplementedException();
        }

        public async Task<Issue> CreateIssueAsync(Project project, string summary, string description)
        {
            var url = $"{credentials.YoutrackIssuesEndpoint}?fields=id,idReadable,summary,description,updated,customFields(name,value(name,minutes,presentation)),links(direction,linkType(name),issues(idReadable))";

            var requestDto = new CreateIssueRequestDto
            {
                Project = new ProjectReferenceDto { Id = project.Id, },
                Summary = summary,
                Description = description,
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", credentials.YoutrackApiKey);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = JsonContent.Create(requestDto);

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var resultDto = await response.Content.ReadFromJsonAsync<IssueDto>();

            return resultDto?.ToModel() ?? throw new InvalidOperationException("Failed to deserialize the created issue.");
        }
    }
}