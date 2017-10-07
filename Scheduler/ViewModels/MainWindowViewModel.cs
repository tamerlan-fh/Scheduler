using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;

namespace Scheduler.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        public ICommand ApplyConfigCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
        public ICommand HideWindowCommand { get; private set; }
        public MainWindowViewModel()
        {
            Interval = SettingsManager.Instance.Interval;
            IsEnabled = SettingsManager.Instance.IsEnabled;
            Message = SettingsManager.Instance.Message;
            StateIsMinimized = SettingsManager.Instance.StateIsMinimized;
            RunTogetherWithWindows = SettingsManager.Instance.RunTogetherWithWindows;

            ApplyConfigCommand = new RelayCommand(param => this.ApplyConfig(), param => { return amended; });
            ExitCommand = new RelayCommand(param => this.Exit());
            HideWindowCommand = new RelayCommand(param => this.HideWindow(param));

            amended = false;
            if (IsEnabled)
                SchedulerManager.Instance.Start();
            else
                SchedulerManager.Instance.Stop();
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
        /// Имеются внесенные поправки
        /// </summary>
        private bool amended;
        private void ApplyConfig()
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

            if (IsEnabled)
                SchedulerManager.Instance.Start();
            else
                SchedulerManager.Instance.Stop();
        }
        private void Exit()
        {
            SchedulerManager.Instance.Stop();
            App.Current.Shutdown();
        }
        private void HideWindow(object window)
        {
            if (window == null || !(window is Window))
                return;

            (window as Window).Close();
        }
        private void AddAutorun()
        {
            string name = "Scheduler";//Application name    
            string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            using (var reg = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run\"))
            {
                if (reg.GetValue(name) == null)
                    reg.SetValue(name, location);
            }
        }
        private void RemoveAutorun()
        {
            string name = "Scheduler";//Application name    
            string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            using (var reg = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run\"))
            {
                if (reg.GetValue(name) != null)
                    reg.DeleteValue(name);
            }
        }
    }
}
