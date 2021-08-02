using System;
namespace MusicApp.Static
{
    public static class Constants
    {
        //public const string API_HOST = "http://192.168.1.8:3010/";
        public const string API_HOST = "https://di-cho.xyz/";
        public const int REQUEST_TIMEOUT = 30000;

        public const string SECURE_SONG_ITEM = "SECURE_SONG_ITEM";
        public const string SAVED_SONG_IDS = "SAVED_SONG_IDS";
        public const string LAYOUT_APPEARED = "LAYOUT_APPEARED";
        public const string GET_SONG_FROM_YOUTUBE = "GET_SONG_FROM_YOUTUBE";
        
        public class ApiEndPoints
        {
            public const string SEARCH_VIDEOS = "/api/youtube/searchVideos";
            public const string SELECT_VIDEOS = "/api/youtube/get";
        }
    }
}
