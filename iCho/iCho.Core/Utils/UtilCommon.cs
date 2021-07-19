using System;
namespace iCho.Core.Utils
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

        public static double GetLocalTimeTotalMiliseconds(this DateTime localtime)
        {
            return localtime.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalMilliseconds;
        }

        public static DateTime TotalMilisecondsToLocalTime(this double totalUtcMiliseconds)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dtfommls = dt.AddMilliseconds(totalUtcMiliseconds);

            return dtfommls.ToLocalTime();
        }
    }
}
