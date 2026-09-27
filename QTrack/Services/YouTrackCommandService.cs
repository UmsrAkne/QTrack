using QTrack.Utils;

namespace QTrack.Services
{
    public sealed class YouTrackCommandService : IYouTrackCommandService
    {
        public YouTrackCommandService()
        {
            AppLogger.Info("GenerateYouTrackCommandService");
            AppLogger.Info(" - HashCode: " + GetHashCode());
        }

        public void ExecuteCommand(string command)
        {
            throw new NotImplementedException();
        }
    }
}