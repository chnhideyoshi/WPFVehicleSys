using System.Windows;

namespace WpfCalendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Topmost = (App.Current as App).IsTopMost;
        }
        private void ExitBtnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        
    }
}
