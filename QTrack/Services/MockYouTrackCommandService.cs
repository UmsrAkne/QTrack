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

        public void ExecuteCommand(string command)
        {
            AppLogger.Info("MockYouTrackCommandService.ExecuteCommand");
            AppLogger.Info(" - Command: " + command);
        }
    }
}