package surfer.server.util.cache;

/**
 * <pre>
 * 快取來源類別
 * </pre>
 *
 * @param <K> 索引型態
 * @param <D> 資料型態
 * @author yinweli
 */
public interface CacheSource<K, D>
{
    /**
     * <pre>
     * 讀取資料
     * </pre>
     * 
     * @param key 索引
     * @return 資料物件
     */
    D load(K key);
    
    /**
     * <pre>
     * 儲存資料
     * </pre>
     * 
     * @param key 索引
     * @param data 資料物件
     */
    void save(K key, D data);
}
