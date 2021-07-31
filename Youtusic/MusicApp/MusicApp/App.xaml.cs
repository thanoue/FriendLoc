using System;
using GalaSoft.MvvmLight.Ioc;
using MusicApp.Pages;
using MusicApp.Static;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI;

namespace MusicApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            XF.Material.Forms.Material.Init(this);

            Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.MaterialModule())
                                  .With(new Plugin.Iconize.Fonts.MaterialDesignIconsModule());

            var navigationService = Bootstrap.Instance.Setup();
            
            var nav =  new MaterialNavigationPage(new HomePage());
            
            navigationService.Initialize(nav);

            MainPage = nav;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
