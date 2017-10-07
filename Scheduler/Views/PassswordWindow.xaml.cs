using Scheduler.ViewModels;
using System.Windows;

namespace Scheduler.Views
{
    /// <summary>
    /// Логика взаимодействия для PassswordWindow.xaml
    /// </summary>
    public partial class PassswordWindow : Window
    {
        public PassswordWindow()
        {
            InitializeComponent();
            this.DataContext = new PasswordViewModel();
        }
    }
}
