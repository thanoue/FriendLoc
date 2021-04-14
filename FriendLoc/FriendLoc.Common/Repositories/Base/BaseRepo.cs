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
using FriendLoc.Common.Services;

namespace FriendLoc.Common.Repositories
{
    public abstract class BaseRepo<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected IGlobalUIService UtilUI => ServiceLocator.Instance.Get<IGlobalUIService>();

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

            task.Progress.ProgressChanged += (s, e) =>
            {
                Console.WriteLine($"Progress: {e.Percentage} %");
                progressCallback?.Invoke(e.Percentage);
            };

            return await task;
        }

        public virtual async Task<T> InsertAsync(T entity)
        {
            return await Handle<T>(async () =>
            {
                entity.Created = DateTime.Now.GetLocalTimeTotalSeconds();
                entity.Updated = DateTime.Now.GetLocalTimeTotalSeconds();
                entity.Id = string.IsNullOrEmpty(entity.Id) ? Guid.NewGuid().ToString() : entity.Id;

                return await Client.Child(Path).Child(entity.Id).PutAsync<T>(entity).ContinueWith((task) =>
                {
                    return entity;
                });
            });
        }

        public async Task<IList<T>> GetAll()
        {
            return await Handle<IList<T>>(async () =>
            {
                var items = new List<T>();

                var collection = await Client.Child(Path).OnceAsync<T>();

                foreach (var item in collection)
                {
                    items.Add(item.Object);
                }

                return items;
            });
        }

        public void NewRecordListening(Action<T> action)
        {
            Client.Child(Path).AsObservable<T>().Subscribe(d => action?.Invoke(d.Object));
        }

        public Task<T> GetById(string id)
        {
            return Handle<T>(async () => { return await Client.Child(Path).Child(id).OnceSingleAsync<T>(); });
        }

        public async Task<T> UpdateById(T entity)
        {
            return await Handle<T>(async () =>
            {
                entity.Updated = DateTime.Now.GetLocalTimeTotalSeconds();

                return await Client.Child(Path).Child(entity.Id).PutAsync<T>(entity).ContinueWith((task) =>
                {
                    return entity;
                });
            });
        }

        public async Task<bool> DeleteById(string id)
        {
            return await Handle<bool>(async () =>
            {
                return await Client.Child(Path).Child(id).DeleteAsync(new TimeSpan(0, 0, 10))
                    .ContinueWith((rs) => { return !rs.IsFaulted; });
            });
        }

        public async Task<U> Handle<U>(Func<Task<U>> func)
        {
            try
            {
                UtilUI.StartLoading();

                return await func.Invoke().ContinueWith<U>((res) =>
                {
                    UtilUI.StopLoading();

                    return res.Result;
                });
            }
            catch (FirebaseException firebaseException)
            {
                UtilUI.ErrorToast(firebaseException.Message);
                UtilUI.StopLoading();
                return default(U);
            }
            catch (FirebaseAuthException firebaseExcep)
            {
                UtilUI.ErrorToast(firebaseExcep.Message);
                UtilUI.StopLoading();

                if (firebaseExcep.Reason != AuthErrorReason.InvalidAccessToken)
                {
                    return default(U);
                }

                var refreshToken = await ServiceInstances.AuthService.Login(UserSession.Instance.LoggedinUser.LoginName,
                    UserSession.Instance.LoggedinUser.Password, (err) => { });

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    return await Handle<U>(func);
                }
                else
                {
                    return default(U);
                }
            }
            catch (Exception ex)
            {
                UtilUI.ErrorToast(ex.Message);
                UtilUI.StopLoading();


                return default(U);
            }
        }
    }
}