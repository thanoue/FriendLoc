using System;
using System.Threading;
using System.Threading.Tasks;

namespace MusicApp.Services
{
    public interface IDownloadService
    {
        Task DownloadFileAsync(string url, IProgress<double> progress, Action<string> onCompleted, CancellationToken token, string fileExtension);
    }
}
