package service.simple;

import server.listener.client.ListenerClient;
import service.simple.processor.Simple1Processor;
import surfer.server.util.service.ServiceStart;

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
