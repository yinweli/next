package surfer.server.service;

import java.util.ArrayList;
import java.util.List;
import java.util.ListIterator;

import org.apache.log4j.Logger;

import surfer.server.util.config.ConfigUtil;
import surfer.server.util.service.ServiceUtil;

/**
 * <pre>
 * 服務管理類別
 * 
 * > 服務類別
 * 服務類別指的是伺服器中執行大型功能(例如搖錢樹)的類別
 * 由於此種類別通常會跨執行緒處理, 因此會作成成員全部靜態化並且加鎖的形式
 * 
 * > 建立服務類別
 * 建立服務類別時有以下三個標註可以使用
 * <code>@ServiceConfig</code>
 *     指定服務的設定類別
 *     伺服器啟動時, 會先經由設定檔機制把設定讀到你指定的設定類別中
 * <code>@ServiceStart</code>
 *     服務帶有啟動函式
 *     伺服器啟動時, 會呼叫啟動函式
 *     啟動函式的格式一律為 public static void start()
 * <code>@ServiceFinish</code>
 *     服務帶有結束函式
 *     伺服器關閉時, 會呼叫結束函式
 *     關閉指的是使用System.exit函式關閉, 若是直接關閉視窗, 不會觸發服務結束
 *     啟動函式的格式一律為 public static void finish()
 * 最後再度提醒, 服務類別的成員最好都是靜態, 並且要記得這個類別會在多執行緒的環境中執行
 * 
 * > 建立服務設定類別
 * 服務設定類別通常裡面會放入很多公開靜態的變數, 這些設定變數必須要加上標註
 * <code>@Property(key = 索引字串, defaultValue = 預設字串)</code>
 * 如此才會在啟動時將設定值從設定檔案填入設定變數中
 * 
 * > 服務管理
 * 只要呼叫 ServiceManager.add() 函式把服務類別加入就可以了
 * 
 * > 服務的啟動與結束順序
 * 服務啟動會依照加入順序順向執行
 * 服務結束會依照加入順序反向執行
 * 
 * > 其他
 * 使用前要先設定
 * ServiceManager.configPath
 * ServiceManager.configExt
 * </pre>
 * 
 * @author yinweli
 */
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
        } //for
        
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
                    log.warn(String.format("%s:initialize failed", itor.getSimpleName()), e);
                }
            }
            else
                log.info(String.format("%s:ignore initialize", itor.getSimpleName()));
        } //for
        
        // 加入服務結束鉤子
        Runtime.getRuntime().addShutdownHook(new Thread(new Runnable() {
            @Override
            public void run()
            {
                ServiceManager.finish();
            }
        }, "Service shutdown"));
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
                    log.warn(String.format("%s:finish failed", clazz.getSimpleName()), e);
                }
            }
            else
                log.info(String.format("%s:ignore finish", clazz.getSimpleName()));
        } //while
    }
}
