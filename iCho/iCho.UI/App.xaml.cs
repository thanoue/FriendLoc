using System;
using iCho.Service;
using iCho.UI.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LoginPage = iCho.UI.Views.Forms.LoginPage;

namespace iCho.UI
{
    public partial class App : Application
    {
        public App()
        {
            RegisterServices();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDU1MTM4QDMxMzkyZTMxMmUzMGtxR3B3NHRnWEQ3bkZNZTY0Mnl6V0VuWndGVXRPOU1nalJWS0NoUG9FeXM9;NDU1MTM5QDMxMzkyZTMxMmUzMG00M1FoWVRLRldCaEpDNmJmVm8xWnh6TEt6ZWFuNGZaK3A1Wlg1RlJBRUE9");

            Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.MaterialModule())
                                  .With(new Plugin.Iconize.Fonts.MaterialDesignIconsModule());

            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

        void RegisterServices()
        {
            ServiceLocator.Instance.Register<IApiClient, ApiClient>();
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
