using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
namespace MusicPlayer.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private bool _listVisible = true;

        public bool ListVisible
        {
            get
            {
                return _listVisible;
            }
            set
            {
                _listVisible = value;
                RaisePropertyChanged("ListVisible");
            }
        }
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
            }
            else
            {
                Messenger.Default.Register<bool>(this, true, p => ListVisible = p);
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}