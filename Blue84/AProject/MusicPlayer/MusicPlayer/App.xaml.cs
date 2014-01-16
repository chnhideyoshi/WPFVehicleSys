using System.Windows;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
        public App()
            : base()
        {
            string[] s = Environment.GetLogicalDrives();
            if (s.Length < 4)
            {
                ShowCursor(0);
            }
            this.DispatcherUnhandledException += (sender, e) => { Shutdown(); };
        }
        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        public static extern void ShowCursor(int status);
        protected override void OnStartup(StartupEventArgs e)
        {
            // Get Reference to the current Process
            Process thisProc = Process.GetCurrentProcess();
            // Check how many total processes have the same name as the current one
            if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
            {
                // If ther is more than one, than it is already running.
                //MessageBox.Show("Application is already running.");
                Application.Current.Shutdown();
                return;
            }

            base.OnStartup(e);
        }

    }
}
