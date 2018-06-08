package service.statistics;

import java.util.concurrent.TimeUnit;

import org.apache.log4j.Logger;

import server.listener.client.ListenerClient;
import service.scheduled.ScheduledService;
import service.statistics.processor.ConnectProcessor;
import service.statistics.processor.DisconnectProcessor;
import service.statistics.processor.PingProcessor;
import surfer.server.util.service.ServiceStart;

@ServiceStart
public class StatisticsService
{
    private static final Logger log = Logger.getLogger(StatisticsService.class);
    
    private static Object lock = new Object();
    /** 客戶端數量 */
    private static long clientCount = 0;
    /** 總ping次數 */
    private static long totalPing = 0;
    /** 總ping時間 */
    private static long totalPeriod = 0;
    
    /**
     * <pre>
     * 服務啟動
     * </pre>
     * 
     * @throws Exception
     */
    public static void start() throws Exception
    {
        ListenerClient.addConnectProcessor(new ConnectProcessor());
        ListenerClient.addDisconnectProcessor(new DisconnectProcessor());
        ListenerClient.addPacketProcessor(new PingProcessor());
        
        ScheduledService.schedule(new Runnable() {
            @Override
            public void run()
            {
                long currentClientCount = 0;
                long currentPing = 0;
                long currentPeriod = 0;
                
                synchronized (lock)
                {
                    currentClientCount = clientCount;
                    currentPing = totalPing;
                    currentPeriod = totalPeriod;
                    
                    totalPing = 0;
                    totalPeriod = 0;
                }
                
                float avgPing = currentClientCount > 0 ? currentPing / (float) currentClientCount : 0.0f;
                float avgPeriod = currentPing > 0 ? currentPeriod / (float) currentPing : 0.0f;
                
                log.info(String
                    .format("client=%5d, ping=%8d, period=%8d, avgPing=%.2f, avgPeriod=%.2fms", currentClientCount, currentPing, currentPeriod, avgPing, avgPeriod));
            }
        }, 10, 10, TimeUnit.SECONDS);
    }
    
    /**
     * <pre>
     * 客戶端連線處理
     * </pre>
     */
    public static void clientConnect()
    {
        synchronized (lock)
        {
            ++clientCount;
        }
    }
    
    /**
     * <pre>
     * 客戶端斷線處理
     * </pre>
     */
    public static void clientDisconnect()
    {
        synchronized (lock)
        {
            --clientCount;
        }
    }
    
    /**
     * <pre>
     * ping處理
     * </pre>
     * 
     * @param period ping時間
     */
    public static void ping(long period)
    {
        synchronized (lock)
        {
            ++totalPing;
            totalPeriod += period;
        }
    }
}
