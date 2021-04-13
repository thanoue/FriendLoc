using System;
using System.IO;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using FriendLoc.Common;
using FriendLoc.Common.Models;
using FriendLoc.Droid.Activities;
using Google.Android.Material.Button;

namespace FriendLoc.Droid.Dialogs
{
    public class ViewQRCodeDialog : BaseDialog
    {
        protected override int LayoutResId => Resource.Layout.dialog_view_qr_code;
        protected override string Title => "View Trip QR Code";
        protected override string TAG => nameof(ViewQRCodeDialog);

        string _qrContent = "";

        ImageView _qrImg;
        MaterialButton _shareBtn;

        public ViewQRCodeDialog(Context context, string qrContent) : base(context)
        {
            _qrContent = qrContent;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _qrImg = view.FindViewById<ImageView>(Resource.Id.qrImg);
            _shareBtn = view.FindViewById<MaterialButton>(Resource.Id.shareBtn);

            var qrData = new QRCodeData()
            {
                Data = _qrContent,
                Image = BitmapFactory.DecodeResource(Context.Resources, Resource.Mipmap.logo)
            };

            var qrCode = (Bitmap) ServiceInstances.QRCodeService.GenerateQRCode(qrData);

            _qrImg.SetImageBitmap(qrCode);

            _shareBtn.Click += delegate
            {
                var newImg = Context.CreateNewFilePath(".png");

                Log.Debug("URL", newImg.AbsolutePath);

                using (var stream = new FileStream(newImg.AbsolutePath, FileMode.Create))
                {
                    qrCode.Compress(Bitmap.CompressFormat.Png, 1, stream);

                    CurrentActivity.ShareImgFile(BaseActivity.REQUEST_SHARE_IMAGE,newImg.AbsolutePath);
                }
            };
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            GC.Collect();
        }
    }
}