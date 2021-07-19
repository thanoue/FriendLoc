using System;
using Android.App;
using iCho.Service;
using Plugin.FacebookClient;
using Plugin.GoogleClient;

namespace iCho.UI.Droid.Services
{
    public class DroidSocialMediaAuth : BaseSocialMediaAuth
    {
        public override string PlatformType => "ANDROID";

        Activity _context;

        public DroidSocialMediaAuth(Activity contex)
        {
            _context = contex;
        }

        public DroidSocialMediaAuth()
        {

        }

        public override void Init()
        {
            FacebookClientManager.Initialize(_context);
            GoogleClientManager.Initialize(_context, "840849713145-3gnke80ungben5rj9vuaa4gbr8av6cv9.apps.googleusercontent.com", "840849713145-3gnke80ungben5rj9vuaa4gbr8av6cv9.apps.googleusercontent.com");
        }
    }
}
