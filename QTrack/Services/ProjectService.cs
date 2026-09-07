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

        public ProjectService(ApiCredentials credentials, HttpClient? httpClient = null)
        {
            this.credentials = credentials;
            this.httpClient = httpClient ?? new HttpClient();
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            const string query = "fields=id,name,shortName,archived";
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
                Name = dto.Name ?? string.Empty,
                ShortName = dto.ShortName ?? string.Empty,
                IsArchive = dto.Archived,
                IsFavorite = false,
            });
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