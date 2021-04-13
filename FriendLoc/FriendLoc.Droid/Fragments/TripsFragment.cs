
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
using FriendLoc.Droid.Adapters;
using FriendLoc.Droid.ViewModels;
using FriendLoc.Entity;

namespace FriendLoc.Droid.Fragments
{
    public class TripsFragment : Fragment
    {
        ListView _tripListView;
        IList<Trip> _trips;
        IList<TripViewModel> _items;
        TripAdapter _tripAdapter;

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

            if (_trips == null && _trips.Count <= 0)
            {
                return;
            }

            _items = new List<TripViewModel>();

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
                    Status = (Entity.TripStatuses)(new Random().Next(0, 2))
                }) ;
            }

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
                    Status = (Entity.TripStatuses)(new Random().Next(0, 2))
                }) ;
            }

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
                    Status = (Entity.TripStatuses)(new Random().Next(0, 2))
                }) ;
            }

            _tripAdapter = new TripAdapter(Context, _items);
            _tripListView.Adapter = _tripAdapter;
        }
    }
}
