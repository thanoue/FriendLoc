using System;
using System.Globalization;
using System.Resources;
using Android.Content;
using FriendLoc.Common.Services;
using FriendLoc.Common.Services.Impl;

namespace FriendLoc.Droid.Services
{
    public class DroidResourceService : ResourceService
    {
        Context _context;
        public DroidResourceService()
        {

        }

        public DroidResourceService(Context context)
        {
            _context = context;
        }

        public override int LoginNameMinLength => _context.Resources.GetInteger(Resource.Integer.login_name_min_length);

        public override int LoginNameMaxLength => _context.Resources.GetInteger(Resource.Integer.login_name_max_length);

        public override int PasswordMinLength => _context.Resources.GetInteger(Resource.Integer.password_min_length);

        public override int PasswordMaxLength => _context.Resources.GetInteger(Resource.Integer.password_max_length);

        public override int TripNameMinLength => _context.Resources.GetInteger(Resource.Integer.trip_name_min_lenth);

        public override int TripNameMaxLength => _context.Resources.GetInteger(Resource.Integer.trip_name_max_lenth);
    }
}
