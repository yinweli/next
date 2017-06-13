using System;
using System.Diagnostics;

namespace FouridStudio
{
    /// <summary>
    /// 自動報告計時類別
    /// </summary>
    public class AutoStopwatch : IDisposable
    {
        /// <summary>
        /// 報告標題
        /// </summary>
        private string title = "";

        /// <summary>
        /// 計時器物件
        /// </summary>
        private Stopwatch stopWatch = new Stopwatch();

        public AutoStopwatch(string title)
        {
            this.title = title;
            this.stopWatch.Start();
        }

        public void Dispose()
        {
            stopWatch.Stop();
            UnityEngine.Debug.Log(string.Format("{0} finish, used time={1}ms", title, stopWatch.ElapsedMilliseconds));
        }
    }
}