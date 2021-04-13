using CoreLocation;
using Firebase.Auth;
using Firebase.Database;
using Foundation;
using FriendLoc.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FriendLoc.Entity;
using UIKit;

namespace FriendLog.IOS
{
    public partial class ViewController : UIViewController
    {
        string _userId = "" ;
        string _tripId = "";

        #region Computed Properties

        public static LocationManager Manager { get; set; }
        #endregion

        public ViewController(IntPtr handle) : base(handle)
        {
            Manager = new LocationManager();

            Manager.LocationUpdated += HandleLocationChanged;

            //if (LocationHelper.IsLocationAvailable())
            //{
            //    LocationHelper.StartListening();
            //}
        }

        public void HandleLocationChanged(object sender, LocationUpdatedEventArgs e)
        {
            CLLocation location = e.Location;

            // ServiceInstances.TripRepository.AddLocation(_tripId, new TripLocation()
            // {
            //     Latitude = location.Coordinate.Latitude,
            //     Longitude = location.Coordinate.Longitude,
            //     TripId = _tripId,
            //     UserId = _userId
            // });

            // Handle foreground updates
            //
            // var output = "Full: Lat: " + location.Coordinate.Latitude + " Long: " + location.Coordinate.Longitude;
            // output += "\n" + $"Speed: {location.Speed}";
            // output += "\n" + $"Accuracy: {location.CourseAccuracy}";
            // output += "\n" + $"Altitude: {location.Altitude}";

            // Debug.WriteLine(output);

            Console.WriteLine("foreground updated");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            startBtn.Enabled = false;
            // Perform any additional setup after loading the view, typically from a nib.

        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            loginBtn.TouchUpInside += async delegate
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCxjza0PW9fg6y4tPlljkP-iBSwOC0XY6g"));

                var login = await authProvider.SignInWithEmailAndPasswordAsync("kha@gmail.com", "Hello_2020");

                if (login != null)
                {
                    _userId = login.User.LocalId;

                    var firebase = new FirebaseClient(
                    "https://friendloc-98ed3-default-rtdb.firebaseio.com/",
                    new FirebaseOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(login.FirebaseToken)
                    });


                    startBtn.Enabled = true;

                    Manager.RequestAlways();
                }
            };

            startBtn.TouchUpInside += async delegate
            {

                var userIds = new Dictionary<string, string>();

                userIds.Add(_userId, _userId);

                var adding = await ServiceInstances.TripRepository.InsertAsync(new Trip()
                {
                    Description = "Test trips",
                    UserIds = userIds
                });

                if (adding != null)
                {
                    _tripId = adding.Id;
                    Manager.StartLocationUpdates();
                }
            };
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}