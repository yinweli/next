using System;
using System.Diagnostics;

public class AutoStopWatch : IDisposable
{
    private string title = "";
    private Stopwatch stopWatch = new Stopwatch();

    public AutoStopWatch(string title)
    {
        this.title = title;
        this.stopWatch.Start();
    }

    public void Dispose()
    {
        stopWatch.Stop();
        Output.output(string.Format("{0} finish, used time={1}ms", title, stopWatch.ElapsedMilliseconds));
    }
}