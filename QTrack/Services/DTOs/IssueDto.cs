using System.Text.Json.Serialization;

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

        [JsonPropertyName("customFields")]
        public List<CustomFieldDto>? CustomFields { get; set; }
    }
}