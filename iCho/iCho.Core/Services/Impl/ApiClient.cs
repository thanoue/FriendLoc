using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using iCho.Core.ApiModel;
using iCho.Core.Utils;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers;

namespace iCho.Service
{
    public class ApiClient : IApiClient
    {
        private IRestClient _client;

        private UserLoginApiModel _user;

        public UserLoginApiModel User => _user;

        public ApiClient()
        {
            _client = new RestClient(Constants.API_HOST);
        }

        public async Task<UserApiModel> Signup(UserApiModel model, byte[] avtData = null)
        {
            return await SendForm<UserApiModel>(model, ApiEndPoints.SIGNUP,false, "avt", avtData);
        }

        public async Task<UserApiModel> UpdateUserLoginInfo(UserApiModel model, byte[] avtData = null)
        {
            return await SendForm<UserApiModel>(model, ApiEndPoints.UPDATE_LOGIN_INFO,true, "avt", avtData);
        }

        public async Task<HouseApiModel> CreateHouse(HouseApiModel model, byte[] avtData = null)
        {
            return await SendForm<HouseApiModel>(model, ApiEndPoints.CREATE_HOUSE,false, "avt", avtData);
        }

        public async Task<UserLoginApiModel> LoginByFacebook(string token)
        {
            return await LoginBySocialMediaToken(ApiEndPoints.LOGIN_BY_FACCEBOOK, token);
        }

        public async Task<UserLoginApiModel> LoginByGoogle(string token)
        {
            return await LoginBySocialMediaToken(ApiEndPoints.LOGIN_BY_GOOGLE, token);
        }

        public async Task<UserLoginApiModel> LoginByLoginName(string loginName, string password)
        {
            return await SendForm<UserLoginApiModel>(new UserLoginApiModel() { LoginName = loginName,Password = password }, ApiEndPoints.LOGIN_BY_LOGIN_NAME,false);
        }

        async Task<T> SendForm<T>(object body, string endPoint, bool isAuthRequired, string fileName ="", byte[] avtData = null)
        {
            if (isAuthRequired && _user == null)
            {
                throw new ServiceInvokeException(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.Unauthorized,
                }, null);
            }

            IRestRequest request = new RestRequest(endPoint, Method.POST);
            request.AddJsonBody(JsonConvert.SerializeObject(body));

            if (avtData != null)
            {
                request.AddFileBytes(fileName, avtData, "file.jpeg");
                request.AddHeader("enctype", "multipart/form-data");
                request.AddHeader("x-access-token", _user.AccessToken);

                request.AlwaysMultipartFormData = true;
            }
            else
            {
                AddDefaultHeaders(request);
            }

            return await SendRequestRestSharp<T>(request);
        }

        void AddDefaultHeaders(IRestRequest request)
        {
            if (_user != null)
                request.AddHeader("x-access-token", _user.AccessToken);

            request.AddHeader("Access-Control-Max-Age", "3600");
            request.AddHeader("charset", "UTF-8");
            request.AddHeader("Content-Type", "application/json");
            request.Timeout = Constants.REQUEST_TIMEOUT;
            request.RequestFormat = DataFormat.Json;
        }

        async Task<UserLoginApiModel> LoginBySocialMediaToken(string endPoint, string token)
        {
            IRestRequest request = new RestRequest(endPoint, Method.POST);

            request.AddJsonBody(JsonConvert.SerializeObject(new TokenApiModel() { Token = token, Type = ServiceInstances.SocialMediaAuthService.PlatformType }));
            AddDefaultHeaders(request);

            var res = await SendRequestRestSharp<UserLoginApiModel>(request);

            if (res != null)
            {
                _user = res;
                return res;
            }
            else
                return null;
        }

        async Task<T> SendRequestRestSharp<T>(IRestRequest request)
        {
            return await Task.Run(() =>
            {
                try
                {
                    IRestResponse<T> response = _client.Execute<T>(request);
                    return response.Data;
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
