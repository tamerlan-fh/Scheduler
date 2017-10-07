using Scheduler.Views;
using System;
using System.Windows.Threading;

namespace Scheduler
{
    class SchedulerManager
    {
        public static SchedulerManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SchedulerManager();
                return instance;
            }
        }
        private static SchedulerManager instance;

        private SchedulerManager()
        {
            timer = new DispatcherTimer();
            timer.Tick += TimerTick;
        }

        private DispatcherTimer timer;
        private void TimerTick(object sender, EventArgs e)
        {
            if (LastInputInfoManager.Instance.GetTimeAfterLastInput() >= SettingsManager.Instance.Interval)
            {
                KeyboardManager.DisableSystemKeys();
                var window = new MessageWindow(SettingsManager.Instance.Message);
                window.ShowDialog();
                KeyboardManager.EnableSystemKeys();
            }
        }
        public void Start()
        {

            if (!SettingsManager.Instance.IsEnabled)
                return;

            timer.Interval = TimeSpan.FromSeconds(1);

            if (!timer.IsEnabled)
                timer.Start();
        }
        public void Stop()
        {
            if (timer.IsEnabled)
                timer.Stop();
        }
    }
}
