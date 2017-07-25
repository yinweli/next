package next.net.netty;

import java.io.IOException;

import io.netty.bootstrap.ServerBootstrap;
import io.netty.channel.ChannelFuture;
import io.netty.channel.ChannelHandler;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.ChannelInboundHandlerAdapter;
import io.netty.channel.ChannelInitializer;
import io.netty.channel.ChannelOption;
import io.netty.channel.EventLoopGroup;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.SocketChannel;
import io.netty.channel.socket.nio.NioServerSocketChannel;
import io.netty.handler.timeout.IdleState;
import io.netty.handler.timeout.IdleStateEvent;
import io.netty.handler.timeout.IdleStateHandler;
import io.netty.util.concurrent.DefaultEventExecutorGroup;
import io.netty.util.concurrent.EventExecutorGroup;
import next.net.netty.connection.Connection;
import next.net.netty.handler.BaseHandler;

/**
 * <pre>
 * Netty伺服器類別
 * 
 * 使用時需要設定各種屬性, 其中最重要的是封包處理物件
 * 封包處理物件需要使用者自己建立, 它必須繼承以下類別之一
 *     BaseHandler 基礎封包處理類別, 使用者需要自行處理封包的編\解碼
 *     HandlerProto 以google protobuf為基礎的封包處理類別
 *     HandlerGson 以google gson為基礎的封包處理類別
 * 以此類別建立實體, 設定到 ServerNetty.Handler
 * 最後再呼叫 ServerNetty.start() 函式來啟動伺服器
 * </pre>
 * 
 * @author yilin_lee
 */
public class NettyServer
{
    /** 監聽的埠號 */
    public int port = 0;
    /** 當瞬間有大量客戶端想要登入時, 系統容許的等待處理的客戶端連線數量, 超過這個數量的客戶端將可能被直接拒絕 */
    public int backLog = 128;
    /**
     * 邏輯業務執行緒數量, 0表示邏輯業務將與讀寫業務共用執行緒, 如果想以CPU核心數來決定執行緒數量,
     * 可以Runtime.getRuntime().availableProcessors()函式來取得核心數
     */
    public int countOfThread = 0;
    /** 客戶端靜止斷線時間(秒), 0表示不會因靜止斷線 */
    public int secondOfIdleKick = 0;
    /** 是否關閉Nagle演算法 */
    public boolean tcpNoDelay = true;
    /** 是否保持客戶端連接 */
    public boolean keepAlive = true;
    /** 封包處理物件 */
    public BaseHandler handler = null;
    
    /** 伺服器控制物件 */
    private ChannelFuture channelFuture = null;
    /** 用於處理連線業務的執行緒池 */
    private EventLoopGroup boss = null;
    /** 用於處理讀寫業務或是邏輯業務的執行緒池 */
    private EventLoopGroup work = null;
    /** 用於處理邏輯業務的執行緒池 */
    private EventExecutorGroup event = null;
    
    /**
     * <pre>
     * 啟動伺服器
     * </pre>
     * 
     * @throws Exception
     */
    public void start() throws Exception
    {
        if (handler == null)
            throw new Exception("handler null");
        
        if (channelFuture != null)
            throw new Exception("server is start");
        
        try
        {
            boss = new NioEventLoopGroup(); // 用於處理連線業務的執行緒池
            work = new NioEventLoopGroup(); // 用於處理讀寫業務或是邏輯業務的執行緒池
            event = countOfThread > 0 ? new DefaultEventExecutorGroup(countOfThread) : null; // 用於處理邏輯業務的執行緒池
            channelFuture = new ServerBootstrap() // 啟動伺服器
                .group(boss, work)
                .channel(NioServerSocketChannel.class)
                .option(ChannelOption.SO_BACKLOG, backLog)
                .option(ChannelOption.TCP_NODELAY, tcpNoDelay)
                .childOption(ChannelOption.SO_KEEPALIVE, keepAlive)
                .childHandler(new ChannelInitializer<SocketChannel>() {
                    @Override
                    protected void initChannel(SocketChannel soc) throws Exception
                    {
                        handler.coder(soc); // 加入編\解碼器
                        
                        ChannelHandler channelHandler = new ChannelInboundHandlerAdapter() {
                            @Override
                            public void channelActive(ChannelHandlerContext ctx) throws Exception
                            {
                                handler.active(new Connection(handler, ctx.channel()));
                            }
                            
                            @Override
                            public void channelInactive(ChannelHandlerContext ctx) throws Exception
                            {
                                handler.inactive(new Connection(handler, ctx.channel()));
                            }
                            
                            @Override
                            public void channelRead(ChannelHandlerContext ctx, Object packet) throws Exception
                            {
                                handler.recv(new Connection(handler, ctx.channel()), packet);
                            }
                            
                            @Override
                            public void channelReadComplete(ChannelHandlerContext ctx) throws Exception
                            {
                                ctx.flush();
                            }
                            
                            @Override
                            public void userEventTriggered(ChannelHandlerContext ctx, Object evt) throws Exception
                            {
                                if (evt instanceof IdleStateEvent == false)
                                    return;
                                
                                IdleStateEvent event = (IdleStateEvent) evt;
                                
                                if (event.state() != IdleState.READER_IDLE)
                                    return;
                                
                                ctx.close();
                            }
                            
                            @Override
                            public void exceptionCaught(ChannelHandlerContext ctx, Throwable cause) throws Exception
                            {
                                if (cause instanceof IOException == false)
                                    throw new Exception(cause);
                            }
                        };
                        
                        // 加入頻道處理物件
                        if (event == null)
                        {
                            soc.pipeline().addLast("idleHandler", new IdleStateHandler(secondOfIdleKick, 0, 0));
                            soc.pipeline().addLast("channelHandler", channelHandler);
                        }
                        else
                        {
                            soc.pipeline().addLast(event, "idleHandler", new IdleStateHandler(secondOfIdleKick, 0, 0));
                            soc.pipeline().addLast(event, "channelHandler", channelHandler);
                        } //if
                    }
                })
                .bind(port)
                .sync();
        } //try
        catch (Exception e)
        {
            channelFuture = null;
            boss = null;
            work = null;
            event = null;
            
            throw e;
        } //catch
    }
    
    /**
     * <pre>
     * 結束伺服器
     * </pre>
     * 
     * @throws Exception
     */
    public void finish() throws Exception
    {
        if (channelFuture == null)
            throw new Exception("server not start");
        
        channelFuture.channel().closeFuture().sync();
        
        if (boss != null)
            boss.shutdownGracefully();
        
        if (work != null)
            work.shutdownGracefully();
        
        if (event != null)
            event.shutdownGracefully();
    }
}
