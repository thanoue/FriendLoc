using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using FriendLoc.Common;
using FriendLoc.Droid.Adapters;
using FriendLoc.Droid.Dialogs;
using FriendLoc.Droid.ViewModels;
using FriendLoc.Entity;
using Google.Android.Material.Button;
using Plugin.CurrentActivity;

namespace FriendLoc.Droid.Fragments
{
    public class TripsFragment : BaseFragment
    {
        ListView _tripListView;
        IList<Trip> _trips;
        IList<TripViewModel> _items;
        TripAdapter _tripAdapter;
        MaterialButton _addTripBtn, _scanQrCodeBtn;

        public TripsFragment(IList<Trip> trips)
        {
            _trips = trips;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.fragment_trips, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _tripListView = view.FindViewById<ListView>(Resource.Id.tripListview);
            _addTripBtn = view.FindViewById<MaterialButton>(Resource.Id.addTripBtn);
            _scanQrCodeBtn = view.FindViewById<MaterialButton>(Resource.Id.scanQRBtn);

            if (_trips == null && _trips.Count <= 0)
            {
                return;
            }

            _scanQrCodeBtn.Click += delegate
            {
                var scanQr = new ScanQRCodeDialog(Context, async (id) =>
                {
                    var duplicate = _trips.FirstOrDefault(p => p.Id.Equals(id));

                    if (duplicate != null)
                    {
                        UtilUI.ErrorToast("You've have already joined in this Trip!");
                        return;
                    }

                   var join = await ServiceInstances.TripRepository.AddMember(id, UserSession.Instance.LoggedinUser.Id);

                    if (join)
                    {
                        ReloadTrips();
                    }
                });

                scanQr.ShowDialog();
            };

            _addTripBtn.Click += delegate
            {
                var addTripDialog = new AddTripDialog(Context, () => { ReloadTrips(); });

                addTripDialog.ShowDialog();
            };

            _items = new List<TripViewModel>();

            DataBinding();

            _tripAdapter = new TripAdapter(Context, OnTripAction, _items);
            _tripListView.Adapter = _tripAdapter;
        }

        void OnTripAction(TripActions action, string id)
        {
            switch (action)
            {
                case TripActions.Share:

                    var dialog = new ViewQRCodeDialog(Context, id);
                    dialog.ShowDialog();

                    break;
            }
        }

        async void ReloadTrips()
        {
            var res  = await ServiceInstances.TripRepository.GetByJoinedUser(UserSession.Instance.LoggedinUser.Id);

            _trips.Clear();

            ((List<Trip>) _trips).AddRange(res);
            
            DataBinding();
            
            _tripAdapter.NotifyDataSetChanged();
        }

        void DataBinding()
        {
            _items.Clear();

            foreach (var trip in _trips)
            {
                _items.Add(new TripViewModel()
                {
                    AvtUrl = trip.ImageUrl,
                    DateRange = trip.StartTime.ToShortDateString() + " - " + trip.EndTime.ToShortDateString(),
                    Id = trip.Id,
                    OwnerId = trip.OwnerId,
                    Milestones = trip.StartPointName + " - " + trip.EndPointName,
                    Name = trip.Name,
                    Status = (Entity.TripStatuses) (new Random().Next(0, 2))
                });
            }
        }
    }
}