package service.scheduled;

import java.util.concurrent.ScheduledFuture;
import java.util.concurrent.TimeUnit;

import next.server.util.scheduled.Scheduled;
import next.server.util.service.ServiceConfig;
import next.server.util.service.ServiceFinish;
import next.server.util.service.ServiceStart;
import next.server.util.thead.ThreadUtil;

@ServiceConfig(configClass = ScheduledConfig.class)
@ServiceStart
@ServiceFinish
public class ScheduledService
{
    /** 排程物件 */
    private static Scheduled scheduled = new Scheduled();
    
    /**
     * <pre>
     * 服務啟動
     * </pre>
     */
    public static void start()
    {
        scheduled.initialize(ThreadUtil.calculateThread(ScheduledConfig.MULTIPLE_THREAD));
    }
    
    /**
     * <pre>
     * 服務結束
     * </pre>
     */
    public static void finish()
    {
        scheduled.clear();
    }
    
    /**
     * <pre>
     * 單次執行工作
     * </pre>
     * 
     * @param runnable 執行物件
     * @param delay 延遲時間
     * @param timeUnit 時間單位物件
     * @return 工作控制物件
     * @throws Exception
     */
    public ScheduledFuture<?> schedule(Runnable runnable, long delay, TimeUnit timeUnit) throws Exception
    {
        return scheduled.schedule(runnable, delay, timeUnit);
    }
    
    /**
     * <pre>
     * 循環執行工作
     * </pre>
     * 
     * @param runnable 執行物件
     * @param delay 首次執行的延遲時間
     * @param period 間隔時間
     * @param timeUnit 時間單位物件
     * @return 工作控制物件
     * @throws Exception
     */
    public ScheduledFuture<?> schedule(Runnable runnable, long delay, long period, TimeUnit timeUnit) throws Exception
    {
        return scheduled.schedule(runnable, delay, period, timeUnit);
    }
    
    /**
     * <pre>
     * 每日循環執行工作
     * </pre>
     * 
     * @param runnable 執行物件
     * @param hourOfStart 開始時(0~23)
     * @param minuteOfStart 開始分(0~59)
     * @return 工作控制物件
     * @throws Exception
     */
    public ScheduledFuture<?> scheduleDaily(Runnable runnable, int hourOfStart, int minuteOfStart) throws Exception
    {
        return scheduled.scheduleDaily(runnable, hourOfStart, minuteOfStart);
    }
    
    /**
     * <pre>
     * 每周循環執行工作
     * </pre>
     * 
     * @param runnable 執行物件
     * @param weekOfStart 開始日(1:周日, 2:周一, 3:周二, 4:周三, 5:周四, 6:周五, 7:周六)
     * @param hourOfStart 開始時(0~23)
     * @param minuteOfStart 開始分(0~59)
     * @return 工作控制物件
     * @throws Exception
     */
    public ScheduledFuture<?> scheduleWeek(Runnable runnable, int weekOfStart, int hourOfStart, int minuteOfStart) throws Exception
    {
        return scheduled.scheduleWeek(runnable, weekOfStart, hourOfStart, minuteOfStart);
    }
}
