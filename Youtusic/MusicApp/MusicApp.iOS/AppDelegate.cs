﻿using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Forms.Platform;
using Foundation;
using GalaSoft.MvvmLight.Ioc;
using MediaManager;
using MusicApp.iOS.Services;
using MusicApp.Services;
using MusicApp.Services.Impl;
using UIKit;

namespace MusicApp.iOS
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
            SimpleIoc.Default.Register<IFileService, IOSFileService>();
            SimpleIoc.Default.Register<ISecureStorageService, IOSSecureStorageService>();
            
            Rg.Plugins.Popup.Popup.Init();
            global::Xamarin.Forms.Forms.Init();
            Plugin.MaterialDesignControls.iOS.Renderer.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();           
            CachedImageRenderer.InitImageSourceHandler();
            CrossMediaManager.Current.Init();

            CrossMediaManager.Apple.Notification = new NotificationManager();

            XF.Material.iOS.Material.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}