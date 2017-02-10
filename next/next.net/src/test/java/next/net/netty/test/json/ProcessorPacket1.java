package next.net.netty.test.json;

import next.net.netty.connection.Connection;
import next.net.netty.handler.HandlerGson.Processor;

public class ProcessorPacket1 implements Processor
{
    @Override
    public Class<?> packetClass()
    {
        return Packet1.class;
    }
    
    @Override
    public void onEvent(Connection connection, final Object object) throws Exception
    {
        Packet1 packet = (Packet1) object;
        
        System.out.println("IP=" + connection.getIP());
        System.out.println(String.format("playerId=%d, money=%d, time=%d, vip=%s", packet.getPlayerId(), packet
            .getMoney(), packet.getTime(), packet.getVip() ? "true" : "false"));
    }
    
}
