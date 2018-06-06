package surfer.server.util.cache;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.concurrent.TimeUnit;

import org.apache.log4j.Logger;

/**
 * <pre>
 * 快取類別
 * 
 * 會自己排程檢查資料是否過期了, 並且將之儲存移除的類別
 * 
 * 使用前要先設定
 * Cache.cacheScheduled
 * </pre>
 *
 * @param <K> 索引型態
 * @param <D> 資料型態
 * @author yinweli
 */
public class Cache<K, D>
{
    public interface CacheScheduled
    {
        /**
         * <pre>
         * 新增快取排程
         * </pre>
         * 
         * @param runnable 執行物件
         * @param period 間隔時間
         * @param timeUnit 時間單位物件
         */
        void schedule(Runnable runnable, int period, TimeUnit timeUnit);
    }
    
    private static final Logger log = Logger.getLogger(Cache.class);
    
    /** 更新排程時間(秒) */
    private static final int SECOND_UPDATE_SCHEDULE = 30;
    
    /** 快取排程介面物件 */
    public static CacheScheduled cacheScheduled = null;
    
    /** 快取名稱 */
    private String name = "";
    /** 快取保留時間(毫秒) */
    private long millisecondOfSave = 0;
    /** 快取來源物件 */
    private CacheSource<K, D> cacheSource = null;
    /** 快取列表 */
    private Map<K, D> caches = new HashMap<K, D>();
    /** 移除計時器列表 */
    private Map<K, CacheTimer> timers = new HashMap<K, CacheTimer>();
    
    /**
     * <pre>
     * 初始化快取物件
     * </pre>
     * 
     * @param name 快取名稱
     * @param millisecondOfSave 快取保留時間(毫秒)
     * @param cacheSource 快取來源物件
     * @throws Exception
     */
    public synchronized void initialize(String name, long millisecondOfSave,
        CacheSource<K, D> cacheSource) throws Exception
    {
        if (cacheScheduled == null)
            throw new Exception("cacheScheduled is null");
        
        if (cacheSource == null)
            throw new Exception("cacheSource is null");
        
        this.name = name;
        this.millisecondOfSave = millisecondOfSave;
        this.cacheSource = cacheSource;
        
        cacheScheduled.schedule(new Runnable() {
            @Override
            public void run()
            {
                update();
            }
        }, SECOND_UPDATE_SCHEDULE, TimeUnit.SECONDS);
    }
    
    /**
     * <pre>
     * 取得資料
     * 如果快取內無此資料, 則會試著去快取來源獲取資料
     * </pre>
     * 
     * @param key 索引
     * @return 資料物件
     */
    public synchronized D get(K key)
    {
        D data = caches.get(key);
        
        if (data == null)
            data = cacheSource.load(key);
        
        if (data != null)
        {
            caches.put(key, data);
            timers.put(key, new CacheTimer(millisecondOfSave));
        } //if
        
        return data;
    }
    
    /**
     * <pre>
     * 取得索引列表
     * </pre>
     * 
     * @return 索引列表
     */
    public synchronized Set<K> keys()
    {
        return new HashSet<K>(caches.keySet());
    }
    
    /**
     * <pre>
     * 取得快取名稱
     * </pre>
     * 
     * @return 快取名稱
     */
    public synchronized String getName()
    {
        return name;
    }
    
    /**
     * <pre>
     * 取得快取數量
     * </pre>
     * 
     * @return 快取數量
     */
    public synchronized int size()
    {
        return caches.size();
    }
    
    /**
     * <pre>
     * 儲存資料
     * 這會立即儲存已在快取內的資料
     * </pre>
     */
    public synchronized void save()
    {
        for (K itor : timers.keySet())
            cacheSource.save(itor, caches.get(itor));
    }
    
    /**
     * <pre>
     * 定時更新
     * </pre>
     */
    private synchronized void update()
    {
        List<K> removeItems = new ArrayList<K>();
        
        for (Map.Entry<K, CacheTimer> itor : timers.entrySet())
        {
            if (itor.getValue().timesUp())
                removeItems.add(itor.getKey());
        } //for
        
        for (K itor : removeItems)
        {
            cacheSource.save(itor, caches.get(itor));
            caches.remove(itor);
            timers.remove(itor);
        } //for
        
        if (removeItems.isEmpty() == false)
            log.info("cache [" + name + "] count=" + caches.size() + ", remove items=" + removeItems.size());
    }
}
