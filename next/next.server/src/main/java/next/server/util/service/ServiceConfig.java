package next.server.util.service;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/**
 * <pre>
 * 標註:指定服務的設定類別
 * </pre>
 * 
 * @author yinweli
 */
@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.TYPE)
public @interface ServiceConfig
{
    /**
     * <pre>
     * 取得服務設定類別物件
     * </pre>
     * 
     * @return 類別物件
     */
    Class<?> configClass();
}
