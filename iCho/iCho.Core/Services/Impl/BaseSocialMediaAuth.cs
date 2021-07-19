using System;
using System.Threading.Tasks;
using iCho.Core.Utils;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;

namespace iCho.Service
{
    public abstract class BaseSocialMediaAuth : ISocialMediaAuth
    {
        IFacebookClient _facebookService = CrossFacebookClient.Current;
        IGoogleClientManager _googleService = CrossGoogleClient.Current;

        public abstract string PlatformType { get; }

        private SocialMediaType _currentType = SocialMediaType.None;
        public SocialMediaType AuthType => _currentType;

        public abstract void Init();

        public BaseSocialMediaAuth()
        {

        }
     
        public Task<string> FacebookLogin()
        {
            var source = new TaskCompletionSource<string>();

            _currentType = SocialMediaType.Facebook;

            try
            {

                if (_facebookService.IsLoggedIn)
                {
                    _facebookService.Logout();
                }

                EventHandler<FBEventArgs<string>> userDataDelegate = null;

                userDataDelegate = async (object sender, FBEventArgs<string> e) =>
                {
                    if (e == null) return;

                    _currentType = SocialMediaType.None;

                    switch (e.Status)
                    {

                        case FacebookActionStatus.Completed:


                            var token = _facebookService.ActiveToken;

                            source.SetResult(token);

                            break;

                        case FacebookActionStatus.Canceled:

                            source.SetResult("");

                            break;
                    }

                    _facebookService.OnUserData -= userDataDelegate;
                };

                _facebookService.OnUserData += userDataDelegate;

                _facebookService.RequestUserDataAsync(new string[] { "email", "first_name", "gender", "last_name", "birthday", "cover" }, new string[] { "email", "public_profile" });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                _currentType = SocialMediaType.None;

                source.SetResult("");
            }

            return source.Task;
        }

        public Task<string> GoogleLogin()
        {
            _currentType = SocialMediaType.Google;

            var source = new TaskCompletionSource<string>();

            try
            {
                if (!string.IsNullOrEmpty(_googleService.AccessToken))
                {
                    //Always require user authentication
                    _googleService.Logout();
                }

                EventHandler<GoogleClientResultEventArgs<GoogleUser>> userLoginDelegate = null;
                userLoginDelegate = async (object sender, GoogleClientResultEventArgs<GoogleUser> e) =>
                {
                    _googleService.OnLogin -= userLoginDelegate;
                    _currentType = SocialMediaType.None;

                    switch (e.Status)
                    {
                        case GoogleActionStatus.Completed:
                         
                            var token = _googleService.IdToken;

                            source.SetResult(token);

                            //await App.Current.MainPage.Navigation.PushModalAsync(new AddContactPage());
                            break;
                        case GoogleActionStatus.Canceled:
                        case GoogleActionStatus.Error:
                        case GoogleActionStatus.Unauthorized:

                            source.SetResult("");
                            break;
                    }

                };

                _googleService.OnLogin += userLoginDelegate;

                _googleService.LoginAsync();

               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _currentType = SocialMediaType.None;
                source.SetResult("");
            }

            return source.Task;
        }
    }
}
