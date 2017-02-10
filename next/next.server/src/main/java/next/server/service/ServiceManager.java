package next.server.service;

import java.util.ArrayList;
import java.util.List;
import java.util.ListIterator;

import next.server.util.config.ConfigUtil;
import next.server.util.service.ServiceUtil;

import org.apache.log4j.Logger;

public class ServiceManager
{
    private static final Logger log = Logger.getLogger(ServiceManager.class);
    
    /** 服務類別函式名稱:服務啟動 */
    private static final String METHOD_START = "start";
    /** 服務類別函式名稱:服務結束 */
    private static final String METHOD_FINISH = "finish";
    
    /** 服務類別物件列表 */
    private static List<Class<?>> services = new ArrayList<>();
    
    /** 設定檔案路徑 */
    public static String configPath = "";
    /** 設定檔案副檔名 */
    public static String configExt = "";
    
    /**
     * <pre>
     * 新增服務類別物件
     * </pre>
     * 
     * @param clazz 服務類別物件
     */
    public static void add(Class<?> clazz)
    {
        services.add(clazz);
    }
    
    /**
     * <pre>
     * 服務啟動
     * 依照加入的順序順向啟動
     * </pre>
     */
    public static void start()
    {
        // 讀取設定檔
        for (Class<?> itor : services)
        {
            Class<?> configClass = ServiceUtil.getConfigClass(itor);
            
            if (configClass != null)
            {
                try
                {
                    ConfigUtil.load(configClass, configPath, configExt);
                    log.info(String.format("%s:load config success", itor.getSimpleName()));
                }
                catch (Exception e)
                {
                    log.warn(String.format("%s:load config failed", itor.getSimpleName()), e);
                }
            }
            else
                log.info(String.format("%s:ignore load config", itor.getSimpleName()));
        }//for
        
        // 啟動服務
        for (Class<?> itor : services)
        {
            if (ServiceUtil.needStart(itor))
            {
                try
                {
                    itor.getMethod(METHOD_START).invoke(null);
                    log.info(String.format("%s:initialize success", itor.getSimpleName()));
                }
                catch (Exception e)
                {
                    log.warn(String.format("%s:initialize failed", itor.getSimpleName()));
                }
            }
            else
                log.info(String.format("%s:ignore initialize", itor.getSimpleName()));
        }//for
    }
    
    /**
     * <pre>
     * 服務結束
     * 依照加入的順序逆向結束
     * </pre>
     */
    public static void finish()
    {
        ListIterator<Class<?>> iterator = services.listIterator(services.size());
        
        while (iterator.hasPrevious())
        {
            Class<?> clazz = iterator.previous();
            
            if (ServiceUtil.needFinish(clazz))
            {
                try
                {
                    clazz.getMethod(METHOD_FINISH).invoke(null);
                    log.info(String.format("%s:finish success", clazz.getSimpleName()));
                }
                catch (Exception e)
                {
                    log.warn(String.format("%s:finish failed", clazz.getSimpleName()));
                }
            }
            else
                log.info(String.format("%s:ignore finish", clazz.getSimpleName()));
        }//while
    }
}
