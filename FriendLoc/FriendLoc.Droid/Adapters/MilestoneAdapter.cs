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
    public class MilestoneAdapter : BaseAdapter<MilestoneViewModel>
    {
        IList<MilestoneViewModel> _items;
        Context _context;
        private Action<MilestoneActions, string> _onAction;
        public MilestoneAdapter(Context context,Action<MilestoneActions, string> onAction, IList<MilestoneViewModel> items)
        {
            _onAction = onAction;
            _context = context;
            _items = items;
        }

        public override MilestoneViewModel this[int position] => _items[position];

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
                view = LayoutInflater.From(_context).Inflate(Resource.Layout.adapter_milestone_item, parent, false);

                viewHolder = new ViewHolder();

                viewHolder.NameTv = view.FindViewById<MaterialTextView>(Resource.Id.nameTv);
                viewHolder.LeftImg = view.FindViewById<ShapeableImageView>(Resource.Id.leftImg);
                viewHolder.PlaceTitleTv = view.FindViewById<MaterialTextView>(Resource.Id.addressTv);
                viewHolder.MenuIcon = view.FindViewById<AppCompatImageButton>(Resource.Id.menuBtn);

                viewHolder.LeftImg.SetScaleType(ImageView.ScaleType.CenterCrop);

                viewHolder.MenuIcon.Click += delegate
                {
                    var popup = new PopupMenu(_context, viewHolder.MenuIcon);

                    var item = _items[viewHolder.Position];
                    int popupResId =  Resource.Menu.milestone_item_popup_menu;

                    popup.SetOnMenuItemClickListener(new OnMenuItemClickListener((menuItem) =>
                    {
                        switch (menuItem.ItemId)
                        {
                            case Resource.Id.editItem:
                                _onAction?.Invoke(MilestoneActions.Edit,item.Id);
                                break;  
                            case Resource.Id.removeItem:
                                _onAction?.Invoke(MilestoneActions.Remove,item.Id);
                                break;    
                            case Resource.Id.copyItem:
                                _onAction?.Invoke(MilestoneActions.Duplicate,item.Id);
                                break;
                        }
                        
                    }));

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
            viewHolder.PlaceTitleTv.Text = _items[position].Address;

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
            public AppCompatImageButton MenuIcon { get; set; }
        }
    }
}