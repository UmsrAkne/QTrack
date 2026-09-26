namespace QTrack.Models
{
    public class Issue : BindableBase
    {
        private string summary = string.Empty;
        private string? description = string.Empty;
        private string? type;
        private int entryNo;
        private int rate;
        private IssueState? state;
        private bool isCompleted;
        private TimeSpan estimatedDuration;
        private TimeSpan actualDuration;

        public string Id { get; init; } = string.Empty;

        public string IdReadable { get; set; } = string.Empty;

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

        public TimeSpan EstimatedDuration
        {
            get => estimatedDuration;
            set => SetProperty(ref estimatedDuration, value);
        }

        public TimeSpan ActualDuration
        {
            get => actualDuration;
            set => SetProperty(ref actualDuration, value);
        }

        public string GetProjectShortName()
        {
            if (string.IsNullOrWhiteSpace(IdReadable) || !IdReadable.Contains('-'))
            {
                return string.Empty;
            }

            return IdReadable[..IdReadable.LastIndexOf('-')];
        }
    }
}