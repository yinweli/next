package next.net.netty;

import io.netty.bootstrap.Bootstrap;
import io.netty.channel.ChannelFuture;
import io.netty.channel.ChannelHandler;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.ChannelInboundHandlerAdapter;
import io.netty.channel.ChannelInitializer;
import io.netty.channel.ChannelOption;
import io.netty.channel.EventLoopGroup;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.SocketChannel;
import io.netty.channel.socket.nio.NioSocketChannel;
import next.net.netty.connection.Connection;
import next.net.netty.handler.BaseHandler;

/**
 * <pre>
 * Netty客戶端類別
 * 
 * 使用時需要設定各種屬性, 其中最重要的是封包處理物件
 * 封包處理物件需要使用者自己建立, 它必須繼承以下類別之一
 *     BaseHandler 基礎封包處理類別, 使用者需要自行處理封包的編\解碼
 *     HandlerGson 以google gson為基礎的封包處理類別
 *     HandlerProto 以google protobuf為基礎的封包處理類別
 * 以此類別建立實體, 設定到 ClientNetty.Handler
 * 最後再呼叫 ClientNetty.start() 函式來啟動客戶端
 * </pre>
 * 
 * @author yilin_lee
 */
public class NettyClient
{
    /** 伺服器位址 */
    public String ip = "";
    /** 伺服器埠號 */
    public int port = 0;
    /** 是否關閉Nagle演算法 */
    public boolean tcpNoDelay = true;
    /** 是否保持與伺服器的連接 */
    public boolean keepAlive = true;
    /** 封包處理物件 */
    public BaseHandler handler = null;
    
    /** 用於處理連線業務, 讀寫業務, 邏輯業務的執行緒池 */
    private EventLoopGroup work = null;
    /** 網路物件 */
    private ChannelFuture future = null;
    
    /**
     * <pre>
     * 啟動客戶端
     * </pre>
     * 
     * @throws Exception
     */
    public void start() throws Exception
    {
        if (handler == null)
            throw new Exception("handler null");
        
        // 建立並啟動網路物件
        work = new NioEventLoopGroup();
        future = new Bootstrap()
            .group(work)
            .channel(NioSocketChannel.class)
            .option(ChannelOption.TCP_NODELAY, tcpNoDelay)
            .option(ChannelOption.SO_KEEPALIVE, keepAlive)
            .handler(new ChannelInitializer<SocketChannel>() {
                @Override
                protected void initChannel(SocketChannel soc) throws Exception
                {
                    ChannelHandler channelHandler = new ChannelInboundHandlerAdapter() {
                        @Override
                        public void channelActive(ChannelHandlerContext ctx) throws Exception
                        {
                            handler.active(new Connection(handler, ctx));
                        }
                        
                        @Override
                        public void channelInactive(ChannelHandlerContext ctx) throws Exception
                        {
                            handler.inactive(new Connection(handler, ctx));
                        }
                        
                        @Override
                        public void channelRead(ChannelHandlerContext ctx, Object packet) throws Exception
                        {
                            handler.recv(new Connection(handler, ctx), packet);
                        }
                        
                        @Override
                        public void userEventTriggered(ChannelHandlerContext ctx, Object event) throws Exception
                        {
                        }
                        
                        @Override
                        public void exceptionCaught(ChannelHandlerContext ctx, Throwable cause) throws Exception
                        {
                            throw new Exception(cause);
                        }
                    };
                    
                    handler.coder(soc); // 加入編\解碼器
                    
                    // 加入頻道處理物件
                    soc.pipeline().addLast("handler", channelHandler);
                }
            })
            .connect(ip, port)
            .sync();
    }
    
    /**
     * <pre>
     * 關閉客戶端
     * </pre>
     * 
     * @throws InterruptedException
     */
    public void stop() throws InterruptedException
    {
        try
        {
            future.channel().closeFuture().sync();
        } // try
        finally
        {
            work.shutdownGracefully();
        } // finally
    }
    
    /**
     * <pre>
     * 傳送封包
     * </pre>
     * 
     * @param packet 封包物件
     * @return 頻道物件
     * @throws Exception
     */
    public ChannelFuture send(final Object packet) throws Exception
    {
        if (handler == null)
            throw new Exception("handler null");
        
        if (packet == null)
            throw new Exception("packet null");
        
        return future.channel().writeAndFlush(handler.send(packet));
    }
}
