namespace QTrack.Models
{
    public class Issue
    {
        public string Id { get; init; } = "";

        public string IdReadable { get; init; } = "";

        public string Summary { get; init; } = "";

        public string? Description { get; init; }

        public string? Priority { get; init; }

        public string? Type { get; init; }

        public string? State { get; init; }

        public string? Assignee { get; init; }
    }
}