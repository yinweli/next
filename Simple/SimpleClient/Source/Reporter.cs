using System;

namespace SimpleClient
{
    public class Reporter
    {
        private const int REPORT_COUNT = 10000;

        private static System.Object syncLock = new System.Object();
        private static long passTime = 0;
        private static long reportCount = 0;

        public static void report(long time)
        {
            lock (syncLock)
            {
                passTime += time;
                reportCount++;

                if (reportCount >= REPORT_COUNT)
                {
                    Console.WriteLine(String.Format("passTime={0}, avgTime={1}", passTime, passTime / reportCount));
                    passTime = 0;
                    reportCount = 0;
                }//if
            }//lock
        }
    }
}