namespace QTrack.Models
{
    public class Project
    {
        public string Name { get; set; } = string.Empty;

        public string ShortName { get; set; } = string.Empty;

        public bool IsFavorite { get; set; }

        public bool IsArchive { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}