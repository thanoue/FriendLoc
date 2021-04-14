using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Content;
using Firebase.Auth;
using FriendLoc.Common;
using FriendLoc.Common.Models;
using FriendLoc.Common.Services;
using FriendLoc.Droid.Activities;
using FriendLoc.Droid.Dialogs;
using FriendLoc.Model;
using Google.Android.Material.Snackbar;
using Java.Interop;
using Newtonsoft.Json;
using Plugin.CurrentActivity;
using PopupMenu = AndroidX.AppCompat.Widget.PopupMenu;

namespace FriendLoc.Droid
{
   

    public static class DroidUtils
    {
        public static void ExecJavaScript(WebView webView, string jscode)
        {
        }

        public static Java.IO.File CreateNewFilePath(this Context context, string ext)
        {
            var imagePath = context.GetExternalFilesDir(Android.OS.Environment.DirectoryDcim);

            Log.Debug("PATH", imagePath.AbsolutePath);

            if (!imagePath.Exists() && !imagePath.Mkdirs())
            {
                Log.Debug("Florid", "failed to create directory");
            }

            string imageFileName = (System.DateTime.Now).ToString("yyyyMMdd_HHmmss") + ext;
            var image = new Java.IO.File(imagePath, imageFileName);

            return image;
        }

        public static void ShareImgFile(this Activity context, int requestCode, string filePath)
        {
            Intent share = new Intent(Intent.ActionSend);
            share.SetType(filePath.Contains(".png") ? "image/png" : "image/jpeg");

            var photoFile = new Java.IO.File(filePath);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                Android.Net.Uri photoURI = FileProvider.GetUriForFile(context,
                    "com.khoideptrai.friendloc.fileprovider",
                    photoFile);

                share.PutExtra(Intent.ExtraStream, photoURI);
            }
            else
                share.PutExtra(Intent.ExtraStream, Android.Net.Uri.FromFile(photoFile));

            context.StartActivityForResult(Intent.CreateChooser(share, "Share Image"), requestCode);
        }

        public static Bitmap OverlayBitmapToCenter(Bitmap bitmap, Bitmap bitmapOverlay)
        {
            if (bitmap == null)
                return null;
            int bitmapWidth = bitmap.Width;
            int bitmapHeight = bitmap.Height;
            int bitmapOverWidth = 120;
            int bitmapOverHeight = 120;

            float marginLeft = (float) (bitmapWidth * 0.5 - bitmapOverWidth * 0.5);
            float marginTop = (float) (bitmapHeight * 0.5 - bitmapOverHeight * 0.5);

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
            var newHeight = (int) (bitmap.Height * ((double) 120 / (double) bitmap.Width));

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

    public class OnMenuItemClickListener : Java.Lang.Object, PopupMenu.IOnMenuItemClickListener
    {
        private Action<IMenuItem> _onItemCicked;

        public OnMenuItemClickListener(Action<IMenuItem> onItemCicked)
        {
            _onItemCicked = onItemCicked;
        }

        public bool OnMenuItemClick(IMenuItem? item)
        {
            _onItemCicked?.Invoke(item);

            return true;
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