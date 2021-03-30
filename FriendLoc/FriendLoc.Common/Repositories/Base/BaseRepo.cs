using System;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using FriendLoc.Common;
using System.IO;
using Firebase.Storage;
using FriendLoc.Entity;

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
            entity.Created = DateTime.Now.GetLocalTimeTotalSeconds();
            entity.Updated = DateTime.Now.GetLocalTimeTotalSeconds();
            entity.Id = string.IsNullOrEmpty(entity.Id) ? Guid.NewGuid().ToString() : entity.Id;

            var res = await Client.Child(Path).Child(entity.Id).PutAsync<T>(entity).ContinueWith((task) =>
            {
                return entity;
            });

            return res;
        }

        public async Task<T> GetById(string id)
        {
            return await Client.Child(Path).Child(id).OnceSingleAsync<T>();
        }

        public async Task<T> UpdateById(T entity)
        {
            entity.Updated = DateTime.Now.GetLocalTimeTotalSeconds();

            return await Client.Child(Path).Child(entity.Id).PutAsync<T>(entity).ContinueWith((task) =>
            {
                return entity;
            });
        }

        public async Task DeleteById(string id)
        {
            await Client.Child(Path).Child(id).DeleteAsync(new TimeSpan(0, 0, 10));
        }

        public void NewRecordListening(Action<T> action)
        {
            Client.Child(Path).AsObservable<T>().Subscribe(d => action?.Invoke(d.Object));
        }
    }
}
