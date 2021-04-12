using System;
using System.Collections.Generic;
using Android.Graphics;
using FriendLoc.Common.Models;
using FriendLoc.Common.Services;
using Newtonsoft.Json;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;

namespace FriendLoc.Droid.Services
{
    public class QRCodeService : IQRCodeService
    {
        public QRCodeService()
        {
        }

        public object GenerateQRCode(QRCodeData data)
        {
            var qrCode = GenerateQRCode(data.Data is string ? (string)data.Data : JsonConvert.SerializeObject(data.Data));

            Bitmap bitmap = data.Image == null ? null : (Bitmap)data.Image;

            var logoInside = DroidUtils.CreateRoundedBitmap(bitmap, 120);

            return DroidUtils.OverlayBitmapToCenter(qrCode, logoInside);
        }

        public void InitScanView(object surfaceView)
        {
        }

        public void StartLoading(Action<string> onScanned)
        {
        }

        public Bitmap GenerateQRCode(string data)
        {
            BitMatrix bitMatrix;

            IDictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>();
            hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
            hints.Add(EncodeHintType.MARGIN, 0);

            try
            {
                bitMatrix = new MultiFormatWriter().encode(data, BarcodeFormat.QR_CODE, 800, 800, hints);
            }
            catch (Java.Lang.IllegalArgumentException illegalargumentexception)
            {
                throw illegalargumentexception;
            }

            int[] pixels = new int[bitMatrix.Width * bitMatrix.Height];
            for (int y = 0; y < bitMatrix.Height; y++)
            {
                for (int x = 0; x < bitMatrix.Width; x++)
                {
                    pixels[y * bitMatrix.Width + x] = bitMatrix[x, y] ? Color.Black : Color.White;
                }
            }

            Bitmap qrCode = Bitmap.CreateBitmap(bitMatrix.Width, bitMatrix.Height, Bitmap.Config.Argb4444);
            qrCode.SetPixels(pixels, 0, 800, 0, 0, bitMatrix.Width, bitMatrix.Height);

            return qrCode;
        }
    }
}
