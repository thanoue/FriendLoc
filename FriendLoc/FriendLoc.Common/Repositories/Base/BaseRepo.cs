using System;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using FriendLoc.Common.Models;
using FriendLoc.Common;

namespace FriendLoc.Common.Repositories
{
    public abstract class BaseRepo<T> : IBaseRepository<T> where T : BaseEntity 
    {
        protected FirebaseClient Client;
        public abstract string Path { get; }

        public BaseRepo()
        {
           
        }

        public void Init(FirebaseClient client)
        {
            Client = client;
        }

        public async Task<T> InsertAsync(T entity)
        {
            entity.Created = DateTime.Now.GetLocalTimeTotalSeconds();
            entity.Updated = DateTime.Now.GetLocalTimeTotalSeconds();
            entity.Id = Guid.NewGuid().ToString();

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
