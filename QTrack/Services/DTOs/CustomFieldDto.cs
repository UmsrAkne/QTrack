using System.Text.Json.Serialization;

namespace QTrack.Services.DTOs
{
    public class CustomFieldDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("$type")]
        public string? Type { get; set; }

        /// <summary>
        /// value に入る値の型が複数（オブジェクト、文字列、数値、nullなど）あるため、
        /// ここでは汎用的な CustomFieldValueDto または JsonElement で受けるのがおすすめです。
        /// </summary>
        [JsonPropertyName("value")]
        public CustomFieldValueDto? Value { get; set; }
    }
}