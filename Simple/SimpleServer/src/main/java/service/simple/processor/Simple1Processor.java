package service.simple.processor;

import next.net.netty.connection.Connection;
import next.net.netty.handler.HandlerProto.Processor;

import org.apache.log4j.Logger;

import packet.simple.SimpleWapper;

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
        /**
         * <pre>
         * 範例封包處理:
         * 把客戶端送來的Simple1封包中的value1, value2相加, 把結果存到Simple2的result欄位中
         * 把客戶端送來的Simple1封包與存放結果的Simple2封包都送回給客戶端
         * </pre>
         */
        
        SimpleWapper.Simple1 packet = (SimpleWapper.Simple1) object;
        SimpleWapper.Simple2 sendPacket = SimpleWapper.Simple2
            .newBuilder()
            .setResult(packet.getValue1() + packet.getValue2())
            .build();
        
        connection.send(packet, sendPacket);
        
        log.info(String.format("%s:%d+%d=%d", packet.getTitle(), packet.getValue1(), packet.getValue2(), sendPacket
            .getResult()));
    }
}
