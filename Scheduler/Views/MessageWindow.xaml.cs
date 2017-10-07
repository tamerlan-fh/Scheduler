using System.Windows;

namespace Scheduler.Views
{
    /// <summary>
    /// Логика взаимодействия для MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindow(string message)
        {
            InitializeComponent();
            Message.Text = message;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
