using System.Text.Json;
using System.Text.Json.Serialization;
using QTrack.Models;

namespace QTrack.Services.DTOs
{
    public class IssueDto
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("idReadable")]
        public string? IdReadable { get; set; }

        [JsonPropertyName("summary")]
        public string? Summary { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("$type")]
        public string? Type { get; set; }

        [JsonPropertyName("updated")]
        public long Updated { get; set; }

        [JsonPropertyName("EntryNo")]
        public int EntryNo { get; set; }

        [JsonPropertyName("Rate")]
        public int Rate { get; set; }

        [JsonPropertyName("customFields")]
        public List<CustomFieldDto>? CustomFields { get; set; }

        public Issue ToModel()
        {
            var issue = new Issue
            {
                IdReadable = IdReadable ?? string.Empty,
                Summary = Summary ?? string.Empty,
                Description = Description,
                Priority = GetCustomFieldValue("Priority"),
                Type = GetCustomFieldValue("Type"),
                State = IssueStateHelper.ToIssueState(GetCustomFieldValue("State")),
                Assignee = GetCustomFieldValue("Assignee"),
                UpdatedAt = DateTimeOffset.FromUnixTimeMilliseconds(Updated).DateTime,
            };

            var rawEntryNo = GetCustomFieldValue("EntryNo");
            if (int.TryParse(rawEntryNo, out var entryNoResult))
            {
                issue.EntryNo = entryNoResult;
            }

            var rawRate = GetCustomFieldValue("Rate");
            if(int.TryParse(rawRate, out var rateResult))
            {
                issue.Rate = rateResult;
            }

            return issue;
        }

        private string? GetCustomFieldValue(string fieldName)
        {
            var field = CustomFields?.FirstOrDefault(f => f.Name == fieldName);
            if (field?.Value == null)
            {
                return null;
            }

            var val = field.Value.Value;

            return val.ValueKind switch
            {
                // オブジェクトの場合 ({ "name": "Show-stopper", ... }) -> nameプロパティを取得
                JsonValueKind.Object => val.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : null,

                // 文字列の場合 ("直接の文字列")
                JsonValueKind.String => val.GetString(),

                // 数値の場合 (100 など)
                JsonValueKind.Number => val.GetRawText(),

                // null や その他
                _ => null,
            };
        }
    }
}