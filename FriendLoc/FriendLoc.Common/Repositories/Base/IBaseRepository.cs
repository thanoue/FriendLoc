using System;
using System.Threading.Tasks;
using Firebase.Database;
using FriendLoc.Common.Models;

namespace FriendLoc.Common.Repositories
{
    public interface IBaseRepository<T> where T: BaseEntity
    {
        void Init(FirebaseClient client);
        Task<T> InsertAsync(T entity);
        Task<T> GetById(string id);
        Task<T> UpdateById(T entity);
        Task DeleteById(string id);
        void NewRecordListening(Action<T> action);
    }
}
