using QTrack.Models;

namespace QTrack.ViewModels
{
    public class IssueListItemViewModel : BindableBase
    {
        private Issue issue;
        private bool isPosting;
        private bool hasError;

        public IssueListItemViewModel(Issue issue)
        {
            Issue = issue;
        }

        public Guid ClientId { get; } = Guid.NewGuid();

        public Issue Issue { get => issue; set => SetProperty(ref issue, value); }

        public bool IsPosting { get => isPosting; set => SetProperty(ref isPosting, value); }

        public bool HasError { get => hasError; set => SetProperty(ref hasError, value); }

        public string? ErrorMessage { get; set; }
    }
}