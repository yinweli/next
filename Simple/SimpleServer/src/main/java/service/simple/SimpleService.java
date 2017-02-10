package service.simple;

import next.server.util.service.ServiceStart;
import server.listener.client.ListenerClient;
import service.simple.processor.Simple1Processor;

@ServiceStart
public class SimpleService
{
    /**
     * <pre>
     * 服務啟動
     * </pre>
     * 
     * @throws Exception
     */
    public static void start() throws Exception
    {
        ListenerClient.addPacketProcessor(new Simple1Processor());
    }
}
