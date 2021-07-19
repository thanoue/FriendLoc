using System;
using System.IO;
using System.Threading.Tasks;

namespace iCho.Service
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
