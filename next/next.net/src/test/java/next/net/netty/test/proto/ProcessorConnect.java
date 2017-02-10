package next.net.netty.test.proto;

import next.net.netty.connection.Connection;
import next.net.netty.handler.HandlerProto.Processor;

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
        
        Packet1Wapper.Packet1.Builder builder1 = Packet1Wapper.Packet1.newBuilder();
        
        builder1.setPlayerId(++id);
        builder1.setMoney(5000000);
        builder1.setTime(System.nanoTime());
        builder1.setVip(true);
        
        Packet2Wapper.Packet2.Builder builder2 = Packet2Wapper.Packet2.newBuilder();
        
        builder2.setName("送錢人");
        builder2.setMessage("送了很多錢");
        
        connection.send(builder1.build(), builder2.build());
    }
}
