using Scheduler.Views;
using System.Windows;
using System.Windows.Input;

namespace Scheduler.ViewModels
{
    class NotifyIconViewModel : ViewModelBase
    {
        public ICommand ShowWindowCommand { get; private set; }
        public ICommand HideWindowCommand { get; private set; }
        public ICommand ExitApplicationCommand { get; private set; }

        public NotifyIconViewModel()
        {
            ShowWindowCommand = new RelayCommand(p => ShowWindow(), p => Application.Current.MainWindow == null);
            HideWindowCommand = new RelayCommand(p => Application.Current.MainWindow.Close(), p => Application.Current.MainWindow != null);
            ExitApplicationCommand = new RelayCommand(p => ExitApplication());
            SchedulerManager.Instance.Start();

            if (!SettingsManager.Instance.StateIsMinimized)
                ShowWindow();
        }

        private void ShowWindow()
        {
            var password = new PassswordWindow();
            if (password.ShowDialog() != true)
                return;

            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();
        }

        private void ExitApplication()
        {
            var password = new PassswordWindow();
            if (password.ShowDialog() != true)
                return;

            SchedulerManager.Instance.Stop();
            Application.Current.Shutdown();
        }
    }
}
