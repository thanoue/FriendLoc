using System;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using FriendLoc.Droid.Activities;
using Google.Android.Material.AppBar;
using Plugin.CurrentActivity;

namespace FriendLoc.Droid.Dialogs
{
    public abstract class BaseDialog : DialogFragment, AndroidX.AppCompat.Widget.Toolbar.IOnMenuItemClickListener
    {
        protected abstract int LayoutResId { get; }
        protected abstract string Title { get; }
        protected abstract string TAG { get; }
        protected virtual DialogTypes DialogTypes => DialogTypes.Popup;
        public BaseActivity CurrentActivity => (BaseActivity)CrossCurrentActivity.Current.Activity;

        LoadingDialog _loadingDialog;

        Context _context;
        public BaseDialog(Context context)
        {
            _context = context;

            switch (DialogTypes)
            {
                case DialogTypes.Popup:

                    SetStyle(Resource.Style.ThemeOverlay_MaterialComponents_MaterialAlertDialog, Resource.Style.ThemeOverlay_MaterialComponents_MaterialAlertDialog);

                    break;

                case DialogTypes.FullScreen:

                    SetStyle(Resource.Style.ShapeAppearanceOverlay_MaterialComponents_MaterialCalendar_Window_Fullscreen, Resource.Style.ShapeAppearanceOverlay_MaterialComponents_MaterialCalendar_Window_Fullscreen);

                    break;
            }
            Cancelable = false;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.DialogAnimation;

            var baseView = inflater.Inflate(Resource.Layout.dialog_base, container, false);

            inflater.Inflate(LayoutResId, baseView.FindViewById<RelativeLayout>(Resource.Id.contentView), true);

            var appBar = baseView.FindViewById<MaterialToolbar>(Resource.Id.topAppBar);

            appBar.SetOnMenuItemClickListener(this);

            appBar.Title = Title;

            return baseView;
        }

        public void StartLoading()
        {
            CrossCurrentActivity.Current.Activity.RunOnUiThread(() =>
            {
                _loadingDialog = new LoadingDialog();
                _loadingDialog.Show(((BaseActivity)CrossCurrentActivity.Current.Activity).SupportFragmentManager, _loadingDialog.Tag);

            });
        }

        public void StopLoading()
        {
            CrossCurrentActivity.Current.Activity.RunOnUiThread(() =>
            {
                _loadingDialog.Dismiss();
            });
        }

        public void ShowDialog()
        {
            this.Show(((BaseActivity)CrossCurrentActivity.Current.Activity).SupportFragmentManager, TAG);
        }

        public bool OnMenuItemClick(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.closeIcon:

                    this.Dismiss();

                    break;
            }
            return true;
        }
    }

    public enum DialogTypes
    {
        FullScreen,
        Popup
    }
}
