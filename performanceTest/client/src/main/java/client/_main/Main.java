package client._main;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;

import client.unit.ClientUnits;

public class Main
{
    private static final Logger log = Logger.getLogger(Main.class);
    
    public static void main(String[] args) throws InterruptedException
    {
        PropertyConfigurator.configure(MainConfig.log4jConfigPath);
        
        if (args.length < 3)
        {
            log.error("arguments format=count, ip, port");
            return;
        } //if
        
        ClientUnits clientUnits = new ClientUnits();
        
        clientUnits.count = Integer.parseInt(args[0]);
        clientUnits.ip = args[1];
        clientUnits.port = Integer.parseInt(args[2]);
        clientUnits.interval = 5000;
        clientUnits.start();
        
        while (true)
        {
            clientUnits.update();
            Thread.sleep(1000);
        } //while
    }
}
