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
        protected IGlobalUIService UtilUI => ServiceLocator.Instance.Get<IGlobalUIService>();

        public FirebaseAuthService()
        {
        }

        FirebaseAuthProvider GetProvider()
        {
            return new FirebaseAuthProvider(new FirebaseConfig(Constants.FirebaseApiKey));
        }

        public async Task<string> Login(string loginName, string password, Action<string> errorCallback)
        {
            UtilUI.StartLoading();

            var authProvider = GetProvider();

            try
            {
                var login = await authProvider.SignInWithEmailAndPasswordAsync(loginName + Constants.EmailSuffix,
                    password);

                ServiceInstances.ResourceService.UserToken = login.FirebaseToken;

                login.FirebaseAuthRefreshed += LoginFirebaseAuthRefreshed;

                var user = await ServiceInstances.UserRepository.GetById(login.User.LocalId);

                UserSession.Instance.LoggedinUser = user;

                ServiceInstances.SecureStorage.Store(Constants.LastestLoggedIn, DateTime.Now.ToString());
                ServiceInstances.SecureStorage.StoreObject(Constants.LoggedinUser, user);

                UtilUI.StopLoading();

                return ServiceInstances.ResourceService.UserToken;
            }
            catch (FirebaseAuthException firebaseExcep)
            {
                errorCallback(UtilCommon.FirebaseAuthExceptionHandler(firebaseExcep));
                UtilUI.StopLoading();

                return "";
            }
            catch (Exception ex)
            {
                UtilUI.StopLoading();
                errorCallback(ex.Message);
                return "";
            }
        }

        private void LoginFirebaseAuthRefreshed(object sender, FirebaseAuthEventArgs e)
        {
            ServiceInstances.ResourceService.UserToken = e.FirebaseAuth.RefreshToken;
            ServiceInstances.SecureStorage.Store(Constants.LastestLoggedIn, DateTime.Now.ToString());
        }

        public async void SignUp(SignUpModel model, Action<string> errorCallback, Action successCallback)
        {
            UtilUI.StartLoading();
            
            var avtUrl =
                await PushImageToServer(model.AvtImgPath, (progress) => { }, Constants.UserAvtStorageFolderName);

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
                var account =
                    await authProvider.CreateUserWithEmailAndPasswordAsync(user.LoginName + Constants.EmailSuffix,
                        user.Password, user.FullName);

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
                        ServiceInstances.ResourceService.UserToken = "";
                        UtilUI.StopLoading();
                        successCallback();
                        return;
                    }
                    else
                    {
                        UtilUI.StopLoading();
                        ServiceInstances.ResourceService.UserToken = "";
                        errorCallback("Got some errors when creating new account! please try again.");
                        return;
                    }
                }
            }
            catch (Firebase.Auth.FirebaseAuthException firebaseExcep)
            {
                UtilUI.StopLoading();
                errorCallback(UtilCommon.FirebaseAuthExceptionHandler(firebaseExcep));
                return;
            }
            catch (Exception ex)
            {
                UtilUI.StopLoading();
                errorCallback(ex.Message);
                return;
            }
        }

        public async Task<string> PushImageToServer(string path, Action<int> progressAction, string folderName)
        {
            if (!string.IsNullOrEmpty(path))
            {
                UtilUI.StartLoading();
                using (var stream = File.OpenRead(path))
                {
                    var imgUrl = await ServiceInstances.UserRepository.UploadFile(stream, folderName, progressAction).ContinueWith(
                        (res) =>
                        {
                            UtilUI.StopLoading();
                            return res.Result;
                        });

                    return imgUrl;
                }
            }

            return "";
        }
    }
}