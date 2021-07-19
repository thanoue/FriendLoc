using Syncfusion.SfCalendar.XForms.iOS;
using  Syncfusion.XForms.iOS.Graphics;
using Syncfusion.XForms.iOS.Border;
using Syncfusion.XForms.iOS.ComboBox;
using Syncfusion.XForms.iOS.TabView;
using Syncfusion.XForms.iOS.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using iCho.Core.Utils;

namespace iCho.UI.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Plugin.MaterialDesignControls.iOS.Renderer.Init();
            global::Xamarin.Forms.Forms.Init();
            SfCalendarRenderer.Init();
            SfGradientViewRenderer.Init();
            SfBorderRenderer.Init();
            SfComboBoxRenderer.Init();
            SfTabViewRenderer.Init();
            SfButtonRenderer.Init();
            LoadApplication(new App());

            FacebookClientManager.Initialize(app, options);

            return base.FinishedLaunching(app, options);
        }

        [Export("applicationDidBecomeActive:")]
        public override void OnActivated(UIApplication application)
        {
            FacebookClientManager.OnActivated();
        }

        [Export("application:openURL:options:")]
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if(ServiceInstances.SocialMediaAuthService == null)
                return base.OpenUrl(app, url, options);

            switch (ServiceInstances.SocialMediaAuthService.AuthType)
            {
                case SocialMediaType.None:
                default:
                    return base.OpenUrl(app, url, options);
                case SocialMediaType.Facebook:
                    return FacebookClientManager.OpenUrl(app, url, options);
                case SocialMediaType.Google:
                    return GoogleClientManager.OnOpenUrl(app, url, options);
            }
        }

        [Export("application:openURL:sourceApplication:annotation:")]
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return FacebookClientManager.OpenUrl(application, url, sourceApplication, annotation);
        }
    }
}
