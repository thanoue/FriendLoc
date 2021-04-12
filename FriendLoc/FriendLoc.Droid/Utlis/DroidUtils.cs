using System;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Webkit;
using Firebase.Auth;
using FriendLoc.Common;
using FriendLoc.Common.Models;
using FriendLoc.Common.Services;
using FriendLoc.Model;
using Java.Interop;
using Newtonsoft.Json;

namespace FriendLoc.Droid
{
    public static class DroidUtils
    {
        public static void ExecJavaScript(WebView webView, string jscode)
        {
           
        }

        public static Bitmap OverlayBitmapToCenter(Bitmap bitmap, Bitmap bitmapOverlay)
        {
            if (bitmap == null)
                return null;
            int bitmapWidth = bitmap.Width;
            int bitmapHeight = bitmap.Height;
            int bitmapOverWidth = 120;
            int bitmapOverHeight = 120;

            float marginLeft = (float)(bitmapWidth * 0.5 - bitmapOverWidth * 0.5);
            float marginTop = (float)(bitmapHeight * 0.5 - bitmapOverHeight * 0.5);

            Bitmap overlayBitmap = Bitmap.CreateBitmap(bitmapWidth, bitmapHeight, bitmap.GetConfig());
            Canvas canvas = new Canvas(overlayBitmap);

            canvas.DrawBitmap(bitmap, new Matrix(), null);
            if (bitmapOverlay != null)
                canvas.DrawBitmap(bitmapOverlay, marginLeft, marginTop, null);

            return overlayBitmap;
        }

        public static Bitmap CreateRoundedBitmap(Bitmap bitmap, int size)
        {
            if (bitmap == null)
                return null;

            var newWidth = size;
            var newHeight = (int)(bitmap.Height * ((double)120 / (double)bitmap.Width));

            var bitmapScalled = Bitmap.CreateScaledBitmap(bitmap, newWidth, newHeight, true);

            Bitmap output = Bitmap.CreateBitmap(size, size, Bitmap.Config.Argb8888);

            var canvas = new Canvas(output);
            var paint = new Paint();
            var rect = new Rect(0, 0, size, size);
            var rectF = new RectF(rect);

            paint.AntiAlias = true;

            canvas.DrawARGB(0, 0, 0, 0);
            canvas.DrawOval(rectF, paint);

            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));

            canvas.DrawBitmap(bitmapScalled, rect, rect, paint);

            return output;
        }

    }

    public class DialogLisenter : Java.Lang.Object, IDialogInterfaceOnClickListener
    {
        Action<int> _onSelected;

        public DialogLisenter(Action<int> onSelected)
        {
            _onSelected = onSelected;
        }

        public void OnClick(IDialogInterface dialog, int which)
        {
            _onSelected?.Invoke(which);
        }
    }

    public class JavascriptClient : Java.Lang.Object
    {
        IWebTrigger _trigger;

        public JavascriptClient(IWebTrigger trigger)
        {
            _trigger = trigger;
        }

        [Android.Webkit.JavascriptInterface]
        [Export("nativeInvoke")]
        public void NativeInvoke(string data)
        {
            var model = JsonConvert.DeserializeObject<WebViewInvokeModel>(data);

            switch (model.Command)
            {
                case Command.documentIsReady:

                    _trigger?.OnReady();

                    break;

                case Command.locationUpdated:

                    _trigger?.LocationUpdated(JsonConvert.DeserializeObject<Coordinate>(model.Data));

                    break;
            }
        }
    }   

    public class MyWebChromeClient : WebChromeClient
    {
        public MyWebChromeClient()
        {
        }

        [Obsolete]
        public override void OnConsoleMessage(string message, int lineNumber, string sourceID)
        {
            base.OnConsoleMessage(message, lineNumber, sourceID);

            Console.WriteLine(string.Format("LOG: {0}, lineNumber: {1}, file: {2}", message, lineNumber, sourceID));
        }
    }
}
