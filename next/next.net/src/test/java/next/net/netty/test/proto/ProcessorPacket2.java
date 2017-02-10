package next.net.netty.test.proto;

import next.net.netty.connection.Connection;
import next.net.netty.handler.HandlerProto.Processor;

public class ProcessorPacket2 implements Processor
{
    @Override
    public Class<?> packetClass()
    {
        return Packet2Wapper.Packet2.class;
    }
    
    @Override
    public void onEvent(Connection connection, final Object object) throws Exception
    {
        Packet2Wapper.Packet2 packet = (Packet2Wapper.Packet2) object;
        
        System.out.println(String.format("name=%s, message=%s", packet.getName(), packet.getMessage()));
    }
}
