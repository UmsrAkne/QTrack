using QTrack.Utils;

namespace QTrack.Services
{
    public sealed class MockYouTrackCommandService : IYouTrackCommandService
    {
        public MockYouTrackCommandService()
        {
            AppLogger.Info("Generate MockYouTrackCommandService");
            AppLogger.Info(" - HashCode: " + GetHashCode());
        }

        public Task ExecuteCommandAsync(string command)
        {
            AppLogger.Info("MockYouTrackCommandService.ExecuteCommandAsync");
            AppLogger.Info(" - Command: " + command);
            return Task.CompletedTask;
        }
    }
}