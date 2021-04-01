using System;
using System.IO;
using System.Threading.Tasks;
using Firebase.Auth;
using FriendLoc.Entity;
using FriendLoc.Model;
using User = FriendLoc.Entity.User;

namespace FriendLoc.Common.Services.Impl
{
    public class FirebaseAuthService : IAuthService
    {
        public FirebaseAuthService()
        {
        }

        FirebaseAuthProvider GetProvider()
        {
            return new FirebaseAuthProvider(new FirebaseConfig(Constants.FirebaseApiKey));
        }

        public async Task<string> RefreshTokenAsync()
        {
            var login = await Login(UserSession.Instance.LoggedinUser.LoginName, UserSession.Instance.LoggedinUser.Password,(err)=> {

            });

            if (!string.IsNullOrEmpty(login))
            {
                return login;
            }
            else
                return "";
        }

        public async Task<string> Login(string loginName, string password, Action<string> errorCallback)
        {
            var authProvider = GetProvider();

            try
            {
                var login = await authProvider.SignInWithEmailAndPasswordAsync(loginName + Constants.EmailSuffix, password);

                ServiceInstances.ResourceService.UserToken = login.FirebaseToken;

                login.FirebaseAuthRefreshed += LoginFirebaseAuthRefreshed;

                var user = await ServiceInstances.UserRepository.GetById(login.User.LocalId);
                UserSession.Instance.LoggedinUser = user;
                ServiceInstances.SecureStorage.Store(Constants.LastestLoggedIn, DateTime.Now.ToString());

                return ServiceInstances.ResourceService.UserToken;
            }
            catch(FirebaseAuthException firebaseExcep)
            {
                errorCallback(UtilCommon.FirebaseAuthExceptionHandler(firebaseExcep));
                return "";
            }
            catch(Exception ex)
            {
                errorCallback(ex.Message);
                return "";
            }
        }

        private void LoginFirebaseAuthRefreshed(object sender, FirebaseAuthEventArgs e)
        {
            ServiceInstances.ResourceService.UserToken = e.FirebaseAuth.RefreshToken;
            ServiceInstances.SecureStorage.Store(Constants.LastestLoggedIn,DateTime.Now.ToString());
        }

        public async void  SignUp(SignUpModel model, Action<string> errorCallback, Action successCallback)
        {
            var avtUrl = await PushUserAvatar(model.AvtImgPath,(progress)=> {

            }); 

            var user = new User()
            {
                AvtUrl = avtUrl,
                CountryCode = model.CountryCode,
                PhoneNumber = model.PhoneNumber,
                FullName = model.FullName,
                LoginName = model.LoginName,
                Password = model.Password,
                Gender = model.Gender
            };

            var authProvider = GetProvider();

            try
            {
                var account = await authProvider.CreateUserWithEmailAndPasswordAsync(user.LoginName + Constants.EmailSuffix, user.Password, user.FullName);

                if (account == null)
                {
                    errorCallback("Got some errors when creating new account! please try again.");
                    return;
                }
                else
                {
                    user.Id = account.User.LocalId;

                    ServiceInstances.ResourceService.UserToken = account.FirebaseToken;

                    if (!string.IsNullOrEmpty(avtUrl)) await account.UpdateProfileAsync(model.FullName, avtUrl);

                    var dbUser = await ServiceInstances.UserRepository.InsertAsync(user);

                    if (dbUser != null)
                    {
                        successCallback();
                        return;
                    }
                    else
                    {
                        errorCallback("Got some errors when creating new account! please try again.");
                        return;
                    }
                }
            }
            catch (Firebase.Auth.FirebaseAuthException firebaseExcep)
            {
                errorCallback(UtilCommon.FirebaseAuthExceptionHandler(firebaseExcep));
                return;
            }
            catch (Exception ex)
            {
                errorCallback(ex.Message);
            }
        }

        public async Task<string> PushUserAvatar(string path,Action<int> progressAction)
        {
            if (!string.IsNullOrEmpty(path))
            {
                using (var stream = File.OpenRead(path))
                {
                    var imgUrl = await ServiceInstances.UserRepository.UploadFile(stream, Constants.UserAvtStorageFolderName, progressAction);

                    return imgUrl;
                }
            }

            return "";

        }
    }
}
