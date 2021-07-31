using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace MusicApp.Views.Popups
{
    public class BasePopup : PopupPage
    {
        public void Dismis()
        {
            PopupNavigation.Instance.RemovePageAsync(this, true);
        }

        public void Show()
        {
            PopupNavigation.Instance.PushAsync(this, true);
        }
    }
}