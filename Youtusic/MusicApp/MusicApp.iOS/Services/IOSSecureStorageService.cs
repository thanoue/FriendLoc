using System;
using Foundation;
using MusicApp.Services.Impl;

namespace MusicApp.iOS.Services
{
    public class IOSSecureStorageService : SecureStorageService
    {
        NSUserDefaults _plit;

        public IOSSecureStorageService()
        {
            _plit = NSUserDefaults.StandardUserDefaults;
        }

        public override bool Exist(string identifier)
        {
            try
            {
                var val = _plit.StringForKey(identifier);

                if (string.IsNullOrEmpty(val))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public override string Fetch(string identifier)
        {
            try
            {
                return _plit.StringForKey(identifier);
            }
            catch
            {
                return string.Empty;
            }
        }

        public override bool FetchBool(string identifier)
        {
            return true;
        }

        public override bool Remove(string identifier)
        {
            try
            {
                _plit.RemoveObject(identifier);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool Store(string identifier, string content)
        {
            try
            {
                _plit.SetString(content, identifier);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}