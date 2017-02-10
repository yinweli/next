package next.net.netty.test.json;

import next.net.netty.connection.Connection;
import next.net.netty.handler.HandlerGson.Processor;

public class ProcessorConnect implements Processor
{
    private int id = 0;
    
    @Override
    public Class<?> packetClass()
    {
        return null;
    }
    
    @Override
    public void onEvent(Connection connection, final Object object) throws Exception
    {
        System.out.println("connect=" + connection.getIP());
        
        Packet1 packet1 = new Packet1();
        
        packet1.setPlayerId(++id);
        packet1.setMoney(5000000);
        packet1.setTime(System.nanoTime());
        packet1.setVip(true);
        
        Packet2 packet2 = new Packet2();
        
        packet2.setName("送錢人");
        packet2.setMessage("送了很多錢");
        
        connection.send(packet1, packet2);
    }
}
