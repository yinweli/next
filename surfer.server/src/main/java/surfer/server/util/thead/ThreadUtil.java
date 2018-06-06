package surfer.server.util.thead;

/**
 * <pre>
 * 執行緒工具類別
 * </pre>
 * 
 * @author yinweli
 */
public class ThreadUtil
{
    /**
     * <pre>
     * 依照輸入倍數計算出執行緒數量
     * </pre>
     * 
     * @param multiple 倍數
     * @return 執行緒數量
     */
    public static int calculateThread(double multiple)
    {
        return (int) (Runtime.getRuntime().availableProcessors() * multiple);
    }
}
