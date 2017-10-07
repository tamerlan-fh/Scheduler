using Microsoft.Win32;
using Scheduler.Properties;
using System;
using System.Collections.Generic;
using System.IO;
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
            //Message = string.Format("{0}", LastInputInfoManager.Instance.GetTimeAfterLastInput());

            if (LastInputInfoManager.Instance.GetTimeAfterLastInput() >= Interval)
            {
                KeyboardManager.DisableSystemKeys();
                var window = new MessageWindow(Message);
                window.ShowDialog();
                KeyboardManager.EnableSystemKeys();
            }
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

        public bool RunTogetherWithWindows
        {
            get { return runTogetherWithWindows; }
            set
            {
                if (runTogetherWithWindows == value) return;
                runTogetherWithWindows = value; OnPropertyChanged("RunTogetherWithWindows"); amended = true;
            }
        }
        private bool runTogetherWithWindows;
        public bool StateIsMinimized
        {
            get { return stateIsMinimized; }
            set
            {
                if (stateIsMinimized == value) return;
                stateIsMinimized = value; OnPropertyChanged("StateIsMinimized"); amended = true;
            }
        }
        private bool stateIsMinimized;
        /// <summary>
        /// Внесенный поправки
        /// </summary>
        private bool amended;

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
            SettingsManager.Instance.RunTogetherWithWindows = RunTogetherWithWindows;
            SettingsManager.Instance.StateIsMinimized = StateIsMinimized;
            SettingsManager.Instance.Save();

            if (RunTogetherWithWindows)
                AddAutorun();
            else
                RemoveAutorun();

            amended = false;
        }
        private void LoadConfig()
        {
            Interval = SettingsManager.Instance.Interval;
            IsEnabled = SettingsManager.Instance.IsEnabled;
            Message = SettingsManager.Instance.Message;
            StateIsMinimized = SettingsManager.Instance.StateIsMinimized;
            RunTogetherWithWindows = SettingsManager.Instance.RunTogetherWithWindows;
        }
        private void StartScheduler()
        {
            if (!IsEnabled)
            {
                if (timer.IsEnabled) timer.Stop();
                return;
            }
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }
        private void Exit()
        {
            if (timer.IsEnabled)
                timer.Stop();
            App.Current.Shutdown();
        }

        private void AddAutorun()
        {
            string name = "Scheduler";//Application name    
            string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            using (var reg = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run\"))
            {
                reg.SetValue(name, location);
            }
        }

        private void RemoveAutorun()
        {
            string name = "Scheduler";//Application name    
            string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            using (var reg = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run\"))
            {
                reg.DeleteValue(name);
            }
        }
    }
}
