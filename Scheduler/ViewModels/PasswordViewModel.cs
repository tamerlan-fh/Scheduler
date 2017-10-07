using Scheduler.Views;
using System.Windows.Input;

namespace Scheduler.ViewModels
{
    class PasswordViewModel : ViewModelBase
    {
        public ICommand ApplyPasswordCommand { get; private set; }
        public ICommand CanselCommand { get; private set; }
        public bool NotSuccess
        {
            get { return notSuccess; }
            set { notSuccess = value; OnPropertyChanged("NotSuccess"); }
        }
        private bool notSuccess;

        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged("Password"); if (NotSuccess) NotSuccess = false; }
        }
        private string password;

        public PasswordViewModel()
        {
            NotSuccess = false;
            ApplyPasswordCommand = new RelayCommand(p => ApplyPassword(p));
            CanselCommand = new RelayCommand(p => Cansel(p));
        }

        private void ApplyPassword(object parameter)
        {
            if (parameter == null || !(parameter is PassswordWindow))
                return;


            if (Password != SettingsManager.Instance.Password)
                NotSuccess = true;
            else
            {
                var passswordWindow = parameter as PassswordWindow;
                passswordWindow.DialogResult = true;
                NotSuccess = false;
                passswordWindow.Close();
            }
        }

        private void Cansel(object parameter)
        {
            if (parameter == null || !(parameter is PassswordWindow))
                return;
            var passswordWindow = parameter as PassswordWindow;
            passswordWindow.DialogResult = false;
            passswordWindow.Close();
        }
    }
}
