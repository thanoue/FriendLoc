using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MusicApp.Services.Impl
{
    public class DownloadService : IDownloadService
    {
        private HttpClient _client;
        private int bufferSize = 4095;

        private readonly IFileService _fileService;


        public DownloadService(IFileService fileService)
        {
            _client = new HttpClient();
            _fileService = fileService;
        }

        public async Task DownloadFileAsync(string url, IProgress<double> progress,Action<string> onCompleted, CancellationToken token,string fileExtension)
        {
            try
            {
                // Step 1 : Get call
                var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
                }

                // Step 2 : Filename
                var fileName = response.Content.Headers?.ContentDisposition?.FileName ?? Guid.NewGuid().ToString()+"." + fileExtension;

                // Step 3 : Get total of data
                var totalData = response.Content.Headers.ContentLength.GetValueOrDefault(-1L);
                var canSendProgress = totalData != -1L && progress != null;

                // Step 4 : Get total of data
                var filePath = Path.Combine(_fileService.GetStorageFolderPath(), fileName);

                // Step 5 : Download data
                using (var fileStream = _fileService.OpenStream(filePath,bufferSize))
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        var totalRead = 0L;
                        var buffer = new byte[bufferSize];
                        var isMoreDataToRead = true;

                        do
                        {
                            token.ThrowIfCancellationRequested();

                            var read = await stream.ReadAsync(buffer, 0, buffer.Length, token);

                            if (read == 0)
                            {
                                isMoreDataToRead = false;

                                onCompleted?.Invoke(fileName);
                            }
                            else
                            {
                                // Write data on disk.
                                await fileStream.WriteAsync(buffer, 0, read);

                                totalRead += read;

                                if (canSendProgress)
                                {
                                    progress.Report((totalRead * 1d) / (totalData * 1d) * 100);
                                }
                            }

                        } while (isMoreDataToRead);
                    }
                }
            }
            catch (Exception e)
            {
                // Manage the exception as you need here.
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
    }
}
