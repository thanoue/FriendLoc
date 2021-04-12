using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace FriendLoc.Droid.Controls
{
    public class CustomRelativeLayout : RelativeLayout
    {
        bool _isDrawn;
        public Action OnDrawn;

        public CustomRelativeLayout(Context context) : base(context)
        {
        }

        public CustomRelativeLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public CustomRelativeLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public CustomRelativeLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected CustomRelativeLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

    }
}
