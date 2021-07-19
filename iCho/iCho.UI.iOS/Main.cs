using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using iCho.Service;
using iCho.UI.iOS.Services;
using UIKit;

namespace iCho.UI.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            RegisterServices();
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }

        static void RegisterServices()
        {
            ServiceLocator.Instance.Register<ISocialMediaAuth, IOSSocialMediaAuth>();
        }
    }
}
