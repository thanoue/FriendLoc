using MusicApp.Views.Popups;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MusicApp.Static
{
    public class StaticUI
    {
        private static StaticUI instance;

        private LoadingPopup _popup;

        public static StaticUI Instance
        {
            get
            {
                if (instance == null)
                    instance = new StaticUI();

                return instance;
            }
        }

        public void ToastMesage(string msg, int msDuration = 3000)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                //await MaterialDialog.Instance.SnackbarAsync(message: msg,
                //                       actionButtonText: "Got It",
                //                       msDuration: msDuration);
            });
       
        }

        public void StartLoading(string message = "Loading...")
        {
            if (_popup != null)
                return;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                _popup = new LoadingPopup(message);

                _popup.Show();
            });
        
        }

        public void StopLoading()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _popup?.Dismis();
                _popup = null;
            });
          
        }
    }
}