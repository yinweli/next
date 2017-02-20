package next.server.util.scheduled;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import java.util.concurrent.RejectedExecutionException;
import java.util.concurrent.RejectedExecutionHandler;
import java.util.concurrent.ScheduledFuture;
import java.util.concurrent.ScheduledThreadPoolExecutor;
import java.util.concurrent.ThreadPoolExecutor;
import java.util.concurrent.TimeUnit;

import org.apache.log4j.Logger;

/**
 * <pre>
 * 排程類別
 * 
 * 可以利用此類別加入需要定時定期執行的工作
 * </pre>
 * 
 * @author yinweli
 */
public class Scheduled
{
    private static class ScheduledWapper implements Runnable
    {
        private static final Logger log = Logger.getLogger(ScheduledWapper.class);
        
        /** 執行物件 */
        private Runnable runnable = null;
        
        public ScheduledWapper(Runnable runnable)
        {
            this.runnable = runnable;
        }
        
        @Override
        public void run()
        {
            long millisecondOfBegin = System.currentTimeMillis();
            
            try
            {
                runnable.run();
            }
            catch (Exception e)
            {
                log.error("exception in runnable execute", e);
            }
            finally
            {
                long millisecondOfRunning = System.currentTimeMillis() - millisecondOfBegin;
                
                if (TimeUnit.SECONDS.convert(millisecondOfRunning, TimeUnit.MILLISECONDS) >= SECOND_WARNING)
                    log.warn(runnable.getClass() + " execute time=" + millisecondOfRunning + "(ms)");
            }
        }
    }
    
    private static class ScheduledRejectedExecutionHandler implements RejectedExecutionHandler
    {
        private static final Logger log = Logger.getLogger(ScheduledRejectedExecutionHandler.class);
        
        @Override
        public void rejectedExecution(Runnable runnable, ThreadPoolExecutor executor)
        {
            if (executor.isShutdown())
                return;
            
            log.warn(runnable + " from " + executor, new RejectedExecutionException());
            
            if (Thread.currentThread().getPriority() > Thread.NORM_PRIORITY)
                new Thread(runnable).start();
            else
                runnable.run();
        }
    }
    
    /** 排程執行工作超時警告時間(秒) */
    private static int SECOND_WARNING = 5;
    /** 每日執行間隔時間(時) */
    private static int HOUR_DAILY_PERIOD = 24;
    /** 每日執行間隔時間(毫秒) */
    private static long MILLISECOND_DAILY_PERIOD = TimeUnit.MILLISECONDS.convert(HOUR_DAILY_PERIOD, TimeUnit.HOURS);
    /** 每周執行間隔時間(時) */
    private static int HOUR_WEEK_PERIOD = 24 * 7;
    /** 每周執行間隔時間(毫秒) */
    private static long MILLISECOND_WEEK_PERIOD = TimeUnit.MILLISECONDS.convert(HOUR_WEEK_PERIOD, TimeUnit.HOURS);
    
    /** 執行緒池物件 */
    private ScheduledThreadPoolExecutor threadPool = null;
    /** 工作列表 */
    private List<ScheduledFuture<?>> scheduledFutures = new ArrayList<>();
    
    /**
     * <pre>
     * 初始化排程
     * </pre>
     * 
     * @param countOfThread 執行緒數量, 不會低於1
     */
    public void initialize(int countOfThread)
    {
        countOfThread = Math.max(1, countOfThread);
        
        threadPool = new ScheduledThreadPoolExecutor(countOfThread);
        threadPool.setRejectedExecutionHandler(new ScheduledRejectedExecutionHandler());
        threadPool.prestartAllCoreThreads();
    }
    
    /**
     * <pre>
     * 清除排程
     * </pre>
     */
    public void clear()
    {
        for (ScheduledFuture<?> itor : scheduledFutures)
            itor.cancel(false);
        
        scheduledFutures.clear();
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
        if (threadPool == null)
            throw new Exception("scheduled not start");
        
        ScheduledFuture<?> scheduledFuture = threadPool.schedule(new ScheduledWapper(runnable), delay, timeUnit);
        
        scheduledFutures.add(scheduledFuture);
        
        return scheduledFuture;
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
        if (threadPool == null)
            throw new Exception("scheduled not start");
        
        ScheduledFuture<?> scheduledFuture = threadPool
            .scheduleAtFixedRate(new ScheduledWapper(runnable), delay, period, timeUnit);
        
        scheduledFutures.add(scheduledFuture);
        
        return scheduledFuture;
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
        long millisecondOfNow = System.currentTimeMillis();
        Calendar calendar = Calendar.getInstance();
        
        calendar.setTimeInMillis(millisecondOfNow);
        calendar.set(Calendar.HOUR_OF_DAY, hourOfStart);
        calendar.set(Calendar.MINUTE, minuteOfStart);
        calendar.set(Calendar.SECOND, 0);
        calendar.set(Calendar.MILLISECOND, 0);
        
        if (millisecondOfNow >= calendar.getTime().getTime())
            calendar.add(Calendar.HOUR, HOUR_DAILY_PERIOD);
        
        long delay = calendar.getTime().getTime() - millisecondOfNow;
        
        return schedule(runnable, delay, MILLISECOND_DAILY_PERIOD, TimeUnit.MILLISECONDS);
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
        long millisecondOfNow = System.currentTimeMillis();
        Calendar calendar = Calendar.getInstance();
        
        calendar.setTimeInMillis(millisecondOfNow);
        calendar.set(Calendar.DAY_OF_WEEK, weekOfStart);
        calendar.set(Calendar.HOUR_OF_DAY, hourOfStart);
        calendar.set(Calendar.MINUTE, minuteOfStart);
        calendar.set(Calendar.SECOND, 0);
        calendar.set(Calendar.MILLISECOND, 0);
        
        if (millisecondOfNow >= calendar.getTime().getTime())
            calendar.add(Calendar.HOUR, HOUR_WEEK_PERIOD);
        
        long delay = calendar.getTime().getTime() - millisecondOfNow;
        
        return schedule(runnable, delay, MILLISECOND_WEEK_PERIOD, TimeUnit.MILLISECONDS);
    }
}
