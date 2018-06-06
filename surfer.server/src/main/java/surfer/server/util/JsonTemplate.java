package surfer.server.util;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Scanner;
import java.util.Set;
import java.util.stream.Collectors;

/**
 * <pre>
 * json樣板資料庫
 * </pre>
 *
 * @param <K> 索引型別
 * @param <D> 資料型別
 */
public class JsonTemplate<K, D>
{
    /**
     * <pre>
     * 樣板資料介面
     * </pre>
     *
     * @param <K> 索引型別
     * @param <D> 資料型別
     */
    public interface Base<K, D>
    {
        /**
         * <pre>
         * 解析json字串
         * </pre>
         * 
         * @param json json字串
         * @return <索引物件, 資料物件>
         */
        public abstract Multi2<K, D> parse(String json);
    }
    
    /** 樣板資料列表 */
    private Map<K, D> templates = new HashMap<>();
    
    /**
     * @param path 樣板資料路徑
     * @param base 樣板資料物件
     */
    public JsonTemplate(String path, Base<K, D> base)
    {
        try
        {
            Scanner scanner = new Scanner(new File(path));
            
            while (scanner.hasNextLine())
            {
                Multi2<K, D> result = base.parse(scanner.nextLine());
                
                templates.put(result.get1(), result.get2());
            } //while
            
            scanner.close();
        }
        catch (FileNotFoundException e)
        {
            e.printStackTrace();
        }
    }
    
    /**
     * <pre>
     * 取得樣板資料
     * </pre>
     * 
     * @param key 索引物件
     * @return 資料物件
     */
    public D get(K key)
    {
        return templates.get(key);
    }
    
    /**
     * <pre>
     * 取得索引列表
     * </pre>
     * 
     * @return 索引列表
     */
    public Set<K> keys()
    {
        return templates.keySet();
    }
    
    /**
     * <pre>
     * 取得資料列表
     * </pre>
     * 
     * @return 資料列表
     */
    public List<D> values()
    {
        return templates.entrySet().stream().map(itor -> itor.getValue()).collect(Collectors.toList());
    }
    
    /**
     * <pre>
     * 取得尋訪列表
     * </pre>
     * 
     * @return 尋訪列表
     */
    public Set<Map.Entry<K, D>> entrys()
    {
        return templates.entrySet();
    }
    
    /**
     * <pre>
     * 取得樣板資料數量
     * </pre>
     * 
     * @return 樣板資料數量
     */
    public int size()
    {
        return templates.size();
    }
}
