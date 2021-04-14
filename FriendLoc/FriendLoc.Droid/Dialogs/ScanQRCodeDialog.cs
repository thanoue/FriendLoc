using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Button;
using Android.Views;
using Android.Widget;
using FriendLoc.Common;
using FriendLoc.Droid.Activities;

namespace FriendLoc.Droid.Dialogs
{
    public class ScanQRCodeDialog : BaseDialog,IImgSelectionObj
    {
        protected override int LayoutResId => Resource.Layout.dialog_scan_qr_code;
        protected override string Title => "Scan QR Code";
        protected override string TAG => nameof(ScanQRCodeDialog);

        private Action<string> _onSanned;

        private MaterialButton _pickFileBtn;
        
        public ScanQRCodeDialog(Context context,Action<string> onSanned) : base(context)
        {
            _onSanned = onSanned;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var scanBg = view.FindViewById<RelativeLayout>(Resource.Id.scanViewBg);
            _pickFileBtn = view.FindViewById<MaterialButton>(Resource.Id.getFromFile);
            
            ServiceInstances.QRCodeService.InitScanView(scanBg);

            scanBg.Post(() =>
            {
                ServiceInstances.QRCodeService.StartScanning((res) =>
                {
                    ScanSucess(res);
                });
            });
            
            _pickFileBtn.Click+= delegate(object sender, EventArgs args)
            {
                CurrentActivity.SetImgSelectionListner(this);
                CurrentActivity.SelectImageFromGallery();
            };
        }

        void ScanSucess(string res)
        {
            ServiceInstances.QRCodeService.StopScanning();
            _onSanned?.Invoke(res);
            this.Dismiss();
        }

        public async void OnImgSelected(string path, Activity activity)
        {
            UtilUI.StartLoading();

            var res = await ServiceInstances.QRCodeService.ScanFromImage(path);

            if (!string.IsNullOrEmpty(res))
            {
                ScanSucess(res);
                UtilUI.StopLoading();
            }
            else
            {
                UtilUI.StopLoading();
            }
        }
    }
}