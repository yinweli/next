package next.server.util.cache;

/**
 * <pre>
 * 快取計時類別
 * </pre>
 * 
 * @author yinweli
 */
public class CacheTimer
{
    /** 移除時間(毫秒) */
    private long millisecondOfRemove;
    
    public CacheTimer(long millisecondOfSave)
    {
        this.millisecondOfRemove = System.currentTimeMillis() + millisecondOfSave;
    }
    
    /**
     * <pre>
     * 檢查是否過期
     * </pre>
     * 
     * @return true表示過期, false則否
     */
    public boolean timesUp()
    {
        return System.currentTimeMillis() >= millisecondOfRemove;
    }
}
