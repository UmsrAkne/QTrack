namespace QTrack.Services
{
    public interface IYouTrackCommandService
    {
        public Task ExecuteCommandAsync(string command, CancellationToken cancellationToken = default);

        public Task ExecuteCommandAsync(IEnumerable<string> issueIds, string command, CancellationToken cancellationToken = default);
    }
}