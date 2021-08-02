using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MusicApp.Model.ApiModels;
using MusicApp.Static;
using Newtonsoft.Json;
using RestSharp;
using static MusicApp.Static.Constants;

namespace MusicApp.Services.Impl
{
    public class ApiClient : IApiClient
    {
        private IRestClient _client;

        public ApiClient()
        {
            _client = new RestClient(API_HOST);
        }

        public async  Task<SelectResponseModel> GetMediaItems(string src)
        {
            IRestRequest request = new RestRequest(ApiEndPoints.SELECT_VIDEOS, Method.POST);

            AddDefaultHeaders(request);
            request.AddJsonBody(JsonConvert.SerializeObject(new SelectApiModel(src)));

            return await SendRequestRestSharp<SelectResponseModel>(request);
        }

        public async Task<SearchResponseModel> SearchVideos(SearchApiModel model)
        {
            IRestRequest request = new RestRequest(ApiEndPoints.SEARCH_VIDEOS, Method.POST);

            AddDefaultHeaders(request);
            request.AddJsonBody(JsonConvert.SerializeObject(model));

            return await SendRequestRestSharp<SearchResponseModel>(request);
        }

        public async Task<PlaylistResponse> GetPlaylistItems(string playlistId,int maxCount,string pageToken)
        {

            var client = new RestClient("https://www.googleapis.com");

            IRestRequest request = new RestRequest("/youtube/v3/playlistItems", Method.GET);
            AddDefaultHeaders(request);

            request.AddParameter("part", "snippet");
            request.AddParameter("maxResults", maxCount.ToString());
            request.AddParameter("playlistId", playlistId);
            request.AddParameter("key", "AIzaSyDQlyytMtymqjfiE_txI4LiTW4guVayKLw");
            request.AddParameter("pageToken", pageToken);


            IRestRequest request2 = new RestRequest("/youtube/v3/playlists", Method.GET);
            AddDefaultHeaders(request2);

            request2.AddParameter("part", "snippet");
            request2.AddParameter("maxResults", "1");
            request2.AddParameter("id", playlistId);
            request2.AddParameter("key", "AIzaSyDQlyytMtymqjfiE_txI4LiTW4guVayKLw");

            var one = SendRequestRestSharp<PlaylistItems>(request, client);
            var two = SendRequestRestSharp<PlaylistInfo>(request2, client);

            await Task.WhenAll(one, two);

            var videos = await one;
            var info = await two;

            return  new PlaylistResponse()
            {
                Items = videos,
                Info = info.Items.FirstOrDefault()?.Snippet
            };
        }

        void AddDefaultHeaders(IRestRequest request)
        {
            request.AddHeader("Access-Control-Max-Age", "3600");
            request.AddHeader("charset", "UTF-8");
            request.AddHeader("Content-Type", "application/json");
            request.Timeout = Constants.REQUEST_TIMEOUT;
            request.RequestFormat = DataFormat.Json;
        }

        async Task<T> SendRequestRestSharp<T>(IRestRequest request,IRestClient client = null)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var restClient = client == null ? _client : client;
                    IRestResponse<string> response = restClient.Execute<string>(request);
                    return JsonConvert.DeserializeObject<T>(response.Data);
                }
                catch (ServiceInvokeException sie)
                {
                    throw sie;
                }
                catch (AggregateException ae)
                {
                    throw ae.Flatten();
                }
                catch (Exception e)
                {
                    throw e;
                }
            });

        }
    }

    public class ServiceInvokeException : Exception
    {
        public HttpResponseMessage ResponseMessage { get; }
        public ErrorResponseMessage ErrorResponseDetails { get; set; }

        public ServiceInvokeException(HttpResponseMessage responseMessage, ErrorResponseMessage errorResponse)
        {
            ResponseMessage = responseMessage;
            ErrorResponseDetails = errorResponse;
        }

        public class ErrorResponseMessage
        {
            public string ErrorMessage { get; set; }
            public string ExceptionType { get; set; }
            public string InnerExceptionType { get; set; } = null;
            public int? ErrorCode { get; set; } = null;
            public string ErrorDetails { get; set; } = null;
        }
    }
}
