using System;
using FriendLoc.Common.Models;
namespace FriendLoc.Common.Services
{
    public interface IQRCodeService
    {
        object GenerateQRCode(QRCodeData qRCodeData);
        void InitScanView(object surfaceView);
        void StartLoading(Action<string> onScanned);
    }
}
