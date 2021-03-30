using FriendLoc.Common;
using FriendLoc.Common.Repositories;
using UIKit;

namespace FriendLog.IOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {

            ServiceLocator.Instance.Register<ITripRepository, TripRepository>();
            ServiceLocator.Instance.Register<IUserRepository, UserRepository>();
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}