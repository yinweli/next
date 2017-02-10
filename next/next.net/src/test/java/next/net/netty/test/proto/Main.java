package next.net.netty.test.proto;

import next.net.netty.NettyServer;
import next.net.netty.handler.HandlerProto;
import next.net.netty.handler.HandlerProto.CoreProto;

import com.google.protobuf.Descriptors.Descriptor;
import com.google.protobuf.Message.Builder;

public class Main
{
    private static HandlerProto handlerProto = new HandlerProto(new CoreProto() {
        public Descriptor getDescriptor()
        {
            return PacketCoreWapper.PacketCore.getDescriptor();
        }
        
        public Object getDefaultInstance()
        {
            return PacketCoreWapper.PacketCore.getDefaultInstance();
        }
        
        public Builder getBuilder()
        {
            return PacketCoreWapper.PacketCore.newBuilder();
        }
    });
    private static NettyServer nettyServer = new NettyServer();
    
    public static void main(String[] args) throws Exception
    {
        handlerProto.addConnectProcessor(new ProcessorConnect());
        handlerProto.addDisconnectProcessor(new ProcessorDisconnect());
        handlerProto.addPacketProcessor(new ProcessorPacket1());
        handlerProto.addPacketProcessor(new ProcessorPacket2());
        
        nettyServer.port = 9527;
        nettyServer.countOfThread = 10;
        nettyServer.secondOfIdleKick = 10;
        nettyServer.handler = handlerProto;
        nettyServer.start();
    }
}
