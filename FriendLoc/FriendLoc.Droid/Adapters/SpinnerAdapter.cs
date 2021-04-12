using System;
using System.Collections.Generic;
using Android.Content;
using Android.Text;
using Android.Views;
using Android.Widget;
using BumpTech.GlideLib;
using FriendLoc.Droid.ViewModels;
using Google.Android.Material.ImageView;
using Google.Android.Material.TextView;

namespace FriendLoc.Droid.Adapters
{
    public class SpinnerAdapter : BaseAdapter<SpinnerItem>, IListAdapter
    {
        IList<SpinnerItem> _items;
        Context _context;

        public SpinnerAdapter(IList<SpinnerItem> items, Context context)
        {
            _items = items;
            _context = context;
        }

        public override SpinnerItem this[int position] => _items[position];

        public override int Count => _items.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            ViewHolder holder = null;

            if (view == null)
            {
                var inflater = LayoutInflater.From(_context);

                view = inflater.Inflate(Resource.Layout.adapter_full_info_spinner, parent, false);

                holder = new ViewHolder();

                holder.LeftImg = view.FindViewById<ShapeableImageView>(Resource.Id.leftImg);
                holder.MainTitle = view.FindViewById<MaterialTextView>(Resource.Id.mainTitleTv);
                holder.SubTitle = view.FindViewById<MaterialTextView>(Resource.Id.subTitleTv);

                holder.SubTitle.Ellipsize = TextUtils.TruncateAt.Middle;
                holder.MainTitle.Ellipsize = TextUtils.TruncateAt.Middle;

                view.Tag = holder;
            }
            else
            {
                holder = (ViewHolder)view.Tag;
            }

            if (_items[position].LeftImgUrl != null)
            {
                holder.LeftImg.SetScaleType(ImageView.ScaleType.CenterCrop);
                Glide.With(_context).Load(_items[position].LeftImgUrl).Into(holder.LeftImg);
            }
            else
            {
                holder.LeftImg.SetScaleType(ImageView.ScaleType.FitCenter);

                if (_items[position].LeftImgResId != 0)
                {
                    holder.LeftImg.SetImageResource(_items[position].LeftImgResId);
                }
                else
                    holder.LeftImg.SetImageResource(Resource.Drawable.ic_add_location_24);
            }


            switch (_items[position].Type)
            {
                case SpinnerTypes.SingleTitle:

                    holder.MainTitle.Visibility = ViewStates.Gone;
                    holder.SubTitle.Text = _items[position].MainTitle;

                    holder.SubTitle.SetMaxLines(2);

                    break;

                case SpinnerTypes.MultiTitle:

                    holder.MainTitle.Visibility = ViewStates.Visible;

                    holder.SubTitle.Text = ((MultiTitleSpinnerItem)_items[position]).SubTitle;
                    holder.MainTitle.Text = _items[position].MainTitle;

                    holder.SubTitle.SetMaxLines(1);


                    break;
            }

            return view;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public ShapeableImageView LeftImg { get; set; }
            public MaterialTextView MainTitle { get; set; }
            public MaterialTextView SubTitle { get; set; }
        }
    }
}
