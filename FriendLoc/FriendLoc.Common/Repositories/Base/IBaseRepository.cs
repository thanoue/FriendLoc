using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Firebase.Database;
using FriendLoc.Entity;

namespace FriendLoc.Common.Repositories
{
    public interface IBaseRepository<T> where T: BaseEntity
    {
        Task<T> InsertAsync(T entity);
        FirebaseClient Client { get; }
        Task<T> GetById(string id);
        Task<IList<T>> GetAll();
        Task<T> UpdateById(T entity);
        Task DeleteById(string id);
        void NewRecordListening(Action<T> action);
        Task<string> UploadFile(Stream imageData, string folderName, Action<int> progressCallback);
    }
}
