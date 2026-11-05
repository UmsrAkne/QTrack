namespace QTrack.Services
{
    public interface ITimerService
    {
        event EventHandler Tick;

        void Start(TimeSpan interval);

        void Stop();
    }
}