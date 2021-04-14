using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Gms.Extensions;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using FriendLoc.Common;
using FriendLoc.Common.Models;
using FriendLoc.Common.Services;
using Java.Util;
using Newtonsoft.Json;
using Xamarin.Google.MLKit.Vision.BarCode;
using Xamarin.Google.MLKit.Vision.BarCode.Internal;
using Xamarin.Google.MLKit.Vision.Common;
using ZXing;
using ZXing.Common;
using ZXing.Mobile;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace FriendLoc.Droid.Services
{
    public class QRCodeService : IQRCodeService
    {
        private ZXingSurfaceView _scannerView;
        private Context _context;
        public QRCodeService()
        {
        }
        public QRCodeService(Context context)
        {
            _context = context;
        }

        public object GenerateQRCode(QRCodeData data)
        {
            var qrCode =
                GenerateQRCode(data.Data is string ? (string)data.Data : JsonConvert.SerializeObject(data.Data));

            Bitmap bitmap = data.Image == null ? null : (Bitmap)data.Image;

            var logoInside = DroidUtils.CreateRoundedBitmap(bitmap, 120);

            return DroidUtils.OverlayBitmapToCenter(qrCode, logoInside);
        }

        public void InitScanView(object surfaceViewBackground)
        {
            var scanOptions = new MobileBarcodeScanningOptions();

            scanOptions.UseFrontCameraIfAvailable = false;
            scanOptions.AutoRotate = true;

            _scannerView = new ZXingSurfaceView(_context, scanOptions);

            if (surfaceViewBackground is ViewGroup)
            {
                ((ViewGroup)surfaceViewBackground).AddView(_scannerView, new ViewGroup.LayoutParams(-1, -1));
            }
        }

        public async Task<string> ScanFromImage(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var memory = new MemoryStream())
                {
                    stream.CopyTo(memory);

                    var bytes = memory.ToArray();

                    BarcodeScannerOptions options = new BarcodeScannerOptions.Builder()
                    .SetBarcodeFormats(Barcode.FormatQrCode, Barcode.FormatDataMatrix).Build();

                    InputImage image;
                    try
                    {
                        image = InputImage.FromFilePath(_context,Android.Net.Uri.FromFile(new Java.IO.File(path)));
                    }
                    catch (IOException e)
                    {
                        ServiceInstances.LoggerService.Error(e.ToString());
                        return "";
                    }

                    var scanner = BarcodeScanning.GetClient(options);

                    var res = "";

                   var codes = await scanner.Process(image)
                        .AddOnFailureListener(new FailureLisenter((exp) =>
                        {
                            ServiceInstances.LoggerService.Error(exp.ToString());
                            res = "";

                        }));

                    if(codes != null)
                    {
                        var results = codes.JavaCast<JavaList<Barcode>>();

                        res = ((Barcode)results.Get(0)).DisplayValue;
                    }

                    return res;
                }
            }


        }
        public void StartScanning(Action<string> onScanned)
        {
            _scannerView?.StartScanning((res) =>
            {
                if (string.IsNullOrEmpty(res.Text))
                    return;

                onScanned?.Invoke(res.Text);

                Vibrator vib = (Vibrator)_context.GetSystemService(Context.VibratorService);
                vib.Vibrate(1000);

            });
        }

        public void StopScanning()
        {
            _scannerView?.StopScanning();
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