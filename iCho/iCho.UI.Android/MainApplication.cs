using System;
using Android.App;
using Android.Runtime;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Syncfusion.SfCalendar.XForms.Droid;

namespace iCho.UI.Droid
{
    [Application(UsesCleartextTraffic = true)]
    public class MainApplication :Application
    {
        public MainApplication()
        {
        }

        protected MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Plugin.MaterialDesignControls.Android.Renderer.Init();
            Plugin.Iconize.Iconize.Init();
            RegisterServices();
        }

        void RegisterServices()
        {

        }
    }
}
