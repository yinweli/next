package next.net.netty.connection;

import java.net.InetSocketAddress;

import io.netty.channel.ChannelHandlerContext;
import io.netty.util.Attribute;
import io.netty.util.AttributeKey;
import next.net.netty.handler.BaseHandler;

/**
 * <pre>
 * 連線類別
 * 
 * 當客戶端連到伺服器時, 伺服器就會產生此物件來管理這條連線
 * 這個物件可以被留下, 但是要注意偵測斷線事件以及使用屬性功能來管理已經被斷線的連線物件
 * </pre>
 * 
 * @author yinweli
 */
public class Connection
{
    /** 封包處理物件 */
    private BaseHandler handler = null;
    /** 連線管理物件 */
    private ChannelHandlerContext ctx = null;
    
    public Connection(BaseHandler handler, ChannelHandlerContext ctx)
    {
        this.handler = handler;
        this.ctx = ctx;
    }
    
    /**
     * <pre>
     * 關閉連線
     * </pre>
     */
    public void close()
    {
        ctx.close();
    }
    
    /**
     * <pre>
     * 傳送封包
     * </pre>
     * 
     * @param ctx 客戶端物件
     * @param packets 封包物件
     * @throws Exception
     */
    public void send(final Object... packets) throws Exception
    {
        if (handler == null)
            throw new Exception("handler null");
            
        if (ctx == null)
            throw new Exception("client null");
            
        if (packets == null)
            throw new Exception("packet null");
            
        ctx.writeAndFlush(handler.send(packets));
    }
    
    /**
     * <pre>
     * 取得客戶端位址
     * </pre>
     * 
     * @return 客戶端位址
     */
    public String getIP()
    {
        return ((InetSocketAddress) ctx.channel().remoteAddress()).getAddress().getHostAddress();
    }
    
    /**
     * <pre>
     * 取得屬性物件
     * </pre>
     * 
     * @param key 屬性索引物件
     * @return 屬性物件
     */
    public <T> Attribute<T> getAttribute(AttributeKey<T> key)
    {
        return ctx.channel().attr(key);
    }
}
