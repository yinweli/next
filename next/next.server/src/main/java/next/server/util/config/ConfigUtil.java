package next.server.util.config;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.Properties;

import next.server.util.configuration.ConfigurableProcessor;

/**
 * <pre>
 * 設定檔工具類別
 * </pre>
 * 
 * @author yinweli
 */
public class ConfigUtil
{
    /**
     * <pre>
     * 讀取設定
     * </pre>
     * 
     * @param clazz 設定類別物件
     * @param path 設定檔案路徑
     * @param ext 設定檔案副檔名
     * @throws FileNotFoundException
     * @throws IOException
     */
    public static void load(Class<?> clazz, String path, String ext) throws FileNotFoundException, IOException
    {
        Properties properties = new Properties();
        
        properties.load(new FileInputStream(new File(path, clazz.getSimpleName() + ext)));
        
        ConfigurableProcessor.process(clazz, properties);
    }
}
