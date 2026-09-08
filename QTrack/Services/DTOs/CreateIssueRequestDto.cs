using System.Text.Json.Serialization;

namespace QTrack.Services.DTOs
{
    public class CreateIssueRequestDto
    {
        [JsonPropertyName("project")]
        public ProjectReferenceDto Project { get; set; } = new();

        [JsonPropertyName("summary")]
        public string Summary { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}