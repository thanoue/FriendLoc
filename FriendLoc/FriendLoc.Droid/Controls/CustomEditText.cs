using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Google.Android.Material.TextField;

namespace FriendLoc.Controls
{
    public class CustomEditText: TextInputLayout
    {
        bool _isDrawn = false;

        public string Text
        {
            get
            {
                return EditText.Text;
            }
            set
            {
                EditText.Text = value;
            }
        }

        public CustomEditText(Context context) : base(context)
        {
        }

        public CustomEditText(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public CustomEditText(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        protected CustomEditText(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override void DispatchDraw(Canvas canvas)
        {
            base.DispatchDraw(canvas);

            if (_isDrawn)
                return;

            _isDrawn = true;

            this.EditText.FocusChange += (sender, e) =>
            {
                if (!e.HasFocus && !string.IsNullOrEmpty(((EditText)sender).Text))
                {
                    this.Error = null;
                }
            };
        }
    }
}
