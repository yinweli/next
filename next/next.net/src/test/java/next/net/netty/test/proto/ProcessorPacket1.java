package next.net.netty.test.proto;

import next.net.netty.connection.Connection;
import next.net.netty.handler.HandlerProto.Processor;

public class ProcessorPacket1 implements Processor
{
    @Override
    public Class<?> packetClass()
    {
        return Packet1Wapper.Packet1.class;
    }
    
    @Override
    public void onEvent(Connection connection, final Object object) throws Exception
    {
        Packet1Wapper.Packet1 packet = (Packet1Wapper.Packet1) object;
        
        System.out.println("IP=" + connection.getIP());
        System.out.println(String.format("playerId=%d, money=%d, time=%d, vip=%s", packet.getPlayerId(), packet
            .getMoney(), packet.getTime(), packet.getVip() ? "true" : "false"));
    }
}
