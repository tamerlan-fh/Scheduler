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
        private MainWindowViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;
            notifyIcon.TrayMouseDoubleClick += NotifyIconTrayMouseDoubleClick;

            if (viewModel.StateIsMinimized)
                this.Hide();
        }

        private void NotifyIconTrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            this.Show();
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
