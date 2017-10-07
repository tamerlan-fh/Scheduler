using Scheduler.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Scheduler
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon notifyIcon;
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                Process thisProc = Process.GetCurrentProcess();
                // Check how many total processes have the same name as the current one
                Process[] procs = Process.GetProcessesByName(thisProc.ProcessName);
                if (procs.Length > 1)
                    Application.Current.Shutdown();

                base.OnStartup(e);
                notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            }
            catch (Exception ex)
            {
                Application.Current.Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
