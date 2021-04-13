using System;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using FriendLoc.Common;
using System.IO;
using Firebase.Storage;
using FriendLoc.Entity;
using Firebase.Auth;
using System.Threading;
using System.Collections.Generic;

namespace FriendLoc.Common.Repositories
{
    public abstract class BaseRepo<T> : IBaseRepository<T> where T : BaseEntity
    {
        public abstract string Path { get; }

        public BaseRepo()
        {

        }

        public FirebaseClient Client =>
        new FirebaseClient(
               Constants.FirebaseDbPath,
               new FirebaseOptions
               {
                   AuthTokenAsyncFactory = () => Task.FromResult(ServiceInstances.ResourceService.UserToken)
               });

        public async Task<string> UploadFile(Stream imageData, string folderName, Action<int> progressCallback)
        {
            var task = new FirebaseStorage(Constants.FirebaseStoragePath)
                            .Child(folderName)
                            .Child(Guid.NewGuid().ToString())
                            .PutAsync(imageData);

            task.Progress.ProgressChanged += (s, e) => { Console.WriteLine($"Progress: {e.Percentage} %"); progressCallback?.Invoke(e.Percentage); };

            return await task;
        }

        public async Task<T> InsertAsync(T entity)
        {
            try
            {
                entity.Created = DateTime.Now.GetLocalTimeTotalSeconds();
                entity.Updated = DateTime.Now.GetLocalTimeTotalSeconds();
                entity.Id = string.IsNullOrEmpty(entity.Id) ? Guid.NewGuid().ToString() : entity.Id;

                var res = await Client.Child(Path).Child(entity.Id).PutAsync<T>(entity).ContinueWith((task) =>
                {
                    return entity;
                });

                return res;
            }
            catch(FirebaseAuthException firebaseExcep)
            {
                if (firebaseExcep.Reason != AuthErrorReason.InvalidAccessToken)
                    return null;

                var refreshToken = await ServiceInstances.AuthService.Login(UserSession.Instance.LoggedinUser.LoginName, UserSession.Instance.LoggedinUser.Password, (err) =>
                {

                });

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    return await InsertAsync(entity);
                }
                else
                    return null;
            }
        }

        public async Task<IList<T>> GetAll()
        {
            try
            {
                var items = new List<T>();

                var collection =  await Client.Child(Path).OnceAsync<T>();

                foreach(var item in collection)
                {
                    items.Add(item.Object);
                }

                return items;
            }
            catch (FirebaseAuthException firebaseExcep)
            {
                if (firebaseExcep.Reason != AuthErrorReason.InvalidAccessToken)
                    return null;

                var refreshToken = await ServiceInstances.AuthService.Login(UserSession.Instance.LoggedinUser.LoginName, UserSession.Instance.LoggedinUser.Password, (err) =>
                {

                });

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    return await GetAll();
                }
                else
                    return null;
            }
        }

        public async Task<T> GetById(string id)
        {
            try
            {
                return await Client.Child(Path).Child(id).OnceSingleAsync<T>();
            }
            catch(FirebaseAuthException firebaseExcep)
            {
                if (firebaseExcep.Reason != AuthErrorReason.InvalidAccessToken)
                    return null;

                var refreshToken = await ServiceInstances.AuthService.Login(UserSession.Instance.LoggedinUser.LoginName, UserSession.Instance.LoggedinUser.Password, (err) =>
                {

                });

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    return await GetById(id);
                }
                else
                    return null;
            }
        }

        public async Task<T> UpdateById(T entity)
        {
            try
            {
                entity.Updated = DateTime.Now.GetLocalTimeTotalSeconds();

                return await Client.Child(Path).Child(entity.Id).PutAsync<T>(entity).ContinueWith((task) =>
                {
                    return entity;
                });
            }
            catch (FirebaseAuthException firebaseExcep)
            {
                if (firebaseExcep.Reason != AuthErrorReason.InvalidAccessToken)
                    return null;

                var refreshToken = await ServiceInstances.AuthService.Login(UserSession.Instance.LoggedinUser.LoginName, UserSession.Instance.LoggedinUser.Password, (err) =>
                {

                });

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    return await UpdateById(entity);
                }
                else
                    return null;
            }
          
        }

        public async Task DeleteById(string id)
        {
            try
            {
                await Client.Child(Path).Child(id).DeleteAsync(new TimeSpan(0, 0, 10));
            }
            catch (FirebaseAuthException firebaseExcep)
            {
                if (firebaseExcep.Reason != AuthErrorReason.InvalidAccessToken)
                    return ;

                var refreshToken = await ServiceInstances.AuthService.Login(UserSession.Instance.LoggedinUser.LoginName, UserSession.Instance.LoggedinUser.Password, (err) =>
                {

                });

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    await DeleteById(id);
                }
                else
                    return;
            }
        }

        public void NewRecordListening(Action<T> action)
        {
            Client.Child(Path).AsObservable<T>().Subscribe(d => action?.Invoke(d.Object));
        }

     
    }
}
