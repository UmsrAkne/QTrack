using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using QTrack.Utils;

namespace QTrack.Services
{
    public sealed class YouTrackCommandService : IYouTrackCommandService
    {
        private readonly HttpClient httpClient;
        private readonly ApiCredentials apiCredentials;

        public YouTrackCommandService(HttpClient httpClient, ApiCredentials apiCredentials)
        {
            AppLogger.Info("GenerateYouTrackCommandService");
            AppLogger.Info(" - HashCode: " + GetHashCode());
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.apiCredentials = apiCredentials ?? throw new ArgumentNullException(nameof(apiCredentials));
        }

        public async Task ExecuteCommandAsync(string command, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentException("コマンド文字列が空です。", nameof(command));
            }

            // 例: "DEB-1 subtask of DEB-2" から "DEB-1" と "subtask of DEB-2" を抽出
            var match = Regex.Match(command.Trim(), @"^([A-Za-z0-9_]+-\d+)\s+(.+)$");
            if (!match.Success)
            {
                throw new ArgumentException("コマンド形式が不正です。先頭に対象の課題ID（例: 'PROJ-123 subtask of PROJ-456'）を指定してください。", nameof(command));
            }

            var targetIssueId = match.Groups[1].Value; // "DEB-1"
            var actualCommand = match.Groups[2].Value; // "subtask of DEB-2"

            // Postman で成功した JSON と同一のペイロードを生成
            var requestBody = new YouTrackCommandRequest
            {
                Query = actualCommand,
                Issues = new List<YouTrackIssueIdentifier>
                {
                    new ()
                    {
                        IdReadable = targetIssueId,
                    },
                },
            };

            var url = apiCredentials.YoutrackCommandsEndpoint;

            using var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = JsonContent.Create(requestBody);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiCredentials.YoutrackApiKey);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException(
                    $"YouTrack コマンドの実行に失敗しました。 StatusCode: {response.StatusCode}, Response: {errorContent}");
            }
        }

        public async Task ExecuteCommandAsync(IEnumerable<string> issueIds, string command, CancellationToken cancellationToken = default)
        {
            var issueList = issueIds.ToList();
            if (issueIds == null || issueList.Count == 0)
            {
                throw new ArgumentException("対象の課題IDが指定されていません。", nameof(issueIds));
            }

            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentException("コマンド文字列が空です。", nameof(command));
            }

            // リクエストボディの組み立て
            var requestBody = new YouTrackCommandRequest
            {
                Query = command,

                // "PROJ-123" のような読みやすいIDの場合は idReadable、内部IDの場合は id を使います
                Issues = issueList.Select(id => new YouTrackIssueIdentifier
                {
                    IdReadable = id,
                }).ToList(),
            };

            // /api/commands へ POST リクエスト
            var response = await httpClient.PostAsJsonAsync("api/commands", requestBody, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException(
                    $"YouTrack コマンドの実行に失敗しました。 StatusCode: {response.StatusCode}, Response: {errorContent}");
            }
        }

        private class YouTrackCommandRequest
        {
            [JsonPropertyName("query")]
            public string Query { get; set; } = string.Empty;

            [JsonPropertyName("issues")]
            public List<YouTrackIssueIdentifier> Issues { get; set; } = new();
        }

        private class YouTrackIssueIdentifier
        {
            [JsonPropertyName("$type")]
            public string Type { get; set; } = "Issue";

            [JsonPropertyName("idReadable")]
            public string IdReadable { get; set; } = string.Empty;
        }
    }
}