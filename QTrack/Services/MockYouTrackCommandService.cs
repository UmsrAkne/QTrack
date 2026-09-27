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
            throw new NotImplementedException();
        }
    }
}