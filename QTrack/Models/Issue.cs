namespace QTrack.Models
{
    public class Issue : BindableBase
    {
        private string summary = "";
        private string? description;
        private string? type;
        private int entryNo;
        private int rate;
        private IssueState? state;
        private bool isCompleted;

        public string Id { get; init; } = "";

        public string IdReadable { get; init; } = "";

        public bool IsCompleted { get => isCompleted; set => SetProperty(ref isCompleted, value); }

        public string Summary { get => summary; set => SetProperty(ref summary, value); }

        public string? Description { get => description; set => SetProperty(ref description, value); }

        public string? Priority { get; init; }

        public string? Type { get => type; set => SetProperty(ref type, value); }

        public IssueState? State { get => state; set => SetProperty(ref state, value); }

        public string? Assignee { get; init; }

        public DateTime UpdatedAt { get; init; }

        public int EntryNo { get => entryNo; set => SetProperty(ref entryNo, value); }

        public int Rate { get => rate; set => SetProperty(ref rate, value); }
    }
}