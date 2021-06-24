
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using BumpTech.GlideLib;
using Com.Nguyenhoanglam.Imagepicker.UI.Imagepicker;
using FriendLoc.Common;
using FriendLoc.Common.Models;
using FriendLoc.Controls;
using FriendLoc.Droid.Activities;
using FriendLoc.Droid.Adapters;
using FriendLoc.Droid.Dialogs;
using FriendLoc.Droid.ViewModels;
using FriendLoc.Entities;
using FriendLoc.Entity;
using Google.Android.Material.Button;
using Google.Android.Material.Chip;
using Google.Android.Material.DatePicker;
using Google.Android.Material.Dialog;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.ImageView;
using Google.Android.Material.TextField;
using Java.Util;
using Newtonsoft.Json;
using Pair = AndroidX.Core.Util.Pair;
using TimeZone = Java.Util.TimeZone;

namespace FriendLoc.Droid.Dialogs
{
    public class AddTripDialog : BaseDialog, IImgSelectionObj, IMaterialPickerOnPositiveButtonClickListener
    {
        protected override int LayoutResId => Resource.Layout.dialog_add_trip;
        protected override string Title => "Add new Trip";
        protected override DialogTypes DialogTypes => DialogTypes.FullScreen;
        protected override string TAG => nameof(AddTripDialog);

        private ShapeableImageView _avtImg;
        private CustomEditText _periodTxt, _nameTxt, _descriptionTxt;
        private MaterialButton _saveBtn;
        private TextInputEditText _periodBtn;
        private Chip _startPoint, _endPoint;

        private IList<SpinnerItem> _mileStones;
        private Trip _trip;
        private string _avtUrl;
        private bool _isStartingTrip;
        private Action _onAdded;

        public AddTripDialog(Context context, Action onAdded) : base(context)
        {
            _onAdded = onAdded;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _avtImg = view.FindViewById<ShapeableImageView>(Resource.Id.avtImg);
            _periodTxt = view.FindViewById<CustomEditText>(Resource.Id.timeRangeTxt);
            _saveBtn = view.FindViewById<MaterialButton>(Resource.Id.saveBtn);
            _periodBtn = view.FindViewById<TextInputEditText>(Resource.Id.periodBtn);
            _startPoint = view.FindViewById<Chip>(Resource.Id.startPointChip);
            _endPoint = view.FindViewById<Chip>(Resource.Id.endPointChip);
            _nameTxt = view.FindViewById<CustomEditText>(Resource.Id.nameTxt);
            _descriptionTxt = view.FindViewById<CustomEditText>(Resource.Id.descriptionTxt);

            view.FindViewById<ExtendedFloatingActionButton>(Resource.Id.updatImgBtn).Click += delegate
            {
                SelectAvt();
            };

            _avtImg.Click += delegate { SelectAvt(); };

            _startPoint.SetChipIconResource(Resource.Drawable.ic_landscape_24);
            _endPoint.SetChipIconResource(Resource.Drawable.ic_landscape_24);

            _startPoint.ChipStartPadding = 30;
            _startPoint.ChipEndPadding = 30;

            _endPoint.ChipStartPadding = 30;
            _endPoint.ChipEndPadding = 30;

            _startPoint.Click += delegate
            {
                SelectPoint(_startPoint);
            };

            _endPoint.Click += delegate
            {
                SelectPoint(_endPoint);
            };

            _periodBtn.Click += delegate
            {
                SelectPeriodTime();
            };

            _periodTxt.Click += delegate
            {
                SelectPeriodTime();
            };

            _saveBtn.Click += _saveBtn_Click;

            _trip = new Trip();
        }

        private async void _saveBtn_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                _isStartingTrip = false;
                return;
            }

