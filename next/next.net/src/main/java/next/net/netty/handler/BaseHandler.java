package next.net.netty.handler;

import io.netty.channel.socket.SocketChannel;
import next.net.netty.connection.Connection;

/**
 * <pre>
 * 基礎封包處理類別
 * 使用者需要自行處理封包的編\解碼
 * 如果想要實作出各種封包協議的處理類別, 都必須繼承此類別, 並最少實作出以下函式
 * <code>
 * public abstract void setCoder(SocketChannel soc) throws Exception;
 * public abstract void recv(Connection connection, Object obj) throws Exception;
 * public abstract Object send(Object obj) throws Exception;
 * </code>
 * 
 * 實做時要注意, 此類別內的函式必須要注意執行緒安全
 * </pre>
 * 
 * @author yilin_lee
 */
public abstract class BaseHandler
{
    /**
     * <pre>
     * 加入編\解碼器通知
     * </pre>
     * 
     * @param soc Socket連線物件
     * @throws Exception
     */
    public abstract void coder(SocketChannel soc) throws Exception;
    
    /**
     * <pre>
     * 頻道啟動通知
     * 當頻道建立時(也就是連線時), 會呼叫此函式一次
     * </pre>
     * 
     * @param connection 連線物件
     * @throws Exception
     */
    public abstract void active(Connection connection) throws Exception;
    
    /**
     * <pre>
     * 頻道關閉通知
     * 當頻道關閉時(也就是斷線時), 會呼叫此函式一次
     * </pre>
     * 
     * @param connection 連線物件
     * @throws Exception
     */
    public abstract void inactive(Connection connection) throws Exception;
    
    /**
     * <pre>
     * 接收封包處理
     * </pre>
     * 
     * @param connection 連線物件
     * @param object 封包物件
     * @throws Exception
     */
    public abstract void recv(Connection connection, final Object object) throws Exception;
    
    /**
     * <pre>
     * 傳送封包處理
     * </pre>
     * 
     * @param objects 封包物件列表
     * @return 傳送物件
     * @throws Exception
     */
    public abstract Object send(final Object... objects) throws Exception;
}
