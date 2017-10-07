using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Scheduler.ViewModels
{
    class TrayWindowViewModel:ViewModelBase
    {
        private DispatcherTimer timer;

        public string Timestamp
        {
            get { return DateTime.Now.ToLongTimeString(); }
        }


        public TrayWindowViewModel()
        {
            timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, OnTimerTick, Application.Current.Dispatcher);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            //fire a property change event for the timestamp
            Application.Current.Dispatcher.BeginInvoke(new Action(() => OnPropertyChanged("Timestamp")));
        }

    }
}
