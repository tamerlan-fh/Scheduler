using Scheduler.ViewModels;
using System.Windows;
using System.Windows.Input;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Scheduler
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            notifyIcon.TrayMouseDoubleClick += NotifyIconTrayMouseDoubleClick;
        }

        private void NotifyIconTrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            this.Show();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
            }
            base.OnStateChanged(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            notifyIcon.Dispose();
            base.OnClosing(e);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
