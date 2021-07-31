using System;
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

        void AddDefaultHeaders(IRestRequest request)
        {
            request.AddHeader("Access-Control-Max-Age", "3600");
            request.AddHeader("charset", "UTF-8");
            request.AddHeader("Content-Type", "application/json");
            request.Timeout = Constants.REQUEST_TIMEOUT;
            request.RequestFormat = DataFormat.Json;
        }

        async Task<T> SendRequestRestSharp<T>(IRestRequest request)
        {
            return await Task.Run(() =>
            {
                try
                {
                    IRestResponse<string> response = _client.Execute<string>(request);
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
