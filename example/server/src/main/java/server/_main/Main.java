package server._main;

import org.apache.log4j.PropertyConfigurator;

import server.listener.client.ListenerClient;
import service.scheduled.ScheduledService;
import service.simple.SimpleService;
import surfer.server.service.ServiceManager;

public class Main
{
    public static void main(String[] args)
    {
        PropertyConfigurator.configure(MainConfig.log4jConfigPath);
        
        ServiceManager.configPath = MainConfig.configPath;
        ServiceManager.configExt = MainConfig.configExt;
        
        ServiceManager.add(ListenerClient.class);
        ServiceManager.add(ScheduledService.class);
        ServiceManager.add(SimpleService.class);
        
        ServiceManager.start();
    }
}
