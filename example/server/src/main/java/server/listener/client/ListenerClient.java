package server.listener.client;

import com.google.protobuf.Descriptors.Descriptor;
import com.google.protobuf.Message.Builder;

import packet._core.CorePacketWapper;
import surfer.net.netty.NettyServer;
import surfer.net.netty.handler.HandlerProto;
import surfer.net.netty.handler.HandlerProto.CoreProto;
import surfer.net.netty.handler.HandlerProto.Processor;
import surfer.server.util.service.ServiceConfig;
import surfer.server.util.service.ServiceFinish;
import surfer.server.util.service.ServiceStart;
import surfer.server.util.thead.ThreadUtil;

@ServiceConfig(configClass = ListenerClientConfig.class)
@ServiceStart
@ServiceFinish
public class ListenerClient
{
    /** 伺服器物件 */
    private static NettyServer nettyServer = new NettyServer();
    /** 封包處理物件 */
    private static HandlerProto handlerProto = new HandlerProto(new CoreProto() {
        @Override
        public Descriptor getDescriptor()
        {
            return CorePacketWapper.CorePacket.getDescriptor();
        }
        
        @Override
        public Object getDefaultInstance()
        {
            return CorePacketWapper.CorePacket.getDefaultInstance();
        }
        
        @Override
        public Builder getBuilder()
        {
            return CorePacketWapper.CorePacket.newBuilder();
        }
    });
    
    /**
     * <pre>
     * 服務啟動
     * </pre>
     * 
     * @throws Exception
     */
    public static void start() throws Exception
    {
        nettyServer.port = ListenerClientConfig.PORT;
        nettyServer.countOfThread = ThreadUtil.calculateThread(ListenerClientConfig.MULTIPLE_THREAD);
        nettyServer.secondOfIdleKick = ListenerClientConfig.SECOND_IDLE_KICK;
        nettyServer.handler = handlerProto;
        nettyServer.start();
    }
    
    /**
     * <pre>
     * 服務結束
     * </pre>
     * 
     * @throws Exception
     */
    public static void finish() throws Exception
    {
        nettyServer.finish();
    }
    
    /**
     * <pre>
     * 新增連線處理者物件
     * </pre>
     * 
     * @param processor 處理者物件
     * @throws Exception
     */
    public static void addConnectProcessor(final Processor processor) throws Exception
    {
        handlerProto.addConnectProcessor(processor);
    }
    
    /**
     * <pre>
     * 新增斷線處理者物件
     * </pre>
     * 
     * @param processor 處理者物件
     * @throws Exception
     */
    public static void addDisconnectProcessor(final Processor processor) throws Exception
    {
        handlerProto.addDisconnectProcessor(processor);
    }
    
    /**
     * <pre>
     * 新增封包處理者物件
     * </pre>
     * 
     * @param processor 處理者物件
     * @throws Exception
     */
    public static void addPacketProcessor(final Processor processor) throws Exception
    {
        handlerProto.addPacketProcessor(processor);
    }
}
