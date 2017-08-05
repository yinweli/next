package next.server.util.dice;

import java.util.Map;
import java.util.TreeMap;

public class Dice<T>
{
    public interface RandomValue
    {
        int rand(int max);
    }
    
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
        for (Map.Entry<Integer, T> itor : dices.entrySet())
        {
            if (itor.getKey() >= value)
                return itor.getValue();
        } //for
        
        return null;
    }
}
