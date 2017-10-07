using Scheduler.ViewModels;
using System.Windows;
using System.Windows.Input;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Scheduler.Views
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
        }
    }
}
