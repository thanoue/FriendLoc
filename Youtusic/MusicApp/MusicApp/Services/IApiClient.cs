using System;
using System.Threading.Tasks;
using MusicApp.Model.ApiModels;

namespace MusicApp.Services
{
    public interface IApiClient
    {
        Task<SearchResponseModel> SearchVideos(SearchApiModel model);
        Task<SelectResponseModel> GetMediaItems(string src);
    }
}
