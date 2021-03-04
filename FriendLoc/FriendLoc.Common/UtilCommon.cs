using System;
namespace FriendLoc.Common
{
    public static class UtilCommon
    {
        public static double GetLocalTimeTotalSeconds(this DateTime localtime)
        {
            return localtime.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalSeconds; 
        }

        public static DateTime TotalSecondsToLocalTime(this double totalUtcSeconds)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dtfommls = dt.AddSeconds(totalUtcSeconds);

            return dtfommls.ToLocalTime();
        }
    }
}
