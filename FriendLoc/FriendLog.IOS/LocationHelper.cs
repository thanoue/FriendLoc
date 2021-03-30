using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CoreLocation;
using Firebase.Database;
using Foundation;
using FriendLoc.Common;
using FriendLoc.Common.Repositories;
using FriendLoc.Entity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using UIKit;

namespace FriendLog.IOS
{
    public static class LocationHelper
    {

        public static bool IsLocationAvailable()
        {
            if (!CrossGeolocator.IsSupported)
                return false;

            return CrossGeolocator.Current.IsGeolocationAvailable;
        }

        public async static Task StartListening()
        {
            if (CrossGeolocator.Current.IsListening)
                return;

            CrossGeolocator.Current.PositionChanged += PositionChanged;
            CrossGeolocator.Current.PositionError += PositionError;

            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(10), 10, true, new ListenerSettings
            {
                ActivityType = Plugin.Geolocator.Abstractions.ActivityType.AutomotiveNavigation,
                AllowBackgroundUpdates = true,
                DeferLocationUpdates = true,
                DeferralDistanceMeters = 1,
                DeferralTime = TimeSpan.FromSeconds(1),
                ListenForSignificantChanges = true,
                PauseLocationUpdatesAutomatically = false,
            });
        }



        private static void PositionChanged(object sender, PositionEventArgs e)
        {

            //If updating the UI, ensure you invoke on main thread
            var position = e.Position;
            var output = "Full: Lat: " + position.Latitude + " Long: " + position.Longitude;
            output += "\n" + $"Time: {position.Timestamp}";
            output += "\n" + $"Heading: {position.Heading}";
            output += "\n" + $"Speed: {position.Speed}";
            output += "\n" + $"Accuracy: {position.Accuracy}";
            output += "\n" + $"Altitude: {position.Altitude}";
            output += "\n" + $"Altitude Accuracy: {position.AltitudeAccuracy}";
            Debug.WriteLine(output);
        }

        private static void PositionError(object sender, PositionErrorEventArgs e)
        {
            Debug.WriteLine(e.Error);
            //Handle event here for errors
        }

        async static Task StopListening()
        {
            if (!CrossGeolocator.Current.IsListening)
                return;

            await CrossGeolocator.Current.StopListeningAsync();

            CrossGeolocator.Current.PositionChanged -= PositionChanged;
            CrossGeolocator.Current.PositionError -= PositionError;
        }
    }


    public class LocationManager : NSData, ICLLocationManagerDelegate
    {
        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };
        protected CLLocationManager locMgr;

        public LocationManager()
        {
            this.locMgr = new CLLocationManager();
            this.locMgr.PausesLocationUpdatesAutomatically = false;

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                locMgr.AllowsBackgroundLocationUpdates = true;
            }

            locMgr.RequestWhenInUseAuthorization();
        }

        public void RequestAlways()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locMgr.RequestAlwaysAuthorization();
            }
        }

        public CLLocationManager LocMgr
        {
            get { return this.locMgr; }
        }

        public void StartLocationUpdates()
        {
            if (CLLocationManager.LocationServicesEnabled)
            {
                //set the desired accuracy, in meters
                locMgr.DesiredAccuracy = 1;
                locMgr.LocationsUpdated += async (object sender, CLLocationsUpdatedEventArgs e) =>
                {
                    var args = e.Locations[e.Locations.Length - 1];
                    CLLocation location = args;

                    var client = new TripRepository();

                    var firebase = new FirebaseClient(
                      "https://friendloc-98ed3-default-rtdb.firebaseio.com/");

                    await client.AddLocation("b4594b25-284e-4459-8560-0aa98aeb6135", new TripLocation()
                    {
                        Latitude = location.Coordinate.Latitude,
                        Longitude = location.Coordinate.Longitude,
                        TripId = "b4594b25-284e-4459-8560-0aa98aeb6135",
                        UserId = "4hWvuBnYtSNp2Vb7UoSGZ8fDjJp2"
                    });

                    //LocationUpdated(this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));
                };
                locMgr.ShowsBackgroundLocationIndicator = true;
                locMgr.DistanceFilter = 100;
                locMgr.StartUpdatingLocation();
            }
        }


    }

    public class LocationUpdatedEventArgs : EventArgs
    {
        CLLocation location;

        public LocationUpdatedEventArgs(CLLocation location)
        {
            this.location = location;
        }

        public CLLocation Location
        {
            get { return location; }
        }
    }
}
