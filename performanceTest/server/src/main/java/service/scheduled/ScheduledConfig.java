package service.scheduled;

import surfer.server.util.configuration.Property;

public class ScheduledConfig
{
    /** 執行緒倍數 */
    @Property(key = "service.scheduled.multiple_thread", defaultValue = "0.5")
    public static double MULTIPLE_THREAD = 0;
}
