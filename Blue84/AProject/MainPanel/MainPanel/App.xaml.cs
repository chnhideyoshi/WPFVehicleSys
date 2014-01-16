using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Runtime.InteropServices;

namespace MainPanel
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
            : base()
        {
            string[] s=Environment.GetLogicalDrives();
            if (s.Length < 4)
            {
                ShowCursor(0);
                IsInEmbededDevice = true;
            }
        }
        public bool IsInEmbededDevice = false;
        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        public static extern void ShowCursor(int status);
        public static ViewModel viewModel = new ViewModel();
    }
}
