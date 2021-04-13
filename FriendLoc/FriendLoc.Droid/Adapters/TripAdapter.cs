using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.View.Menu;
using AndroidX.AppCompat.Widget;
using BumpTech.GlideLib;
using FriendLoc.Common;
using FriendLoc.Droid.ViewModels;
using Google.Android.Material.ImageView;
using Google.Android.Material.TextView;
using PopupMenu = AndroidX.AppCompat.Widget.PopupMenu;

namespace FriendLoc.Droid.Adapters
{
    public class TripAdapter : BaseAdapter<TripViewModel>
    {
        IList<TripViewModel> _items;
        Context _context;
        private Action<TripActions, string> _onAction;
        public TripAdapter(Context context,Action<TripActions, string> onAction, IList<TripViewModel> items)
        {
            _onAction = onAction;
            _context = context;
            _items = items;
        }

        public override TripViewModel this[int position] => _items[position];

        public override int Count => _items.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            ViewHolder viewHolder = null;

            if (view == null)
            {
                view = LayoutInflater.From(_context).Inflate(Resource.Layout.adapter_trip_item, parent, false);

                viewHolder = new ViewHolder();

                viewHolder.NameTv = view.FindViewById<MaterialTextView>(Resource.Id.nameTv);
                viewHolder.LeftImg = view.FindViewById<ShapeableImageView>(Resource.Id.leftImg);
                viewHolder.PlaceTitleTv = view.FindViewById<MaterialTextView>(Resource.Id.milestonesTv);
                viewHolder.TimeTitleTv = view.FindViewById<MaterialTextView>(Resource.Id.timeRangeTv);
                viewHolder.MenuIcon = view.FindViewById<AppCompatImageButton>(Resource.Id.menuBtn);

                viewHolder.LeftImg.SetScaleType(ImageView.ScaleType.CenterCrop);

                viewHolder.MenuIcon.Click += delegate
                {
                    var popup = new PopupMenu(_context, viewHolder.MenuIcon);

                    var item = _items[viewHolder.Position];
                    int popupResId = 0;

                    popup.SetOnMenuItemClickListener(new OnMenuItemClickListener((menuItem) =>
                    {
                        switch (menuItem.ItemId)
                        {
                            case Resource.Id.shareItem:
                                _onAction?.Invoke(TripActions.Share,item.Id);
                                break;  
                            case Resource.Id.startItem:
                                _onAction?.Invoke(TripActions.Start,item.Id);
                                break;
                            case Resource.Id.stopItem:
                                _onAction?.Invoke(TripActions.Stop,item.Id);
                                break; 
                            case Resource.Id.removeItem:
                                _onAction?.Invoke(TripActions.Remove,item.Id);
                                break; 
                            case Resource.Id.leaveItem:
                                _onAction?.Invoke(TripActions.Leave,item.Id);
                                break;    
                            case Resource.Id.duplicateItem:
                                _onAction?.Invoke(TripActions.Copy,item.Id);
                                break;
                        }
                        
                    }));

                    if (item.Status == Entity.TripStatuses.Runnning)
                    {
                        popupResId = Resource.Menu.trip_popup_menu_playing;
                    }
                    else
                    {
                        popupResId = Resource.Menu.trip_popup_menu_created;
                    }

                    popup.MenuInflater.Inflate(popupResId, popup.Menu);

                    if (popup.Menu is MenuBuilder)
                    {
                        var menuBuilder = popup.Menu as MenuBuilder;
                        menuBuilder.SetOptionalIconsVisible(true);

                        foreach (var menu in menuBuilder.VisibleItems)
                        {
                            var iconMarginPx = (int) TypedValue.ApplyDimension(ComplexUnitType.Dip, 10f,
                                _context.Resources.DisplayMetrics);

                            menu.SetIcon(new InsetDrawable(menu.Icon, iconMarginPx, 0, iconMarginPx, 0));
                        }
                    }

                    popup.Show();
                };

                view.Tag = viewHolder;
            }
            else
            {
                viewHolder = (ViewHolder) view.Tag;
            }

            viewHolder.Position = position;
            viewHolder.NameTv.Text = _items[position].Name;
            viewHolder.PlaceTitleTv.Text = _items[position].Milestones;
            viewHolder.TimeTitleTv.Text = _items[position].DateRange;

            if (!string.IsNullOrEmpty(_items[position].AvtUrl))
            {
                Glide.With(_context).Load(_items[position].AvtUrl).Into(viewHolder.LeftImg);
            }
            else
            {
                viewHolder.LeftImg.SetImageResource(Resource.Drawable.ic_landscape_24);
            }

            return view;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public int Position { get; set; }
            public ShapeableImageView LeftImg { get; set; }
            public MaterialTextView NameTv { get; set; }
            public MaterialTextView PlaceTitleTv { get; set; }
            public MaterialTextView TimeTitleTv { get; set; }
            public AppCompatImageButton MenuIcon { get; set; }
        }
    }
}