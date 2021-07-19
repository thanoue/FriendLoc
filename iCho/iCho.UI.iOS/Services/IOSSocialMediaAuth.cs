using System;
using iCho.Service;
using Plugin.GoogleClient;

namespace iCho.UI.iOS.Services
{
    public class IOSSocialMediaAuth : BaseSocialMediaAuth
    {
        public IOSSocialMediaAuth()
        {
        }

        public override string PlatformType => "IOS";

        public override void Init()
        {
            GoogleClientManager.Initialize("840849713145-627cs6vkefmd1mp82it04l6q4pjihvd5.apps.googleusercontent.com");
        }
    }
}
