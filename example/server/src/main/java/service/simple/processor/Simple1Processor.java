package service.simple.processor;

import org.apache.log4j.Logger;

import packet.simple.SimpleWapper;
import surfer.net.netty.connection.Connection;
import surfer.net.netty.handler.HandlerProto.Processor;

public class Simple1Processor implements Processor
{
    private static final Logger log = Logger.getLogger(Simple1Processor.class);
    
    @Override
    public Class<?> packetClass()
    {
        return SimpleWapper.Simple1.class;
    }
    
    @Override
    public void onEvent(Connection connection, Object object) throws Exception
    {
        SimpleWapper.Simple1 packet = (SimpleWapper.Simple1) object;
        SimpleWapper.Simple2 sendPacket = SimpleWapper.Simple2.newBuilder().setEchoTime(packet.getEchoTime()).build();
        
        connection.send(packet, sendPacket);
    }
    
}
