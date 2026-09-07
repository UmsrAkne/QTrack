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
    }
}