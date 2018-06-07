package service.statistics.processor;

import packet.ping.PingWapper;
import service.statistics.StatisticsService;
import surfer.net.netty.connection.Connection;
import surfer.net.netty.handler.HandlerProto.Processor;

public class PingProcessor implements Processor
{
    @Override
    public Class<?> packetClass()
    {
        return PingWapper.Ping.class;
    }
    
    @Override
    public void onEvent(Connection connection, Object object) throws Exception
    {
        PingWapper.Ping packet = (PingWapper.Ping) object;
        
        StatisticsService.ping(packet.getPeriod());
        
        connection.send(PingWapper.Pong.newBuilder().setTime(packet.getTime()).build());
    }
}
