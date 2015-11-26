using System;

namespace G1mist.Nancy.Common
{
    public static class CommonTools
    {
        public static DateTime GetTime(string timeStamp)
        {
            var dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var lTime = long.Parse(timeStamp + "0000000");
            var toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
    }
}
