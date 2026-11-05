using System.Windows.Threading;

namespace QTrack.Services
{
    public class TimerService : ITimerService
    {
        private readonly DispatcherTimer dispatcherTimer = new ();

        public TimerService()
        {
            dispatcherTimer.Tick += (_, args) => Tick?.Invoke(this, args);
        }

        public event EventHandler? Tick;

        public void Start(TimeSpan interval)
        {
            dispatcherTimer.Interval = interval;
            dispatcherTimer.Start();
        }

        public void Stop()
        {
            dispatcherTimer.Stop();
        }
    }
}