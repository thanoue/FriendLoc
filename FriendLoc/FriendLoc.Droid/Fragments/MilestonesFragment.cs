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
using FriendLoc.Entities;
using FriendLoc.Entity;
using Google.Android.Material.Button;
using Plugin.CurrentActivity;

namespace FriendLoc.Droid.Fragments
{
    public class MilestonesFragment : BaseFragment
    {
        ListView _tripListView;
        IList<Milestone> _trips;
        IList<MilestoneViewModel> _items;
        MilestoneAdapter _tripAdapter;
        MaterialButton _addBtn;

        public MilestonesFragment(IList<Milestone> trips)
        {
            _trips = trips;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.fragment_milestones, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _tripListView = view.FindViewById<ListView>(Resource.Id.tripListview);
            _addBtn = view.FindViewById<MaterialButton>(Resource.Id.addBtn);

            if (_trips == null && _trips.Count <= 0)
            {
                return;
            }

            _addBtn.Click += delegate
            {
                var addTripDialog = new AddMilestoneDialog(Context);

                addTripDialog.OnSelected = (coordinate, s) =>
                {
                    ReloadMilestones();
                };

                addTripDialog.ShowDialog();
            };

            _items = new List<MilestoneViewModel>();

            DataBinding();

            _tripAdapter = new MilestoneAdapter(Context,MilestoneItemAction , _items);
            _tripListView.Adapter = _tripAdapter;
        }

        void MilestoneItemAction(MilestoneActions action,string index)
        {
            
        }

        async void ReloadMilestones()
        {
            var res = await ServiceInstances.UserMilestoneRepository.GetAllByUser(UserSession.Instance.LoggedinUser.Id);

            _trips.Clear();

            ((List<Milestone>) _trips).AddRange(res);

            DataBinding();

            _tripAdapter.NotifyDataSetChanged();
        }

        void DataBinding()
        {
            _items.Clear();

            foreach (var trip in _trips)
            {
                _items.Add(new MilestoneViewModel()
                {
                    AvtUrl = trip.DisplayImgUrl,
                    Id = trip.Id,
                    Name = trip.Name,
                    Address = trip.Address,
                });
            }
        }
    }
}