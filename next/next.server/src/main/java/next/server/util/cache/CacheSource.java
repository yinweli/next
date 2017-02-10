package next.server.util.cache;

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