            if (!string.IsNullOrEmpty(_avtUrl))
            {
                var path = await ServiceInstances.AuthService.PushImageToServer(_avtUrl, (process) =>
                {

                }, Constants.MileStoneStorageFolderName);

                if (!string.IsNullOrEmpty(path))
                {
                    _avtUrl = path;
                    SaveChanges();
                }
                else
                {
                    SaveChanges();
                }
            }
            else
            {
                SaveChanges();
            }
        }

        async void SaveChanges()
        {
            var startPoint = JsonConvert.DeserializeObject<Coordinate>(((Java.Lang.String)_startPoint.Tag).ToString());
            var endPoint = JsonConvert.DeserializeObject<Coordinate>(((Java.Lang.String)_endPoint.Tag).ToString());

            _trip.ImageUrl = _avtUrl;
            _trip.Name = _nameTxt.Text;
            _trip.Description = _descriptionTxt.Text;
            _trip.StartPointLatitude = startPoint.Latitude;
            _trip.StartPointLongitute = startPoint.Longitude;
            _trip.EndPointLatitude = endPoint.Latitude;
            _trip.EndPointLongitute = endPoint.Longitude;
            _trip.OwnerId = UserSession.Instance.LoggedinUser.Id;
            _trip.Status = TripStatuses.Created;
            _trip.StartPointName = _startPoint.Text;
            _trip.EndPointName = _endPoint.Text;

            _trip = await ServiceInstances.TripRepository.InsertAsync(_trip);

            this.Dismiss();

            _onAdded?.Invoke();

            var qrCodeDialog = new ViewQRCodeDialog(CurrentActivity, _trip.Id);

            qrCodeDialog.ShowDialog();
        }

        bool ValidateFields()
        {
            var res = true;

            if (_startPoint.Tag == null)
            {
                _startPoint.Error = "This is required";
                res = false;
            }

            if (_endPoint.Tag == null)
            {
                _endPoint.Error = "This is required";
                res = false;
            }

            if (string.IsNullOrEmpty(_nameTxt.Text) || _nameTxt.Text.Length < ServiceInstances.ResourceService.TripNameMinLength)
            {
                _nameTxt.Error = "Trip Name min length is " + ServiceInstances.ResourceService.TripNameMinLength.ToString();
                res = false;
            }

            if (_nameTxt.Text.Length > ServiceInstances.ResourceService.TripNameMaxLength)
            {
                _nameTxt.Error = "Trip Name max length is " + ServiceInstances.ResourceService.TripNameMaxLength.ToString();
                res = false;
            }

            return res;
        }

        async void SelectPoint(Chip chip)
        {
            if (_mileStones == null)
            {
                _mileStones = new List<SpinnerItem>();
                var milestones =
                    (await ServiceInstances.UserMilestoneRepository.GetAllByUser(UserSession.Instance.LoggedinUser.Id));

                foreach (var milestone in milestones)
                {
                    _mileStones.Add(new MultiTitleSpinnerItem()
                    {
                        LeftImgUrl = (milestone.DisplayImgUrl),
                        MainTitle = milestone.Name,
                        SubTitle = milestone.Address,
                        LeftImgResId = Resource.Drawable.ic_place_24,
                        Value = new Coordinate()
                        {
                            Latitude = milestone.Latitude,
                            Longitude = milestone.Longitude
                        }
                    });
                }

                _mileStones.Add(new SingleTitleSpinnerItem()
                {
                    MainTitle = "Add new Milestone",
                    LeftImgResId = Resource.Drawable.ic_add_circle_24
                });

                SelectMilestone(chip);
            }
            else
            {
                SelectMilestone(chip);
            }
        }

        void SelectMilestone(Chip chip)
        {
            new MaterialAlertDialogBuilder(Context).SetAdapter(new SpinnerAdapter(_mileStones, Context), new DialogLisenter((pos) =>
            {
                if (pos == _mileStones.Count - 1)
                {
                    var dialog = new AddMilestoneDialog(Context);

                    dialog.OnSelected = (coor, name) =>
                    {
                        _mileStones = null;

                        chip.Text = name;
                        chip.Tag = new Java.Lang.String(JsonConvert.SerializeObject(coor));
                        chip.SetError("", null);
                    };

                    dialog.ShowDialog();

                    return;
                }

                chip.Text = _mileStones[pos].MainTitle;
                chip.SetError("", null);
                chip.Tag = new Java.Lang.String(JsonConvert.SerializeObject(_mileStones[pos].Value));

            })).SetTitle("Select Milestone").Show();
        }

        void SelectPeriodTime()
        {
            var datePicker = MaterialDatePicker.Builder.DateRangePicker()
                                 .SetTitleText("Select Trip Period")
                                 .SetTheme(Resource.Style.ThemeOverlay_MaterialComponents_MaterialCalendar)
                                 .Build();
            datePicker.AddOnPositiveButtonClickListener(this);

            datePicker.Show(CurrentActivity.SupportFragmentManager, "Date Range");
        }

        void SelectAvt()
        {
            CurrentActivity.SetImgSelectionListner(this);
            CurrentActivity.SelectImageFromGallery();
        }

        public void OnImgSelected(string path, Activity activity)
        {
            using (var bitmap = BitmapFactory.DecodeFile(path))
            {
                _avtUrl = path;
                _avtImg.SetScaleType(ImageView.ScaleType.CenterCrop);
                Glide.With(activity).Load(bitmap).Into(_avtImg);
            }
        }

        public void OnPositiveButtonClick(Java.Lang.Object p0)
        {
            try
            {
                var pair = (Pair)p0;

                var start = pair.First.JavaCast<Java.Lang.Long>().LongValue();
                var end = pair.Second.JavaCast<Java.Lang.Long>().LongValue();

                var startDate = (new DateTime()).AddYears(1969) + TimeSpan.FromMilliseconds(start);
                var endDate = (new DateTime()).AddYears(1969) + TimeSpan.FromMilliseconds(end);

                _periodTxt.Text = startDate.ToShortDateString() + " - " + endDate.ToShortDateString();

                _trip.StartTime = startDate;
                _trip.EndTime = endDate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
