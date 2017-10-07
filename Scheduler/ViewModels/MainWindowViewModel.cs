using Scheduler.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Scheduler.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        public ICommand ApplyConfigCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }

        public MainWindowViewModel()
        {
            timer = new DispatcherTimer();
            timer.Tick += TimerTick;

            LoadConfig();
            ApplyConfigCommand = new RelayCommand(param => this.ApplyConfig(), param => { return amended; });
            ExitCommand = new RelayCommand(param => this.Exit());

            StartScheduler();
            amended = false;
        }


        private DispatcherTimer timer;
        private void TimerTick(object sender, EventArgs e)
        {
            TimeLeft -= timer.Interval;
            if (TimeLeft.TotalSeconds > 1) return;

            // MessageBox.Show(Message, "Планировщик", MessageBoxButton.OK, MessageBoxImage.Information);
            var window = new MessageWindow(Message);
            window.ShowDialog();

            //if (ModeInTime)
            //    TimeLeft = TimeSpanBetween(new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second), Time);
            //else
            //    TimeLeft = Interval;
            TimeLeft = Interval;
        }

        public string Message
        {
            get { return message; }
            set
            {
                if (Message == value) return;
                message = value; OnPropertyChanged("Message"); amended = true;
            }
        }
        private string message;

        public TimeSpan Interval
        {
            get { return interval; }
            set
            {
                if (Interval == value) return;
                interval = value; OnPropertyChanged("Interval"); amended = true;
            }
        }
        private TimeSpan interval;

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (IsEnabled == value) return;
                isEnabled = value; OnPropertyChanged("IsEnabled"); amended = true;
            }
        }
        private bool isEnabled;

        /// <summary>
        /// Внесенный поправки
        /// </summary>
        private bool amended;

        /// <summary>
        /// Оставшееся время до выполнения задания
        /// </summary>
        public TimeSpan TimeLeft
        {
            get { return timeLeft; }
            set { timeLeft = value; OnPropertyChanged("TimeLeft"); }
        }
        private TimeSpan timeLeft;
        private void ApplyConfig()
        {
            SaveConfig();
            StartScheduler();
        }
        private void SaveConfig()
        {
            SettingsManager.Instance.Interval = Interval;
            SettingsManager.Instance.IsEnabled = IsEnabled;
            SettingsManager.Instance.Message = Message;
            SettingsManager.Instance.Save();
            amended = false;
        }
        private void LoadConfig()
        {
            Interval = SettingsManager.Instance.Interval;
            IsEnabled = SettingsManager.Instance.IsEnabled;
            Message = SettingsManager.Instance.Message;
        }
        private void StartScheduler()
        {
            if (!IsEnabled)
            {
                if (timer.IsEnabled) timer.Stop();
                TimeLeft = TimeSpan.FromTicks(0);
                return;
            }

            TimeLeft = Interval;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }
        private void Exit()
        {
            if (timer.IsEnabled)
                timer.Stop();
            App.Current.Shutdown();
        }
        //private TimeSpan TimeSpanBetween(TimeSpan start, TimeSpan end)
        //{
        //    var ts = end - start;
        //    if (ts.TotalDays < 0) ts += TimeSpan.FromDays((int)Math.Round(Time.TotalDays, 0) + 1);
        //    if (ts.TotalDays > 1) ts += TimeSpan.FromDays(-(int)Math.Round(Time.TotalDays, 0));
        //    return ts;
        //}
    }
}
