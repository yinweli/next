package client.unit;

import com.google.protobuf.Descriptors.Descriptor;
import com.google.protobuf.Message.Builder;

import packet._core.CorePacketWapper;
import packet.ping.PingWapper;
import surfer.net.netty.NettyClient;
import surfer.net.netty.connection.Connection;
import surfer.net.netty.handler.HandlerProto;
import surfer.net.netty.handler.HandlerProto.CoreProto;
import surfer.net.netty.handler.HandlerProto.Processor;

public class ClientUnit
{
    /** 客戶端物件 */
    private NettyClient nettyClient = new NettyClient();
    /** 封包處理物件 */
    private HandlerProto handlerProto = new HandlerProto(new CoreProto() {
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
    /** 是否已連線 */
    private boolean connected = false;
    /** ping時間 */
    private long period = 0;
    
    /**
     * <pre>
     * 取得是否已連線
     * </pre>
     * 
     * @return 是否已連線
     */
    public boolean getConnected()
    {
        return connected;
    }
    
    /**
     * <pre>
     * 客戶端服務啟動
     * </pre>
     * 
     * @param ip 伺服器位址
     * @param port 伺服器埠號
     * @return 客戶端物件
     * @throws Exception
     */
    public ClientUnit start(String ip, int port) throws Exception
    {
        nettyClient.ip = ip;
        nettyClient.port = port;
        nettyClient.handler = handlerProto;
        handlerProto.addConnectProcessor(new Processor() {
            @Override
            public Class<?> packetClass()
            {
                return null;
            }
            
            @Override
            public void onEvent(Connection connection, Object object) throws Exception
            {
                connected = true;
            }
        });
        handlerProto.addDisconnectProcessor(new Processor() {
            @Override
            public Class<?> packetClass()
            {
                return null;
            }
            
            @Override
            public void onEvent(Connection connection, Object object) throws Exception
            {
                connected = false;
            }
        });
        handlerProto.addPacketProcessor(new Processor() {
            @Override
            public Class<?> packetClass()
            {
                return PingWapper.Pong.class;
            }
            
            @Override
            public void onEvent(Connection connection, Object object) throws Exception
            {
                PingWapper.Pong packet = (PingWapper.Pong) object;
                
                period = System.currentTimeMillis() - packet.getTime();
                
                sendPing();
            }
        });
        nettyClient.start();
        
        sendPing();
        
        return this;
    }
    
    /**
     * <pre>
     * 傳送ping封包
     * </pre>
     * 
     * @throws Exception
     */
    private void sendPing() throws Exception
    {
        nettyClient.send(PingWapper.Ping.newBuilder().setTime(System.currentTimeMillis()).setPeriod(period).build());
    }
}
