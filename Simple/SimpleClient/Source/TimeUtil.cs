using System;

namespace SimpleClient
{
    public class TimeUtil
    {
        private static DateTime baseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);

        public static long currentMillisecond()
        {
            return (long)(DateTime.Now - baseTime).TotalMilliseconds;
        }
    }
}