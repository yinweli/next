package surfer.server.util;

import java.util.concurrent.TimeUnit;

/**
 * <pre>
 * 時間值類別
 * </pre>
 */
public class TimeValue
{
    /** 時間值(ms) */
    private long time = 0;
    
    /**
     * @param time 時間值
     * @param timeUnit 時間值單位
     */
    public TimeValue(long time, TimeUnit timeUnit)
    {
        this.time = timeUnit.toMillis(time);
    }
    
    /**
     * <pre>
     * 取得秒
     * </pre>
     * 
     * @return 秒
     */
    public long toSecond()
    {
        return TimeUnit.MILLISECONDS.toSeconds(time);
    }
    
    /**
     * <pre>
     * 取得毫秒
     * </pre>
     * 
     * @return 毫秒
     */
    public long toMillis()
    {
        return time;
    }
    
    @Override
    public String toString()
    {
        return TimeUnit.SECONDS.convert(time, TimeUnit.MILLISECONDS) + " second";
    }
}
