package next.net.netty.test.json;

import next.net.netty.connection.Connection;
import next.net.netty.handler.HandlerGson.Processor;

public class ProcessorPacket2 implements Processor
{
    @Override
    public Class<?> packetClass()
    {
        return Packet2.class;
    }
    
    @Override
    public void onEvent(Connection connection, final Object object) throws Exception
    {
        Packet2 packet = (Packet2) object;
        
        System.out.println(String.format("name=%s, message=%s", packet.getName(), packet.getMessage()));
    }
}
