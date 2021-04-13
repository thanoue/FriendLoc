using System;
using FriendLoc.Common.Models;
namespace FriendLoc.Common.Services
{
    public interface IQRCodeService
    {
        object GenerateQRCode(QRCodeData qRCodeData);
        void InitScanView(object surfaceView);
        void StartScanning(Action<string> onScanned);
        void StopScanning();
        string ScanFromImage(string path);
    }
}
