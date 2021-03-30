using System;
using Android.OS;
using Android.Webkit;
using FriendLoc.Common.Services;
using FriendLoc.Common.Services.Impl;
using Newtonsoft.Json;

namespace FriendLoc.Droid.Services
{
    public class DroidNativeTrigger : NativeTrigger
    {
        WebView _webView;

        public DroidNativeTrigger()
        {

        }

        public override void Excute(string command, string data)
        {
            var jsCode = string.Format("getDataFromNativeApp( \'{0}\' , \'{1}\')", command, data);

            if (_webView != null)
            {
                _webView.Post(() =>
                {
                    try
                    {
                        if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
                        {
                            _webView.EvaluateJavascript(jsCode, null);
                        }
                        else
                        {
                            _webView.LoadUrl("javascript:" + jsCode);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message); 
                        // TODO: How to handle exceptions?
                    }
                }
                );
            }
            else
            {
                throw new NullReferenceException("WebView is null!");
            }
        }

        public override void Init(object data)
        {
            if (!(data is WebView))
            {
                throw new Exception("data param has to be Android WebView!");
            }
            _webView = (WebView)data;
        }
    }
}
