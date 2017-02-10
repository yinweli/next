package server.listener.client.config;

import next.server.util.configuration.Property;

public class ListenerClientConfig
{
    /** 埠號 */
    @Property(key = "listener.client.port", defaultValue = "3001")
    public static int PORT = 0;
    
    /** 執行緒倍數 */
    @Property(key = "listener.client.multiple_thread", defaultValue = "1.5")
    public static double MULTIPLE_THREAD = 0;
    
    /** 客戶端靜止斷線時間(秒), 0表示不會因靜止斷線 */
    @Property(key = "listener.client.second_idle_kick", defaultValue = "600")
    public static int SECOND_IDLE_KICK = 0;
}
