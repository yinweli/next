package surfer.server.util;

import java.util.Map;
import java.util.TreeMap;

public class Dice<T>
{
    /** 骰子內容列表 */
    private Map<Integer, T> dices = new TreeMap<Integer, T>();
    /** 骰子最大值 */
    private int max = 0;
    
    /**
     * <pre>
     * 新增項目
     * </pre>
     * 
     * @param probability 機率值
     * @param data 資料物件
     */
    public void add(int probability, T data)
    {
        dices.put(max += probability, data);
    }
    
    /**
     * <pre>
     * 以輸入值獲得項目
     * 如果輸入值為隨機值, 便會隨機取得項目
     * </pre>
     * 
     * @param value 值
     * @return 資料物件
     */
    public T roll(int value)
    {
        return dices
            .entrySet()
            .stream()
            .filter(itor -> itor.getKey() >= value)
            .map(itor -> itor.getValue())
            .findFirst()
            .orElse(null);
    }
    
    /**
     * <pre>
     * 取得骰子最大值
     * </pre>
     * 
     * @return 骰子最大值
     */
    public int getMax()
    {
        return max;
    }
    
    /**
     * <pre>
     * 取得是否為空
     * </pre>
     * 
     * @return true表示為空, false則否
     */
    public boolean isEmpty()
    {
        return dices.isEmpty();
    }
}
