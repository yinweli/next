package service.statistics.processor;

import service.statistics.StatisticsService;
import surfer.net.netty.connection.Connection;
import surfer.net.netty.handler.HandlerProto.Processor;

public class ConnectProcessor implements Processor
{
    @Override
    public Class<?> packetClass()
    {
        return null;
    }
    
    @Override
    public void onEvent(Connection connection, Object object) throws Exception
    {
        StatisticsService.clientConnect();
    }
}
