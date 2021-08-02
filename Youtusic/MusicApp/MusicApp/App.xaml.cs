using System;
using GalaSoft.MvvmLight.Ioc;
using MusicApp.Pages;
using MusicApp.Static;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI;

namespace MusicApp
{
    public partial class App : Application
    {
        bool _isFirstResume = true;

        public App(string id = "")
        {
            InitializeComponent();

            XF.Material.Forms.Material.Init(this);

            Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.MaterialModule())
                                  .With(new Plugin.Iconize.Fonts.MaterialDesignIconsModule());

            var navigationService = Bootstrap.Instance.Setup();
            
            var nav =  new MaterialNavigationPage(new HomePage(id));
            
            navigationService.Initialize(nav);

            MainPage = nav;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override async void OnResume()
        {
            if (Device.RuntimePlatform == Device.Android)
                return;

            if (_isFirstResume)
            {
                _isFirstResume = false;
                return;
            }

            var hasText = Clipboard.HasText;

            if (!hasText)
                return;

            var text = await Clipboard.GetTextAsync();

            var id = Utils.GetId(text);

            if(id != null)
            {
                Clipboard.SetTextAsync("");

                MessagingCenter.Send<object, UrlModel>(this, Constants.GET_SONG_FROM_YOUTUBE, id);
            }
        }
    }
}
