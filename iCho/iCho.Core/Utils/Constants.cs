using System;

namespace iCho.Core.Utils
{
    public static class Constants
    {
        public const string API_HOST = "http://192.168.1.10:3010/";
        //public const string API_HOST = "https://di-cho.xyz/";
        public const int REQUEST_TIMEOUT = 10000;
    }

    public static class ApiEndPoints
    {
        public const string AUTH_PREFIX = "api/auth/";
        public const string LOGIN_BY_FACCEBOOK = AUTH_PREFIX + "loginByFacebook";
        public const string LOGIN_BY_GOOGLE = AUTH_PREFIX +  "loginByGoogle";
        public const string SIGNUP = AUTH_PREFIX + "signup";
        public const string UPDATE_LOGIN_INFO = AUTH_PREFIX + "updateUserLoginInfo";
        public const string LOGIN_BY_LOGIN_NAME = AUTH_PREFIX + "loginByLoginName";

        public const string HOUSE_PREFIX = "/api/house/";
        public const string CREATE_HOUSE = HOUSE_PREFIX + "create";
    }
}
