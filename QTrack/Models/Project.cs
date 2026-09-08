namespace QTrack.Models
{
    public class Project
    {
        public string Name { get; set; } = string.Empty;

        public string ShortName { get; set; } = string.Empty;

        public bool IsFavorite { get; set; }

        public bool IsArchive { get; set; }

        public DateTime UpdatedAt { get; set; }

        // 主に課題の新規投稿の際に使用する。
        public string Id { get; set; } = string.Empty;
    }
}