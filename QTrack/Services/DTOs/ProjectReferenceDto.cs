using System.Text.Json.Serialization;

namespace QTrack.Services.DTOs
{
    /// <summary>
    /// プロジェクト参照用DTO。
    /// 一つしかプロパティを持たないクラスだが、型安全のためにやむを得ず作成。
    /// </summary>
    public class ProjectReferenceDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
    }
}