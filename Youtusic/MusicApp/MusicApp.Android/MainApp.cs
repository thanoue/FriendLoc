using System;
using Android.App;
using Android.Runtime;

namespace MusicApp.Droid
{
    [Application(UsesCleartextTraffic = true)]
    public class MainApp : Application
    {
        public MainApp()
        {
        }

        protected MainApp(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();
        }
    }
}
