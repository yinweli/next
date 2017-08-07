package next.server.util;

import org.apache.commons.lang.time.StopWatch;

/**
 * <pre>
 * 自動化StopWatch
 * </pre>
 */
public class AutoStopWatch implements AutoCloseable
{
    /**
     * <pre>
     * 關閉執行介面
     * </pre>
     */
    public interface CloseExecute
    {
        /**
         * <pre>
         * 關閉執行函式
         * </pre>
         * 
         * @param time 執行時間(毫秒)
         */
        public void execute(long time);
    }
    
    /** StopWatch物件 */
    private StopWatch stopWatch = new StopWatch();
    /** 關閉執行物件 */
    private CloseExecute closeExecute = null;
    
    public AutoStopWatch()
    {
    }
    
    /**
     * @param closeExecute 關閉執行物件
     */
    public AutoStopWatch(CloseExecute closeExecute)
    {
        this.closeExecute = closeExecute;
    }
    
    @Override
    public void close() throws Exception
    {
        stop();
        
        if (closeExecute != null)
            closeExecute.execute(getTime());
    }
    
    /**
     * <pre>
     * 啟動計時
     * </pre>
     * 
     * @return AutoWatch物件
     */
    public AutoStopWatch start()
    {
        stopWatch.start();
        
        return this;
    }
    
    /**
     * <pre>
     * 停止計時
     * </pre>
     * 
     * @return AutoWatch物件
     */
    public AutoStopWatch stop()
    {
        stopWatch.stop();
        
        return this;
    }
    
    /**
     * <pre>
     * 取得執行時間
     * </pre>
     * 
     * @return 執行時間(毫秒)
     */
    public long getTime()
    {
        return stopWatch.getTime();
    }
}
