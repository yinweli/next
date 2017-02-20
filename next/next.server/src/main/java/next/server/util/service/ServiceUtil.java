package next.server.util.service;

/**
 * <pre>
 * 服務工具類別
 * </pre>
 * 
 * @author yinweli
 */
public class ServiceUtil
{
    /**
     * <pre>
     * 取得服務設定類別物件
     * 當服務類別沒有 @ServiceConfigClass 標註時, 這個函式會傳回null
     * </pre>
     * 
     * @param clazz 服務類別物件
     * @return 類別物件
     */
    public static Class<?> getConfigClass(Class<?> clazz)
    {
        ServiceConfig serviceConfigClass = clazz.getAnnotation(ServiceConfig.class);
        
        return serviceConfigClass != null ? serviceConfigClass.configClass() : null;
    }
    
    /**
     * <pre>
     * 檢查服務類別是否需要執行啟動程序
     * </pre>
     * 
     * @param clazz 服務類別物件
     * @return true表示需要執行啟動程序, false則否
     */
    public static boolean needStart(Class<?> clazz)
    {
        return clazz.getAnnotation(ServiceStart.class) != null;
    }
    
    /**
     * <pre>
     * 檢查服務類別是否需要執行結束程序
     * </pre>
     * 
     * @param clazz 服務類別物件
     * @return true表示需要執行結束程序, false則否
     */
    public static boolean needFinish(Class<?> clazz)
    {
        return clazz.getAnnotation(ServiceFinish.class) != null;
    }
}
