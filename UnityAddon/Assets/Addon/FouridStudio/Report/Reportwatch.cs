using System;
using System.Diagnostics;

namespace FouridStudio
{
    /// <summary>
    /// 報告計時類別
    /// </summary>
    public class Reportwatch : IDisposable
    {
        private Stopwatch stopWatch = new Stopwatch();
        private string message = "";

        public Reportwatch(string message)
        {
            this.message = message;
        }

        public void Dispose()
        {
            report(false);
        }

        /// <summary>
        /// 重置計時
        /// </summary>
        public void reset()
        {
            stopWatch.Reset();
        }

        /// <summary>
        /// 啟動計時
        /// </summary>
        public void start()
        {
            stopWatch.Start();
        }

        /// <summary>
        /// 停止計時
        /// </summary>
        public void stop()
        {
            stopWatch.Stop();
        }

        /// <summary>
        /// 報告計時
        /// </summary>
        /// <param name="restart">重新啟動計時旗標</param>
        public void report(bool restart = true)
        {
            Report.Instance.log(message + "(time=" + stopWatch.ElapsedMilliseconds + "ms)");

            if (restart == false)
                return;

            reset();
            start();
        }
    }
}