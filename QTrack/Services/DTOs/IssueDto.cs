using System.Text.Json.Serialization;
using QTrack.Models;

namespace QTrack.Services.DTOs
{
    public class IssueDto
    {
        private string? GetCustomFieldValue(string fieldName) =>
            CustomFields?.FirstOrDefault(f => f.Name == fieldName)?.Value?.Name;

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

        public Issue ToModel()
        {
            return new Issue
            {
                IdReadable = IdReadable ?? string.Empty,
                Summary = Summary ?? string.Empty,
                Description = Description,
                Priority = GetCustomFieldValue("Priority"),
                Type = GetCustomFieldValue("Type"),
                State = GetCustomFieldValue("State"),
                Assignee = GetCustomFieldValue("Assignee"),
            };
        }
    }
}