namespace QTrack.Services
{
    public interface IYouTrackCommandService
    {
        public Task ExecuteCommandAsync(string command);
    }
}