package next.net.netty.test.proto;

import next.net.netty.connection.Connection;
import next.net.netty.handler.HandlerProto.Processor;

public class ProcessorDisconnect implements Processor
{
    @Override
    public Class<?> packetClass()
    {
        return null;
    }
    
    @Override
    public void onEvent(Connection connection, final Object object) throws Exception
    {
        System.out.println("disconnect=" + connection.getIP());
    }
}
