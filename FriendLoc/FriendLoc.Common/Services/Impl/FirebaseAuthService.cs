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

        public async Task Login(string loginName, string password, Action<string> errorCallback, Action successCallback)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FirebaseApiKey));

            try
            {
                var login = await authProvider.SignInWithEmailAndPasswordAsync(loginName + Constants.EmailSuffix, password);

                ServiceInstances.ResourceService.UserToken = login.FirebaseToken;

                var user = await ServiceInstances.UserRepository.GetById(login.User.LocalId);
                UserSession.Instance.LoggedinUser = user;


                successCallback();
                return ;
            }
            catch(FirebaseAuthException firebaseExcep)
            {
                errorCallback(UtilCommon.FirebaseAuthExceptionHandler(firebaseExcep.ResponseData));
                return;
            }
            catch(Exception ex)
            {
                errorCallback(ex.Message);
                return;
            }
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

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FirebaseApiKey));

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
                errorCallback(UtilCommon.FirebaseAuthExceptionHandler(firebaseExcep.ResponseData));
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
